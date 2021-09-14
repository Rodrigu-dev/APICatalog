using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _context.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var produtos = _context.ProdutoRepository.Get().ToList();
                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as Produtos do banco de dados");
            }
            
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            try
            {
                var produto = _context.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"O Produto com id={id} não foi encontrado");
                }
                
                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter os Produtos do banco de dados");
            }
           
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                _context.ProdutoRepository.Add(produto); // Adiciona na Memória 
                _context.Commit(); // Inclui os dados no Banco de dados

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar um novo Produto");
            }
           
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                
                if (id != produtoDto.ProdutoId) 
                {
                    return BadRequest($"Não foi possivel atualizar o Produto com id={id} ");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _context.ProdutoRepository.Update(produto);
                _context.Commit();
                return Ok($"Produto com id={id} foi atualizada com sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o Produto com id={id}");
            }
           

        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            try
            {
                var produto = _context.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"Produto com id={id} não foi encontrado");
                }

                _context.ProdutoRepository.Delete(produto);
                _context.Commit();
                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return produtoDto;
            }
            catch (System.Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar Excluir o Produto com id={id}");
            }
           

        }


    }
}
