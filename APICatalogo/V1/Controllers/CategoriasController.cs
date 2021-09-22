using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[Controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriaProdutos();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> 
            Get([FromQuery] CategoriasParameters categoriasParameters)
        {
           
                var categoria = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categoria.TotalCount,
                    categoria.PageSize,
                    categoria.CurrentPage,
                    categoria.TotalPages,
                    categoria.HasNext,
                    categoria.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDto;
        }

        /// <summary>
        /// Obtem Categoria pelo Id
        /// </summary>
        /// <param name="id">Codigo da Categoria</param>
        /// <returns> Objetos Categoria </returns>
        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
               
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                
                return categoriaDto;
        }

        /// <summary>
        /// Incluir nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///     
        ///     Post api/categoria
        ///     {
        ///         "categoriaId": 1,
        ///         "nome": "categoria1",
        ///         "imagemUrl": "http://rbc.net/2.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto">objeto categoria</param>
        /// <returns>O objeto Categoria Incluida</returns>
        /// <remarks>Retorna um objeto Categoria Incluído</remarks>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                
                _uof.CategoriaRepository.Add(categoria); // Adiciona na Memória 
                await _uof.Commit(); // Inclui os dados no Banco de dados

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
               
                _uof.CategoriaRepository.Update(categoria); 
                await _uof.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
           
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada");
                }
              
                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
        }

    }
}
