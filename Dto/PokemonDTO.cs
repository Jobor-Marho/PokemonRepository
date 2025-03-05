using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto{
    public class PokemonDTO{

        public int Id  {get; set;}
        [Required(ErrorMessage = "Pokemon name is required")]
        public string Name {get; set;} = "";
        public DateTime BirthDate {get; set;}
    }
}