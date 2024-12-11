using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEmpleados.Modelos.Modelos
{
    public class Curso
    {
        // Propiedad para el nombre del curso
        public string Nombre { get; set; }

        // Agregar la propiedad Id para almacenar el ID de Firebase
        public string Id { get; set; }  // Esta es la propiedad que falta
    }
}
