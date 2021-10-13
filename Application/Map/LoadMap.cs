using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.Maps;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.Map
{
    public class LoadMap
    {
        public class Query:IRequest<Result<MapList>>{
            public long MapListID { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<MapList>>
        {
            private readonly DataContext context;
            private readonly IConfiguration configuration;
            private string _configMapPath;

            public Handler(DataContext context,IConfiguration configuration)
            {
                this.context = context;
                this.configuration = configuration;

                _configMapPath = configuration["FilesDriveIISPath"];
                _configMapPath = _configMapPath.Replace("video/","Map/");
            }

            public async Task<Result<MapList>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await context.MapLists.Where(x=>x.MapListID == request.MapListID).FirstOrDefaultAsync();
                if(result == null)
                    return Result<MapList>.Failure("No Record Found");
                if(result.ImageSrc != null)
                    result.ImageSrc = Path.Combine(_configMapPath,result.ImageSrc);
                return Result<MapList>.Success(result);
            }
        }
    }
}