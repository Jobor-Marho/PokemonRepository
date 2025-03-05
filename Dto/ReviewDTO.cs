using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto{
    public class ReviewDTO{
        public int Id {get; set;}
        [Required(ErrorMessage = "Review Title is required")]
        public string Title {get; set;} = "";
        [Required(ErrorMessage = "Review Text is required")]
        public string Text {get; set;} = "";
        public int Rating {get; set;}
    }
}