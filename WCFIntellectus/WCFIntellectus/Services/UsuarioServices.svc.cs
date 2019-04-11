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
                WCFIntellectus.Model.tblusuario tblusuario = new Model.tblusuario() { Correo = usuario.Correo, IdUsuario = -1, Nick = usuario.Nick, Password = usuario.Password };

                intellectusdbEntities.tblusuario.Add(tblusuario);
                intellectusdbEntities.SaveChanges();

                insertarRespuesta.Id = tblusuario.IdUsuario;

                insertarRespuesta.Error = false;
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

        public MultipleRespuesta<Usuario> ConsultarPorBusqueda(string busqueda)
        {
            MultipleRespuesta<Usuario> respuesta = new MultipleRespuesta<Usuario>();

            try
            {
                List<Usuario> usuarios = intellectusdbEntities.tblusuario.Where(x => x.Nick.Contains(busqueda)).Select(x => new Usuario() { ID = x.IdUsuario, Nick = x.Nick, Correo = x.Correo, Password = x.Password }).ToList();
                respuesta.Entidades = usuarios;
                respuesta.Error = false;
            }
            catch(Exception ex)
            {
                respuesta.Entidades = null;
                respuesta.Error = true;
                respuesta.Errores.Add("Error",ex.Message);
            }
            

            return respuesta;
        }
    }
}
