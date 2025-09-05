using AutoMapper;
using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface IGenreRepository
    {
        IEnumerable<BLGenre> GetGenres();
        BLGenre GetByName(string name);
        BLGenre Create(string name);
        string Update(string genreToUpdate, string newGenre);
        bool Delete(string name);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly RwaLibraryContext _dbContext;
        private readonly IMapper _mapper;

        public GenreRepository(RwaLibraryContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<BLGenre> GetGenres()
        {
            try
            {
                var genres = _dbContext.Genres.ToList();
                _dbContext.WriteLog(2, "GenreController.GetGenres", "Retrieved all genres");
                return _mapper.Map<IEnumerable<BLGenre>>(genres);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.GetGenres", "Error retrieving genres", ex.Message);
                throw;
            }
        }

        public BLGenre GetByName(string name)
        {
            try
            {
                var genre = _dbContext.Genres.FirstOrDefault(g => g.Name == name);
                if (genre == null)
                {
                    _dbContext.WriteLog(2, "GenreController.GetByName", $"Genre \"{name}\" not found");
                    return null;
                }

                _dbContext.WriteLog(2, "GenreController.GetByName", $"Retrieved genre \"{name}\"");
                return _mapper.Map<BLGenre>(genre);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.GetByName", $"Error retrieving genre \"{name}\"", ex.Message);
                throw;
            }
        }

        public BLGenre Create(string name)
        {
            try
            {
                bool exists = _dbContext.Genres.Any(g => g.Name == name);
                if (exists)
                {
                    _dbContext.WriteLog(2, "GenreController.Create", $"Genre \"{name}\" already exists");
                    return null;
                }

                var entity = new Genre { Name = name };
                _dbContext.Genres.Add(entity);
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "GenreController.Create", $"Genre \"{name}\" created");
                return _mapper.Map<BLGenre>(entity);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Create", $"Error creating genre \"{name}\"", ex.Message);
                throw;
            }
        }

        public string Update(string genreToUpdate, string newGenre)
        {
            try
            {
                var genre = _dbContext.Genres.FirstOrDefault(g => g.Name == genreToUpdate);
                if (genre == null)
                {
                    _dbContext.WriteLog(2, "GenreController.Update", $"Genre \"{genreToUpdate}\" not found");
                    return $"Genre \"{genreToUpdate}\" not found.";
                }

                bool duplicate = _dbContext.Genres.Any(g => g.Name == newGenre && genreToUpdate != newGenre);
                if (duplicate)
                {
                    _dbContext.WriteLog(2, "GenreController.Update", $"Genre \"{newGenre}\" already exists");
                    return $"Genre \"{newGenre}\" already exists.";
                }

                genre.Name = newGenre;
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "GenreController.Update", $"Genre \"{genreToUpdate}\" updated to \"{newGenre}\"");
                return $"Genre \"{genreToUpdate}\" updated to \"{newGenre}\".";
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Update", $"Error updating genre \"{genreToUpdate}\"", ex.Message);
                throw;
            }
        }

        public bool Delete(string name)
        {
            try
            {
                var genre = _dbContext.Genres.FirstOrDefault(g => g.Name == name);
                if (genre == null)
                {
                    _dbContext.WriteLog(2, "GenreController.Delete", $"Genre \"{name}\" not found");
                    return false;
                }

                _dbContext.Genres.Remove(genre);
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "GenreController.Delete", $"Genre \"{name}\" deleted");
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "GenreController.Delete", $"Error deleting genre \"{name}\"", ex.Message);
                throw;
            }
        }
    }

}
