using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
   // [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableQuery]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[Controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uofContext;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uofContext = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna uma coleção de objetos Categoria
        /// </summary>
        /// <returns>Lista de Categorias</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllAsync()
        {
            var categorias = await _uofContext.CategoriaRepository.GetAllCategory();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoryProducts()
        {
            var categorias = await _uofContext.CategoriaRepository.GetCategoryProducts();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpGet("paginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>>
            GetPagination([FromQuery] CategoriasParameters categoriasParameters)
        {
           
                var categoria = await _uofContext.CategoriaRepository.GetCategory(categoriasParameters);

                Response.Headers.Add("X-Total-Registros", JsonConvert.SerializeObject(categoria.TotalCount));
                Response.Headers.Add("X-Numero-Paginas", JsonConvert.SerializeObject(categoria.TotalPages));

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDto;
        }

        /// <summary>
        /// Obtem Categoria pelo Id
        /// </summary>
        /// <param name="id">Codigo da Categoria</param>
        /// <returns> Objetos Categoria </returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int? id)
        {
            try
            {
                var categoria = await _uofContext.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                    //throw new Exception();
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDto;

            }
            catch (Exception)
            {
                return BadRequest();
            }
                
        }

        /// <summary>
        /// Incluir nova categoria
        /// </summary>
        /// <param name="categoriaDto">objeto categoria</param>
        /// <returns>O objeto Categoria Incluida</returns>
        /// <remarks>Retorna um objeto Categoria Incluído</remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                
                _uofContext.CategoriaRepository.Add(categoria); // Adiciona na Memória 
                await _uofContext.Commit(); // Inclui os dados no Banco de dados

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", 
                    new { id = categoria.CategoriaId }, categoriaDTO);
                      
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            
                if (id != categoriaDto.CategoriaId) 
                {
                    return BadRequest($"Não foi possivel atualizar a categoria com id={id} ");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);
               
                _uofContext.CategoriaRepository.Update(categoria); 
                await _uofContext.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
           
                var categoria = await _uofContext.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada");
                }
              
                _uofContext.CategoriaRepository.Delete(categoria);
                await _uofContext.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
        }

    }
}
