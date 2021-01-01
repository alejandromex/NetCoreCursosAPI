using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistencia;
using Dominio;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aplicacion.ManejadorError;
using System.Net;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto {get;set;}
            public string Email{get;set;}
            public string Password{get;set;}
            public string Username{get;set;}
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{

            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext context;
            private readonly UserManager<Usuario> userManager;
            private IJwtGenerador jwtGenerador;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                this.context = context;
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await context.Users.Where(user => user.Email == request.Email).AnyAsync();
                if(existe)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {ok = false ,error = "Ya existe un usuario registrado con ese email"});
                }

                var existeUsername = await context.Users.Where(user => user.UserName == request.Username).AnyAsync();
                if(existeUsername)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {ok = false ,error = "Ya existe un usuario registrado con ese username"});
                }

                var usuario = new Usuario();
                usuario.Email = request.Email;
                usuario.NombreCompleto = request.NombreCompleto;
                usuario.UserName = request.Username;    


                var resultado = await userManager.CreateAsync(usuario, request.Password);
                if(resultado.Succeeded)
                {
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador.CrearToken(usuario, null),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }
                throw new System.Exception();
            }
        }
    }
}