using System.Collections.Generic;
using System.IO;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xabe.FFmpeg;
using System.Threading;
using API.Classes;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace API.Controllers
{

    [AllowAnonymous]
    public class StreamController : BaseApiController
    {
//        private CancellationTokenSource cancellationTokenSource;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private string PathName; //= @"";
        private static string  VirtualPathName;// = @"";
        private readonly string searchPattern;// = "*.mp4";
        private string configFolderName;
        private readonly IConfiguration __config;
        
        public StreamController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, TokenService token, IConfiguration config)
        {
            __config = config;
            _token = token;
            _signInManager = signInManager;
            _userManager = userManager;

            PathName = __config["FilesDriveIISPath"];
            VirtualPathName = __config["FilesDrivePath"];
            searchPattern = __config["FileSearchPattern"];
            configFolderName = __config["configFolderName"];
        
//            cancellationTokenSource = new CancellationTokenSource();    
        }

        [HttpGet]
        [Route("CancelToken/{FilePath}")]
        public ActionResult<bool> cancelTokken(string FilePath){
            try{
                FilePath = GlobalFunction.ConvertASCIIIntoString(FilePath);
                string output = extractPath(FilePath);
                foreach (FFMPEGClass item in InsertFFMPEG.FFMPEGobj)
                {
                    if(item.GUID == output)
                    {
                        item.cancelToken.Cancel();
                        InsertFFMPEG.FFMPEGobj.Remove(item);
                    }

                }
                return true;
            }
            catch(Exception x){
                Logger.LogException(x);
                return false;
            }

        }
        [HttpGet]
        [Route("deleteFile/{FileName}")]
        public ActionResult<bool> DeleteFiles(String FileName){
            try{
                bool isSingleRecord = true;
                if(FileName == "0"){
                    isSingleRecord=false;
                    FileName="";
                } 

                FileName = extractPath(FileName);
                if(isSingleRecord){
                    int index = -1;
                    bool isfound = false;
                    foreach (FFMPEGClass item in InsertFFMPEG.FFMPEGobj)
                    {
                        index++;
                        if(FileName == item.GUID)
                        {
                            item.cancelToken.Cancel();
                            isfound=true;
                            break;
                        }
                    }
                    if(isfound)
                        InsertFFMPEG.FFMPEGobj.RemoveAt(index);
                }
                else{
                    foreach (FFMPEGClass item in InsertFFMPEG.FFMPEGobj)
                    {
                            item.cancelToken.Cancel();
                    }
                    InsertFFMPEG.FFMPEGobj.RemoveRange(0,InsertFFMPEG.FFMPEGobj.Count);
                }

                
                //string path=extractPath("");
                DirectoryInfo di = new DirectoryInfo(FileName);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo directory in di.GetDirectories())
                {
                    foreach(FileInfo file in directory.GetFiles()){
                        file.Delete();
                    }
                    directory.Delete();
                }

                return Ok(true);
            }
            catch(Exception x){
                Logger.LogException(x);
                return BadRequest(false);
            }
        }
        [HttpGet]
        [Route("{FilePath}")]
        public  ActionResult<bool> isFileExist(string FilePath){
            try{                
                bool found = false;
                FilePath = GlobalFunction.ConvertASCIIIntoString(FilePath);
                FilePath = extractPath(FilePath);
                while(!found){
                    if (System.IO.File.Exists(FilePath))
                        return found=true;
                }
                return Ok(true);
                /*
                Thread.Sleep(1000);
                var result = await WhenFileCreated(FilePath);
                return Ok(true);*/

            }
            catch(Exception x){
                Logger.LogException(x);
                return BadRequest(false);
            }
        }
        private static string extractPath(string FilePath){
            string[] filepath = FilePath.Split('/');
            string output = VirtualPathName.Replace("video","TempFiles");
            foreach(string file in filepath){
                output = Path.Combine(output,file);
            }
            return output;
        }
        /*
        public Task<bool> WhenFileCreated(string path)
        {
            path = GlobalFunction.ConvertASCIIIntoString(path);
            path = extractPath(path);
            if (System.IO.File.Exists(path))
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path));

            FileSystemEventHandler createdHandler = null;
            createdHandler = (s, e) =>
            {
                //if (e.Name == Path.GetFileName(path))
               // {
                    tcs.TrySetResult(true);
                    watcher.Created -= createdHandler;
                    watcher.Dispose();
                //}
            };

            watcher.Filter ="*.m3u8";

            watcher.Created += createdHandler;
           
            watcher.EnableRaisingEvents = true;

            return tcs.Task;
        }*/
        /*
        public Task WhenFileCreated(string path)
        {
            path = GlobalFunction.ConvertASCIIIntoString(path);
            path = extractPath(path);
            if (System.IO.File.Exists(path))
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();
            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path));

            FileSystemEventHandler createdHandler = null;
            createdHandler = (s, e) =>
            {
                //if (e.Name == Path.GetFileName(path))
               // {
                    tcs.TrySetResult(true);
                    watcher.Created -= createdHandler;
                    watcher.Dispose();
                //}
            };

            watcher.Filter ="*.m3u8";

            watcher.Created += createdHandler;
           
            watcher.EnableRaisingEvents = true;

            return tcs.Task;
        }*/

        [HttpGet]
        [Route("{url}/{FilePath}")]
        public async Task<ActionResult<CameraIPResult>> StreamIP(string url,string FilePath){
            url = GlobalFunction.ConvertASCIIIntoString(url);
            FilePath = GlobalFunction.ConvertASCIIIntoString(FilePath);
            CameraIPResult returnObject = new CameraIPResult();
            string output = extractPath(FilePath);

            

            if(await setStreamingCalculation(output,url))
            {
                returnObject = new CameraIPResult(){IPfound=true,FileServerAddress=output };
            }
            else
            {
                returnObject = new CameraIPResult(){IPfound=false,FileServerAddress="" };
            }
            return Ok(returnObject);
        } 
        private void callFFMPEG(IMediaInfo mediaInfo,string output){

            CancellationTokenSource canceltoken = new CancellationTokenSource();

            var conversionResult = Task.Run(() => Xabe.FFmpeg.FFmpeg.Conversions.New()
                            .AddStream(mediaInfo.Streams)
                            .SetOutput(output).AddParameter("-profile:v baseline -fflags -nobuffer -probesize 32 -level 3.0 -start_number 0 -hls_time 2 -hls_list_size 0 -f hls", ParameterPosition.PostInput)
                            .Start(canceltoken.Token));

            InsertFFMPEG.ADDFFMPEG(output,conversionResult,canceltoken);

            /*
            Task.Run(() => FFmpeg.Conversions.New()
                            .AddStream(mediaInfo.Streams)
                            .SetOutput(output)
                            .Start(no.Token));
*/
/*
            Task.Run(() =>  FFmpeg.Conversions.New()
                            .AddStream(mediaInfo.Streams)
                            .SetOutput(output).AddParameter("-profile:v baseline -fflags -nobuffer -probesize 32 -level 3.0 -start_number 0 -hls_time 2 -hls_list_size 0 -f hls", ParameterPosition.PostInput)
                            .Start(no.Token)); */
        }
        private async Task<bool> setStreamingCalculation(string output,string url){
            try{

            var mediaInfo = await Xabe.FFmpeg.FFmpeg.GetMediaInfo(url);
            callFFMPEG(mediaInfo,output);
            return true;            

            }
            catch(Exception x){
                Logger.LogException(x);
                return false;
            }   
        }

    }
}
