using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Api.Dto;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository Repository { get; }

        public UsersController(IUserRepository repository)
        {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<FullUser> Get()
        {
            ICollection<User> usersToConvert = Repository.List();
            List<FullUser> allUsers = new();

            // Convert all from SecretSanta.Data.User to SecretSanta.Api.Dto.FullUser
            foreach(User user in usersToConvert)
            {
                FullUser convertedUser = UserToFullUser(user);
                allUsers.Add(convertedUser);
            }

            return allUsers;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FullUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<FullUser?> Get(int id)
        {
            User? user = Repository.GetItem(id);
            if (user is null) return NotFound();

            return UserToFullUser(user);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            if (Repository.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(FullUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<FullUser?> Post([FromBody] FullUser? user)
        {
            if (user is null)
            {
                return BadRequest();
            }
            // Convert Dto type to Data then back
            User dataUser = FullUserToUser(user);
            User returnedUser = Repository.Create(dataUser);
            return UserToFullUser(returnedUser);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Put(int id, [FromBody] UpdateUser? user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            User? foundUser = Repository.GetItem(id);
            if (foundUser is not null)
            {
                foundUser.FirstName = user.FirstName ?? "";
                foundUser.LastName = user.LastName ?? "";

                Repository.Save(foundUser);
                return Ok();
            }
            return NotFound();
        }

        private FullUser UserToFullUser(User user)
        {
            FullUser convertedUser = new() {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            return convertedUser;
        }

        private User FullUserToUser(FullUser user)
        {
            User convertedUser = new() {
                Id = user.Id ?? 0,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
            };

            return convertedUser;
        }
    }
}
