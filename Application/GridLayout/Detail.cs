using Application.Core;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.DTO.Grid;
using System.Collections.Generic;
using Domain;

namespace Application.GridLayout
{
    public class Detail
    {
        public class Query : IRequest<Result<GridLayoutDto>> { 
        public long GridLayoutMasterID { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<GridLayoutDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<GridLayoutDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                GridLayoutDto returnObject = new GridLayoutDto();
                GridLayoutMaster gridLayoutMaster = await _context.GridLayoutMasters.SingleOrDefaultAsync(m => m.GridLayoutMasterID == request.GridLayoutMasterID);
                returnObject.Master = gridLayoutMaster;
                //To show all fields data, un-comment this line
                /*List<GridLayoutDetail> gridLayoutDetail = await _context.GridLayoutDetails.Where(x=>x.GridLayoutMasterID == request.GridLayoutMasterID).ToListAsync();
                returnObject.Detail = gridLayoutDetail;
                */

                var gridLayoutDetailResult = await _context.GridLayoutDetails.Where(x=>x.GridLayoutMasterID == request.GridLayoutMasterID).ToListAsync();
                List<GridLayoutDetail> listofcameraip = new List<GridLayoutDetail>(); 
                foreach (var item in gridLayoutDetailResult)
                {
                    listofcameraip.Add(new GridLayoutDetail(){CameraIP=item.CameraIP});
                }
                returnObject.Detail =listofcameraip;

/*
                var query = (from m in _context.GridLayoutMasters
                             join d in _context.GridLayoutDetails on m.GridLayoutMasterID equals d.GridLayoutMasterID
                             where m.GridLayoutMasterID == request.GridLayoutMasterID
                             select new GridLayoutDto {GridLayoutMasterID= m.GridLayoutMasterID,LayoutName= m.LayoutName,NoofColumns= m.NoofColumns,CameraIP= d.CameraIP });
*/

                //var overall = await query.ToListAsync().ConfigureAwait(false); 


                return Result<GridLayoutDto>.Success(returnObject);
            }
        }
    }
}
