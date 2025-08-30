using BL.BLModels;
using BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public interface ILogRepository
    {
        public PagedResult<Log> GetPaged(int? level, int page = 1, int pageSize = 50);  
    }
    public class LogRepository : ILogRepository
    {
        private readonly RwaLibraryContext _dbContext = new();

        public LogRepository(RwaLibraryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public PagedResult<Log> GetPaged(int? level, int page = 1, int pageSize = 50)
        {
            IQueryable<Log> query = _dbContext.Logs.AsQueryable();
            if (level is 1 or 2 or 3) query = query.Where(l => l.LogLevel == level);

            var total = query.Count();
            var items = query
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Log>(items, page, pageSize, total);
        }
    }
}
