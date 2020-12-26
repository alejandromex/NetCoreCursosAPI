using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using Dominio;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // End point -> Ruta -> http://localhost:5001/Cursos
    public class CursosController : MiControllerBase
    {
        
        [HttpGet]

        public async Task<ActionResult<List<CursoDTO>>> Get()
        {
            return await mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> Get(Guid id)
        {
            return await mediator.Send(new ConsultaPorId.CursoUnico{Id = id});
        }
        
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await mediator.Send(new Eliminar.Ejecuta{Id = id});
        }

    }
}