using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Category;
using Models.pokemeon;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;

        // Initialize with our category repository and mapper
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            return Ok(categories);
        }

        // Get Category By ID
        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int Id)
        {
            if (!_categoryRepository.CategoryExists(Id))
                return NotFound("Does not Exist");
            var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(Id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        // Get Pokemons By Category Id
        [HttpGet("pokemon/{CatId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int CatId)
        {
            if (!_categoryRepository.CategoryExists(CatId))
                return NotFound();
            var pokemons = _mapper.Map<IEnumerable<PokemonDTO>>(_categoryRepository.GetPokemonByCategory(CatId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(pokemons);

        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);
            var category = _categoryRepository.GetCategories()
        .FirstOrDefault(c => c.Name.Trim().Equals(categoryCreate.Name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (category != null)
            {

                var msg = new Msg
                {
                    Status = false,
                    Message = "Category already exists."
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Category>(categoryCreate);
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Category successfully created." };
            return Ok(mssg);
        }
        [HttpPut("{catId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int catId, [FromBody] CategoryDTO updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);
            if (catId != updatedCategory.Id)
            {
                var msg = new Msg { Status = false, Message = "Invalid catId supplied" };
                return StatusCode(StatusCodes.Status400BadRequest, msg);
            }
            if (!_categoryRepository.CategoryExists(catId))
            {
                var msg = new Msg { Status = false, Message = "CatId does not exist" };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }


            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while saving." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }

            var mssg = new Msg { Status = true, Message = "Category successfully updated." };
            return Ok(mssg);

        }

        [HttpDelete("{catId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int catId)
        {
            if (!_categoryRepository.CategoryExists(catId))
            {
                var msg = new Msg { Status = false, Message = "CatId does not exist" };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }
            var category = _categoryRepository.GetCategory(catId);
            if (!_categoryRepository.DeleteCategory(category))
            {
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Category successfully deleted." };
            return Ok(mssg);
        }

    }
}