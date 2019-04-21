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
                Usuario usuario = new Usuario() { ID = usuariotbl.IdUsuario, Correo = usuariotbl.Correo, Nick = usuariotbl.Nick, Password = usuariotbl.Password };


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
        public ActualizarRespuesta<PerfilCompuesto> ActualizarPerfilCompuesto(PerfilCompuesto perfilCompuesto)
        {
            ActualizarRespuesta<PerfilCompuesto> actualizarRespuesta = new ActualizarRespuesta<PerfilCompuesto>();
            actualizarRespuesta.Errores = new Dictionary<string, string>();
            try
            {
                Perfil p = perfilCompuesto.Perfil;
                Usuario u = perfilCompuesto.Usuario;
                WCFIntellectus.Model.tblperfil tblperfil = new Model.tblperfil() { IdPerfil = (int)p.IdPerfil, Avatar = p.Avatar, Descripcion = p.Descripcion, Disponibilidad = p.Disponibilidad, FechaRegistro = p.FechaRegistro, IdUsuario = (int)p.IdUsuario, NombreReal = p.NombreReal, Online = p.Online };
                WCFIntellectus.Model.tblusuario tblusuario = new Model.tblusuario() { IdUsuario = (int)u.ID, Correo = u.Correo, Nick = u.Nick, Password = u.Password};

                using (var dbTransacciones = intellectusdbEntities.Database.BeginTransaction())
                {
                    try
                    {
                        var entidadPerfil = intellectusdbEntities.tblperfil.Find(tblperfil.IdPerfil);
                        var entidadUsuario = intellectusdbEntities.tblusuario.Find(tblusuario.IdUsuario);

                        intellectusdbEntities.Entry(entidadPerfil).CurrentValues.SetValues(tblperfil);
                        intellectusdbEntities.Entry(entidadUsuario).CurrentValues.SetValues(tblusuario);

                        intellectusdbEntities.SaveChanges();

                        dbTransacciones.Commit();


                        actualizarRespuesta.Error = false;
                        actualizarRespuesta.Entidad = perfilCompuesto;
                        actualizarRespuesta.Id = tblusuario.IdUsuario;

                    }
                    catch (Exception es)
                    {
                        dbTransacciones.Rollback();
                        actualizarRespuesta.Errores.Add("Error", es.Message);
                        actualizarRespuesta.Error = true;
                    }
                }
            }
            catch(Exception es)
            {
                actualizarRespuesta.Errores.Add("Error", es.Message);
                actualizarRespuesta.Error = true;
            }

            return actualizarRespuesta;
        }

        public MultipleRespuesta<PerfilCompuesto> ObtenerListaAmigosPerfilCompuesto(long idCliente)
        {
            MultipleRespuesta<PerfilCompuesto> multipleRespuesta = new MultipleRespuesta<PerfilCompuesto>();


            try
            {
                AmigosServices amigosServices = new AmigosServices();

                MultipleRespuesta<SolicitudAmistad> amigos = amigosServices.ConsultarAmigos((int)idCliente);

                List<PerfilCompuesto> listaAmigos = new List<PerfilCompuesto>();

                if (!amigos.Error)
                {
                    foreach (var amigo in amigos.Entidades)
                    {
                        if (amigo.IdSolicitante == idCliente)//obtener el id del amigo mediante el idsolicitado
                        {
                            Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == amigo.IdSolicitado).Single();
                            Usuario usuario = new Usuario() {  Correo = tblusuario.Correo, ID = tblusuario.IdUsuario, Nick = tblusuario.Nick, Password = tblusuario.Password};

                            Model.tblperfil tblperfil = intellectusdbEntities.tblperfil.Where(x => x.IdUsuario == amigo.IdSolicitado).Single();
                            Perfil perfil = new Perfil() {IdUsuario = tblperfil.IdUsuario, Avatar = tblperfil.Avatar, Descripcion = tblperfil.Descripcion, Disponibilidad = tblperfil.Disponibilidad, FechaRegistro = tblperfil.FechaRegistro, IdPerfil = tblperfil.IdPerfil, NombreReal = tblperfil.NombreReal, Online = tblperfil.Online };

                            listaAmigos.Add(new PerfilCompuesto() { Usuario = usuario, Perfil =  perfil});
                        }
                        else
                        {
                            Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == amigo.IdSolicitante).Single();
                            Usuario usuario = new Usuario() { Correo = tblusuario.Correo, ID = tblusuario.IdUsuario, Nick = tblusuario.Nick, Password = tblusuario.Password };

                            Model.tblperfil tblperfil = intellectusdbEntities.tblperfil.Where(x => x.IdUsuario == amigo.IdSolicitante).Single();
                            Perfil perfil = new Perfil() { IdUsuario = tblperfil.IdUsuario, Avatar = tblperfil.Avatar, Descripcion = tblperfil.Descripcion, Disponibilidad = tblperfil.Disponibilidad, FechaRegistro = tblperfil.FechaRegistro, IdPerfil = tblperfil.IdPerfil, NombreReal = tblperfil.NombreReal, Online = tblperfil.Online };

                            listaAmigos.Add(new PerfilCompuesto() { Usuario = usuario, Perfil = perfil });
                        }
                    }

                    multipleRespuesta.Entidades = listaAmigos;
                    multipleRespuesta.Error = false;
                }
                else
                {
                    multipleRespuesta.Error = true;
                    multipleRespuesta.Errores = new Dictionary<string, string>();
                    multipleRespuesta.Errores.Add("Error", amigos.Errores["Error"]);
                }
            }
            catch (Exception ex)
            {
                multipleRespuesta.Error = true;
                multipleRespuesta.Errores = new Dictionary<string, string>();
                multipleRespuesta.Errores.Add("Error", ex.Message);
            }


            return multipleRespuesta;
        }
    }
}
