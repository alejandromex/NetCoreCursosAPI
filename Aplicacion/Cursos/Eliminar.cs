using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using System;
using Aplicacion.ManejadorError;
using System.Net;
using Dominio;
using System.Linq;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id{get;set;}
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext context)
            {
                this.context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructoresDB = context.CursoInstructors.Where(x => x.CursoId == request.Id);

                foreach(var instructor in instructoresDB)
                {
                    context.CursoInstructors.Remove(instructor);
                }

                var comentariosDB = context.Comentarios.Where(x => x.CursoId == request.Id);
                
                foreach(var comentario in comentariosDB)
                {
                    context.Comentarios.Remove(comentario);
                }

                var precioDB = context.Precios.Where(x => x.CursoId == request.Id).FirstOrDefault();

                if(precioDB != null)
                {
                    context.Precios.Remove(precioDB);
                }

                var curso = await context.Cursos.FindAsync(request.Id);
                if(curso == null)
                {
                    // throw new System.Exception("No se pudo eliminar el curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {curso =  "No se encontro el curso"});

                }
                context.Remove(curso);

                var resultado = await context.SaveChangesAsync();
                if(resultado> 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios");

            }
        }
    }
}