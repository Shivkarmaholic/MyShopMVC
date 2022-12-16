using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Repositories.Interfaces;
using MyShop.Services;

namespace MyShop.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;       
        private readonly JwtSettings jwtSettings;
        private readonly ILogger logger;
        private readonly IProductRegistration _product;

        public ProductController(ILoggerFactory loggerFactory, JwtSettings jwt, IConfiguration config,IProductRegistration product)
        {
            logger = loggerFactory.CreateLogger<HomeController>();            
            jwtSettings = jwt;
            _product= product;
        }

        [HttpGet]
        [Route("AllProducts")]
        public async Task<IActionResult> AllProducts()
        {
             BaseResponseModel baseResponse = new BaseResponseModel();
            var products= await _product.GetAllProducts();
            if (products != null)           
            {
                var rtnmsg = string.Format("Product Controller: Products (Received Products SuccessFully) ");
                logger.LogDebug(rtnmsg);
                baseResponse.Response1 = products;
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return View("AllProducts", baseResponse);
            }
            else if (products == null)
            {

                var rtnmsg = string.Format("Product Controller: Products (Product List is Empty)");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return Ok(baseResponse);
            }
            else
            {

                var rtnmsg = string.Format("Product Controller: Products (Bad Request)");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                baseResponse.StatusMessage = "Bad Request";
                return Ok(baseResponse);
            }
                       
        }

        [HttpGet]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct()
        {

            return View();

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddProduct(ProductModel model)
        {
            BaseResponseModel baseResponse = new BaseResponseModel();
            var products = await _product.InsertProduct(model);
            if (products == 1)
            {
                var rtnmsg = string.Format("Product Controller: AddProduct (Product Added SuccessFully) ");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return RedirectToAction("AllProducts", "Product", baseResponse);
            }
            else if (products == -1)
            {

                var rtnmsg = string.Format("Product Controller: AddProduct (Product Already Exists)");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return Ok(baseResponse);
            }
            else
            {

                var rtnmsg = string.Format("Product Controller: AddProduct (Bad Request)");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return Ok(baseResponse);
            }
           

        }
    }
}
