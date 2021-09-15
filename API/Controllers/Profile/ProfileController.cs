using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers.Profile
{
    [AllowAnonymous]
    public class ProfileController : BaseApiController
    {
        private readonly DataContext _context;
        public ProfileController(DataContext context){
            _context = context;
        }        
        [HttpGet("GetUserViews")]
        public async Task<IActionResult> GetUserViews(){
            List<UserView> resultfound = await  _context.UserViews.Where(x=>x.IsActive==true && x.IsCancel==false).ToListAsync();
            if(resultfound == null || resultfound.Count == 0){
                return HandleResult<List<UserView>>(Result<List<UserView>>.Failure("No Record Found"));
            }else{
                return HandleResult<List<UserView>>(Result<List<UserView>>.Success(resultfound));
            }
        }
        [HttpGet("GetUserViewById/{id}")]
        public async Task<IActionResult> GetUserViewById(int id){
            var resultFound = await _context.UserViews.Where(x=>x.UserViewID==id).FirstOrDefaultAsync();
            if(resultFound == null)
                return HandleResult<string>(Result<string>.Failure("No Record Found"));
            else
                return HandleResult<string>(Result<string>.Success(resultFound.UserViewName));
        }
    }
}