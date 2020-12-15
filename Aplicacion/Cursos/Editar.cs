using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Persistencia;
using FluentValidation;
using Aplicacion.ManejadorError;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public int CursoId{get;set;}
            public string Titulo{get;set;}
            public string Descripcion{get;set;}
            public DateTime? FechaPublicacion{get;set;}
        }
        
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty().NotNull();
                RuleFor(x => x.FechaPublicacion).NotEmpty().NotNull();
            }
        }

        public  class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context; 
            public Manejador(CursosOnlineContext context)
            {
                this.context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await context.Cursos.FindAsync(request.CursoId);
                if(curso == null)
                {
                    throw new ManejadorExcepcion(System.Net.HttpStatusCode.NotFound, new {curso = "No se encontro el curso a editar"});
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                
                var valor = await context.SaveChangesAsync();
                if(valor>0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el curso");

            }
        }
    }
}