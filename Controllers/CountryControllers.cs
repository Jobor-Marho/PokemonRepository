using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Country;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase{
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _countryRepository = repository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Country>))]
        public IActionResult GetCountries(){
            var countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryById(int id){
            if(!_countryRepository.CountryExists(id)){
                return NotFound();
            }
            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryById(id));
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(country);
        }

        [HttpGet("{cid}/owners")]
        [ProducesResponseType(200, Type = typeof(List<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersFromACountry(int cid){
            if(!_countryRepository.CountryExists(cid)){
                return NotFound();
            }
            var owners = _mapper.Map<List<OwnerDTO>>(_countryRepository.GetOwnersFromACountry(cid));
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(owners);
        }

        // Get country of owner
        [HttpGet("owner/{ownerid}")]
        [ProducesResponseType(200, Type=typeof(Country))]
        [ProducesResponseType(400)]

        public IActionResult GetCountryOfOwner(int ownerid){
            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryOfOwner(ownerid));
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDTO country)
        {
            if (country == null)
                return BadRequest(ModelState);
            var existingCountry = _countryRepository.GetCountries()
        .FirstOrDefault(c => c.Name.Trim().Equals(country.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingCountry != null)
            {

                var msg = new Msg
                {
                    Status = false,
                    Message = "Country already exists."
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Country>(country);
            if (!_countryRepository.CreateCountry(categoryMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Country successfully created." };
            return Ok(mssg);
        }

        [HttpPut("{countryid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryid, [FromBody] CountryDTO country){
            if(country == null){
                return BadRequest(ModelState);
            }
            if(!_countryRepository.CountryExists(countryid)){
                return NotFound();
            }
            var countryMap = _mapper.Map<Country>(country);
            if(!_countryRepository.UpdateCountry(countryMap)){
                var msg = new Msg { Status = false, Message = "Something went wrong while updating." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Country successfully updated." };
            return Ok(mssg);
        }

        [HttpDelete("{countryid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteCountry(int countryid){
            if(!_countryRepository.CountryExists(countryid)){
                return NotFound();
            }
            var country = _countryRepository.GetCountryById(countryid);
            if(!_countryRepository.DeleteCountry(country)){
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Country successfully deleted." };
            return Ok(mssg);
        }

    }
}
