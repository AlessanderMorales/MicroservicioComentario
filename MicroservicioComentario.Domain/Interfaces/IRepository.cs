using MicroservicioComentario.Domain.Entities;
using System.Collections.Generic;

namespace MicroservicioComentario.Domain.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        IEnumerable<Comentario> GetByTarea(int idTarea);
        IEnumerable<Comentario> GetByDestinatario(int idUsuario);
        IEnumerable<T1> Query<T1>(string query, object parameters);
    }
}
