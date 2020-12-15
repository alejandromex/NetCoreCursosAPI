using MediatR;
using System;
using System.Threading;
using Dominio;
using System.Threading.Tasks;
using Persistencia;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo{get;set;}
            public string Descripcion{get;set;}
            public DateTime? FechaPublicacion{get;set;}
        }

        //Logica de validacion
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty().NotNull();
                RuleFor(x => x.FechaPublicacion).NotEmpty().NotNull();
            }
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
                var curso = new Curso();
                curso.Titulo = request.Titulo;
                curso.Descripcion = request.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion;

                context.Curso.Add(curso);
                var valor = await context.SaveChangesAsync();
                if(valor>0)
                {
                    return Unit.Value;
                }
                
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}