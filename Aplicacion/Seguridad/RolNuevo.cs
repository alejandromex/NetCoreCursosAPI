using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System;

namespace Aplicacion.Seguridad
{
    public class RolNuevo
    {
        public class Ejecuta : IRequest{
            public string Nombre{get;set;}
        }   

        public class ValidaEjecuta : AbstractValidator<Ejecuta> {
            
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly RoleManager<IdentityRole> _roleManager;
            public Manejador(RoleManager<IdentityRole> roleManager)
            {
                this._roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.Nombre);
                if(role != null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {error ="Rol ya existente"}); 
                }
                
                var resultado = await _roleManager.CreateAsync(new IdentityRole(request.Nombre));
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo guardar el rol");
            }
        }
    }
}