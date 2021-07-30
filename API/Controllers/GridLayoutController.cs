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
using System.Xml.Linq;
using Domain;
using MediatR;
using Application.GridLayout;
using API.DTOs;

namespace API.Controllers
{
    [AllowAnonymous]
    public class GridLayoutController : BaseApiController
    {
        private readonly IConfiguration __config;
        private static string  VirtualPathName;
        public GridLayoutController(IConfiguration config)
        {
            __config = config;
            VirtualPathName = __config["FilesDrivePath"];
        }
        [HttpGet("getGridLayout")]
        public async Task<ActionResult<List<GridLayoutMaster>>> GetGridLayout()
        {
            return HandleResult(await Mediator.Send(new List.Query())); 
        }
        [HttpGet("getGridLayoutMaster")]
        public async Task<ActionResult<List<GridLayoutMaster>>> GetGridLayoutMaster()
        {
            return HandleResult(await Mediator.Send(new Master.Query())); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGridLayoutDetail(long id)
        {
            return HandleResult(await Mediator.Send(new Detail.Query { GridLayoutMasterID = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGridLayout(GridLayoutDto dto)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Master=dto.Master, Detail = dto.Detail }));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGridLayout(long id){
            return HandleResult(await Mediator.Send(new Delete.Command { GridLayoutMasterID = id }));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(long id,GridLayoutDto dto)
        {
            dto.Master.GridLayoutMasterID = id;
            return HandleResult(await Mediator.Send(new Edit.Command{Master=dto.Master,Detail=dto.Detail}));
        }

        [HttpGet]
        public ActionResult<List<objects>> GetListOfCameras(){
            List<objects> objForXML = new List<objects>();
            string configFilePath = VirtualPathName.Replace("video","config");
            try
            {
                string FilePath = Path.Combine(configFilePath, "objects.xml");
                using (StreamReader sr = new StreamReader(FilePath,true))
                {
                    XDocument doc = XDocument.Load(sr);

                    foreach (XElement element in doc.Descendants("objects").Descendants("cameras").Descendants("camera").Descendants("settings"))
                    {

                        objForXML.Add(new objects()
                        {
                            VideoSourceString = element.Elements("videosourcestring").FirstOrDefault().Value,
                            SourceIndex = Convert.ToInt32(element.Elements("sourceindex").FirstOrDefault().Value),
                        });
                    }                   
                }

                return (Ok(objForXML));
             }catch (Exception ex){
                return (Ok(objForXML));
            }
            
        }
        
    }
}