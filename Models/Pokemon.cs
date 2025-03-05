using joinedTable.PokemonCategory;
using joinedTable.PokemonOwner;


namespace Models.pokemeon{
    public class Pokemon{
        public int Id  {get; set;}
        public string Name {get; set;} = "";
        public DateTime BirthDate {get; set;}
        //building many-to-one-relationship btw pokemon & reviews i.e many(pokemon) -> one(review)
        public ICollection<Review.Review>? Reviews {get; set;}

        // building the many to many relationship. ie (relationship btw two tables -
        // joined tables)
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories {get; set;}
    }
}