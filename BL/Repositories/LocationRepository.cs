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
    public interface ILocationRepository
    {
        IEnumerable<BLLocation> GetLocations();
        BLLocation GetByName(string name);
        BLLocation Create(string name);
        string Update(string locationToUpdate, string newLocation);
        bool Delete(string name);
    }

    public class LocationRepository : ILocationRepository
    {
        private readonly RwaLibraryContext _dbContext;
        private readonly IMapper _mapper;

        public LocationRepository(RwaLibraryContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public IEnumerable<BLLocation> GetLocations()
        {
            try
            {
                var locations = _dbContext.Locations.ToList();
                _dbContext.WriteLog(2, "LocationController.GetLocations", "Retrieved all locations");
                return _mapper.Map<IEnumerable<BLLocation>>(locations);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.GetLocations", "Error retrieving locations", ex.Message);
                throw;
            }
        }

        public BLLocation GetByName(string name)
        {
            try
            {
                var location = _dbContext.Locations.FirstOrDefault(l => l.Name == name);
                if (location == null)
                {
                    _dbContext.WriteLog(2, "LocationController.GetByName", $"Location \"{name}\" not found");
                    return null;
                }

                _dbContext.WriteLog(2, "LocationController.GetByName", $"Retrieved location \"{name}\"");
                return _mapper.Map<BLLocation>(location);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.GetByName", $"Error retrieving location \"{name}\"", ex.Message);
                throw;
            }
        }

        public BLLocation Create(string name)
        {
            try
            {
                bool exists = _dbContext.Locations.Any(l => l.Name == name);
                if (exists)
                {
                    _dbContext.WriteLog(2, "LocationController.Create", $"Location \"{name}\" already exists");
                    return null;
                }

                var entity = new Location { Name = name };
                _dbContext.Locations.Add(entity);
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "LocationController.Create", $"Location \"{name}\" created");
                return _mapper.Map<BLLocation>(entity);
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Create", $"Error creating location \"{name}\"", ex.Message);
                throw;
            }
        }

        public string Update(string locationToUpdate, string newLocation)
        {
            try
            {
                var location = _dbContext.Locations.FirstOrDefault(l => l.Name == locationToUpdate);
                if (location == null)
                {
                    _dbContext.WriteLog(2, "LocationController.Update", $"Location \"{locationToUpdate}\" not found");
                    return $"Location \"{locationToUpdate}\" not found.";
                }

                bool duplicate = _dbContext.Locations.Any(l => l.Name == newLocation && locationToUpdate != newLocation);
                if (duplicate)
                {
                    _dbContext.WriteLog(2, "LocationController.Update", $"Location \"{newLocation}\" already exists");
                    return $"Location \"{newLocation}\" already exists.";
                }

                location.Name = newLocation;
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "LocationController.Update", $"Location \"{locationToUpdate}\" updated to \"{newLocation}\"");
                return $"Location \"{locationToUpdate}\" updated to \"{newLocation}\".";
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Update", $"Error updating location \"{locationToUpdate}\"", ex.Message);
                throw;
            }
        }

        public bool Delete(string name)
        {
            try
            {
                var location = _dbContext.Locations.FirstOrDefault(l => l.Name == name);
                if (location == null)
                {
                    _dbContext.WriteLog(2, "LocationController.Delete", $"Location \"{name}\" not found");
                    return false;
                }

                _dbContext.Locations.Remove(location);
                _dbContext.SaveChanges();

                _dbContext.WriteLog(2, "LocationController.Delete", $"Location \"{name}\" deleted");
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.WriteLog(3, "LocationController.Delete", $"Error deleting location \"{name}\"", ex.Message);
                throw;
            }
        }
    }


}
