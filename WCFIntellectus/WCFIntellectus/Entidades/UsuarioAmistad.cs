using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Entidades
{
    [DataContract]
    public class UsuarioAmistad
    {
        [DataMember]
        public Usuario Usuario { get; set; }
        [DataMember]
        public SolicitudAmistad SolicitudAmistad { get; set; }
        [DataMember]
        public bool EsSolicitante { get; set; }
    }
}