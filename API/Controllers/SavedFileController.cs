using System.Collections.Generic;
using System.IO;
using System.Linq;
using API.Services;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using API.Classes;

namespace API.Controllers
{


    [AllowAnonymous]
    public class SavedFileController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private string PathName; //= @"";
        private string VirtualPathName;// = @"";
        private readonly string searchPattern;// = "*.mp4";
        private string configFolderName;
 
        private readonly IConfiguration __config;
        public SavedFileController(UserManager<AppUser> userManager,
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

        [HttpGet("{**pathdirection}")]
        public ActionResult<IEnumerable<FilePath>> getFiledetail(string pathdirection)
        {
            if (pathdirection.Contains("%2F"))
            {
                PathName += pathdirection.Replace("%2F", "/") + "/";
                VirtualPathName += pathdirection.Replace("%2F", "\\") + "\\";
            }
            else
            {
                PathName += pathdirection + "/";
                VirtualPathName += pathdirection.Replace("/", "\\") + "\\";
            }

            return getFilesPath();
        }

        [HttpGet]
        public ActionResult<IEnumerable<FilePath>> getFileNames()
        {
            return getFilesPath();
        }

        private ActionResult getFilesPath(){
            List<FilePath> files2 = new List<FilePath>();

            DirectoryInfo di = new DirectoryInfo(VirtualPathName);
            var d = new DirectoryInfo(VirtualPathName);
            var v = d.GetDirectories().Select(subDirectory => subDirectory.Name).ToList();
            foreach (var dir in v)
            {
                files2.Add(new FilePath() { path = dir, isDirectory = true, isSamePath = false });
            }


            FileInfo[] files =
                di.GetFiles(searchPattern, SearchOption.AllDirectories);

            foreach (var item in files)
            {
                bool isSamePath;
                if (item.DirectoryName + "\\" == VirtualPathName)
                    isSamePath = true;
                else
                    isSamePath = false;
                files2.Add(new FilePath() { path = PathName + item.Name, isDirectory = false, isSamePath = isSamePath });
            }

            return  Ok(files2.ToList());

        }
    }

}