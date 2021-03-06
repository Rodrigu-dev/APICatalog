using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
  //  [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableQuery]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[Controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uofContext;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uofContext = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAllAsync()
        {
            var produtos = await _uofContext.ProdutoRepository.GetAllProducts();

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProductsPrice()
        {
            var produtos = await _uofContext.ProdutoRepository.GetProductsByPrice();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
            
        }

        /// <summary>
        /// Exibe uma relação dos produtos por pagina
        /// </summary>
        /// <param name="produtosParameters"></param>
        /// <returns>Retorna uma lista de objetos Produtos por pagina</returns>
        [HttpGet("paginacao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> 
            GetPagination([FromQuery] ProdutosParameters produtosParameters)
        {
                var produtos = await _uofContext.ProdutoRepository.GetProductsPagination(produtosParameters);

                Response.Headers.Add("X-Total-Registros", JsonConvert.SerializeObject(produtos.TotalCount));
                Response.Headers.Add("X-Numero-Paginas", JsonConvert.SerializeObject(produtos.TotalPages));

                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
                return produtosDto;
                       
        }

        /// <summary>
        /// Obtem o Produto pelo seu identificador Id
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> GetById(int id)
        {
                var produto = await _uofContext.ProdutoRepository.GetById(p => p.ProdutoId == id);
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

                _uofContext.ProdutoRepository.Add(produto); // Adiciona na Memória 
                await _uofContext.Commit(); // Inclui os dados no Banco de dados

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

                _uofContext.ProdutoRepository.Update(produto);
                await _uofContext.Commit();
                return Ok($"Produto com id={id} foi atualizada com sucesso");
           
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
                var produto = await _uofContext.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"Produto com id={id} não foi encontrado");
                }

                _uofContext.ProdutoRepository.Delete(produto);
                await _uofContext.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return produtoDto;
           
        }


    }
}
