using Dapper;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Domain.Interfaces;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace MicroservicioComentario.Infrastructure.Repository
{
    public class ComentarioRepository : IRepository<Comentario>
    {
        private readonly MySqlConnection _connection;

        public ComentarioRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Comentario> GetAll()
        {
            string sql = @"
                SELECT 
                    id_comentario AS IdComentario,
                    contenido AS Contenido,
                    fecha AS Fecha,
                    estado AS Estado,
                    id_tarea AS IdTarea,
                    id_usuario AS IdUsuario,
                    id_destinatario AS IdDestinatario
                FROM Comentario
                WHERE estado = 1
                ORDER BY fecha DESC";

            return _connection.Query<Comentario>(sql);
        }

        public Comentario GetById(int id)
        {
            string sql = @"
                SELECT 
                    id_comentario AS IdComentario,
                    contenido AS Contenido,
                    fecha AS Fecha,
                    estado AS Estado,
                    id_tarea AS IdTarea,
                    id_usuario AS IdUsuario,
                    id_destinatario AS IdDestinatario
                FROM Comentario
                WHERE id_comentario = @Id";

            return _connection.QueryFirstOrDefault<Comentario>(sql, new { Id = id });
        }

        public void Add(Comentario c)
        {
            string sql = @"
                INSERT INTO Comentario
                (contenido, id_tarea, id_usuario, id_destinatario)
                VALUES (@Contenido, @IdTarea, @IdUsuario, @IdDestinatario)";

            _connection.Execute(sql, c);
        }

        public void Update(Comentario c)
        {
            string sql = @"
                UPDATE Comentario
                SET contenido = @Contenido,
                    id_destinatario = @IdDestinatario
                WHERE id_comentario = @IdComentario";

            _connection.Execute(sql, c);
        }

        public void Delete(int id)
        {
            _connection.Execute(
                "UPDATE Comentario SET estado = 0 WHERE id_comentario = @Id",
                new { Id = id });
        }

        public IEnumerable<Comentario> GetByTarea(int idTarea)
        {
            return _connection.Query<Comentario>(
                "CALL sp_comentarios_por_tarea(@IdTarea)",
                new { IdTarea = idTarea });
        }

        public IEnumerable<Comentario> GetByDestinatario(int idUsuario)
        {
            return _connection.Query<Comentario>(
                "CALL sp_comentarios_para_usuario(@IdUsuario)",
                new { IdUsuario = idUsuario });
        }

        public IEnumerable<T1> Query<T1>(string sql, object parameters)
        {
            return _connection.Query<T1>(sql, parameters);
        }
    }
}
