using System;
using System.Collections.Generic;
using System.Linq;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using API.Classes;
using System.Xml.Linq;
using System.IO;

namespace API.Controllers
{
    [AllowAnonymous]
    public class CameraViewController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private string PathName; //= @"";
        private string VirtualPathName;// = @"";
        private readonly string searchPattern;// = "*.mp4";
        private string configFolderName;
        private readonly IConfiguration __config;

        public CameraViewController(UserManager<AppUser> userManager,
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

        }

        [HttpGet]
        public ActionResult<IEnumerable<cameraAttribute>> getCameraURL()
        {
            List<cameraAttribute> obj =new List<cameraAttribute>();

            foreach (string fileName in Directory.GetFiles(configFolderName,"*.xml",SearchOption.AllDirectories))
            {
                using(StreamReader sr = new StreamReader(fileName,true))
                {

                    XDocument doc = XDocument.Load(sr);
                    foreach (XElement element in doc.Descendants("objects").Descendants("cameras").Descendants("camera").Descendants("settings"))
                    {
                        string url = element.Element("videosourcestring").Value;
                        obj.Add(new cameraAttribute() { URL=url,GUID=Guid.NewGuid().ToString()} );
                    }
                }
            }         
            return Ok(obj.ToList());
        }


        
    }
}