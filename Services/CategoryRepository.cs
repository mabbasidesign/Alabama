using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly AlabamaDBContext _db;
        public CategoryRepository(AlabamaDBContext db)
        {
            _db = db;
        }

        public bool CategoryExists(int categoryId)
        {
            return _db.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _db.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Remove(category);
            return Save();
        }

        public ICollection<Book> GetAllBooksForCategory(int categoryId)
        {
            return _db.BookCategories.Where(a => a.CategoryId == categoryId).Select(a => a.Book).ToList();
        }

        public ICollection<Category> GetAllCategoriesForABook(int bookId)
        {
            return _db.BookCategories.Where(a => a.BookId == bookId).Select(a => a.Category).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _db.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _db.Categories.Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id == categoryId).FirstOrDefault();

            return category == null ? true : false;
        }

        public bool Save()
        {
            var save = _db.SaveChanges();
            return save >= 0 ? false : true;
        }

        public bool UpdateCategory(Category category)
        {
            _db.Categories.Update(category);
            return Save();
        }
    }
}
