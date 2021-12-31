using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly AlabamaDBContext _db;
        public ReviewRepository(AlabamaDBContext db)
        {
            _db = db;
        }

        public ICollection<Review> GetReviews()
        {
            return _db.Reviews.OrderBy(r => r.Rating).ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _db.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsOfABook(int bookId)
        {
            return _db.Reviews.Where(r => r.Book.Id == bookId).ToList();
        }

        public Book GetBookOfAReview(int reviewId)
        {
            var bookId = _db.Reviews.Where(r => r.Id == reviewId).Select(r => r.Book.Id).FirstOrDefault();
            return _db.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public bool ReviewExists(int reviewId)
        {
            return _db.Reviews.Any(r => r.Id == reviewId);
        }

        public bool CreateReview(Review review)
        {
            _db.Add(review);
            return Save();
        }

        public bool UpdateReview(Review review)
        {
            _db.Update(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _db.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _db.RemoveRange(reviews);
            return Save();
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
