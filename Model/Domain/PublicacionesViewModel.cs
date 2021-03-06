using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class PublicacionesViewModel
    {
        public List<Publicaciones> ListaPublicaciones { get; set; }
        public Publicaciones Publicacion { get; set; }
        public Usuario Usuario { get; set; }
        public Usuario UsuarioSesion { get; set; }
        public Comentarios Comentarios { get; set; }
        public List<Comentarios> ListaComentarios { get; set; }
    }
}
