using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _token;

        public DataContext _context { get; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService token,DataContext context)
        {
            _token = token;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

        }
        [HttpGet("GetCurrentDate")]
        public async Task<ActionResult<DateTime>> GetCurrentDate(){
            DateTime returnDateTime = new DateTime();
            try
            {
                
                var GlobalResult = await _context.Database.ExecuteSqlRawAsync("Delete from Globals");
                int rValue = await _context.Database.ExecuteSqlRawAsync(@"Insert into Globals(CurrentDate) select GetDate()");
                if (rValue == 0) {
                    return DateTime.Now; }
                var rGlobalResult = await _context.Globals.FromSqlRaw<global>("Select top 1 CurrentDate from Globals").ToListAsync();
                if (rGlobalResult.Count > 0)
                    returnDateTime = rGlobalResult[0].CurrentDate;
                else
                    returnDateTime = DateTime.Now;
                return returnDateTime;

            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email)){
                ModelState.AddModelError("email","Email already Exist");
                return ValidationProblem();
            }
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName)){
                ModelState.AddModelError("username","UserName already Exist");
                return ValidationProblem();
            }
            var user = new AppUser{
                DisplayName=registerDto.DisplayName,
                Email=registerDto.Email,
                UserName=registerDto.UserName,
                PhoneNumber = registerDto.ContactNo
            };
            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if(result.Succeeded)
            {
                return CreateUserObject(user);
            }
            return BadRequest("Problem Registering User");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        } 

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDto>>> getAllUsers(){
            var users = await _userManager.Users.ToListAsync();
            //Excluding the admin user
            var userList = users.Where(x => x.Id != "4ef591bb-00e5-4864-99fd-2eb74ffb65f7").ToList();
            List<UserDto> objectToReturn = new List<UserDto>();
            foreach (var item in userList)
            {
                objectToReturn.Add(new UserDto(){ContactNo=item.PhoneNumber,
                DisplayName=item.DisplayName,
                Email=item.Email,
                UserName=item.UserName,
                id=Guid.Parse(item.Id)});
            }
            return objectToReturn;
        }

        private UserDto CreateUserObject(AppUser user){
            return new UserDto{
                DisplayName=user.DisplayName,
                Image=null,
                Token = _token.CreateToken(user),
                UserName=user.UserName,
                id=Guid.Parse(user.Id),
            };
        }
    }
}