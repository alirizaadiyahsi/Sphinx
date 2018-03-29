using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphinx.EntityFramework
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
    }
}
