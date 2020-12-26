using MediatR;
using Persistencia;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta: IRequest{
            
            public string Alumno{get;set;}
            public int Puntaje{get;set;}
            public string ComentarioTexto{get;set;}
            public Guid CursoId{get;set;}

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.ComentarioTexto).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                this._context = context;
            }
            
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Comentario comentario = new Comentario();
                comentario.Alumno = request.Alumno;
                comentario.ComentarioTexto = request.ComentarioTexto;
                comentario.Puntaje = request.Puntaje;
                comentario.CursoId = request.CursoId;
                comentario.ComentarioId = Guid.NewGuid();

                _context.Comentarios.Add(comentario);

                var resultados = await _context.SaveChangesAsync();
                if(resultados > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo agregar el comentario");
            }
        }
    }
}