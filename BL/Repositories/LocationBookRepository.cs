using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface ILocationBookRepository
    {
        IEnumerable<BLLocationBook> GetAll();
        BLLocationBook Create(string bookName, string locationName);
        string Update(string originalBookName, string originalLocationName, string newBookName, string newLocationName);
        bool Delete(string bookName, string locationName);
    }

    public class LocationBookRepository : ILocationBookRepository
    {
        private readonly RwaLibraryContext _dbContext;
        private readonly IMapper _mapper;

        public LocationBookRepository(RwaLibraryContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<BLLocationBook> GetAll()
        {
            var entities = _dbContext.LocationBooks
                .Include(lb => lb.Book)
                .Include(lb => lb.Location)
                .ToList();

            return _mapper.Map<IEnumerable<BLLocationBook>>(entities);
        }

        public BLLocationBook Create(string bookName, string locationName)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Name == bookName);
            if (book == null) throw new Exception($"Book \"{bookName}\" not found.");

            var location = _dbContext.Locations.FirstOrDefault(l => l.Name == locationName);
            if (location == null) throw new Exception($"Location \"{locationName}\" not found.");

            bool exists = _dbContext.LocationBooks.Any(lb => lb.BookId == book.Id && lb.LocationId == location.Id);
            if (exists) throw new Exception($"This mapping already exists: {bookName} - {locationName}");

            var entity = new LocationBook
            {
                BookId = book.Id,
                LocationId = location.Id,
                Name = $"{book.Name} - {location.Name}"
            };

            _dbContext.LocationBooks.Add(entity);
            _dbContext.SaveChanges();

            return _mapper.Map<BLLocationBook>(entity);
        }

        public string Update(string originalBookName, string originalLocationName, string newBookName, string newLocationName)
        {
            var bookOriginal = _dbContext.Books.FirstOrDefault(b => b.Name == originalBookName);
            var locationOriginal = _dbContext.Locations.FirstOrDefault(l => l.Name == originalLocationName);
            if (bookOriginal == null || locationOriginal == null)
                return "Original book or location not found.";

            var entity = _dbContext.LocationBooks
                .FirstOrDefault(lb => lb.BookId == bookOriginal.Id && lb.LocationId == locationOriginal.Id);
            if (entity == null)
                return "Original mapping not found.";

            var newBook = _dbContext.Books.FirstOrDefault(b => b.Name == newBookName);
            var newLocation = _dbContext.Locations.FirstOrDefault(l => l.Name == newLocationName);
            if (newBook == null || newLocation == null)
                return "New book or location not found.";

            bool exists = _dbContext.LocationBooks.Any(lb => lb.BookId == newBook.Id && lb.LocationId == newLocation.Id && lb.Id != entity.Id);
            if (exists)
                return "This mapping already exists.";

            entity.BookId = newBook.Id;
            entity.LocationId = newLocation.Id;
            entity.Name = $"{newBook.Name} - {newLocation.Name}";

            _dbContext.SaveChanges();
            return "Success";
        }

        public bool Delete(string bookName, string locationName)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Name == bookName);
            var location = _dbContext.Locations.FirstOrDefault(l => l.Name == locationName);
            if (book == null || location == null) return false;

            var entity = _dbContext.LocationBooks.FirstOrDefault(lb => lb.BookId == book.Id && lb.LocationId == location.Id);
            if (entity == null) return false;

            _dbContext.LocationBooks.Remove(entity);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
