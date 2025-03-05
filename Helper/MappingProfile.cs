using AutoMapper;
using Models.Category;
using Models.Country;
using Models.Owner;
using Models.pokemeon;
using Models.Review;
using Models.Reviewer;
using PokemonReviewApp.Dto;
using PokenmonReviewApp.Dto;

namespace PokemonReviewApp.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Reviewer, ReviewerDTO>().ReverseMap();
        }
    }
}