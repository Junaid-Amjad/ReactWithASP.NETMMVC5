using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Diagnostics;
using API.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using API.Classes;

namespace API.Controllers
{
    [AllowAnonymous]
    public class StreamFFMPEGController : BaseApiController
    {
        private readonly IConfiguration __config;
        private readonly ffmegRecording __ffmpegRecording;
        private static string  VirtualPathName;


        public StreamFFMPEGController(IConfiguration config, ffmegRecording ffmpegrecoding)
        {
            __config = config;
            __ffmpegRecording = ffmpegrecoding;
            VirtualPathName = __config["FilesDrivePath"];
        }
        private static string extractPath(string FilePath){
            string[] filepath = FilePath.Split('/');
            string output = VirtualPathName.Replace("video","TempFiles");
            foreach(string file in filepath){
                output = Path.Combine(output,file);
            }
            return output;
        }
        [HttpGet("{FilePath}")]
        public ActionResult<GlobalMessage> GetFileFound(string FilePath){
            try{
                FilePath = GlobalFunction.ConvertASCIIIntoString(FilePath);
                FilePath = extractPath(FilePath);
                if(System.IO.File.Exists(FilePath)){
                    return Ok(new GlobalMessage(){Message="File Found",StatusID=1,StatusDescription=$"Success: {FilePath}"});
                }
                else{
                    return Ok(new GlobalMessage(){Message="File Not Found",StatusID=2,StatusDescription=$"Error: {FilePath}"});
                }
            }
            catch(Exception ex){
                return BadRequest(new GlobalMessage(){Message=ex.ToString(),StatusID=3,StatusDescription="Exception"});
            }
        }
        [HttpGet("{URL}/{FilePath}")]
        public ActionResult<CameraIPResult> Get(String URL, String FilePath){
            try
            {
                if (FilePath == null)
                    return BadRequest(new CameraIPResult() { IPfound = false, Message = "FilePath is null", ProcessID = 0 });
                if (URL == null)
                    return BadRequest(new CameraIPResult() { IPfound = false, Message = "URL is null", ProcessID = 0 });
                URL = GlobalFunction.ConvertASCIIIntoString(URL);
                FilePath = GlobalFunction.ConvertASCIIIntoString(FilePath);

                string[] FileSplit = FilePath.Split('/');
                FilePath = FileSplit[FileSplit.Length - 2];
                string Output = FileSplit[FileSplit.Length - 1];

                FilePath = extractPath(FilePath);
                int pid = __ffmpegRecording.isFileunderprocessing(FilePath);
                if(pid>0)
                {
                 return Ok(new CameraIPResult() { IPfound = true, Message = "Streaming started", ProcessID = pid });
                }

                if (!System.IO.Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                Process p = __ffmpegRecording.Start($" -i {URL} -profile:v baseline -fflags -nobuffer -probesize 32 -level 3.0 -start_number 0 -hls_time 2 -hls_list_size 0 -f hls {Output}", FilePath);
                if (p == null)
                    return BadRequest(new CameraIPResult() { IPfound = false, Message = "Process is null", ProcessID = 0 });
                return Ok(new CameraIPResult() { IPfound = true, Message = "Streaming started", ProcessID = p.Id });

            }
            catch (Exception ex)
            {
                return BadRequest(new CameraIPResult() { IPfound = false, Message = ex.Message, ProcessID = 0 });
            }
        }

        [HttpGet]
        [Route("deleteFile/{PID}")]
        public ActionResult<GlobalMessage> DeleteFiles(String PID){
        try{
                bool isSingleRecord = true;
                if(PID == "0"){
                    isSingleRecord=false;
                    PID="";
                }
                if(isSingleRecord){
                    Stop(Convert.ToInt32(PID));
                } 
                else{
                    var savedfilePathResult = __ffmpegRecording.getAllTheNameOfThePath();
                    
                    if(savedfilePathResult == null){
                        return Ok(new GlobalMessage(){Message="No Record found",StatusID=1,StatusDescription="Success"});
                    }
                    foreach (var itemresult in savedfilePathResult)
                    {Stop(itemresult.Key);}
                }
                return Ok(new GlobalMessage() { Message = "File Removed", StatusID = 1, StatusDescription = "Success" });

            }
            catch (Exception ex){
         return BadRequest(new GlobalMessage(){Message=ex.ToString(),StatusID=3,StatusDescription="Exception"});   
        }

        }
        // GET: api/StreamFFMPEG/stop
        [HttpGet("stop/{pid}")]
        public ActionResult<GlobalMessage> Stop(int pid)
        {
            if (__ffmpegRecording.Stop(pid, true))
                return Ok(new GlobalMessage() { Message = "Streaming Stoped Successfully", StatusID = 1, StatusDescription = "Success" });
            else
                return BadRequest(new GlobalMessage() { Message = "Streaming Stoped Failed", StatusID = 2, StatusDescription = "Error" });
        }

    }
}