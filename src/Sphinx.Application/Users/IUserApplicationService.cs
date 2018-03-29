using System.Collections.Generic;
using System.Threading.Tasks;
using Sphinx.Domain;

namespace Sphinx.Application.Users
{
    public interface IUserApplicationService
    {
        Task<List<ApplicationUser>> GetAllAsync();
    }
}