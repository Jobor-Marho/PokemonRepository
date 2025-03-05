using System.ComponentModel.DataAnnotations;

namespace PokenmonReviewApp.Dto
{
    public class ReviewerDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Reviewer first name is required")]
        public string FirstName { get; set; } = "";
        
        [Required(ErrorMessage = "Reviewer last name is required")]
        public string LastName { get; set; } = "";
    }
}