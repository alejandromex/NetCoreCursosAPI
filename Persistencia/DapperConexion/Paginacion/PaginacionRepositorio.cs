using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Dapper;
using System.Data;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IfactoryConnection _factoryConnection;

        public PaginacionRepositorio(IfactoryConnection factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }


        public async Task<PaginacionModel> devolverPaginacion(string storeProcedure,
        int numeroPagina, int cantidadElementos, 
        IDictionary<string, object> parametrosFiltro, 
        string ordenamientoColumna)
        {
            PaginacionModel paginacion = new PaginacionModel();
            List<IDictionary<string,object>> listaReporte = null;
            int totalRecords = 0;
            int totalPaginas = 0;
            try{
                var connection = _factoryConnection.GetDbConnection();
                DynamicParameters parametros = new DynamicParameters();

                foreach(var param in parametrosFiltro)
                {
                    parametros.Add("@"+param.Key, param.Value);
                }

                parametros.Add("@NumeroPagina", numeroPagina);
                parametros.Add("@CantidadElementos", cantidadElementos);
                parametros.Add("@Ordenamiento", ordenamientoColumna);
                
                parametros.Add("@TotalRecords",totalRecords, DbType.Int32, ParameterDirection.Output);
                parametros.Add("@TotalPaginas",totalPaginas, DbType.Int32, ParameterDirection.Output);

                var resultados = await connection.QueryAsync(storeProcedure, parametros, commandType: CommandType.StoredProcedure);
                listaReporte = resultados.Select(x => (IDictionary<string,object>)x).ToList();
                paginacion.ListaRecords = listaReporte;
                paginacion.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                paginacion.TotalRecords = parametros.Get<int>("@TotalRecords");
                
            }   
            catch(Exception ex)
            {
                throw new Exception("Error al ejecutar paginacion: ",ex);
            } 
            finally{
                _factoryConnection.CloseConnection();
            }   
            return paginacion;
        }
    }
}