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
            var user = await _userManager.Users.Where(x => x.Email == loginDto.Email && x.IsActive == true && x.IsCancel == false).FirstOrDefaultAsync(); ;
//            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return Unauthorized();
        }
        [HttpDelete("DisableAccount/{id}")]
        public async Task<ActionResult<bool>> DisableData(Guid id)
        {
            var user = await _userManager.Users.Where(x => x.Id == id.ToString()).FirstOrDefaultAsync();
            if (user == null) return Unauthorized();
            user.IsActive = false;
            user.IsCancel = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return true;
            else
                return false;
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
            var user = new AppUser {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.ContactNo,
                EntryDate = DateTime.Now,
                IsActive = true,
                IsCancel = false,
                UserID = registerDto.UserID,
                SystemIP = registerDto.UserIP,
                SystemName = registerDto.UserSystem
            };
            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if(result.Succeeded)
            {
                _context.LogFiles.Add(new LogFile()
                {
                    Description = "Adding New User '"+user.Id+"' in Database",
                    TransactionID = user.Id,
                    EntryDate = DateTime.Now,
                    sqlCommand = _userManager.ToString(),
                    UserID=registerDto.UserID,
                    UserIP = registerDto.UserIP,
                    UserSystem=registerDto.UserSystem
                });

                await _context.SaveChangesAsync();

                return CreateUserObject(user);
            }
            return BadRequest("Problem Registering User");
        }
        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<UserDto>> UpdatePassword(Guid guid,RegisterDto registerDto)
        {
            var userInfo = await _userManager.Users.Where(z => z.Id == guid.ToString()).FirstOrDefaultAsync();
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userInfo);
            var result = await _userManager.ResetPasswordAsync(userInfo, resetToken, registerDto.Password);
            if (result.Succeeded)
            {
                _context.LogFiles.Add(new LogFile()
                {
                    Description = "Password of user "+guid.ToString()+" is updated",
                    TransactionID = guid.ToString(),
                    EntryDate = DateTime.Now,
                    sqlCommand = _userManager.ToString(),
                    UserID = registerDto.UserID,
                    UserIP = registerDto.UserIP,
                    UserSystem = registerDto.UserSystem
                });

                await _context.SaveChangesAsync();
                return CreateUserObject(userInfo);

            }
            else
            {
                string Errors="";
                foreach (var error in result.Errors){
                    Errors += error.Description;
                }
                ModelState.AddModelError("password",Errors);
                return ValidationProblem();
            }

        }
        [HttpPut("UpdateUser/{guid}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid guid,RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(x =>x.Email == registerDto.Email && x.Id != guid.ToString()))
            {
                ModelState.AddModelError("email", "Email already Exist");
                return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName && x.Id != guid.ToString()))
            {
                ModelState.AddModelError("username", "UserName already Exist");
                return ValidationProblem();
            }
            var userInfo = await _userManager.Users.Where(z => z.Id == guid.ToString()).FirstOrDefaultAsync();
            userInfo.DisplayName = registerDto.DisplayName;
            userInfo.Email = registerDto.Email;
            userInfo.UserName = registerDto.UserName;
            userInfo.PhoneNumber = registerDto.ContactNo;
            userInfo.ImagePath = registerDto.ImagePath;
            userInfo.UserViewID = registerDto.UserViewID;

            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                _context.LogFiles.Add(new LogFile()
                {
                    Description = "User " + guid.ToString() + " is updated",
                    TransactionID = guid.ToString(),
                    EntryDate = DateTime.Now,
                    sqlCommand = _userManager.ToString(),
                    UserID = registerDto.UserID,
                    UserIP = registerDto.UserIP,
                    UserSystem = registerDto.UserSystem
                });

                await _context.SaveChangesAsync();
                return CreateUserObject(userInfo);
            }
            return BadRequest(result.Errors.ToList());
        }
        //[Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        } 

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDto>>> getAllUsers(){
            var users = await _userManager.Users.Where(x=>x.IsActive == true && x.IsCancel == false).ToListAsync();
            //Excluding the admin user
            var userList = users.Where(x => x.UserName != "Admin").ToList();
            List<UserDto> objectToReturn = new List<UserDto>();
            foreach (var item in userList)
            {
                objectToReturn.Add(new UserDto(){ContactNo=ConvertNullToString(item.PhoneNumber),
                DisplayName=ConvertNullToString(item.DisplayName),
                Email=ConvertNullToString(item.Email),
                UserName=ConvertNullToString(item.UserName),
                id=Guid.Parse(item.Id),
                Image=ConvertNullToString(item.ImagePath),
                UserViewID=Convert.ToInt16(item.UserViewID)});
            }
            return objectToReturn;
        }
        [HttpGet("GetUserData/{id}")]
        public async Task<ActionResult<UserDto>> GetUserData(Guid id){
            var user = await _userManager.Users.Where(x => x.Id == id.ToString()).FirstOrDefaultAsync();            
            UserDto userDto = new UserDto(){ContactNo=ConvertNullToString(user.PhoneNumber),
                DisplayName=ConvertNullToString(user.DisplayName),
                Email=ConvertNullToString(user.Email),
                UserName=ConvertNullToString(user.UserName),
                id=Guid.Parse(user.Id),
                Image = ConvertNullToString(user.ImagePath),
                UserViewID = Convert.ToInt16(user.UserViewID)};
            return userDto;
        }
        private string ConvertNullToString(object value){
            return value == null ? "" : value.ToString();
        }
        private UserDto CreateUserObject(AppUser user){
            return new UserDto{
                DisplayName=ConvertNullToString(user.DisplayName),
                Image=ConvertNullToString(user.ImagePath),
                Token = ConvertNullToString(_token.CreateToken(user)),
                UserName=ConvertNullToString(user.UserName),
                id=Guid.Parse(user.Id),
                UserViewID=Convert.ToInt16(user.UserViewID)
            };
        }
    }
}