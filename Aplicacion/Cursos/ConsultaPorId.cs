using MediatR;
using Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Aplicacion.ManejadorError;

namespace Aplicacion.Cursos
{
    public class ConsultaPorId
    {
        public class CursoUnico : IRequest<Curso>
        {
            public int Id{get;set;}
        }   

        public class Manejador : IRequestHandler<CursoUnico, Curso>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext context)
            {
                this.context = context;
            }

            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Cursos.FindAsync(request.Id);
                if(curso == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new {curso = "No se encontro el curso"});
                }
                return curso;
            }
        }
    }
}