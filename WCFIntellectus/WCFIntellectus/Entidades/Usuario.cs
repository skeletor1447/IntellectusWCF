using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFIntellectus.Entidades
{
    [DataContract]
    public class Usuario
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public String Correo { get; set; }
        [DataMember]
        public String Nick { get; set; }
        
    }
}