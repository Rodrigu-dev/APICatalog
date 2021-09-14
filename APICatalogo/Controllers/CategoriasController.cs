using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categorias = _context.CategoriaRepository.GetCategoriaProdutos().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                var categoria = _context.CategoriaRepository.Get().ToList();
                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }

        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
               
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                
                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                
                _context.CategoriaRepository.Add(categoria); // Adiciona na Memória 
                _context.Commit(); // Inclui os dados no Banco de dados

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar uma nova Categoria");
            }
           
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId) 
                {
                    return BadRequest($"Não foi possivel atualizar a categoria com id={id} ");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);
               
                _context.CategoriaRepository.Update(categoria); 
                _context.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   $"Erro ao tentar atualizar a Categoria com id={id}");
            }
            

        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada");
                }
              
                _context.CategoriaRepository.Delete(categoria);
                _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar excluir a Categoria de id={id}");
            }
            

        }

    }
}
