using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MicroservicioComentario.Infrastructure.Persistence
{
    public class MySqlConnectionSingleton
    {
        private readonly string _connectionString;

        public MySqlConnectionSingleton(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ComentariosConnection");
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
