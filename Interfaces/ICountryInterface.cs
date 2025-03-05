using Models.Country;
using Models.Owner;

namespace PokemonReviewApp.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();

        // Detail endpoint
        Country GetCountryById(int id);
        Country GetCountryOfOwner(int ownerid);
        ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int id);

        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}