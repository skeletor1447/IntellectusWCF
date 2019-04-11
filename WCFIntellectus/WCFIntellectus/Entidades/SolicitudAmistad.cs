using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Entidades
{
    [DataContract]
    public class SolicitudAmistad
    {
        [DataMember]
        public long IdSolicitudAmistad { get; set; }
        [DataMember]
        public long IdSolicitante { get; set; }
        [DataMember]
        public long IdSolicitado { get; set; }
    }
}