using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Utileria
{ 
    [DataContract]
    public class EliminarRespuesta<T>
    {
        [DataMember]
        public bool Error { get; set; }
        [DataMember]
        public Dictionary<String, String> Errores { get; set; }
        [DataMember]
        public T Entidad { get; set; }
    }
}