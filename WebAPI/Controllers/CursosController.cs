using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using Dominio;
using System.Threading.Tasks;
using Aplicacion.Cursos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // End point -> Ruta -> http://localhost:5001/Cursos
    public class CursosController : ControllerBase
    {
        private readonly IMediator mediator;
        public CursosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get()
        {
            return await mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Get(int id)
        {
            return await mediator.Send(new ConsultaPorId.CursoUnico{Id = id});
        }
        
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(int id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id)
        {
            return await mediator.Send(new Eliminar.Ejecuta{Id = id});
        }

    }
}