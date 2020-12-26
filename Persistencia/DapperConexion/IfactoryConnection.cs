using System.Data;

namespace Persistencia.DapperConexion
{
    public interface IfactoryConnection
    {
         void CloseConnection();
         IDbConnection GetDbConnection();
    }
}