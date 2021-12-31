using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AlabamaDBContext _db;
        public AuthorRepository(AlabamaDBContext db)
        {
            _db = db;
        }

        public ICollection<Author> GetAuthors()
        {
            return _db.Authors.OrderBy(a => a.FirstName).ToList();
        }
        public Author GetAuthor(int authorId)
        {
            return _db.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public bool AuthorExists(int authorId)
        {
            return _db.Authors.Any(a => a.Id == authorId);
        }

        public bool CreateAuthor(Author author)
        {
            _db.Add(author);
            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            _db.Authors.Remove(author);
            return Save();
        }

        public ICollection<Author> GetAuthorsOfABook(int bookId)
        {
            return _db.BookAuthors.Where(a => a.BookId == bookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int authorId)
        {
            return _db.BookAuthors.Where(a => a.AuthorId == authorId).Select(a => a.Book).ToList();
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            _db.Update(author);
            return Save();
        }
    }
}
