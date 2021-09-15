using Application.Core;
using AutoMapper;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Right
{
    public class Edit
    {
        public class Command: IRequest<Result<Unit>>
        {
            public RightsAllotmentM rightsAllotmentM { get; set; }
            public List<RightsAllotmentD> rightsAllotmentD { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext,IMapper mapper)
            {
                this._dataContext = dataContext;
                this._mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                using (IDbContextTransaction transaction = _dataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var Master = await _dataContext.RightsAllotmentMs.FindAsync(request.rightsAllotmentM.RightsAllotmentID);
                        if(Master == null)
                        {
                            await transaction.RollbackAsync();
                            return Result<Unit>.Failure("No Master Record Found");
                        }
//                        _mapper.Map(request.rightsAllotmentM, Master);
                        Master.UpdateDateTime = request.rightsAllotmentM.UpdateDateTime;
                        Master.UpdateSystemIP = request.rightsAllotmentM.UpdateSystemIP;
                        Master.UpdateSystemName = request.rightsAllotmentM.UpdateSystemName;
                        Master.UpdateUserID = request.rightsAllotmentM.UpdateUserID;
                        Master.TotalRightValue = request.rightsAllotmentM.TotalRightValue;
                        var result = await _dataContext.SaveChangesAsync() > 0;

                        _dataContext.LogFiles.Add(new Domain.LogFile()
                        {
                            TransactionID = request.rightsAllotmentM.RightsAllotmentID.ToString(),
                            Description = "Updating User in the Database",
                            EntryDate = request.rightsAllotmentM.UpdateDateTime??DateTime.Now,
                            sqlCommand = request.rightsAllotmentM.ToString(),
                            UserID = request.rightsAllotmentM.UpdateUserID,
                            UserIP = request.rightsAllotmentM.UpdateSystemIP,
                            UserSystem = request.rightsAllotmentM.UpdateSystemName
                        });
                        await _dataContext.SaveChangesAsync();


                        var gridLayoutDetail = await _dataContext.RightsAllotmentDs.Where(m => m.RightsAllotmentMID == request.rightsAllotmentM.RightsAllotmentID).ToListAsync();
                        bool isUpdated = false;
                        foreach (var item in gridLayoutDetail)
                        {
                            _dataContext.Remove(item);
                            isUpdated = await _dataContext.SaveChangesAsync() > 0;
                        }
                        if (!isUpdated) { transaction.Rollback();return Result<Unit>.Failure("Detail Transaction Not Found"); }
                        foreach (var item in request.rightsAllotmentD)
                        {
                            item.RightsAllotmentMID = request.rightsAllotmentM.RightsAllotmentID;
                            _dataContext.RightsAllotmentDs.Add(item);
                            await _dataContext.SaveChangesAsync();
                            _dataContext.RightsAllotmentHistories.Add(new RightsAllotmentHistory()
                            {
                                RightsAllotmentMID = item.RightsAllotmentMID,
                                RightID = item.RightID,
                                EntryDate = (DateTime)request.rightsAllotmentM.UpdateDateTime,
                                UserID = request.rightsAllotmentM.UserID,
                                UserIP = request.rightsAllotmentM.UpdateSystemIP,
                                UserSystem = request.rightsAllotmentM.UpdateSystemName,
                                EnteredUserID = request.rightsAllotmentM.UpdateUserID,
                                TotalRightValue=request.rightsAllotmentM.TotalRightValue

                            });
                            await _dataContext.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                        return Result<Unit>.Success(Unit.Value);


                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return Result<Unit>.Failure(ex.InnerException.ToString());
                    }
                }
            }
        }

    }
}