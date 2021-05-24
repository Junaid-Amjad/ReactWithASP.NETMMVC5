using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SavedFileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private string PathName = @"http://strandusa.localtest.me/video/";
        private string VirtualPathName = @"E:\Strantin-Work\Strand-Git-Testing-2\Strand\StrandUSAIIS\video\";
        private readonly string searchPattern = "*.mp4";
        public SavedFileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService token)
        {
            _token = token;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpGet("{**slug}")]
        public ActionResult<IEnumerable<FilePath>> getFiledetail(string slug){
            if(slug.Contains("%2F")){
                PathName += slug.Replace("%2F","/")+"/";
                VirtualPathName += slug.Replace("%2F","\\")+"\\";
            }
            else{
                PathName += slug+"/";
                VirtualPathName += slug.Replace("/","\\")+"\\";
            }

            List<FilePath> file = new List<FilePath>();

            DirectoryInfo di = new DirectoryInfo(VirtualPathName);
            var d = new DirectoryInfo(VirtualPathName);
            var v = d.GetDirectories().Select(subDirectory => subDirectory.Name).ToList();
            foreach (var dir in v)
            {
                file.Add(new FilePath(){path=dir,isDirectory=true,isSamePath=false});
            }


            FileInfo[] files =
                di.GetFiles(searchPattern,SearchOption.AllDirectories);

            foreach (var item in files)
            {
                bool isSamePath;
                if(item.DirectoryName+"\\" == VirtualPathName) 
                    isSamePath=true;
                else
                    isSamePath=false; 
                file.Add(new FilePath(){path= PathName+item.Name,isDirectory=false,isSamePath=isSamePath});
            }


            return file;

        }

        [HttpGet]
        public ActionResult<IEnumerable<FilePath>> getFileNames(){
            List<FilePath> files2=new List<FilePath>();

            DirectoryInfo di = new DirectoryInfo(VirtualPathName);
            var d = new DirectoryInfo(VirtualPathName);
            var v = d.GetDirectories().Select(subDirectory => subDirectory.Name).ToList();
            foreach (var dir in v)
            {
                files2.Add(new FilePath(){path=dir,isDirectory=true,isSamePath=false});
            }


            FileInfo[] files =
                di.GetFiles(searchPattern,SearchOption.AllDirectories);

            foreach (var item in files)
            {
                bool isSamePath;
                if(item.DirectoryName+"\\" == VirtualPathName) 
                    isSamePath=true;
                else
                    isSamePath=false; 
                files2.Add(new FilePath(){path= PathName+item.Name,isDirectory=false,isSamePath=isSamePath});
            }

            return files2.ToList();
        }
 
    }
    

    public class FilePath{
        public bool isDirectory{get;set;} 
        public string path{get;set;}
        public bool isSamePath { get; set; }
    }

}