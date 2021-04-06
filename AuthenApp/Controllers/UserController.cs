using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenApp.UserModel;
using AuthenApp.ViewDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenApp.Controllers
{
    [ApiController]
    [Route("api/blog/authenticate")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        readonly UserManager<SignUp> user;
        private readonly SignInManager<SignUp> login;

        
        readonly IMapper mapper;
        public UserController(UserManager<SignUp> user, IMapper mapper, SignInManager<SignUp> login)
        {
            this.user = user;
            this.mapper = mapper;
            this.login = login;
        }
        [HttpPost("signup")]
        public async Task<ActionResult> SignUp(SignUpDTO newuser) 
        {
                var signup = mapper.Map<SignUp>(newuser);
                var EmailisValid = await user.FindByEmailAsync(signup.Email);

                if (newuser.Password.Equals(newuser.RetypePassword) && EmailisValid == null)
                {
                    IdentityResult identity = await user.CreateAsync(signup, signup.Password);
                    
                    if (identity.Succeeded)
                    {
                    await user.AddClaimAsync(signup, new Claim(ClaimTypes.Role, "User"));
                    await user.AddToRoleAsync(signup, "User");
                    var result = await login.PasswordSignInAsync(signup.UserName, signup.Password, false, true);
                        if (result.Succeeded)
                        {
                            return this.StatusCode(StatusCodes.Status201Created, $"Welcome,{signup.UserName} Your account has been created");
                        }

                    }
                    else
                    {
                    return this.StatusCode(StatusCodes.Status400BadRequest, $"Invalid password or email, follow the password requirements {identity.Errors} ");
                    }
                }
                return BadRequest("This email is currently used by another user");
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model) 
        {
            var logindetails = mapper.Map<SignUp>(model);
            var result = await login.PasswordSignInAsync(logindetails.UserName, logindetails.Password, false, true);
            await login.CreateUserPrincipalAsync(logindetails);
            if (result.Succeeded) 
            {
                return this.StatusCode(StatusCodes.Status200OK, $"Welcome,{logindetails.UserName} Start Blogging");
            }
            return BadRequest("invalid username or password");
        }
        [HttpPost("{username}/signout")]
        public async Task<ActionResult> Signout(string username) 
        {
            var currentuser = await user.FindByNameAsync(username);
            if(currentuser == null) 
            {
                return NotFound();
            }
            await  login.SignOutAsync();
            return Ok();
        }
    }
}
