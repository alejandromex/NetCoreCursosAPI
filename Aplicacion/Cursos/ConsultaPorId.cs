using MediatR;
using Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Aplicacion.ManejadorError;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class ConsultaPorId
    {
        public class CursoUnico : IRequest<CursoDTO>
        {
            public Guid Id{get;set;}
        }   

        public class Manejador : IRequestHandler<CursoUnico, CursoDTO>
        {
            private readonly CursosOnlineContext context;
            private readonly IMapper mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<CursoDTO> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context.Cursos
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x=>x.InstructoresLink)
                .ThenInclude(y => y.Instructor).FirstOrDefaultAsync(a => a.CursoId == request.Id);
                if(curso == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new {curso = "No se encontro el curso"});
                }

                var cursoDto = mapper.Map<Curso, CursoDTO>(curso);

                return cursoDto;
            }
        }
    }
}