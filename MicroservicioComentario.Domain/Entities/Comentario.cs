using System;

namespace MicroservicioComentario.Domain.Entities
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public string Contenido { get; set; } = "";
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public int IdTarea { get; set; }
        public int IdUsuario { get; set; }
        public int? IdDestinatario { get; set; }
    }
}
