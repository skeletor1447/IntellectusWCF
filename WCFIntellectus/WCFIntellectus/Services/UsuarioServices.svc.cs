using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFIntellectus.Entidades;
using WCFIntellectus.Utileria;

namespace WCFIntellectus.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "UsuarioServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione UsuarioServices.svc o UsuarioServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class UsuarioServices : IUsuarioServices
    {
        WCFIntellectus.Model.IntellectusdbEntities intellectusdbEntities = new Model.IntellectusdbEntities();
        public UnicaRespuesta<Usuario> ConsultarPorCorreoYPassword(string correo, string password)
        {
            UnicaRespuesta<Usuario> respuesta = new UnicaRespuesta<Usuario>();
            respuesta.Errores = new Dictionary<string, string>();
            if(String.IsNullOrEmpty(correo) || String.IsNullOrEmpty(correo.Trim()))
            {
                respuesta.Error = true;
                respuesta.Errores.Add("Correo", "El campo correo está vacio");
            }

            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
            {
                respuesta.Error = true;
                respuesta.Errores.Add("Password", "El campo password está vacio");
            }


            try
            {
                WCFIntellectus.Model.tblusuario usuariotbl = intellectusdbEntities.tblusuario.Where(x => x.Correo == correo && x.Password == password).Single();
                Usuario usuario = new Usuario() { ID = usuariotbl.IdUsuario, Correo = usuariotbl.Correo, Nick = usuariotbl.Nick};

                respuesta.Error = false;
                respuesta.Entidad = usuario;
            }
            catch
            {
                respuesta.Error = true;
                respuesta.Errores.Add("CampoErroneo", "Correo o password incorrecto.");
            }
            

            return respuesta;
        }
    }
}
