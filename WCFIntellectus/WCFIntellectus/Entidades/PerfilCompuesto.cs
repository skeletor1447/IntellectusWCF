using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Entidades
{
    [DataContract]
    public class PerfilCompuesto
    {
        [DataMember]
        public Perfil Perfil { get; set; }
        [DataMember]
        public Usuario Usuario { get; set; }
    }
}