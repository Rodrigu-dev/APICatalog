using APICatalogo.DTOs;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
   // [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiVersion("2.0")]
    [Route("api/[Controller]")]
    [ApiController]
    
    public class TesteController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public TesteController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("produtos2")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var cat = await _uof.CategoriaRepository.GetCategoriaProdutos();
            var catDto = _mapper.Map<List<CategoriaDTO>>(cat);
            return catDto;
        }

       
            

        

    }
}
