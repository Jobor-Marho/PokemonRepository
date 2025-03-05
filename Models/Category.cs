using joinedTable.PokemonCategory;

namespace Models.Category{
    public class Category{
        public int Id {get; set;}
        public string Name {get; set;} = "";

        // building the many to many relationship. ie (relationship btw two tables -
        // joined tables)
        public ICollection<PokemonCategory> PokemonCategories {get; set;}
    }
}