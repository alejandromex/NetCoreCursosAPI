using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Persistencia;
using FluentValidation;
using Aplicacion.ManejadorError;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId{get;set;}
            public string Titulo{get;set;}
            public string Descripcion{get;set;}
            public DateTime? FechaPublicacion{get;set;}
            public List<Guid> ListaInstructor{get;set;}
            public decimal? Precio{get;set;}
            public decimal? Promocion {get;set;}
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


                // Actualizar precio de curso

                var precioEntidad = context.Precios.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad != null)
                {
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else{
                    precioEntidad = new Precio{
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId,
                    };

                    await context.Precios.AddAsync(precioEntidad);
                }

                // Fin

                
                //Validar si llegan instructores y actualizarlos
                if(request.ListaInstructor != null)
                {
                    if(request.ListaInstructor.Count > 0)
                    {
                        // Eliminamos los instructores actuales del curso
                        var InstructoresDB = context.CursoInstructors.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructorEliminar in InstructoresDB)
                        {
                            context.CursoInstructors.Remove(instructorEliminar);
                        }
                        // FIN ///////

                        
                        foreach(var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor{
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            context.CursoInstructors.Add(nuevoInstructor);
                        }
                    }
                }

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