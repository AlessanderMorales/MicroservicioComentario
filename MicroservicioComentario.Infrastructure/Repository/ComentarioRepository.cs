using Dapper;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Domain.Interfaces;
using MicroservicioComentario.Infrastructure.Persistence;

namespace MicroservicioComentario.Infrastructure.Repository
{
    public class ComentarioRepository : IRepository<Comentario>
    {
        private readonly MySqlConnectionSingleton _connection;

        public ComentarioRepository(MySqlConnectionSingleton connection)
        {
            _connection = connection;
        }

        public IEnumerable<Comentario> GetAll()
        {
            using var conn = _connection.CreateConnection();

            return conn.Query<Comentario>(
                @"SELECT 
                    id_comentario AS IdComentario,
                    contenido AS Contenido,
                    fecha,
                    estado,
                    id_tarea AS IdTarea,
                    id_usuario AS IdUsuario,
                    id_destinatario AS IdDestinatario
                  FROM Comentario
                  WHERE estado = 1
                  ORDER BY fecha DESC");
        }

        public Comentario GetById(int id)
        {
            using var conn = _connection.CreateConnection();

            return conn.QueryFirstOrDefault<Comentario>(
                @"SELECT 
                    id_comentario AS IdComentario,
                    contenido AS Contenido,
                    fecha,
                    estado,
                    id_tarea AS IdTarea,
                    id_usuario AS IdUsuario,
                    id_destinatario AS IdDestinatario
                  FROM Comentario
                  WHERE id_comentario = @Id;",
                new { Id = id });
        }

        public void Add(Comentario entity)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"INSERT INTO Comentario (contenido, id_tarea, id_usuario, id_destinatario, estado)
                  VALUES (@Contenido, @IdTarea, @IdUsuario, @IdDestinatario, @Estado)",
                  entity);
        }

        public void Update(Comentario entity)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"UPDATE Comentario
                  SET contenido = @Contenido, estado = @Estado
                  WHERE id_comentario = @IdComentario",
                  entity);
        }

        public void Delete(int id)
        {
            using var conn = _connection.CreateConnection();

            conn.Execute(
                @"UPDATE Comentario SET estado = 0 WHERE id_comentario = @Id;",
                new { Id = id });
        }
    }
}
