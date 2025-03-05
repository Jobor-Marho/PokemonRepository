using Data.DataContext;
using Models.Country;
using Models.Owner;
using Models.pokemeon;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwnerByGym(string gym)
        {
            return _context.Owners.Where(o => o.Gym == gym).FirstOrDefault()!;
        }

        public Owner GetOwnerById(int id)
        {
            return _context.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public Country GetOwnerCountry(int cid)
        {
            return _context.Owners.Where(o => o.Country.Id == cid).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonsForOwner(int owId)
        {
            return _context.Owners.Where(o => o.Id == owId).SelectMany(op => op.PokemonOwners).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int id)
        {
            return _context.Owners.Any(o => o.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}