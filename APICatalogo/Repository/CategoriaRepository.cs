using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {

        }

        public async Task<PagedList<Categoria>> GetCategory(CategoriasParameters categoriasParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get()
                                    .OrderBy(on => on.CategoriaId),
                                     categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }
        public async Task<IEnumerable<Categoria>> GetCategoryProducts()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> GetAllCategory()
        {
            return await Get().ToListAsync();
        }


    }
}
