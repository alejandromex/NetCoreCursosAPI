using MediatR;
using Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;

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
                var curso = await context.Curso.FindAsync(request.Id);
                return curso;
            }
        }
    }
}