using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Data;
using SecretSanta.Business;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository UserRepository { get; }

        public UsersController(IUserRepository userRepository)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // /api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return UserRepository.List();
        }

        // /api/users/<index>
        [HttpGet("{index}")]
        public User Get(int index)
        {
            if(index < 0){
                throw new ArgumentOutOfRangeException(nameof(index));
            }
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
        public void Post([FromBody] User user)
        {
            DataDeleteMe.Users.Add(user);
        }

        [HttpPut("{index}")]
        public void Put(int index, [FromBody]User user)
        {
            DataDeleteMe.Users[index] = user; 
        }
    }
}