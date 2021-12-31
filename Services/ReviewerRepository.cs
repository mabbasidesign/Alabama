using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class ReviewerRepository: IReviewerRepository
    {
        private readonly AlabamaDBContext _db;
        public ReviewerRepository(AlabamaDBContext db)
        {
            _db = db;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _db.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _db.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _db.Reviewers.OrderBy(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
            var reviewerId = _db.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewer.Id).FirstOrDefault();

            return _db.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _db.Reviewers.OrderBy(r => r.FirstName).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _db.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _db.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Save()
        {
            var saved =_db.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _db.Update(reviewer);
            return Save();
        }
    }
}
