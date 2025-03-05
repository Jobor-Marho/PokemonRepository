using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto{
    public class CountryDTO{
        public int Id { get; set; }
        [Required(ErrorMessage = "Country name is required")]
        public string Name { get; set; } = "";
    }
}