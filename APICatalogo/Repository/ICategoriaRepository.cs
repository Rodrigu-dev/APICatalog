using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategory(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetCategoryProducts();
        Task<IEnumerable<Categoria>> GetAllCategory();
    }
}
