using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class ObtenerRolesPorUsuario
    {
        public class Ejecuta : IRequest<List<string>>{
            
            public string UserName{get;set;}
        }

        public class Manejador : IRequestHandler<Ejecuta, List<string>>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _rolManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> rolManager)
            {
                this._userManager = userManager;
                this._rolManager = rolManager;
            }
            public async Task<List<string>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);
                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {Mensaje = "No se encontro el usuario"});
                }

                var roles = await _userManager.GetRolesAsync(usuario);
                return new List<string>(roles);

            }

        }


    }
}