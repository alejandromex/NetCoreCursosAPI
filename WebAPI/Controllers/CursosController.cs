using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using Dominio;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;
using System;
using Persistencia.DapperConexion.Paginacion;

namespace WebAPI.Controllers
{
    public class CursosController : MiControllerBase
    {
        [Authorize(Roles="Admin")]
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

        [HttpPost("report")]
        public async Task<ActionResult<PaginacionModel>> Report(PaginacionCurso.Ejecuta data)
        {
            return await mediator.Send(data);
        }

    }
}