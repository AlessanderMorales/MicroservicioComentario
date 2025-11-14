using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroservicioComentario.Domain.Entities;
using MicroservicioComentario.Domain.Interfaces;

namespace MicroservicioComentario.Application.Services
{
    public class ComentarioService
    {
        private readonly IRepository<Comentario> _repo;

        public ComentarioService(IRepository<Comentario> repo)
        {
            _repo = repo;
        }

        public IEnumerable<Comentario> GetAll() => _repo.GetAll();
        public Comentario GetById(int id) => _repo.GetById(id);
        public void Add(Comentario c) => _repo.Add(c);
        public void Update(Comentario c) => _repo.Update(c);
        public void Delete(int id) => _repo.Delete(id);
    }
}

