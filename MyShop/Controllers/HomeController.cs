using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using MyShop.Models;
using MyShop.Repositories.Interfaces;
using MyShop.Services;
using System.Diagnostics;

namespace MyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRegistration userRegistrationAsync;
        private readonly JwtSettings jwtSettings;
        private readonly ILogger logger;
        private readonly IProductRegistration _product;

        public HomeController(ILoggerFactory loggerFactory,IUserRegistration userRegistration, JwtSettings jwt,IConfiguration config,IProductRegistration product)
        {
            logger = loggerFactory.CreateLogger<HomeController>();
            userRegistrationAsync = userRegistration;
            jwtSettings = jwt;  
            _product= product;
        }

        public async Task<IActionResult> Index()
        {
            var ret = await _product.GetAllProducts();
            return View(ret);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Register()
        {
            return new ViewResult();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Guid guid = Guid.NewGuid();
                    //user.Id = guid;
                    user.UserTypeId = 0;
                    //user.CreatedDate = DateTime.Now;
                    //user.CreatedBy = guid;
                    //user.ModifiedDate = DateTime.Now;
                    //user.ModifiedBy = guid;
                    //user.IsDeleted = false;
                    var x = await userRegistrationAsync.InsertUser(user);

                    if(x!=null)
                        return RedirectToAction("Login");
                }   
                catch (Exception ex)
                {
                    return new ViewResult();
                }
            }
            return new ViewResult();
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("[action]")]
        public IActionResult Login()
        {
            return new ViewResult();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> ValidateUser(UserLoginModel userLogin)
        {
            
            var token = new UserTokens();
           
            BaseResponseModel baseResponse = new BaseResponseModel();
            var user = await userRegistrationAsync.ValidateUser(userLogin);
            
            if (user != null)
            {
               
                token = JwtHelpers.GenTokenkey(new UserTokens()
                {
                    EmailId = user.EmailId,                   
                    UserName = user.UserName,
                    Id = user.Id,
                }, jwtSettings);

                HttpContext.Session.SetString("username", user.UserName);
                HttpContext.Session.SetString("userid", Convert.ToString(user.Id));
                HttpContext.Session.SetString("bearer",token.Token);                
            
                var rtnmsg = string.Format("Validation Successful");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return RedirectToAction("AllProducts", "Product");
            }
            else if (user == null)
            {

                var rtnmsg = string.Format("Login Failed. Please Enter Valid Username or Password.");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return Ok(baseResponse);
            }
            else
            {

                var rtnmsg = string.Format("UserLoginController : UserValidation Action");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                baseResponse.StatusMessage = "Bad Request";
                return Ok(baseResponse);
            }
            
        }


    }
}