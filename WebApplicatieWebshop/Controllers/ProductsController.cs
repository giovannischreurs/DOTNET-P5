using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApplicatieWebshop.Models;

namespace WebApplicatieWebshop.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ConfigContext _product;

        public ProductsController(ConfigContext context)
        {
            _product = context;

            /*if (_product.products.Count() == 0)//dummy data
            {
                _product.products.Add(new Product { name = "Banaan", price = 1.49, amount = 2 });
                _product.products.Add(new Product { name = "Abrikoos", price = 1.19, amount = 2 });
                _product.products.Add(new Product { name = "Papaya", price = 0.19, amount = 2 });
                _product.SaveChanges();
            }*/
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return await _product.products.ToListAsync();
            }
            catch { return NotFound(); }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Product>> GetProduct(String name)
        {
            try
            {
                return await _product.products.Where(p => p.name == name).FirstAsync();
            }
            catch { return NotFound(); }
        }

        [HttpPost("decrease")] //vooraad wijzigen
        public async Task<ActionResult<bool>> DecreaseAmountProductsItem([FromBody] JObject data)
        {
            try
            {
                var productName = data["productName"].ToString();
                var result = await _product.products.Where(i => i.name == productName).FirstOrDefaultAsync();
                result.amount--;
                await _product.SaveChangesAsync();
                return true;
            }
            catch { return NotFound(); }
        }
    }
}
