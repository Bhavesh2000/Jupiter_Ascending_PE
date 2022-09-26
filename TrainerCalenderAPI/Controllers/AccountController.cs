using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrainerCalenderAPI.DbContexts;
using TrainerCalenderAPI.Models;
using TrainerCalenderAPI.Models.Dto;
using TrainerCalenderAPI.Repository.IRepository;

namespace TrainerCalenderAPI.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        ResponseDto _response;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IJWTManagerRepository _jwtManagerRepository;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IJWTManagerRepository jwtManagerRepository)
        {
            _db = db;
            this._response = new ResponseDto();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtManagerRepository = jwtManagerRepository;
        }

        [HttpGet]
        [Route("Register")]
        public async Task<object> Register()
        {
            try
            {
                if (!_roleManager.RoleExistsAsync(Helper.Helper.Admin).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Admin));
                    await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Trainer));
                }
                _response.Result = Ok();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        [HttpPost]
        [Route("Register")]
        //[Authorize]
        [Authorize(Roles = "Admin")]
        public async Task<object> Register(RegisterViewDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Name = model.Name,

                    };
                    var IsUserNamePresent = await _db.Users
                        .AnyAsync(u => u.UserName == user.UserName);
                    var IsEmailPresent = await _db.Users.AnyAsync(u => u.Email == user.Email);
                    if (IsUserNamePresent)
                    {
                        _response.Result = BadRequest();
                        _response.DisplayMessage = "UserName Already Present";
                    }
                    else if (IsEmailPresent)
                    {
                        _response.Result = BadRequest();
                        _response.DisplayMessage = "Email Already Present";
                    }
                    else
                    {
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            if (model.RoleName == "TRAINER")
                            {
                                var t = new Trainer
                                {
                                    Id = user.Id
                                };
                                await _db.Trainers.AddAsync(t);

                            }
                            await _userManager.AddToRoleAsync(user, model.RoleName);
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _response.Result = Ok();
                        }
                    }
                    
                }
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            
            return _response;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> Login(LoginViewDto model)
        {
            //try
            //{
                if (ModelState.IsValid)
                {
                    var signedUser = await _userManager.FindByEmailAsync(model.Email);
                    if (signedUser == null)
                    {
                        return Unauthorized(model);
                    }
                    var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Ok(result);
                        //_response.Result = Ok(result);
                    }
                    else
                    {
                        //_response.Result = Unauthorized(model);
                        ModelState.AddModelError("", "Invalid Login Attempt");
                    }
                    //var token = await _jwtManagerRepository.AuthenticateAsync(model);
                    //if(token == null)
                    //{
                    //    return Unauthorized();
                    //}

                    //return Ok(token);
                }
            //}
            //catch (Exception ex)
            //{
            //    _response.IsSuccess = false;
            //    _response.ErrorMessages = new List<string>() { ex.ToString() };
            //}
            
            return Unauthorized(model);



        }

        [HttpPost]
        [Route("Logout")]
        //[Authorize]
        public async Task<IActionResult> Logout()
        {
            //try
            //{
                var userName = User.FindFirstValue(ClaimTypes.Name);
            await _signInManager.SignOutAsync();
                return Ok("Sign Out Successfull");
            //    _response.Result = Ok("Sign Out Successfull");
            //}
            //catch (Exception ex)
            //{
            //    _response.IsSuccess = false;
            //    _response.ErrorMessages = new List<string>() { ex.ToString() };
            //}
            //return _response;
            
        }






    }
}
