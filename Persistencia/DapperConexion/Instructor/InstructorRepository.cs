using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepository : IInstructor
    {
        private readonly IfactoryConnection factoryConnection;

        public InstructorRepository(IfactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }
        public async Task<int> Actualiza(InstructorModel parametros)
        {
            var storeProcedure = "usp_instructor_editar";
            try{
                var connection = factoryConnection.GetDbConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure,
                                        new {
                                            InstructorId = parametros.InstructorId,
                                            Nombre = parametros.Nombre,
                                            Apellidos = parametros.Apellidos,
                                            Grado = parametros.Grado
                                        },
                                        commandType: CommandType.StoredProcedure
                                        );
                factoryConnection.CloseConnection();
                return resultado;

            }
            catch(Exception e)
            {
                throw new Exception("Error al editar: ", e);
            }
        }

        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_instructor_eliminar";
            try{
                var connection = factoryConnection.GetDbConnection();
                var resultados = await connection.ExecuteAsync(storeProcedure,
                new{
                    InstructorId = id
                },
                  commandType: CommandType.StoredProcedure  
                );

                factoryConnection.CloseConnection();
                return resultados;

            }
            catch(Exception e)
            {
                throw new Exception("Error al tratar de eliminar el instructor: ",e);
            }
        }

        public async Task<int> Nuevo(InstructorModel parametros)
        {
            var storeProcedure = "usp_instructor_nuevo";
            try
            {
                var connection = factoryConnection.GetDbConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure,
                        new
                        {
                            InstructorId = Guid.NewGuid(),
                            Nombre = parametros.Nombre,
                            Apellidos = parametros.Apellidos,
                            Grado = parametros.Grado
                        },
                        commandType: CommandType.StoredProcedure);
                factoryConnection.CloseConnection();
                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo ingresar el nuevo instructor: ", ex);
            }

        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";
            try
            {
                var connection = factoryConnection.GetDbConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la consulta de datos: ", ex);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_instructor_obtenerid";
            try{

                var connection = factoryConnection.GetDbConnection();
                InstructorModel instructor = null;
                instructor = await connection.QueryFirstAsync<InstructorModel>(storeProcedure,
                    new {
                        InstructorId = id
                    },
                    commandType: CommandType.StoredProcedure
                 );

                factoryConnection.CloseConnection();
                 return instructor;   


            }catch(Exception e)
            {
                throw new Exception("error al consultar el instructor: ", e);
            }
        }
    }
}