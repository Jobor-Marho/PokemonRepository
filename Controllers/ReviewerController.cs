using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Reviewer;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;
using PokenmonReviewApp.Dto;

namespace PokenmonReviewApp.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase{
        IReviewerRepository _reviewerRepository;
        IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper){
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Reviewer>))]
        public IActionResult GetReviewers(){
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId){
            if (reviewerId <= 0){
                return BadRequest("Invalid Reviewer ID.");
            }
            if(!_reviewerRepository.ReviewerExists(reviewerId)){
                return NotFound();
            }
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewerReviews(int reviewerId){
            if (reviewerId <= 0){
                return BadRequest("Invalid Reviewer ID.");
            }
            if(!_reviewerRepository.ReviewerExists(reviewerId)){
                return NotFound();
            }
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewerReviews(reviewerId));
            if (!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate){
            if(reviewerCreate == null){
                var msg = new Msg()
                {
                    Message = "Invalid Reviewer data supplied.",
                    Status = false,
                };
                return StatusCode(StatusCodes.Status400BadRequest, msg);
            }

            var existingReviewer = _reviewerRepository.GetReviewers()
        .FirstOrDefault(r => $"{r.FirstName} {r.LastName}".Trim().Equals($"{reviewerCreate.FirstName} {reviewerCreate.LastName}".Trim(), StringComparison.OrdinalIgnoreCase));

            if (existingReviewer != null)
            {

                var msg = new Msg
                {
                    Status = false,
                    Message = "Reviewer already exists."
                };
                return StatusCode(StatusCodes.Status422UnprocessableEntity, msg);
            }
            var reviewer = _mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.CreateReviewer(reviewer))
            {
                var msg = "Something went wrong while saving the review.";
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Reviewer successfully created." };
            return Ok(mssg);
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewer){
            if(reviewer == null){
                return BadRequest(ModelState);
            }
            if(reviewerId != reviewer.Id){
                var msg = new Msg { Status = false, Message = "Invalid reviewerId supplied." };
                return StatusCode(StatusCodes.Status400BadRequest, msg);
            }
            if(!_reviewerRepository.ReviewerExists(reviewerId)){
                var msg = new Msg { Status = false, Message = "Reviewer does not exist." };
                return StatusCode(StatusCodes.Status404NotFound, msg);
            }
            var reviewerMap = _mapper.Map<Reviewer>(reviewer);
            if(!_reviewerRepository.UpdateReviewer(reviewerMap)){
                var msg = new Msg { Status = false, Message = "Something went wrong while updating." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Reviewer successfully updated." };
            return Ok(mssg);
        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult DeleteReviewer(int reviewerId){
            if(reviewerId <= 0){
                return BadRequest("Invalid Reviewer ID.");
            }
            if(!_reviewerRepository.ReviewerExists(reviewerId)){
                return NotFound();
            }
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            if(reviewer == null){
                return NotFound();
            }
            if(!_reviewerRepository.DeleteReviewer(reviewer)){
                var msg = new Msg { Status = false, Message = "Something went wrong while deleting." };
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
            var mssg = new Msg { Status = true, Message = "Reviewer successfully deleted." };
            return Ok(mssg);
        }
    }
}