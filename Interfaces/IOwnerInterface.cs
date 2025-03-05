using Models.Country;
using Models.Owner;
using Models.pokemeon;

namespace PokemonReviewApp.Interfaces{
    public interface IOwnerRepository{
        ICollection<Owner> GetOwners();

        Owner GetOwnerById(int id);

        Owner GetOwnerByGym(string gym);
        Country GetOwnerCountry(int cid);
        ICollection<Pokemon> GetPokemonsForOwner(int owId);
        bool OwnerExists(int id);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}