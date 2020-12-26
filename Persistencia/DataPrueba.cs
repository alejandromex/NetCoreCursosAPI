using Dominio;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        public async static Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if(!usuarioManager.Users.Any())
            {
                var usuario = new Usuario{
                    NombreCompleto = "Alejandro Ceballos",
                    UserName = "MrRobot",
                    Email = "alexandro_sin@hotmail.com"
                    };
                await usuarioManager.CreateAsync(usuario, "Pedro#123");
            }
        }
    }
}