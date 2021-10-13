using Application.Core;
using Domain.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Map
{
    public class SaveMapPosition
    {
        public class Command : IRequest<Result<Unit>>{
            public MapPosition mapPosition { get; set; } 
        }
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            public DataContext DataContext { get; }
            private static object lockobj = new object();
            static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1,1);

            public Handler(DataContext dataContext)
            {
                DataContext = dataContext;
            }

            
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                await semaphoreSlim.WaitAsync();
                using (IDbContextTransaction transaction = DataContext.Database.CurrentTransaction ?? await DataContext.Database.BeginTransactionAsync())//System.Data.IsolationLevel.RepeatableRead
                {
                    try
                    {
                        var AlreadySavedValues = await DataContext.MapPositions.Where(x => x.MapListID == request.mapPosition.MapListID).FirstOrDefaultAsync();
                        if (AlreadySavedValues != null)
                        {
                            //if(request.mapPosition.SavedStatusID == 1){
                                AlreadySavedValues.Rotation= request.mapPosition.Rotation;
                            //}
                            //else{
                                if(request.mapPosition.SavedStatusID == 2){
                                    AlreadySavedValues.X = request.mapPosition.X;
                                    AlreadySavedValues.Y = request.mapPosition.Y;

                                }
                            //}

                            AlreadySavedValues.UserID = request.mapPosition.UserID;
                            AlreadySavedValues.SystemIP = request.mapPosition.SystemIP;
                            AlreadySavedValues.SystemName = request.mapPosition.SystemName;
                            AlreadySavedValues.Width = request.mapPosition.Width;
                            AlreadySavedValues.Height = request.mapPosition.Height;
                            AlreadySavedValues.SavedStatusID = request.mapPosition.SavedStatusID;
                            DataContext.MapPositions.Update(AlreadySavedValues);
                            var result = await DataContext.SaveChangesAsync() > 0;
                            if (!result) { await transaction.RollbackAsync(); 
                            return Result<Unit>.Failure("Record Not Saved"); }
                        }
                        else{
                            DataContext.MapPositions.Add(request.mapPosition);
                            var result = await DataContext.SaveChangesAsync() > 0;
                            if (!result) { await transaction.RollbackAsync(); 
                                                    return Result<Unit>.Failure("Record Not Saved"); }
                        }
                        DataContext.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = request.mapPosition.MapListID.ToString(),
                            Description = "Adding and deleting Map Position in the DataBase",
                            EntryDate = DateTime.Now,
                            sqlCommand = request.mapPosition.ToString(),
                            UserID = request.mapPosition.UserID.ToString(),
                            UserIP = request.mapPosition.SystemIP,
                            UserSystem = request.mapPosition.SystemName
                        });

                        await DataContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure(ex.ToString());
                    }
                    finally{
                        semaphoreSlim.Release();
                    }

                }
 
            }
        }
    }
}