using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProductsPagination(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProductsByPrice();
        Task<IEnumerable<Produto>> GetAllProducts();


    }
}
