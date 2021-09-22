using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[Controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
            
        }

        /// <summary>
        /// Exibe uma relação dos produtos
        /// </summary>
        /// <param name="produtosParameters"></param>
        /// <returns>Retorna uma lista de objetos Produtos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> 
            Get([FromQuery] ProdutosParameters produtosParameters)
        {
                var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));


                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;
                       
        }

        /// <summary>
        /// Obtem o Produto pelo seu identificador Id
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"O Produto com id={id} não foi encontrado");
                }
                
                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
                      
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {
                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produto); // Adiciona na Memória 
                await _uof.Commit(); // Inclui os dados no Banco de dados

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
                       
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody] ProdutoDTO produtoDto)
        {
                if (id != produtoDto.ProdutoId) 
                {
                    return BadRequest($"Não foi possivel atualizar o Produto com id={id} ");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();
                return Ok($"Produto com id={id} foi atualizada com sucesso");
           
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"Produto com id={id} não foi encontrado");
                }

                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return produtoDto;
           
        }


    }
}
