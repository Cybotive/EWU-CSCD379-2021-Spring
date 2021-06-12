using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftsController : ControllerBase
    {
        private IGiftRepository Repository { get; }

        public GiftsController(IGiftRepository repository)
        {
            Repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IEnumerable<Dto.Gift> Get()
        {
            return Repository.List().Select(x => Dto.Gift.ToDto(x)!);
        }

        [HttpGet("{id}")]
        public ActionResult<Dto.Gift?> Get(int id)
        {
            Dto.Gift? gift = Dto.Gift.ToDto(Repository.GetItem(id));
            if (gift is null) return NotFound();
            return gift;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult Delete(int id)
        {
            if (Repository.Remove(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Dto.Gift), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public ActionResult<Dto.Gift?> Post([FromBody] Dto.Gift gift)
        {
            bool giftExists = Repository.GetItem(gift.Id) is not null;

            if (giftExists)
                return Conflict("Gift already exists.");

            return Dto.Gift.ToDto(Repository.Create(Dto.Gift.FromDto(gift)!));
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult Put(int id, [FromBody] Dto.UpdateGift? gift)
        {
            Data.Gift? foundGift = Repository.GetItem(id);
            if (foundGift is not null)
            {
                foundGift.Title = gift?.Title ?? "";
                foundGift.Description = gift?.Description ?? "";
                foundGift.Url = gift?.Url ?? "";
                foundGift.Priority = gift?.Priority ?? 0;

                Repository.Save(foundGift);
                return Ok();
            }
            return NotFound();
        }
    }
}
