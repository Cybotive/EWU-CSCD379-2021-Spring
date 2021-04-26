using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // /api/users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return DataDeleteMe.Users;
        }

        // /api/users/<index>
        [HttpGet("{index}")]
        public string Get(int index)
        {
            return DataDeleteMe.Users[index];
        }

        //DELETE /api/users/<index>
        [HttpDelete("{index}")]
        public void Delete(int index)
        {
            DataDeleteMe.Users.RemoveAt(index);
        }

        // POST /api/users
        [HttpPost]
        public void Post([FromBody] string userName)
        {
            DataDeleteMe.Users.Add(userName);
        }

        [HttpPut("{index}")]
        public void Put(int index, [FromBody]string userName)
        {
            DataDeleteMe.Users[index] = userName; 
        }
    }
}