using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain.Maps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Application.Map;
using MediatR;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Application;
using Application.DTO.Map;

namespace API.Controllers.Map
{
    [AllowAnonymous]
    public class MapController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration configuration;
        private string _configMapPath;

        public MapController(DataContext dataContext,IConfiguration configuration )
        {
            _dataContext = dataContext;
            this.configuration = configuration;
            _configMapPath = configuration["MapFolderName"];
        }
        [HttpPost("setPositionOnTheMap")]
        public async Task<IActionResult> setPositionOnTheMap(MapPosition MapPosition){
            return HandleResult(await Mediator.Send(new SaveMapPosition.Command { mapPosition = MapPosition }));
        }
        [HttpGet("getMapPosition/{MapListId}")]
        public async Task<IActionResult> getMapPosition(Int64 MapListId){
            var result = await _dataContext.MapPositions.Where(x=>x.MapListID == MapListId).FirstOrDefaultAsync();
            if(result == null)
                return HandleResult<MapPosition>(Result<MapPosition>.Success(new MapPosition(){
                X=0,Y=0,Height=0,Width=0}));
            return HandleResult<MapPosition>(Result<MapPosition>.Success(result));
        }
        [HttpGet("getMapCategories")]
        public async Task<IActionResult> GetMapCategories(){
            var listOfCategories = await _dataContext.MapCategories.Where(x=> x.IsActive==true && x.IsCancel==false ).ToListAsync();
            if(listOfCategories.Count == 0){
                return HandleResult<List<MapCategories>>(Result<List<MapCategories>>.Failure("No Record Found"));
            }else{
                return HandleResult<List<MapCategories>>(Result<List<MapCategories>>.Success(listOfCategories));
            }
        }
        [HttpPut("updateMap")]
        public async Task<IActionResult> UpdateMap([FromForm]MapWithPosition fileImage){
            if(fileImage.MapList.MapListID > 0 && (fileImage.MapList.ImageSrc == "" )){
                var result = await _dataContext.MapLists.Where(x=> x.MapListID == fileImage.MapList.MapListID).FirstOrDefaultAsync();
                if(result != null && result.ImageSrc != null && result.ImageSrc != ""){
                    if(System.IO.File.Exists(Path.Combine(_configMapPath,result.ImageSrc))){
                        System.IO.File.Delete(Path.Combine(_configMapPath,result.ImageSrc));
                    }
                }
            }
            if(fileImage.MapList.ImageFile != null){
                fileImage.MapList.ImageSrc =  await SaveImage(fileImage.MapList.ImageFile,fileImage.MapList.UserID);
            }else{
                if(fileImage.MapList.ImageSrc != null && fileImage.MapList.ImageSrc.Length <= 0)
                    fileImage.MapList.ImageSrc=null;    
            }
           fileImage.MapList.EntryDate = DateTime.Now;
            if (fileImage.MapList.ImageSrc == "" && fileImage.MapList.ImageSrc != null)
                return HandleResult(Result<Unit>.Failure("Issue in uploading File"));
            return HandleResult(await Mediator.Send(new UpdateMap.Query{mapList = fileImage.MapList,mapPosition=fileImage.MapPosition}));
        
        }
        [HttpDelete("deleteRecord")]
        public async Task<IActionResult> deleteRecord(ParameterForDelete deleteobject){
            return HandleResult(await Mediator.Send(new delete.Command{DeleteClass = deleteobject}));
        }
        [HttpGet("getMapRecord/{id}")]
        public async Task<IActionResult> loadMapRecord(long id){
            return HandleResult(await Mediator.Send(new LoadMap.Query{MapListID = id}));
        }
        [HttpPost("saveMapRecord")]
        public async Task<IActionResult> saveMapRecord([FromForm]MapWithPosition fileImage){
            if(fileImage.MapList.ImageFile != null){
                fileImage.MapList.ImageSrc =  await SaveImage(fileImage.MapList.ImageFile,fileImage.MapList.UserID);
            }else{
                fileImage.MapList.ImageSrc=null;
            }
           fileImage.MapList.EntryDate = DateTime.Now;
            if (fileImage.MapList.ImageSrc == "" && fileImage.MapList.ImageSrc != null)
                return HandleResult(Result<Unit>.Failure("Issue in uploading File"));
           return HandleResult(await Mediator.Send(new SaveMap.Command { MapList = fileImage.MapList,MapPosition=fileImage.MapPosition }));
        }   
        [HttpGet("getDataInXML")]
        public async Task<IActionResult> getDataInXML(){
            string xmlStringValue = "";
            using(var command = _dataContext.Database.GetDbConnection().CreateCommand()){
                command.CommandText="exec GetMapData";
//                command.CommandText = "exec GetMapData";
                command.CommandType = CommandType.Text;
                await _dataContext.Database.OpenConnectionAsync();
                using (var result = command.ExecuteReader())
                {
                    while(await result.ReadAsync()){
                        xmlStringValue = result.GetString(0);
                    }                     
                }
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmlStringValue);
            string JsonText = JsonConvert.SerializeXmlNode(xDoc);
//            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(JsonText);
            return HandleResult(Result<String>.Success(JsonText));
        }

        [HttpGet("getMapData/{UserID}")]
        public async Task<IActionResult> getMapData(Guid UserID){
            var result = await _dataContext.MapLists.Where(x=> x.IsActive==true && x.IsCancel==false && x.UserID == UserID).ToListAsync();
            return HandleResult(Result<List<MapList>>.Success(result));
        }
        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile,Guid userID){
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ','-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            if (!Directory.Exists(Path.Combine(_configMapPath, "Images", userID.ToString())))//hostEnvironment.ContentRootPath
            {
                Directory.CreateDirectory(Path.Combine(_configMapPath, "Images", userID.ToString()));
            }
            var imagePath = Path.Combine(_configMapPath, "Images",userID.ToString(), imageName);
            var savedImagePath = Path.Combine("Images",userID.ToString(), imageName);
            using (var filestream = new FileStream(imagePath,FileMode.Create))
            {
                await imageFile.CopyToAsync(filestream);
            }
            return savedImagePath;
        }

    }
}