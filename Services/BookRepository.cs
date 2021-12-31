using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly AlabamaDBContext _db;
        public BookRepository(AlabamaDBContext alabamaDBContext)
        {
            _db = alabamaDBContext;
        }

        public ICollection<Book> GetBooks()
        {
            return _db.Books.OrderBy(b => b.Title).ToList();
        }

        public Book GetBook(int bookId)
        {
            return _db.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string bookIsbn)
        {
            return _db.Books.Where(b => b.Isbn == bookIsbn).FirstOrDefault();
        }
        public decimal GetBookRating(int bookId)
        {
            var reviews = _db.Reviews.Where(r => r.Book.Id == bookId);

            if (reviews.Count() >= 0)
                return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public bool BookExists(int bookId)
        {
            return _db.Books.Any(b => b.Id == bookId);
        }

        public bool BookExists(string bookIsbn)
        {
            return _db.Books.Any(b => b.Isbn == bookIsbn);
        }

        public bool IsDuplicateIsbn(int bookId, string bookIsbn)
        {
            var book = _db.Books.Where(b => b.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() && b.Id == bookId).FirstOrDefault();

            return book == null ? false : true;
        }

        public bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _db.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _db.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor
                {
                    Author = author,
                    Book = book
                };
                _db.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory
                {
                    Category = category,
                    Book = book
                };
                _db.Add(bookCategory);
            }

            _db.Add(book);

            return Save();
        }

        public bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _db.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _db.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var bookAthorsToDelete = _db.BookAuthors.Where(b => b.BookId == book.Id);
            var bookCategoriesToDelete = _db.BookCategories.Where(b => b.BookId == book.Id);

            _db.RemoveRange(bookAthorsToDelete);
            _db.RemoveRange(bookCategoriesToDelete);

            foreach (var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _db.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory()
                {
                    Category = category,
                    Book = book
                };
                _db.Add(bookCategory);
            }

            _db.Update(book);

            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _db.Remove(book);
            return Save();
        }

        public bool Save()
        {
            var save = _db.SaveChanges();

            return save >= 0 ? false : true;
        }
    }
}
