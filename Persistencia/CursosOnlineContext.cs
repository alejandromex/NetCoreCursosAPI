using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Dominio;
using System;

namespace Persistencia
{
    public class CursosOnlineContext : DbContext
    {
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.InstructorId, ci.CursoId});
        }

        public DbSet<Comentario> Comentarios{get;set;}
        public DbSet<Curso> Curso{get;set;}
        public DbSet<CursoInstructor> CursoInstructor{get;set;}
        public DbSet<Instructor> Instructores{get;set;}
        public DbSet<Precio> Precios{get;set;}

    }
}