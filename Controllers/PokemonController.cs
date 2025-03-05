using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.pokemeon;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Services;


namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOwnerRepository _ownerRepository;

        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, ICategoryRepository categoryRepository, IOwnerRepository ownerRepository, IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _reviewRepository = reviewRepository;
        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }

        // Get Pokemon by ID
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (pokeId <= 0)
            {
                return BadRequest("Invalid Pokemon ID.");
            }

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        // Get Pokemon Rating by Pokemon ID
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (pokeId <= 0)
            {
                return BadRequest("Invalid Pokemon ID.");
            }

            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(rating);
        }

        // Get Pokemon by Name
        [HttpGet("{name}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(string name)
        {
            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(name));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDTO pokemondata)

        {

            if (pokemondata == null)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepository.OwnerExists(ownerId) || !_categoryRepository.CategoryExists(categoryId))
            {
                var msg = new Msg
                {
                    Status = false,
                    Message = "Owner or Category does not exist."
                };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }
            var pokemon = _pokemonRepository.GetPokemons().FirstOrDefault(p => p.Name.Trim().Equals(pokemondata.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (pokemon != null)
            {
                var msg = new Msg
                {
                    Status = false,
                    Message = "Pokemon already exists."
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pokemonMap = _mapper.Map<Pokemon>(pokemondata);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Pokemon successfully created." };
            return Ok(mssg);
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId, [FromQuery]int ownerId,[FromQuery]int catId ,[FromBody] PokemonDTO pokemon)
        {
            if (pokemon == null)
            {
                return BadRequest(ModelState);
            }
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonMap = _mapper.Map<Pokemon>(pokemon);
            if (!_pokemonRepository.UpdatePokemon(ownerId, catId, pokemonMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Pokemon successfully updated." };
            return Ok(mssg);
        }
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var reviewsToDelete = _reviewRepository.GetReviewsForPokemon(pokeId);
            var pokemon = _pokemonRepository.GetPokemon(pokeId);
            if(pokemon == null){
                return NotFound();
            }
            if(_reviewRepository.DeleteReviews(reviewsToDelete.ToList())){
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            if (!_pokemonRepository.DeletePokemon(pokemon))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Pokemon successfully deleted." };
            return StatusCode(StatusCodes.Status204NoContent, mssg);
        }

    }

}
