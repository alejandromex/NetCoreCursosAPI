using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Contratos;
using System.Collections.Generic;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData>
        {
            
        }

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager;
            private readonly IJwtGenerador jwtGenerador;
            private readonly IUsuarioSesion usuarioSesion;
            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion )
            {
                this.userManager = userManager;
                this.jwtGenerador = jwtGenerador;
                this.usuarioSesion = usuarioSesion;
            }
            
            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await userManager.FindByNameAsync(usuarioSesion.ObtenerUsuarioSesion());
                var listRoles = await userManager.GetRolesAsync(usuario);
                List<string> roles = new List<string>(listRoles);
                UsuarioData usuarioD = new UsuarioData();
                usuarioD.NombreCompleto = usuario.NombreCompleto;
                usuarioD.Username = usuario.UserName;
                usuarioD.Token = jwtGenerador.CrearToken(usuario,roles);
                usuarioD.Imagen = null;
                usuarioD.Email = usuario.Email;
                return usuarioD; 
            }
        }
    }
}