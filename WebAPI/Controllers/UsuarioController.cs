using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioData>> Register(Registrar.Ejecuta parametros)
        {
            return await mediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await mediator.Send(new UsuarioActual.Ejecutar());
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioData>> ActualizarUsuario(UsuarioActualizar.Ejecuta data)
        {
            return await mediator.Send(data);
        }
    }
}