using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId{get;set;}
            public string Nombre{get;set;}
            public string Apellidos{get;set;}
            public string Grado{get;set;}
        }

        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Grado).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                InstructorModel instructor = new InstructorModel();
                instructor.Nombre = request.Nombre;
                instructor.Apellidos = request.Apellidos;
                instructor.Grado = request.Grado;
                instructor.InstructorId = request.InstructorId;
                var resultado = await _instructorRepositorio.Actualiza(instructor);
                if(resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo editar el usuario");
            }
        }
    }
}