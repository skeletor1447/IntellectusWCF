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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "AmigosServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione AmigosServices.svc o AmigosServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class AmigosServices : IAmigosServices
    {
        Model.IntellectusdbEntities intellectusdbEntities = new Model.IntellectusdbEntities();

        public MultipleRespuesta<SolicitudAmistad> ConsultarSolicitudesEnviadas(int soliciante)
        {
            MultipleRespuesta<SolicitudAmistad> multipleRespuesta = new MultipleRespuesta<SolicitudAmistad>();

            try
            {
                List<SolicitudAmistad> solicitudAmigos = intellectusdbEntities.tblsolicitudamistad.Where(x => x.IdSolicitante == soliciante && x.Estado != "Aceptado").Select(x => new SolicitudAmistad() { IdSolicitudAmistad = x.IdSolicitudAmistad, IdSolicitante = x.IdSolicitante, IdSolicitado = x.IdSolicitado }).ToList();

                multipleRespuesta.Entidades = solicitudAmigos;
                multipleRespuesta.Error = false;
            }
            catch(Exception ex)
            {
                multipleRespuesta.Error = true;
                multipleRespuesta.Errores = new Dictionary<string, string>();
                multipleRespuesta.Errores.Add("Error", ex.Message);
            }


            return multipleRespuesta;
        }

        public MultipleRespuesta<SolicitudAmistad> ConsultarSolicitudesRecividas(int soliciante)
        {
            MultipleRespuesta<SolicitudAmistad> multipleRespuesta = new MultipleRespuesta<SolicitudAmistad>();

            try
            {
                List<SolicitudAmistad> solicitudAmigos = intellectusdbEntities.tblsolicitudamistad.Where(x => x.IdSolicitado == soliciante && x.Estado != "Aceptado").Select(x => new SolicitudAmistad() { IdSolicitudAmistad = x.IdSolicitudAmistad, IdSolicitante = x.IdSolicitante, IdSolicitado = x.IdSolicitado }).ToList();

                multipleRespuesta.Entidades = solicitudAmigos;
                multipleRespuesta.Error = false;
            }
            catch (Exception ex)
            {
                multipleRespuesta.Error = true;
                multipleRespuesta.Errores = new Dictionary<string, string>();
                multipleRespuesta.Errores.Add("Error", ex.Message);
            }

            return multipleRespuesta;
        }

        public InsertarRespuesta SolicitudDeAmistad(int solicitante, int solicitado)
        {
            InsertarRespuesta respuesta = new InsertarRespuesta();

            Model.tblsolicitudamistad tblsolicitudamistad = new Model.tblsolicitudamistad() { IdSolicitudAmistad = -1, IdSolicitante = solicitante, IdSolicitado = solicitado, Estado = "Pendiente"};

            try
            {
                intellectusdbEntities.tblsolicitudamistad.Add(tblsolicitudamistad);
                intellectusdbEntities.SaveChanges();
                respuesta.Error = false;
                respuesta.Id = tblsolicitudamistad.IdSolicitudAmistad;
            }
            catch(Exception ex)
            {
                respuesta.Error = true;
                respuesta.Errores = new Dictionary<string, string>();
                respuesta.Errores.Add("Error", ex.Message);
            }

            
            return respuesta;
        }
    }
}
