using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.Map
{
    public class delete
    {
        public class Command:IRequest<Result<Unit>>{
            public ParameterForDelete DeleteClass { get; set; }
        }
        public class Hanlder : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;
            private readonly IConfiguration configuration;
            private string _configMapPath;

            public Hanlder(DataContext context,IConfiguration configuration)
            {
                this.context = context;
                this.configuration = configuration;
                _configMapPath = configuration["MapFolderName"];
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                     try{
                 
                        await RecursiveDeleteFunction(Convert.ToInt64(request.DeleteClass.TransactionID));
                        var parentValues = await context.MapLists.Where(x => x.MapListID == Convert.ToInt64(request.DeleteClass.TransactionID)).ToListAsync();
                        foreach (var item in parentValues){
                            if(item.ImageSrc == null)continue;
                            string FilePath = Path.Combine(_configMapPath,item.ImageSrc);
                            if (File.Exists(FilePath)){
                                File.Delete(FilePath);
                            }
                        }
                        context.MapLists.RemoveRange(parentValues);
                        await context.SaveChangesAsync();
                        context.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = request.DeleteClass.TransactionID,
                            Description = "Deleting Record in the Database",
                            EntryDate = DateTime.Now,
                            sqlCommand = "MapLists",
                            UserID = request.DeleteClass.UserID,
                            UserIP = request.DeleteClass.UserIP,
                            UserSystem = request.DeleteClass.UserSystem
                        });
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);

                     }
                     catch(Exception ex){
                         await transaction.RollbackAsync();
                         return Result<Unit>.Failure(ex.ToString());
                     }
                }

            }
            public async Task RecursiveDeleteFunction(long MapListID){
                var ChildValues = await context.MapLists.Where(x => x.ParentID == MapListID).ToListAsync();
                foreach (var item in ChildValues)
                {

                    if(item.ImageSrc != null){
                        string FilePath = Path.Combine(_configMapPath,item.ImageSrc);
                        if (File.Exists(FilePath)){
                            File.Delete(FilePath);
                        }
                    }
                    await RecursiveDeleteFunction(item.MapListID);
                    context.MapLists.Remove(item);
                }
            }
        }
    }
}