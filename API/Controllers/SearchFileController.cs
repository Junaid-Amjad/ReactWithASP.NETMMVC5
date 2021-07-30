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
using System.Xml.Linq;

namespace API.Controllers
{
    [AllowAnonymous]
    public class SearchFileController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;
        private static string VirtualPathName;
        private static string httpPathName;
        private readonly IConfiguration __config;

        public SearchFileController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, TokenService token, IConfiguration config)
        {
            __config = config;
            _token = token;
            _signInManager = signInManager;
            _userManager = userManager;

            VirtualPathName = __config["FilesDrivePath"];
            httpPathName = __config["FilesDriveIISPath"];
        }

        [HttpGet]
        [Route("GetXMLListOFCamera/{stringobjectparameters}")]
        public ActionResult<List<SearchFilesToXML>> getFileFromXML(string stringobjectparameters)
        {
            long dotnetticks = 621355968000000000;
            long StartTickDate = 0;
            long EndTickDate = 0;
            int index = 0;
            int OrderByID = 0;
            var stringobjectparameterssplit = stringobjectparameters.Split(',');
            foreach (var stringparametervalues in stringobjectparameterssplit)
            {
                switch (index)
                {
                    case 0:
                        {
                            StartTickDate = Convert.ToInt64(stringparametervalues);
                            index++;
                            break;
                        }
                    case 1:
                        {
                            EndTickDate = Convert.ToInt64(stringparametervalues);
                            index++;
                            break;
                        }
                    case 2:
                        {
                            OrderByID = Convert.ToInt32(stringparametervalues);
                            index++;
                            break;
                        }
                    default:
                        {
                            index++;
                            break;
                        }
                }
            }

            StartTickDate = (StartTickDate * 10000) + dotnetticks;
            EndTickDate = (EndTickDate * 10000) + dotnetticks;
            List<SearchFilesToXML> objForXML = new List<SearchFilesToXML>();

            var directoryInfo = Directory.EnumerateFiles(VirtualPathName, "event.xml", SearchOption.AllDirectories);
            try
            {
                foreach (var dirwithFile in directoryInfo)
                {
                    XDocument doc = XDocument.Load(dirwithFile);

                    foreach (XElement element in doc.Descendants("Triggers").Descendants("Trigger"))
                    {

                        objForXML.Add(new SearchFilesToXML()
                        {
                            CameraName = element.Attribute("CameraName").Value,
                            CameraDirectoryName = element.Attribute("CameraDirectoryName").Value,
                            FileName = element.Attribute("FileName").Value,
                            TriggerType = Convert.ToInt16(element.Attribute("TriggerType").Value),
                            CreatedTickDate = Convert.ToInt64(element.Attribute("CreatedTickDate").Value),
                            FolderPath = Path.Combine(httpPathName, element.Attribute("CameraDirectoryName").Value),
                            FullFolderandFileName = Path.Combine(httpPathName, element.Attribute("CameraDirectoryName").Value + "/", element.Attribute("FileName").Value),
                            length = Convert.ToInt32(element.Attribute("Length").Value)

                        });
                    }
                }
                var resultafterapplyingSearchingFilter = (from r in objForXML.AsEnumerable() where r.CreatedTickDate >= StartTickDate && r.CreatedTickDate <= EndTickDate select r);
                List<SearchFilesToXML> objSearchFileToXML = new List<SearchFilesToXML>();
                switch (OrderByID)
                {
                    case 1:
                        {
                            objSearchFileToXML = resultafterapplyingSearchingFilter.OrderBy(p => p.CreatedTickDate).ToList();
                            break;
                        }
                    case 2:
                        {
                            objSearchFileToXML = resultafterapplyingSearchingFilter.OrderByDescending(p => p.CreatedTickDate).ToList();
                            break;
                        }
                    case 3:
                        {
                            objSearchFileToXML = resultafterapplyingSearchingFilter.OrderBy(p => p.length).ToList();
                            break;
                        }
                    case 4:
                        {
                            objSearchFileToXML = resultafterapplyingSearchingFilter.OrderByDescending(p => p.length).ToList();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }


                return objSearchFileToXML;
            }
            catch (Exception ex)
            {
                return objForXML;
            }
        }

        [HttpGet]
        [Route("GetXMLListOFCamera/{StartTickDate}/{EndTickDate}")]
        public ActionResult<List<SearchFilesToXML>> getFileFromXML(long StartTickDate, long EndTickDate)
        {
            long dotnetticks = 621355968000000000;
            StartTickDate = (StartTickDate * 10000) + dotnetticks;
            EndTickDate = (EndTickDate * 10000) + dotnetticks;
            List<SearchFilesToXML> objForXML = new List<SearchFilesToXML>();

            var directoryInfo = Directory.EnumerateFiles(VirtualPathName, "event.xml", SearchOption.AllDirectories);
            try
            {
                foreach (var dirwithFile in directoryInfo)
                {
                    XDocument doc = XDocument.Load(dirwithFile);

                    foreach (XElement element in doc.Descendants("Triggers").Descendants("Trigger"))
                    {

                        objForXML.Add(new SearchFilesToXML()
                        {
                            CameraName = element.Attribute("CameraName").Value,
                            CameraDirectoryName = element.Attribute("CameraDirectoryName").Value,
                            FileName = element.Attribute("FileName").Value,
                            TriggerType = Convert.ToInt16(element.Attribute("TriggerType").Value),
                            CreatedTickDate = Convert.ToInt64(element.Attribute("CreatedTickDate").Value),
                            FolderPath = Path.Combine(VirtualPathName, element.Attribute("CameraDirectoryName").Value),
                            FullFolderandFileName = Path.Combine(VirtualPathName, element.Attribute("CameraDirectoryName").Value, element.Attribute("FileName").Value)

                        });
                    }
                }
                var resultafterapplyingSearchingFilter = (from r in objForXML.AsEnumerable() where r.CreatedTickDate >= StartTickDate && r.CreatedTickDate <= EndTickDate select r).ToList();

                return resultafterapplyingSearchingFilter;
            }
            catch (Exception ex)
            {
                return objForXML;
            }
        }
    }
}