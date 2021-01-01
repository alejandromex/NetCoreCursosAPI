using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Aplicacion.ManejadorError;
using System.Net;
using FluentValidation;
using Aplicacion.Contratos;
using System;
using System.Collections.Generic;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email {get;set;}
            public string Password{get;set;}
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
           
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {

            private readonly UserManager<Usuario> userManager;
            private readonly SignInManager<Usuario> signInManager;
            private readonly IJwtGenerador jwtGenerador;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByEmailAsync(request.Email);
                var listRoles = await userManager.GetRolesAsync(usuario);
                List<string> roles = new List<string>();
                if(listRoles != null)
                {
                    foreach(string rol in listRoles)
                    {
                        roles.Add(rol);
                    }
                }

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new {usuario = "Email no localizado"});
                }

                var resultado = await signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if(resultado.Succeeded)
                {
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        Token = jwtGenerador.CrearToken(usuario,roles),
                        Username = usuario.UserName,
                        Imagen = null
                    };
                }
                
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new {usuario = "Password incorrecto"});
            }
        }
    }
}