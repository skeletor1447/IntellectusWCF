using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFIntellectus.Entidades;

namespace WCFIntellectus.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IUsuarioServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IUsuarioServices
    {
        [OperationContract]
        Utileria.UnicaRespuesta<Entidades.Usuario> ConsultarPorCorreoYPassword(String correo, String password);
        [OperationContract]
        Utileria.InsertarRespuesta Registrar(Usuario usuario);
        [OperationContract]
        Utileria.UnicaRespuesta<Entidades.Usuario> Consultar(long id);
        [OperationContract]
        Utileria.MultipleRespuesta<UsuarioAmistad> ConsultarPorBusqueda(long idCliente,String busqueda);
    }
}
