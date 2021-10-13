using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.Maps;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace Application.Map
{
    public class UpdateMap
    {
        public class Query : IRequest<Result<MapList>>
        {
            public MapList mapList { get; set; }
            public MapPosition mapPosition{get;set;}
        }
        public class Handler : IRequestHandler<Query, Result<MapList>>
        {
            private readonly DataContext _context;
            private readonly IHttpContextAccessor httpContextAccessor;
            private static readonly HttpClient client = new HttpClient();

            static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);
            string baseURL="";

            public Handler(DataContext context,IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                this.httpContextAccessor = httpContextAccessor;
                this.baseURL = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                
            }

            public async Task<Result<MapList>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                await semaphoreSlim.WaitAsync();

                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try{
                        var result = await _context.MapLists.Where(x=>x.MapListID == request.mapList.MapListID).FirstOrDefaultAsync();
                        if (result == null){
                            return Result<MapList>.Failure("No Record Found");
                        }
                        var RevertBack = result;
                        result.MapListName = request.mapList.MapListName;
                        result.MapCategoriesID = request.mapList.MapCategoriesID;
                        result.UpdateDateTime = request.mapList.EntryDate;
                        result.UpdateUserID = request.mapList.UserID.ToString();
                        result.UpdateUserIP = request.mapList.SystemIP;
                        result.UpdateUserSystem =request.mapList.SystemName;
                        result.ImageSrc = request.mapList.ImageSrc;
                        result.ImageFile = request.mapList.ImageFile;
                        result.IsActive = request.mapList.IsActive;
                        result.IsCancel = request.mapList.IsCancel;
                        result.CameraIP = request.mapList.CameraIP;

                        await _context.SaveChangesAsync();
                        _context.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = request.mapList.MapListID.ToString(),
                            Description = "Updating Map in the DataBase",
                            EntryDate = DateTime.Now,
                            sqlCommand=request.mapList.ToString(),
                            UserID=request.mapList.UserID.ToString(),
                            UserIP=request.mapList.SystemIP,
                            UserSystem=request.mapList.SystemName
                        }) ;
                        MapPosition mapPosition = new MapPosition(){
                            MapListID=request.mapList.MapListID,
                            Rotation=request.mapPosition.Rotation,
                            X=0,
                            Y=0,
                            SystemIP=request.mapList.SystemIP,
                            SystemName=request.mapList.SystemName,
                            UserID=request.mapList.UserID.ToString(),
                            SavedStatusID=1,
                            Width=request.mapPosition.Width,
                            Height=request.mapPosition.Height                            
                        };
/*
                        var AlreadySavedValues = await _context.MapPositions.Where(x => x.MapListID == request.mapList.MapListID).FirstOrDefaultAsync();
                        if (AlreadySavedValues != null)
                        {
                            AlreadySavedValues.Rotation= request.rotation;
                            AlreadySavedValues.UserID = request.mapList.UserID.ToString();
                            AlreadySavedValues.SystemIP = request.mapList.SystemIP;
                            AlreadySavedValues.SystemName = request.mapList.SystemName;
                            _context.MapPositions.Update(AlreadySavedValues);
                            var resultExist = await _context.SaveChangesAsync() > 0;
                            if (!resultExist) { await transaction.RollbackAsync(); 
                            return Result<MapList>.Failure("Record Not Saved"); }
                        }*/


                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        if(client.BaseAddress == null)
                            client.BaseAddress = new Uri(baseURL);
                        var response = await client.PostAsJsonAsync("/api/Map/setPositionOnTheMap",mapPosition);
                        if(response.IsSuccessStatusCode){
                            return Result<MapList>.Success(result);                        
                        }else{
                            result.MapListName = RevertBack.MapListName;
                            result.MapCategoriesID = RevertBack.MapCategoriesID;
                            result.UpdateDateTime = RevertBack.EntryDate;
                            result.UpdateUserID = RevertBack.UserID.ToString();
                            result.UpdateUserIP = RevertBack.SystemIP;
                            result.UpdateUserSystem =RevertBack.SystemName;
                            result.ImageSrc = RevertBack.ImageSrc;
                            result.ImageFile = RevertBack.ImageFile;
                            result.IsActive = RevertBack.IsActive;
                            result.IsCancel = RevertBack.IsCancel;
                            result.CameraIP = RevertBack.CameraIP;

                            await _context.SaveChangesAsync();
                            _context.LogFiles.Add(new Domain.LogFile()
                            {
                                TransactionID = request.mapList.MapListID.ToString(),
                                Description = "Reverting of Updating Map in the DataBase",
                                EntryDate = DateTime.Now,
                                sqlCommand=request.mapList.ToString(),
                                UserID=request.mapList.UserID.ToString(),
                                UserIP=request.mapList.SystemIP,
                                UserSystem=request.mapList.SystemName
                            }) ;
                            await _context.SaveChangesAsync();

                            return Result<MapList>.Failure("Saving Error when setting position");
                        }
                    }catch(Exception ex){
                        await transaction.RollbackAsync();
                        return Result<MapList>.Failure(ex.ToString());
                    }
                    finally{
                        semaphoreSlim.Release();
                    }


                }
            }
        }
    }
}