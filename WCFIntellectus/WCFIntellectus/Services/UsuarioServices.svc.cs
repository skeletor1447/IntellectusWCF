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

        public InsertarRespuesta Registrar(Usuario usuario)
        {
            InsertarRespuesta insertarRespuesta = new InsertarRespuesta();

            try
            {

                using (var transaccion = intellectusdbEntities.Database.BeginTransaction())
                {
                    WCFIntellectus.Model.tblusuario tblusuario = new Model.tblusuario() { Correo = usuario.Correo, IdUsuario = -1, Nick = usuario.Nick, Password = usuario.Password };

                    try
                    {

                        intellectusdbEntities.tblusuario.Add(tblusuario);
                        intellectusdbEntities.SaveChanges();

                        WCFIntellectus.Model.tblperfil tblperfil = new Model.tblperfil() { IdPerfil = -1, IdUsuario = tblusuario.IdUsuario, Online = false, FechaRegistro = DateTime.Now};

                        intellectusdbEntities.tblperfil.Add(tblperfil);
                        intellectusdbEntities.SaveChanges();

                        insertarRespuesta.Id = tblusuario.IdUsuario;

                        insertarRespuesta.Error = false;

                        transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        insertarRespuesta.Id = -1;
                        insertarRespuesta.Error = true;
                        insertarRespuesta.Errores = new Dictionary<string, string>();
                        insertarRespuesta.Errores.Add("Error", ex.Message);

                        transaccion.Rollback();
                    }
                }
                
            }
            catch(Exception ex)
            {
                insertarRespuesta.Id = -1;
                insertarRespuesta.Error = true;
                insertarRespuesta.Errores = new Dictionary<string, string>();
                insertarRespuesta.Errores.Add("Error", ex.Message);
            }

            return insertarRespuesta;
        }
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

        public UnicaRespuesta<Usuario> Consultar(long id)
        {
            UnicaRespuesta<Usuario> respuesta = new UnicaRespuesta<Usuario>();
            respuesta.Errores = new Dictionary<string, string>();

            try
            {
                WCFIntellectus.Model.tblusuario usuariotbl = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == (int)id).Single();
                Usuario usuario = new Usuario() { ID = usuariotbl.IdUsuario, Correo = usuariotbl.Correo, Nick = usuariotbl.Nick };

                respuesta.Error = false;
                respuesta.Entidad = usuario;
            }
            catch(Exception ex)
            {
                respuesta.Error = true;
                respuesta.Errores.Add("",ex.Message);
            }
            return respuesta;
        }

        public MultipleRespuesta<UsuarioAmistad> ConsultarPorBusqueda(long idClient,string busqueda)
        {
            MultipleRespuesta<UsuarioAmistad> respuesta = new MultipleRespuesta<UsuarioAmistad>();
            AmigosServices amigosServices = new AmigosServices();
            try
            {
                List<Usuario> usuarios = intellectusdbEntities.tblusuario.Where(x => x.Nick.Contains(busqueda)).Select(x => new Usuario() { ID = x.IdUsuario, Nick = x.Nick, Correo = x.Correo, Password = x.Password }).ToList();
                List<SolicitudAmistad> solicitudAmistadesRecibidas = amigosServices.ConsultarSolicitudesRecibidas((int)idClient).Entidades;
                List<SolicitudAmistad> solicitudAmistadesEnviadas = amigosServices.ConsultarSolicitudesEnviadas((int)idClient).Entidades;

                List<UsuarioAmistad> listaUsuariosAmistad = usuarios.Select(x => new UsuarioAmistad() { Usuario = x, SolicitudAmistad = null}).ToList();

                List<SolicitudAmistad> listaMatchEnvidas = solicitudAmistadesEnviadas.Where(y => listaUsuariosAmistad.Exists(x => y.IdSolicitado == x.Usuario.ID)).ToList();
                List<SolicitudAmistad> listaMatchRecibidas = solicitudAmistadesRecibidas.Where(y => listaUsuariosAmistad.Exists(x => y.IdSolicitante == x.Usuario.ID)).ToList();


                foreach (var usuario in listaUsuariosAmistad)
                {
                    usuario.EsSolicitante = null;

                    foreach(var enviado in listaMatchEnvidas)
                    {
                        
                        if(usuario.Usuario.ID == enviado.IdSolicitado)
                        {
                            usuario.SolicitudAmistad = enviado;
                            listaMatchEnvidas.Remove(enviado);
                            usuario.EsSolicitante = true;
                            break;
                        }
                    }

                    foreach (var recibido in listaMatchRecibidas)
                    {

                        if (usuario.Usuario.ID == recibido.IdSolicitante)
                        {
                            usuario.SolicitudAmistad = recibido;
                            listaMatchRecibidas.Remove(recibido);
                            usuario.EsSolicitante = false;
                            break;
                        }

                    }
                }

                respuesta.Entidades = listaUsuariosAmistad;
                respuesta.Error = false;
            }
            catch(Exception ex)
            {
                respuesta.Entidades = null;
                respuesta.Error = true;
                respuesta.Errores = new Dictionary<string, string>();
                respuesta.Errores.Add("Error",ex.Message);
            }
            


            return respuesta;
        }

        public MultipleRespuesta<UsuarioAmistad> ConsultarSolicitudesPorCliente(int idcliente)
        {
            MultipleRespuesta<UsuarioAmistad> respuesta = new MultipleRespuesta<UsuarioAmistad>();
            AmigosServices amigosServices = new AmigosServices();
            try
            {
                
                List<SolicitudAmistad> solicitudAmistadesRecibidas = amigosServices.ConsultarSolicitudesRecibidas(idcliente).Entidades;
                List<SolicitudAmistad> solicitudAmistadesEnviadas = amigosServices.ConsultarSolicitudesEnviadas(idcliente).Entidades;


                List<UsuarioAmistad> usuarioAmistadLista = new List<UsuarioAmistad>();
                
                foreach(var recibida in solicitudAmistadesRecibidas)
                {
                    WCFIntellectus.Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == recibida.IdSolicitante).Single();
                    usuarioAmistadLista.Add(new UsuarioAmistad() {  SolicitudAmistad = recibida, EsSolicitante = false, Usuario = new Usuario() {  ID = tblusuario.IdUsuario, Correo = tblusuario.Correo, Nick = tblusuario.Nick, Password = tblusuario.Password} });
                }

                foreach (var enviada in solicitudAmistadesEnviadas)
                {
                    WCFIntellectus.Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == enviada.IdSolicitado).Single();
                    usuarioAmistadLista.Add(new UsuarioAmistad() { SolicitudAmistad = enviada, EsSolicitante = true, Usuario = new Usuario() { ID = tblusuario.IdUsuario, Correo = tblusuario.Correo, Nick = tblusuario.Nick, Password = tblusuario.Password } });
                }

                respuesta.Entidades = usuarioAmistadLista;
                respuesta.Error = false;
            }
            catch (Exception ex)
            {
                respuesta.Entidades = null;
                respuesta.Error = true;
                respuesta.Errores = new Dictionary<string, string>();
                respuesta.Errores.Add("Error", ex.Message);
            }



            return respuesta;
        }

        public MultipleRespuesta<UsuarioAmistad> ConsultarAmigos(int idcliente)
        {
            MultipleRespuesta<UsuarioAmistad> multipleRespuesta = new MultipleRespuesta<UsuarioAmistad>();


            try
            {
                AmigosServices amigosServices = new AmigosServices();

                MultipleRespuesta<SolicitudAmistad> amigos = amigosServices.ConsultarAmigos(idcliente);

                List<UsuarioAmistad> listaAmigos = new List<UsuarioAmistad>();

                if(!amigos.Error)
                {
                    foreach(var amigo in amigos.Entidades)
                    {
                        if(amigo.IdSolicitante == idcliente)
                        {
                            Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == amigo.IdSolicitado).Single();
                            listaAmigos.Add(new UsuarioAmistad() { Usuario = new Usuario() { ID = tblusuario.IdUsuario, Correo = tblusuario.Correo, Nick = tblusuario.Nick, Password = tblusuario.Password }, EsSolicitante = true, SolicitudAmistad = amigo });
                        }
                        else
                        {
                            Model.tblusuario tblusuario = intellectusdbEntities.tblusuario.Where(x => x.IdUsuario == amigo.IdSolicitante).Single();
                            listaAmigos.Add(new UsuarioAmistad() { Usuario = new Usuario() { ID = tblusuario.IdUsuario, Correo = tblusuario.Correo, Nick = tblusuario.Nick, Password = tblusuario.Password }, EsSolicitante = false, SolicitudAmistad = amigo });
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
            catch(Exception ex)
            {
                multipleRespuesta.Error = true;
                multipleRespuesta.Errores = new Dictionary<string, string>();
                multipleRespuesta.Errores.Add("Error", ex.Message);
            }


            return multipleRespuesta;
        }
    }
}
