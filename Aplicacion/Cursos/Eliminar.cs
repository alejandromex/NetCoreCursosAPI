using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using System;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public int Id{get;set;}
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
                var curso = await context.Curso.FindAsync(request.Id);
                if(curso == null)
                {
                    throw new System.Exception("No se pudo eliminar el curso");
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