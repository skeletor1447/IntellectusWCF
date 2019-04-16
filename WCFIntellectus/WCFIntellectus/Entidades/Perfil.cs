using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Entidades
{
    [DataContract]
    public class Perfil
    {
        [DataMember]
        public long IdPerfil { get; set; }
        [DataMember]
        public long IdUsuario { get; set; }
        [DataMember]
        public String NombreReal { get; set; }
        [DataMember]
        public String Descripcion { get; set; }
        [DataMember]
        public String Disponibilidad { get; set; }
        [DataMember]
        public DateTime FechaRegistro { get; set; }
        [DataMember]
        public byte[] Avatar { get; set; }
        [DataMember]
        public bool Online { get; set; }
        
    }
}