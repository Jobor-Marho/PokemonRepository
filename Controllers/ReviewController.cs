using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Review;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;

namespace PokemonReviewApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (reviewId <= 0)
            {
                return BadRequest("Invalid Review ID.");
            }
            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReviewById(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("reviewer/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewByReviewer(int reviewerId)
        {
            if (reviewerId <= 0)
            {
                return BadRequest("Invalid Reviewer ID.");
            }

            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReviewByReviwer(reviewerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForPokemon(int pokemonId)
        {
            if (pokemonId <= 0)
            {
                return BadRequest("Invalid Pokemon ID.");
            }

            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsForPokemon(pokemonId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDTO reviewCreate)
        {
            if (!_reviewRepository.ReviewExists(reviewerId) || !_pokemonRepository.PokemonExists(pokemonId))
            {
                var msg = new Msg()
                {
                    Message = "Invalid Reviewer or Pokemon ID.",
                    Status = false,
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }
            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }

            var review = _mapper.Map<Review>(reviewCreate);

            review.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            review.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(review))
            {
                var msg = "Something went wrong while saving the review.";
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Review successfully created." };
            return Ok(mssg);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId,[FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDTO review){
            if(review == null){
                return BadRequest(ModelState);
            }
            if(reviewId != review.Id){
                return BadRequest(ModelState);
            }
            if (!_reviewerRepository.ReviewerExists(reviewerId) || !_pokemonRepository.PokemonExists(pokemonId))
            {
                var msg = new Msg()
                {
                    Message = "Invalid Reviewer or Pokemon ID.",
                    Status = false,
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }
            var reviewMap = _mapper.Map<Review>(review);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                var msg = "Something went wrong while updating the review.";
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Review successfully updated." };
            return Ok(mssg);
        }
        
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReview(int reviewId){
            if(!_reviewRepository.ReviewExists(reviewId)){
                return NotFound();
            }
            var review = _reviewRepository.GetReviewById(reviewId);
            if(!_reviewRepository.DeleteReview(review)){
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Review successfully deleted." };
            return Ok(mssg);
        }

    }
}