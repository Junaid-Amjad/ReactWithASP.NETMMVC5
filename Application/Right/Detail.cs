using Application.Core;
using Application.DTO.UserRight;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Right
{
    public class Detail
    {
        public class Query : IRequest<Result<UserRightsDto>>
        {
            public String UserID { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<UserRightsDto>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<Result<UserRightsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                UserRightsDto returnObject = new UserRightsDto();
                RightsAllotmentM rightsAllotmentM = _dataContext.RightsAllotmentMs.SingleOrDefault(m => m.UserID == request.UserID && m.IsActive == true && m.IsCancel == false);
                returnObject.RightsAllotmentMaster = rightsAllotmentM;
                List<RightsAllotmentD> rightsAllotmentD = new List<RightsAllotmentD>();
                if(rightsAllotmentM != null){
                    var rightsAllotmentDetailResult = await _dataContext.RightsAllotmentDs.Where(x => x.RightsAllotmentMID == rightsAllotmentM.RightsAllotmentID).ToListAsync();
                    foreach (var item in rightsAllotmentDetailResult)
                    {
                        rightsAllotmentD.Add(item);
                    }
                }
                returnObject.RightsAllotmentDetail = rightsAllotmentD;
                return Result<UserRightsDto>.Success(returnObject);
            }
        }
    }
}