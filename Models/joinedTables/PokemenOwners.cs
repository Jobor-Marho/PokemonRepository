using Models.Owner;
using Models.pokemeon;

namespace joinedTable.PokemonOwner{
    public class PokemonOwner{
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner {get; set;}
    }
}