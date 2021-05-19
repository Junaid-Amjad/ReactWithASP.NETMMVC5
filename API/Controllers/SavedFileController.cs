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
        private readonly string PathName = @"http://strandusa.localtest.me/";
        private readonly string VirtualPathName = @"E:\Strantin-Work\Strand-Git-Testing-2\Strand\StrandUSAIIS\";
        private readonly string searchPattern = "*.mp4";
        public SavedFileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService token)
        {
            _token = token;
            _signInManager = signInManager;
            _userManager = userManager;

        }
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

      /*  [HttpGet]
        public ActionResult<IEnumerable<FilePath>> getFileNames(){
            List<FilePath> files=new List<FilePath>();

            var apiurl = PathName;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(apiurl);
            
            HttpResponseMessage ResponseMessage = client.GetAsync(apiurl).Result; // The .Result part will make this method not async
            if (ResponseMessage.IsSuccessStatusCode)
            {
                var ResponseData = ResponseMessage.Content.ReadAsStringAsync().Result;
                var href = ResponseData.Split("<A");
                
                foreach (var item in href)
                {
                    if(!item.Contains("HREF")) continue;
                    var hrefContent = item.Substring(item.IndexOf("\""),item.IndexOf(">")-item.IndexOf("\""));      
                    hrefContent= Regex.Replace(hrefContent,@"""",string.Empty);//"<.*?>"
                    if(hrefContent.Substring(hrefContent.Length-3) != "mp4") continue;
                    files.Add(new FilePath(){path= PathName+hrefContent});              
                }
            }
            return files.ToList();
        }*/
    }
    

    public class FilePath{
        public bool isDirectory{get;set;} 
        public string path{get;set;}
        public bool isSamePath { get; set; }
    }

}