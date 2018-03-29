using Microsoft.AspNetCore.Mvc.Filters;
using Sphinx.EntityFramework;

namespace Sphinx.Web.Host.ActionFilters
{
    public class SphinxDbContextActionFilter : IActionFilter
    {
        private readonly SphinxDbContext _context;

        public SphinxDbContextActionFilter(SphinxDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _context.SaveChanges();
        }
    }
}