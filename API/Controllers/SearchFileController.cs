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
    public class SearchFileController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private static string VirtualPathName;
        private readonly IConfiguration __config;

        public SearchFileController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, TokenService token, IConfiguration config)
        {
            __config = config;
            _token = token;
            _signInManager = signInManager;
            _userManager = userManager;

            VirtualPathName = __config["FilesDrivePath"];
        }

        [HttpGet]
        [Route("GetXMLListOFCamera")]
        public ActionResult<SearchFilesToXML> getFileFromXML()
        {
            var directoryInfo = Directory.EnumerateFiles(VirtualPathName, "event.xml", SearchOption.AllDirectories);
            try
            {
                foreach (var dirwithFile in directoryInfo)
                {
                    var item = dirwithFile;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }


        }



    }
}