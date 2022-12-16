using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using MyShop.Repositories.Interfaces;
using System.Drawing.Printing;

namespace MyShop.ViewComponents
{
    
    public class ProductViewComponent:ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync(ProductModel prod)
        {
            return View(prod);
        }
    }
}
