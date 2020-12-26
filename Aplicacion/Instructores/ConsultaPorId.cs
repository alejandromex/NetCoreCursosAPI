using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class ConsultaPorId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid InstructorId{get;set;}
        }

        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor _instructorRepositorio;
            public Manejador(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }
            public Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = _instructorRepositorio.ObtenerPorId(request.InstructorId);
                if(instructor != null)
                {
                    return instructor;
                }
                throw new Exception("Error al consultar el instructor");
            }
        }

    }
}