using System.Threading.Tasks;
using MediatR;
using Aplicacion.Comentarios;
using Microsoft.AspNetCore.Mvc;
using System;
using Dominio;

namespace WebAPI.Controllers
{
    public class ComentariosController : MiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await mediator.Send(new Eliminar.Ejecuta{ComentarioId = id});
        }
    }
}