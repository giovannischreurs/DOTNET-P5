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
    public class UsersController : ControllerBase
    {
        private readonly ConfigContext _user;

        public UsersController(ConfigContext context)
        {
            _user = context;

            /*if (_user.users.Count() == 0)
            {
                _user.users.Add(new User { username = "nadir", password="nadir123", money = 20.5 });
                _user.users.Add(new User { username = "berkay", password = "nadir123", money = 5.5 });
                _user.users.Add(new User { username = "suhayeb", password = "nadir123", money = 9.5 });
                _user.SaveChanges();
            }*/
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return await _user.users.ToListAsync();
            }
            catch { return NotFound(); }
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(String username)
        {
            try
            {
                var user = await _user.users.Where(u => u.username == username).FirstAsync();

                Console.WriteLine("test: " + user);
                return user;
            }
            catch { return NotFound(); }
        }

        [HttpPost("register")]
        public async Task<ActionResult<bool>> PostRegisterUser([FromBody] User user)
        {
            var checkUser = await _user.users.Where(u => u.username == user.username).FirstOrDefaultAsync();
            if (checkUser != null)//user exists
            {
                return false;
            }
            else//no user
            {
                _user.users.Add(new User { username = user.username, password = user.password, money = 50 });
                _user.SaveChanges();
                return true;
            }
        }

        [HttpPost("{decrease}")]
        public bool DecreaseSaldo([FromBody] JObject data)
        {
            try
            {
                var username = data["username"].ToString();
                var cost = Convert.ToDouble(data["cost"]);
                _user.users.Where(u => u.username == username).First().money -= cost;
                _user.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public async Task<ActionResult<User>> PostUser(User user)
        {
            _user.users.Add(user);
            await _user.SaveChangesAsync();

            return CreatedAtAction(nameof(User), new { id = user.Id, username = user.username, password = user.password, money = user.money }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> PostGetUser([FromBody] User user)
        {
            var result = await _user.users.Where(u => u.username == user.username && u.password == user.password).FirstAsync();
            if (result == null)
            {
                return NoContent();
            }
            return true;
        }
    }
}
