using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolAgregar
    {
        public class Ejecuta : IRequest{

            public string UserName{get;set;}
            public string RolNombre{get;set;}            
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta> {

            public ValidaEjecuta()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                this._userManager = userManager;
                this._roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.RolNombre);
                if(role == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {Mensaje = "Error al consultar el rol"});
                }

                var usuario = await _userManager.FindByNameAsync(request.UserName);

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {Mensaje = "Error al consultar el usuario"});
                }

                var respuesta = await _userManager.AddToRoleAsync(usuario, request.RolNombre);
                if(respuesta.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("Error al asignar el rol ("+request.RolNombre+") al usuario "+usuario.NombreCompleto);
            }
        }
    }
}