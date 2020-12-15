namespace Dominio
{
    public class Precio
    {
        public int PrecioId{get;set;}
        public float PrecioActual{get;set;}
        public float Promocion{get;set;}
        public int CursoId{get;set;}
        public Curso Curso{get;set;}
    }
}