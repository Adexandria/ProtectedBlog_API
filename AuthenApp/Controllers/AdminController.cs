using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenApp.UserModel;
using AuthenApp.ViewDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenApp.Controllers
{

    [ApiController]
    [Route("api/blog/admin")]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        readonly UserManager<SignUp> user;
        private readonly SignInManager<SignUp> login;
        private readonly RoleManager<IdentityRole> role;

        readonly IMapper mapper;
        public AdminController(UserManager<SignUp> user, IMapper mapper, SignInManager<SignUp> login, RoleManager<IdentityRole> role)
        {
            this.user = user;
            this.mapper = mapper;
            this.login = login;
            this.role = role;
        }
        
        [HttpPost()]
        
        public async Task<ActionResult> SignUpAdmin(SignUpDTO newuser)
        {

            var signup = mapper.Map<SignUp>(newuser);
            var EmailisValid = await user.FindByEmailAsync(signup.Email);
            if (newuser.Password.Equals(newuser.RetypePassword) && EmailisValid == null)
            {
                IdentityResult identity = await user.CreateAsync(signup, signup.Password);

                if (identity.Succeeded)
                {
                    await user.AddClaimAsync(signup, new Claim(ClaimTypes.Role, "Admin"));
                    await user.AddToRoleAsync(signup, "Admin");
                    var result = await login.PasswordSignInAsync(signup.UserName, signup.Password, false, true);
                    if (result.Succeeded)
                    {
                        return this.StatusCode(StatusCodes.Status201Created, $"Welcome Admin,{signup.UserName} Your account has been created");
                    }

                }
                else
                {
                    return this.StatusCode(StatusCodes.Status400BadRequest, $"Invalid password,follow the password requirements {identity.Errors}");
                }


            }
            return BadRequest("This email is currently used ");


        }
    }
}

    

