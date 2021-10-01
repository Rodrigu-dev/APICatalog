using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }

        public async Task<PagedList<Produto>> GetProductsPagination(ProdutosParameters produtosParameters)
        {
            return await PagedList<Produto>.ToPagedList(Get()
                                     .OrderBy(on => on.ProdutoId),
                                      produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProductsByPrice()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAllProducts()
        {
            return await Get().ToListAsync();
        }

    }
}
