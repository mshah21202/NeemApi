using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public AccountController(DataContext dataContext, IMapper mapper, ITokenService tokenService)
        {
            _context = dataContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            await _context.Users.AddAsync(user);
            var userDto = new UserDto
            {
                Username = registerDto.Username,
                Token = await _tokenService.CreateToken(user)
            };
            if (await _context.SaveChangesAsync() > 0) return Ok(userDto);

            return BadRequest("Failed to register");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return NotFound();

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.PasswordHash[i]) return Unauthorized();
            }

            var userDto = new UserDto
            {
                Username = loginDto.Username,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(userDto);
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

            using var hmac = new HMACSHA512();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(changePasswordDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }
    }
}
