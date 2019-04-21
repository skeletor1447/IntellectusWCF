using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFIntellectus.Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "ServidorServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione ServidorServices.svc o ServidorServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServidorServices : IServidorServices
    {
        Model.IntellectusdbEntities IntellectusdbEntities = new Model.IntellectusdbEntities();
        public bool OnlineAOffline(long idCliente)
        {
            try
            {
                Model.tblperfil tblperfil = IntellectusdbEntities.tblperfil.Where(x => x.IdUsuario == idCliente).Single();
                tblperfil.Online = false;

                IntellectusdbEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
