using Alabama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alabama.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AlabamaDBContext _db;
        public CountryRepository(AlabamaDBContext db)
        {
            _db = db;
        }

        public bool CountryExists(int countryId)
        {
            return _db.Country.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            _db.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _db.Remove(country);
            return Save();
        }

        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {

            return _db.Authors.Where(a => a.Country.Id == countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _db.Country.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _db.Country.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfAnAuthor(int authorId)
        {
            return _db.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country = _db.Country.Where(c => c.Name.Trim().ToUpper() == countryName && c.Id == countryId).FirstOrDefault();

            return country == null ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _db.Update(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
