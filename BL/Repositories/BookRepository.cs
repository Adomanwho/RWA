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
        public PagedResult<BLBook> GetPaged(string? search, int page = 1, int pageSize = 10);//Mapiraj u API-u
        public BLBook GetById(int id);//Mapiraj u API-u i logika
        public BLBook Create(BLBook book);//Mapiraj iz dto u bl, posalji tu, rezultat mapiraj nazad
        public string Update(int id, BLBook request);//Malo cheesy
        public bool Delete(int id);//same
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
                var existsGenre = _dbContext.Genres.Any(g => g.Id == book.GenreId);
                if (!existsGenre) return null;

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

        public bool Delete(int id)
        {
            try
            {
                var book = _dbContext.Books.Find(id);
                if (book == null) return false;
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
                _dbContext.WriteLog(2, "BookController.Delete", $"Book {id} deleted");
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.Delete", $"Error deleting book {id}", ex.Message);
                throw;
            }
        }

        public BLBook GetById(int id)
        {
            var book =  _dbContext.Books.Include(b => b.Genre).FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                _dbContext.WriteLog(2, "BookController.GetById", $"Book {id} not found");
                return null;
            }
            _dbContext.WriteLog(2, "BookController.GetById", $"Book {id} retrieved");
            return _mapper.Map<BLBook>(book);
        }

        public PagedResult<BLBook> GetPaged(string? search, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 10 : pageSize;

            var query = _dbContext.Books.Include(b => b.Genre).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(b => b.Name.Contains(s) || b.AuthorFirstName.Contains(s) || b.AuthorLastName.Contains(s));
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
                    GenreId = b.GenreId
                })
                .ToList();

            _dbContext.WriteLog(2, "BookController.GetPaged", $"Read books (search='{search}', page={page}, size={pageSize})");
            return new PagedResult<BLBook>(items, page, pageSize, total);
        }

        public string Update(int id, BLBook request)
        {
            try
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
                if (book == null) return "The book wasn't found by id.";

                var existsGenre = _dbContext.Genres.Any(g => g.Id == request.GenreId);
                if (!existsGenre) return "The genre doesn't exist.";

                book.Name = request.Name;
                book.AuthorFirstName = request.AuthorFirstName;
                book.AuthorLastName = request.AuthorLastName;
                book.NumberOfPages = request.NumberOfPages;
                book.GenreId = request.GenreId;
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "BookController.Update", $"Book {id} updated");
                return "Book updated successfully.";
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "BookController.Update", $"Error updating book {id}", ex.Message);
                throw;
            }
        }
    }
}
