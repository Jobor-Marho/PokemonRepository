using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Country;
using Models.Owner;
using Models.pokemeon;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(owners);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerById(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
            {
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwnerById(id));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(owner);
        }

        [HttpGet("{gym}/owner")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerByGym(string gym)
        {
            if (string.IsNullOrEmpty(gym))
            {
                return BadRequest();
            }
            var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwnerByGym(gym));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(owner);
        }

        [HttpGet("country/{cid}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerCountry(int cid)
        {
            if (cid <= 0)
            {
                return BadRequest();
            }

            var country = _mapper.Map<CountryDTO>(_ownerRepository.GetOwnerCountry(cid));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(country);
        }
        [HttpGet("{owId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsForOwner(int owId)
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonsForOwner(owId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemons);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDTO owner)
        {
            if (!_countryRepository.CountryExists(countryId))
            {
                var msg = new Msg
                {
                    Status = false,
                    Message = "Country does not exist."
                };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }

            if (owner == null)
                return BadRequest(ModelState);
            var existingOwner = _ownerRepository.GetOwners()
        .FirstOrDefault(o => $"{o.FirstName} {o.LastName}".Trim().Equals($"{owner.FirstName} {owner.LastName}".Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingOwner != null)
            {

                var msg = new Msg
                {
                    Status = false,
                    Message = "Owner already exists."
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerMap = _mapper.Map<Owner>(owner);

            //Add the country to owner
            ownerMap.Country = _countryRepository.GetCountryById(countryId);
            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Owner successfully created." };
            return Ok(mssg);
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);
            if (ownerId != updatedOwner.Id)
            {
                var msg = new Msg { Status = false, Message = "Invalid ownerId supplied" };
                return StatusCode(StatusCodes.Status400BadRequest, msg);
            }
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                var msg = new Msg { Status = false, Message = "OwnerId does not exist" };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }
            var ownerMap = _mapper.Map<Owner>(updatedOwner);
            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while updating." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Owner successfully updated." };
            return Ok(mssg);
        }
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteOwner(int ownerId){

            if(ownerId == null){
                var msg = new Msg{
                    Message = "Owner Id is required",
                    Status = false
                };
                return StatusCode(StatusCodes.Status400BadRequest, msg);
            }
            var owner_to_be_deleted = _ownerRepository.GetOwnerById(ownerId);
            if(owner_to_be_deleted == null){
                var msg = new Msg{
                    Message = "Owner does not exist",
                    Status = false
                };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }

            if(!_ownerRepository.DeleteOwner(owner_to_be_deleted)){
                var msg = new Msg{
                    Message = "Something went wrong while deleting the owner object.",
                    Status = false
                };
                return StatusCode(StatusCodes.Status404NotFound,msg);
            }
            var mssg = new Msg{
                    Message = "Owner was deleted successfully",
                    Status = true
                };
                return StatusCode(StatusCodes.Status204NoContent, mssg);
        }
    }
}