using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.Dtos.IdentityDto;
using Talabat.Api.Dtos.OrderDtos;
using Talabat.Api.Errors;
using Talabat.Api.Extenstions;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Data.Repositories;

namespace Talabat.Api.Controllers
{

    public class AccountsController :BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IAuthService authService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _userManager.FindByEmailAsync(loginDto.Email);
            if(user == null) {
                return Unauthorized(new ApiResponse(401));
            }
            var checkpassword=await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(checkpassword.Succeeded is false) {
                return Unauthorized(new ApiResponse(401));
            }
            return Ok(new UserDto()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token =await authService.CreateTokenAsync(user, _userManager)
            }); ;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if(CheckEmailExists(register.Email).Result.Value) {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email Alrady Exist" } });
                //return BadRequest(new ApiResponse(400) {Message="this email already exist" });

            }
            var user = new AppUser()
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                UserName = register.Email.Split('@')[0],
                PhoneNumber = register.PhoneNumber,
            };
            var res = await _userManager.CreateAsync(user, register.Password);
            if(res.Succeeded is false) { return BadRequest(new ApiResponse(404)); };
            return Ok(new UserDto()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token= await authService.CreateTokenAsync(user, _userManager)
            });

        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName=user.DisplayName,
                Email=user.Email,
                Token= await authService.CreateTokenAsync(user, _userManager)
            });

        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByEmailAddressAsync(User);
            var usermap=mapper.Map<AddressDto>(user);
            return Ok(usermap);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var address = mapper.Map<AddressDto,Address>(addressDto);
            var user = await _userManager.FindUserByEmailAddressAsync(User);
            address.Id = user?.Address?.Id??0;
            user.Address =address ;
            var res= await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(addressDto);
        }

        [HttpGet("Emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
            
        }

    }
}
