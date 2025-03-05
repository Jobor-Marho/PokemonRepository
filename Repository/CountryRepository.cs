using Data.DataContext;
using Models.Country;
using Models.Owner;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context){
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault()!;
        }

        public Country GetCountryOfOwner(int ownerid){
            return _context.Countries.Where(c => c.Owners.Any(o => o.Id == ownerid)).FirstOrDefault()!;
        }
        public ICollection<Owner> GetOwnersFromACountry(int countryId){
            return _context.Countries.Where(c => c.Id == countryId).SelectMany(c => c.Owners).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}