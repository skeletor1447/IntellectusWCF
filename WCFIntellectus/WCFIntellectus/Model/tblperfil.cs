//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFIntellectus.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblperfil
    {
        public int IdPerfil { get; set; }
        public int IdUsuario { get; set; }
        public string NombreReal { get; set; }
        public string Descripcion { get; set; }
        public byte[] Avatar { get; set; }
        public string Disponibilidad { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public bool Online { get; set; }
    }
}