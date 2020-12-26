using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        
        public class Ejecuta : IRequest
        {
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

            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                InstructorModel instructor = new InstructorModel();
                instructor.Nombre = request.Nombre;
                instructor.Apellidos = request.Apellidos;
                instructor.Grado = request.Grado;
                var resultado = await _instructorRepository.Nuevo(instructor);
                if(resultado > 0)
                {
                    return Unit.Value;
                }
                
                throw new Exception("Error al instertar el instructor");
            }
        }
    }
}