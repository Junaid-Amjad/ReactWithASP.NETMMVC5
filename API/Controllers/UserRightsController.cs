using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.UserRights;
using Application.Right;
using Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [AllowAnonymous]
    public class UserRightsController : BaseApiController
    {
        public UserRightsController(IConfiguration config)
        {

        }
        [HttpGet("rights")]
        public async Task<ActionResult<List<Rights>>> GetRights(){
            return HandleResult(await Mediator.Send(new List.Query()));
        }
        [HttpPost("saverights")]
        public async Task<IActionResult> CreateUserRights(UserRightDto userRightsDto){
            return HandleResult(await Mediator.Send(new CreateRights.Command { UserRightsMaster = userRightsDto.RightsAllotmentMaster, UserRightsDetail = userRightsDto.RightsAllotmentDetail }));
        }
        [HttpPut("updaterights")]
        public async Task<IActionResult> UpdateUserRights(UserRightDto userRightDto)
        {
            return HandleResult(await Mediator.Send(new Edit.Command {rightsAllotmentM=userRightDto.RightsAllotmentMaster,rightsAllotmentD=userRightDto.RightsAllotmentDetail }));
        }
        [HttpGet("GetUserRightDetail/{UserID}")]
        public async Task<IActionResult> GetUserRightDetail(String UserID)
        {
            return HandleResult(await Mediator.Send(new Detail.Query { UserID = UserID }));
        }

    }
}