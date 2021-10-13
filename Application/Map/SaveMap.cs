using Application.Core;
using Domain.Maps;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Map
{
    public class SaveMap
    {
        public class Command : IRequest<Result<Unit>>
        {
            public MapList MapList { get; set; }
            public MapPosition MapPosition {get;set;}
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext dataContext;
            private readonly IHttpContextAccessor httpContextAccessor;
            private static readonly HttpClient client = new HttpClient();
            static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);
            string baseURL="";
            public Handler(DataContext dataContext,IHttpContextAccessor httpContextAccessor)
            {
                this.dataContext = dataContext;
                this.httpContextAccessor = httpContextAccessor;
                this.baseURL = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                        
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                await semaphoreSlim.WaitAsync();
                using (IDbContextTransaction transaction = dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        dataContext.MapLists.Add(request.MapList);
                        var result = await dataContext.SaveChangesAsync() > 0;
                        if (!result) { await transaction.RollbackAsync();return Result<Unit>.Failure("Saving Error "); }
                        dataContext.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = request.MapList.MapListID.ToString(),
                            Description = "Adding New Map in the DataBase",
                            EntryDate = DateTime.Now,
                            sqlCommand=request.MapList.ToString(),
                            UserID=request.MapList.UserID.ToString(),
                            UserIP=request.MapList.SystemIP,
                            UserSystem=request.MapList.SystemName
                        }) ;

                        MapPosition mapPosition = new MapPosition(){
                            MapListID=request.MapList.MapListID,
                            Rotation=request.MapPosition.Rotation,
                            X=0,
                            Y=0,
                            SystemIP=request.MapList.SystemIP,
                            SystemName=request.MapList.SystemName,
                            UserID=request.MapList.UserID.ToString(),
                            SavedStatusID=1,
                            Width=request.MapPosition.Width,
                            Height=request.MapPosition.Height      
                        };
                        /*
                        dataContext.MapPositions.Add(mapPosition);
                        dataContext.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = mapPosition.MapList.ToString(),
                            Description = "Adding and deleting Map Position in the DataBase",
                            EntryDate = DateTime.Now,
                            sqlCommand = mapPosition.ToString(),
                            UserID = mapPosition.UserID.ToString(),
                            UserIP = mapPosition.SystemIP,
                            UserSystem = mapPosition.SystemName
                        });*/
                        await dataContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        if(client.BaseAddress == null)
                            client.BaseAddress = new Uri(baseURL);
                        var response = await client.PostAsJsonAsync("/api/Map/setPositionOnTheMap",mapPosition);
                        if(response.IsSuccessStatusCode){
                            return Result<Unit>.Success(Unit.Value);                        
                        }else{
                            dataContext.MapLists.Remove(request.MapList);
                            await dataContext.SaveChangesAsync();
                            return Result<Unit>.Failure("Saving Error when setting position");
                        }

                    }catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure(ex.Message);

                    }
                    finally{
                        semaphoreSlim.Release();
                    }
                }
            }
        }
    }
}
