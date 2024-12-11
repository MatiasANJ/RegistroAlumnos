using Firebase.Database;
using Firebase.Database.Query;
using RegistroEmpleados.Modelos.Modelos;

namespace RegistroEmpleados.AppMovil.Vistas;

public partial class CrearEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://bdestudiantes-72395-default-rtdb.firebaseio.com/"); // Cambiar URL al proyecto correcto

    public CrearEstudiante()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        // Validar que los campos no sean nulos o vacíos
        if (string.IsNullOrWhiteSpace(primerNombreEntry.Text) ||
            string.IsNullOrWhiteSpace(primerApellidoEntry.Text) ||
            string.IsNullOrWhiteSpace(correoEntry.Text) ||
            string.IsNullOrWhiteSpace(edadEntry.Text) ||
            string.IsNullOrWhiteSpace(cursoEntry.Text))  // Verificar que el campo del curso no esté vacío
        {
            await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
            return;
        }

        // Validar conversión de edad
        if (!int.TryParse(edadEntry.Text, out int edad))
        {
            await DisplayAlert("Error", "La edad debe ser un número válido.", "OK");
            return;
        }

        // Crear o buscar el curso en Firebase
        Curso curso = await CrearOObtenerCurso(cursoEntry.Text);

        // Crear el estudiante
        var estudiante = new Estudiante
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            FechaInsripcion = fechaInscripcionPicker.Date,
            Edad = edad,
            Curso = curso
        };

        try
        {
            // Guardar el estudiante en Firebase
            await client.Child("Estudiantes").PostAsync(estudiante);
            await DisplayAlert("Éxito", $"El estudiante {estudiante.PrimerNombre} {estudiante.PrimerApellido} fue guardado correctamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar el estudiante: {ex.Message}", "OK");
        }
    }

    private async Task<Curso> CrearOObtenerCurso(string cursoNombre)
    {
        // Verificar si el curso ya existe en Firebase
        var cursos = await client.Child("Cursos").OnceAsync<Curso>();
        var cursoExistente = cursos.FirstOrDefault(c => c.Object.Nombre.Equals(cursoNombre, StringComparison.OrdinalIgnoreCase));

        if (cursoExistente != null)
        {
            // Si el curso ya existe, lo devolvemos
            return cursoExistente.Object;
        }

        // Si el curso no existe, lo creamos
        var nuevoCurso = new Curso { Nombre = cursoNombre };
        var cursoPost = await client.Child("Cursos").PostAsync(nuevoCurso);

        // Asignamos el nuevo curso con el ID generado por Firebase
        nuevoCurso.Id = cursoPost.Key;
        return nuevoCurso;
    }
}
