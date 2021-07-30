using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Classes;

namespace API.Extension
{
    public class ffmegRecording
    {
        private static ConcurrentDictionary<int, Process> _processes = new ConcurrentDictionary<int, Process>();
        private static ConcurrentDictionary<int, string> _processesPath = new ConcurrentDictionary<int, string>();
        

        public ConcurrentDictionary<int,string> getAllTheNameOfThePath(){
            try{
                return _processesPath;
            }
            catch(Exception e){
                return null;
            }
        }

        public Process Start(string FFMPEGQuery, string InputFileFolder)
        {
            try
            {
                //I used cmd rather than FFMPEG because facing issue with the ffmpeg command in the dotnet environment.
                //Add FFMPEG as a global environment (Path) in windows                
                var processStartInfo = new ProcessStartInfo()
                {
                    Arguments = $"/c cd /d {InputFileFolder} && ffmpeg {FFMPEGQuery} ",
                    FileName = "cmd.exe",
                    RedirectStandardInput = true, // Must be set to true
                    UseShellExecute = false       // Must be set to false
                };

                Process p = Process.Start(processStartInfo);

                // Add the Process to the Dictionary with Key: Process ID and value as the running process
                _processes.TryAdd(p.Id, p);
                _processesPath.TryAdd(p.Id, InputFileFolder);

                return p;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int isFileunderprocessing(string FilePathName){
            try {
                //Check if value exist in _processesPath
                return _processesPath.FirstOrDefault(x => x.Value.Equals(FilePathName)).Key;
            }
            catch(Exception ex){
                return -1;
            }

        }

        private Process GetProcessByPid(int pid)
        {
            return _processes.FirstOrDefault(p => p.Key == pid).Value;
        }

        public bool Stop(int pid, bool isRemoveFile = false)
        {
            try
            {                
                Process p = GetProcessByPid(pid);
                if(pid > 0){
                //Remove the Process. 
                p.StandardInput.WriteLine("q\n");
                p.Kill();
                }
                string FilePathName = "";
                if (isRemoveFile)
                {
                    if(pid>0){
                        if (_processesPath.TryGetValue(pid, out FilePathName))
                        {
                            Thread.Sleep(3000);
                            Directory.Delete(FilePathName, true);
                        }
                    }
                }

                _processes.TryRemove(pid, out p);
                _processesPath.TryRemove(pid, out FilePathName);

                // Free up resources
                if(pid > 0){
                    p.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}