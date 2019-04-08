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
        public UnicaRespuesta<Usuario> ConsultarPorCorreoYPassword(string correo, string password)
        {
            UnicaRespuesta<Usuario> respuesta = new UnicaRespuesta<Usuario>();





            return respuesta;
        }
    }
}
