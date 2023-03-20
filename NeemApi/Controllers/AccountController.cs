using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemApi.Data;
using NeemApi.DTOs;
using NeemApi.Entities;
using NeemApi.Extensions;
using NeemApi.Interfaces;
using Org.BouncyCastle.Crypto.Macs;
using System.Security.Cryptography;
using System.Text;

namespace NeemApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DataContext dataContext, IMapper mapper, ITokenService tokenService)
        {
            _context = dataContext;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<MemberDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = new MemberDto
            {
                Username = registerDto.Username,
                Name = registerDto.Name,
                Email = registerDto.Email,
                Token = await _tokenService.CreateToken(user)
            };
            return Ok(userDto);

            return BadRequest("Failed to register");
        }

        [HttpPost("login")]
        public async Task<ActionResult<MemberDto>> Login(LoginDto loginDto)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return NotFound();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            var userDto = new MemberDto
            {
                Username = loginDto.Username,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<MemberDto>> GetUser()
        {
            var user = await _context.Users.FindAsync(User.GetUserId());

            return Ok(new MemberDto {
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Username = user.UserName
            });
        }

        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword([FromQuery]string email)
        {
            // Check if email belongs to user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return NotFound();

            var rand = new Random(DateTime.Now.Millisecond);

            var pin = rand.Next(1000, 10000); // 1000 -> 9999

            user.Pin = pin;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();


            var mailRequest = new MailRequest
            {
                ToEmail = email,
                Subject = "Reset your account password",
                Body = "Your security PIN is " + pin.ToString()
            };

            return Ok();
        }

        [HttpPost("reset-confirm")]
        public async Task<ActionResult<UserDto>> ConfirmPin([FromQuery]int pin, string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return NotFound();

            if (user.Pin != pin) return Unauthorized();

            var userDto = new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(User.GetUserId());

            await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            await _context.SaveChangesAsync();

            return Ok();
        }
        [Authorize]
        [HttpPost("update-user")]
        public async Task<ActionResult> UpdateUserInfo(UpdateUserDto updateUserDto)
        {
            var user = _context.Users.Find(User.GetUserId());
            
            if (updateUserDto.Username != user.UserName)
                if (await UserExists(updateUserDto.Username)) return BadRequest("Username already taken");
            user.UserName = updateUserDto.Username;
            if (updateUserDto.Email != user.Email)
                await _userManager.ChangeEmailAsync(user, updateUserDto.Email, await _userManager.GenerateChangeEmailTokenAsync(user, updateUserDto.Email));
            if (updateUserDto.Phone != user.Phone)
                await _userManager.ChangePhoneNumberAsync(user, updateUserDto.Phone, await _userManager.GenerateChangePhoneNumberTokenAsync(user, updateUserDto.Email));
            user.Name = updateUserDto.Name;
            if (updateUserDto.NewPassword != null)
                await ChangePassword(new ChangePasswordDto 
                { NewPassword = updateUserDto.NewPassword,
                  CurrentPassword = updateUserDto.CurrentPassword
                });



            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> PhoneExists(string phone)
        {
            return await _context.Users.AnyAsync(x => x.Phone == phone);
        }
    }
}

