using MediatR;
using System;
using System.Threading;
using Dominio;
using System.Threading.Tasks;
using Persistencia;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo{get;set;}
            public string Descripcion{get;set;}
            public decimal Precio{get;set;}
            public decimal Promocion{get;set;}
            public DateTime? FechaPublicacion{get;set;}
            public List<Guid> ListaInstructor {get;set;}

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
                Guid _cursoId = Guid.NewGuid();
                var curso = new Curso();
                curso.Titulo = request.Titulo;
                curso.Descripcion = request.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion;
                curso.CursoId = _cursoId;

                var precioEntidad = new Precio{
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };

                context.Precios.Add(precioEntidad);

                context.Cursos.Add(curso);

                if(request.ListaInstructor != null)
                {
                    CursoInstructor cursoInstructor = null;
                    foreach(var id in request.ListaInstructor)
                    {
                        cursoInstructor = new CursoInstructor{
                            CursoId = curso.CursoId,
                            InstructorId = id
                        };

                        context.CursoInstructors.Add(cursoInstructor);
                    }
                }

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