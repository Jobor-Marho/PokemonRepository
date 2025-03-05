using System.Runtime.Intrinsics.Arm;
using AutoMapper;
using Data.DataContext;
using Models.Category;
using Models.pokemeon;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;


        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id)!;
        }

        public ICollection<Pokemon> GetPokemonByCategory(int CatId){
            return _context.PokemonCategories.Where(pc => pc.CategoryId == CatId).Select(pc => pc.Pokemon).ToList();
        }

        public bool CategoryExists(int catId)
        {
            return _context.Categories.Any(c => c.Id == catId);
        }

        public bool CreateCategory(Category category)
        {
            //change tracker
            _context.Add(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }
    }
}