using Model;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IFotoService
    {
        string AddFotoPerfil(Usuario user);
    }

    public class FotoService : IFotoService
    {

        private readonly SocialRedDbContext _context;

        public FotoService(SocialRedDbContext context)
        {
            _context = context;
        }
        public string AddFotoPerfil(Usuario user)
        {
            string mensaje = "La foto no fue actualizada";

            try
            {
                Usuario details = _context.Usuario.Find(user.Id);
                details.Avatar = user.Avatar;
                


                _context.Usuario.Update(details);
                _context.SaveChanges();


                mensaje = "La foto fue actualizada";
            }
            catch (Exception)
            {

                throw;
            }



            return mensaje;
        }
    }
}
