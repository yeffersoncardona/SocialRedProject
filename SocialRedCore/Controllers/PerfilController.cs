using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Domain;
using Service;
using System;
using System.IO;

namespace EchonyCore.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPublicacionesService _publiService;
        private readonly IComentariosService _comentService;
        private readonly IFotoService _fotoService;


        public PerfilController
            (IUsuarioService usuarioService, IPublicacionesService publiService,
            IComentariosService comentariosService, IFotoService fotoService)
        {
            _usuarioService = usuarioService;
            _publiService = publiService;
            _comentService = comentariosService;
            _fotoService = fotoService;

        }

        // GET: Perfil
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            int idSesion = (int)HttpContext.Session.GetInt32("id");
            UsuarioViewModel us = new UsuarioViewModel();
            us.UsuarioSesion = _usuarioService.GetUsuarioById(new Usuario { Id = idSesion });
            us.UsuarioSecundario = _usuarioService.GetUsuarioById(new Usuario { Id = idSesion });
            us.publicaciones = _publiService.GetPublicacionesPrueba(new Usuario { Id = idSesion });

            return View("Perfil", us);
        }

        [HttpGet]
        public IActionResult Usuario(Usuario user)
        {
            UsuarioViewModel model = new UsuarioViewModel();


            int idSesion = (int)HttpContext.Session.GetInt32("id");
            // string nickName = HttpContext.Session.GetString("nick");

            model.UsuarioSesion = _usuarioService.GetUsuarioById(new Usuario { Id = idSesion });
            model.UsuarioSecundario = _usuarioService.GetUsuario(user);
            model.publicaciones = _publiService.GetPublicacionesPrueba(user);



            if (model.UsuarioSecundario.Id.Equals(model.UsuarioSesion.Id))
            {
                return View("Perfil", model);
            }
            else
            {
                return View("PerfilSecundario", model);
            }
        }

        [HttpPost]
        public IActionResult AddPublicacion(Publicaciones p)
        {
            if (p.UsuarioId == 0)
            {
                return Json(false);
            }
            else
            {
                _publiService.AddPublicacion(p);
                return Json("Publicacion agregada");
            }

        }

        public JsonResult AddComment(Comentarios c)
        {
            c.Fecha_Publicacion = DateTime.Now;
            if (c.Contenido_comentario != null)
            {
                bool exito = _comentService.AddComentario(c);

                return Json(exito);
            }

            return Json(false);


        }
        [HttpPost]
        public JsonResult AddFotoPublicacion(IFormFile foto, int id)
        {
            Usuario u = new Usuario();
            u = _usuarioService.GetUsuarioById(new Usuario { Id = id });
            if (!ModelState.IsValid)
            {
                return Json(false);
                // return Redirect(Url.Action("Usuario", "Perfil", new Usuario { NickName = u.NickName, Id = u.Id}));
            }
            //\wwwroot\images\Fotos_Usuarios\20180321212835.png
            var ruta = "wwwroot\\images\\Fotos_Usuarios\\" + foto.FileName;
            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                Usuario user = new Usuario();
                user.Id = id;
                user.Avatar = foto.FileName;

                foto.CopyTo(stream);
                _fotoService.AddFotoPerfil(user);
                //_fotoService.AddFotoPerfil(new Foto { Id = foto_id, RutaFoto = foto.FileName});
                //_fotoService.AddFotoPerfil(new Foto { Id = foto_id, Img = foto , RutaFoto = foto.FileName});
            }
            return Json(foto.FileName);
            //return Redirect(Url.Action("Usuario", "Perfil", new Usuario { NickName = u.NickName, Id = u.Id }));
        }
        [HttpPost]
        public IActionResult AgregarPublicacion([FromBody] UsuarioViewModel model, [FromBody] IFormFile foto, [FromBody] Publicaciones publi)
        {
            string photoName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpeg";
            DateTime fecha = DateTime.Now;
            bool exito = false;
            model.Publicacion.Fecha = fecha;
            var ruta = "";
            if (foto != null)
            {
                ruta = $"wwwroot/images/{photoName}";
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    foto.CopyTo(stream);
                    string rutaBd = $"/images/{photoName}";
                    model.Publicacion.Foto = rutaBd;
                    exito = _publiService.AgregarPublicacion(model.Publicacion);
                }
            }
            else if (model.Publicacion.Contenido != null)
            {
                exito = _publiService.AgregarPublicacion(model.Publicacion);
            }
            return Ok(exito);
        }

        [HttpPost]
        public IActionResult AddPublication([FromBody] UsuarioViewModel model, [FromBody] IFormFile foto, [FromBody] Publicaciones publi)
        {
            string photoName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpeg";
            DateTime fecha = DateTime.Now;
            bool exito = false;
            model.Publicacion.Fecha = fecha;
            var ruta = "";
            if (foto != null)
            {
                ruta = $"wwwroot/images/{photoName}";
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    foto.CopyTo(stream);
                    string rutaBd = $"/images/{photoName}";
                    model.Publicacion.Foto = rutaBd;
                    exito = _publiService.AgregarPublicacion(model.Publicacion);
                }
            }
            else if (model.Publicacion.Contenido != null)
            {
                exito = _publiService.AgregarPublicacion(model.Publicacion);
            }
            return Ok(exito);
        }

        public PartialViewResult Publicaciones(int userId)
        {

            int idSesion = (int)HttpContext.Session.GetInt32("id");
            PublicacionesViewModel model = new PublicacionesViewModel();
            model.ListaPublicaciones = _publiService.GetPublicaciones(userId); ;
            model.UsuarioSesion = _usuarioService.GetUsuarioById(new Usuario { Id = idSesion });
            model.Usuario = _usuarioService.GetUsuarioById(new Usuario { Id = userId });
            model.ListaComentarios = _comentService.GetAllComentarios();
            return PartialView(model);
        }

        public IActionResult GetAllPosts()
        {
            var posts = _publiService.GetPublicaciones();
            return Ok(posts);
        }

        

    }



}