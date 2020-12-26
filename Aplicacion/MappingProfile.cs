using AutoMapper;
using Dominio;
using Aplicacion.Cursos;
using System.Linq;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDTO>()
            .ForMember(x => x.Instructores, y => y.MapFrom( z => z.InstructoresLink.Select(a => a.Instructor).ToList()))
            .ForMember(x => x.Comentarios, y => y.MapFrom(  z => z.ComentarioLista))
            .ForMember(x => x.Precio, y => y.MapFrom(   z => z.PrecioPromocion));
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();


        }
    }
}