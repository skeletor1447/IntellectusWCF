﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFIntellectus.Entidades;
using WCFIntellectus.Utileria;

namespace WCFIntellectus.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "PerfilServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione PerfilServices.svc o PerfilServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class PerfilServices : IPerfilServices
    {
        Model.IntellectusdbEntities intellectusdbEntities = new Model.IntellectusdbEntities();
        public UnicaRespuesta<PerfilCompuesto> ConsultarPerfilCompuesto(long idCliente)
        {
            UnicaRespuesta<PerfilCompuesto> respuesta = new UnicaRespuesta<PerfilCompuesto>();


            respuesta.Errores = new Dictionary<string, string>();

            try
            {
                PerfilCompuesto perfilCompuesto = new PerfilCompuesto();

                WCFIntellectus.Model.tblusuario usuariotbl = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == idCliente).Single();
                Usuario usuario = new Usuario() { ID = usuariotbl.IdUsuario, Correo = usuariotbl.Correo, Nick = usuariotbl.Nick };


                WCFIntellectus.Model.tblperfil tblperfil = intellectusdbEntities.tblperfil.Where(x => x.IdUsuario == idCliente).Single();
                Perfil perfil = new Perfil() {IdPerfil = tblperfil.IdPerfil, IdUsuario = usuario.ID, NombreReal = tblperfil.NombreReal, Descripcion = tblperfil.Descripcion, Disponibilidad = tblperfil.Disponibilidad, FechaRegistro = tblperfil.FechaRegistro, Avatar = tblperfil.Avatar, Online = tblperfil.Online };

                perfilCompuesto.Usuario = usuario;
                perfilCompuesto.Perfil = perfil;

                respuesta.Error = false;
                respuesta.Entidad = perfilCompuesto;
            }
            catch (Exception ex)
            {
                respuesta.Error = true;
                respuesta.Errores.Add("", ex.Message);
            }
            return respuesta;
        }
    }
}
