using MediatR;
using System.Collections.Generic;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<Curso>>
        {
            
        }

        public class Manejador : IRequestHandler<ListaCursos, List<Curso>>
        {
            private readonly CursosOnlineContext context;
            public Manejador(CursosOnlineContext context)
            {
                this.context = context;
            }
            
            public async Task<List<Curso>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await this.context.Cursos.ToListAsync();
                return cursos;
            }
        }

    }
}