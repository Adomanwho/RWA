using AutoMapper;
using Azure.Core;
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
    public interface IBookRepository
    {
        public PagedResult<BLBook> GetPaged(string? search, int? genreId, int page = 1, int pageSize = 10);//Mapiraj u API-u
        public BLBook GetByName(string name);//Mapiraj u API-u i logika
        public BLBook Create(BLBook book);//Mapiraj iz dto u bl, posalji tu, rezultat mapiraj nazad
        public string Update(string name, BLBook request);//Malo cheesy
        public bool Delete(string name);//same
    }

    public class BookRepository : IBookRepository
    {

        private readonly RwaLibraryContext _dbContext = new();
        private readonly IMapper _mapper;

        public BookRepository(RwaLibraryContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public BLBook Create(BLBook book)
        {
            try
            {
                bool existsBook = _dbContext.Books.Any(b => b.Name == book.Name);
                if (existsBook) return null;

                var entity = new Book
                {
                    Name = book.Name,
                    AuthorFirstName = book.AuthorFirstName,
                    AuthorLastName = book.AuthorLastName,
                    NumberOfPages = book.NumberOfPages,
                    GenreId = book.GenreId
                };
                _dbContext.Books.Add(entity);
                _dbContext.SaveChanges();

                var bLBook = _dbContext.Books.Include(b => b.Genre)
                    .Where(b => b.Id == entity.Id)
                    .Select(b => _mapper.Map<BLBook>(b))
                    .First();

                _dbContext.WriteLog(2, "BookController.Create", $"Book {entity.Id} created");
                return bLBook;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.Create", "Error creating book", ex.Message);
                throw;
            }
        }

        public bool Delete(string name)
        {
            try
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Name == name);
                if (book == null) return false;
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
                _dbContext.WriteLog(2, "BookController.Delete", $"Book \" {name} \" not found");
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.Delete", $"Error deleting book  \"{name}\"", ex.Message);
                throw;
            }
        }

        public BLBook GetByName(string name)
        {
            var book =  _dbContext.Books.Include(b => b.Genre).FirstOrDefault(b => b.Name == name);
            if (book == null)
            {
                _dbContext.WriteLog(2, "BookController.GetByName", $"Book \"{name}\" not found");
                return null;
            }
            _dbContext.WriteLog(2, "BookController.GetByName", $"Book \"{name}\" not found");
            return _mapper.Map<BLBook>(book);
        }

        public PagedResult<BLBook> GetPaged(string? search, int? genreId, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 10 : pageSize;

            var query = _dbContext.Books.Include(b => b.Genre).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(b => b.Name.Contains(s) || b.AuthorFirstName.Contains(s) || b.AuthorLastName.Contains(s));
            }

            if (genreId.HasValue && genreId.Value > 0)
            {
                query = query.Where(b => b.GenreId == genreId.Value);
            }

            var total = query.Count();
            var items = query
                .OrderBy(b => b.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BLBook
                {
                    Id = b.Id,
                    Name = b.Name,
                    AuthorFirstName = b.AuthorFirstName,
                    AuthorLastName = b.AuthorLastName,
                    NumberOfPages = b.NumberOfPages,
                    GenreId = b.GenreId,
                    Genre = b.Genre
                })
                .ToList();

            _dbContext.WriteLog(2, "BookController.GetPaged", $"Read books (search='{search}', page={page}, size={pageSize})");
            return new PagedResult<BLBook>(items, page, pageSize, total);
        }

        public string Update(string name, BLBook request)
        {
            try
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Name == name);
                if (book == null) return "The book with this name wasn't found.";

                var existsGenre = _dbContext.Genres.Any(g => g.Id == request.GenreId);
                if (!existsGenre) return "The genre doesn't exist.";
                                                              //match u bazi sa onim na kaj se zeli upd ali nije isto kao knjiga koja se zeli upd
                var existsBookName = _dbContext.Books.Any(b => b.Name == request.Name && b.Name != name);//nije isto ime i dozvoljava mijenjanje iste knjige
                if (existsBookName) return "The book you are trying to update to (the name you entered) already exists. (Change the name)";

                book.Name = request.Name;
                book.AuthorFirstName = request.AuthorFirstName;
                book.AuthorLastName = request.AuthorLastName;
                book.NumberOfPages = request.NumberOfPages;
                book.GenreId = request.GenreId;
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "BookController.Update", $"Book \"{name}\" not found");
                return "Book updated successfully.";
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.Update", $"Error updating book \"{name}\"", ex.Message);
                throw;
            }
        }
    }
}
