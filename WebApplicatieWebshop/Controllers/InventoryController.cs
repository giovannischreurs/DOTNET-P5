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
    public class InventoryController : ControllerBase
    {
        private readonly ConfigContext _inventory;

        public InventoryController(ConfigContext context)
        {
            _inventory = context;
            /*if (_inventory.inventory.Count() == 0)
            {
                _inventory.inventory.Add(new Inventory { username = "nadir", name = "Peren", amount = 1 });
                _inventory.inventory.Add(new Inventory { username = "nadir", name = "Appels", amount = 2 });
                _inventory.inventory.Add(new Inventory { username = "nadir", name = "Citroenen", amount = 4 });
                _inventory.inventory.Add(new Inventory { username = "suyaheb", name = "Sinaasappels", amount = 1 });
                _inventory.inventory.Add(new Inventory { username = "suyaheb", name = "Druiven", amount = 5 });
                _inventory.SaveChanges();
            }*/
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoryByName(String username)
        {
            try
            {
                return await _inventory.inventory.Where(i => i.username == username).ToListAsync();
            }
            catch { return NotFound(); }
        }

        [HttpPost("increase")]
        public async Task<ActionResult<bool>> IncreaseAmountInventoryItem(JObject data)
        {
            try
            {
                var username = data["username"].ToString();
                var productName = data["productName"].ToString();
                var result = await _inventory.inventory.Where(i => (i.username == username) && (i.name == productName)).FirstAsync();
                result.amount++;
                _inventory.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpPost("{add}")]
        public bool AddInventoryItem([FromBody] JObject data)
        {
            try
            {
                var username = data["username"].ToString();
                var productName = data["productName"].ToString();
                var product = _inventory.inventory.Where(i => i.name == productName && i.username == username).FirstOrDefault();
                Console.WriteLine("giovanni: " + product);
                if (product != null)//product exists +1
                {
                    product.amount++;
                }
                else //does not exist, add new one
                {
                    _inventory.inventory.Add(new Inventory { username = username, name = productName, amount = 1 });
                }
                
                _inventory.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            try
            {
                return await _inventory.inventory.ToListAsync();
            }
            catch { return NotFound(); }
        }
    }
}
