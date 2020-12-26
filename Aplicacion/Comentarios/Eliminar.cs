using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Persistencia;
using Aplicacion.ManejadorError;
using System.Net;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{
            public Guid ComentarioId{get;set;}
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = await _context.Comentarios.FindAsync(request.ComentarioId);
                if(comentario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {error="No se encontro el comentario"});
                }

                _context.Remove(comentario);
                var resultados = await _context.SaveChangesAsync();

                if(resultados > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("Error al eliminar el comentario");

            }
        }
    }
}