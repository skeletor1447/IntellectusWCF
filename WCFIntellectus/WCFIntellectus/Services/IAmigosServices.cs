using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFIntellectus.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IAmigosServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IAmigosServices
    {
        [OperationContract]
        Utileria.InsertarRespuesta SolicitudDeAmistad(int solicitante,int solicitado);
        [OperationContract]
        Utileria.MultipleRespuesta<Entidades.SolicitudAmistad> ConsultarSolicitudesEnviadas(int soliciante);
        [OperationContract]
        Utileria.MultipleRespuesta<Entidades.SolicitudAmistad> ConsultarSolicitudesRecibidas(int soliciante);
        [OperationContract]
        Utileria.EliminarRespuesta<Entidades.SolicitudAmistad> EliminarSolicitud(int idsolicitudamistad);
    }
}
