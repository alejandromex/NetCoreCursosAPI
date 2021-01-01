using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RoleController : MiControllerBase
    {
        
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> CrearRole(RolNuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult<Unit>> EliminarRole(string name)
        {
            
            return await mediator.Send(new RolEliminar.Ejecuta{Nombre = name});
        }

        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> GetRoles()
        {
            return await mediator.Send(new RolLista.Ejecuta());
        }

        [HttpPost("agregarRoleUsuario")]
        public async Task<ActionResult<Unit>> AsignarRole(UsuarioRolAgregar.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpDelete("eliminarRoleUsuario")]
        public async Task<ActionResult<Unit>> EliminarRole(UsuarioRolEliminar.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<List<string>>> ObtenerRole(string username)
        {
            return await mediator.Send(new ObtenerRolesPorUsuario.Ejecuta{UserName = username});
        }
    }
}