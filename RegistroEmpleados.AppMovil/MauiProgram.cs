using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEmpleados.Modelos.Modelos;

namespace RegistroEmpleados.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            
            _ = Registrar();

            return builder.Build();
        }

        public static async Task Registrar()
        {
            try
            {
                var firebaseUrl = "https://bdestudiantes-72395-default-rtdb.firebaseio.com/";
                Console.WriteLine($"Conectando a Firebase en {firebaseUrl}");
                var client = new FirebaseClient(firebaseUrl);

                Console.WriteLine("Obteniendo cursos existentes...");
                var cargos = await client.Child("Cursos").OnceAsync<Curso>();

                if (cargos.Count == 0)
                {
                    Console.WriteLine("No se encontraron cursos. Agregando datos iniciales...");
                    await client.Child("Cursos").PostAsync(new Curso { Nombre = "1ro Básico" });
                    await client.Child("Cursos").PostAsync(new Curso { Nombre = "4To Básico" });
                    await client.Child("Cursos").PostAsync(new Curso { Nombre = "1ro Medio" });
                    await client.Child("Cursos").PostAsync(new Curso { Nombre = "2do Medio" });
                    Console.WriteLine("Datos iniciales agregados.");
                }
                else
                {
                    Console.WriteLine($"Se encontraron {cargos.Count} cursos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar los cursos: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detalles del error interno: {ex.InnerException.Message}");
                }
            }
        }

    }
}
