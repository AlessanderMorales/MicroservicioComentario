using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroservicioComentario.Domain.Entities
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public string Contenido { get; set; } = "";
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public int IdTarea { get; set; }
        public int IdUsuario { get; set; }  // Autor
        public int? IdDestinatario { get; set; }
    }
}
