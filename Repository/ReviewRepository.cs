using Data.DataContext;
using Models.Review;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public Review GetReviewById(int id)
        {
            return _context.Reviews.Where(r => r.Id == id).FirstOrDefault()!;
        }

        public Review GetReviewByReviwer(int rid)
        {
            return _context.Reviews.Where(r => r.Reviewer!.Id == rid).FirstOrDefault()!;
        }


        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.Id == id);
        }

        public ICollection<Review> GetReviewsForPokemon(int pid)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pid).ToList();
        }

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }
    }
}