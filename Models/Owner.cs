using joinedTable.PokemonOwner;

namespace Models.Owner
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gym { get; set; } = "";
        // Building one-to-many relationship btw Owner and Country
        public Country.Country? Country {get; set;}
        // building the many to many relationship. ie (relationship btw two tables -
        // joined tables)
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}