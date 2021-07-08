using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _context.Produtos.ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto); // Adiciona na Memória 
            _context.SaveChanges(); // Inclui os dados no Banco de dados

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] Produto produto)
        {
            /// ESSA VERIFICAÇÃO NÃO PRECISA MAIS SER FEITA POR SER AUTOMATICA PELO FRAMEWORK
            //if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            
            if (id != produto.ProdutoId) // Verificando se o ID que está sendo passado é do produto a alterar.
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified; // Alterando o estado da entidade
            _context.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;

        }


    }
}
