using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData> {
            public string NombreCompleto{get;set;}
            public string Email{get;set;}
            public string Password{get;set;}    
            public string UserName{get;set;}

        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly CursosOnlineContext _context;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public Manejador(UserManager<Usuario> userManager, CursosOnlineContext context, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                this._userManager = userManager;
                this._context = context;
                this._jwtGenerador = jwtGenerador;
                this._passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(request.UserName);

                if(usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje ="Usuario a actualizar no encontrado"});
                }

                var email = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();
                if(email)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new {mensaje ="Email ya existe "});
                }

                usuario.NombreCompleto = request.NombreCompleto;
                usuario.Email = request.Email;
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, request.Password);
                var resultados = await _userManager.UpdateAsync(usuario);
                var roles = await _userManager.GetRolesAsync(usuario);
                List<string> rolesLista = new List<string>(roles);
                if(resultados.Succeeded)
                {
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Email = usuario.Email,
                        Username = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario,rolesLista)
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Error al actualizar, revise sus datos"});




            }
        }
    }
}