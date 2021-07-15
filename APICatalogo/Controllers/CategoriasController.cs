using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(x => x.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }

        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados");
            }
        }



        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria); // Adiciona na Memória 
                _context.SaveChanges(); // Inclui os dados no Banco de dados

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar uma nova Categoria");
            }
           
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId) // Verificando se o ID que está sendo passado é do produto a alterar.
                {
                    return BadRequest($"Não foi possivel atualizar a categoria com id={id} ");
                }

                _context.Entry(categoria).State = EntityState.Modified; // Alterando o estado da entidade
                _context.SaveChanges();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   $"Erro ao tentar atualizar a Categoria com id={id}");
            }
            

        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada");
                }
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return categoria;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar excluir a Categoria de id={id}");
            }
            

        }

    }
}
