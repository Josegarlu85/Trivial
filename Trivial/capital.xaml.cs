namespace Trivial;

// Clase principal de la página de juego
public partial class capital : ContentPage
{
    // Variables para controlar el estado del juego
    private int preguntaActual = 0;               // Índice de la pregunta actual
    private int aciertos = 0;
    private int fallos = 0;
    private string capitalCorrectaActual = "";    // Capital correcta de la pregunta actual

    // Lista de preguntas, cada elemento es un país y su capital
    private List<(string pais, string capital)> listaPreguntas = new()
    {
        ("Francia", "París"),
        ("Italia", "Roma"),
        ("España", "Madrid"),
        ("Alemania", "Berlín"),
        ("Portugal", "Lisboa"),
        ("Grecia", "Atenas"),
        ("Japón", "Tokio"),
        ("México", "Ciudad de México"),
        ("Argentina", "Buenos Aires"),
        ("Egipto", "El Cairo")
    };

    // Generador de nmeros aleatorios para mezclar opciones
    private Random generadorAleatorio = new();

    public capital()
    {
        InitializeComponent();
    }

    // Evento que se ejecuta al pulsar boton de volver a inicio
    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage"); // Navega a la página principal
    }

    // Metodo para iniciar el juego al pulsar su boyton 
    private void OnStartGameClicked(object sender, EventArgs e)
    {
        // Reinicia el juego
        aciertos = 0;
        fallos = 0;
        preguntaActual = 0;

        // Muestra el panel de juego dandole el valor true y oculta el de bienvenida con false, estos estan en el xaml
        PanelJuego.IsVisible = true;
        PanelBienvenida.IsVisible = false;

        // Pasamos a la primera pregunta con este metodo
        MostrarSiguientePregunta();
    }

    // Metodo para ir a la siguiente pregunta del juego
    private void MostrarSiguientePregunta()
    {
        // 1️ Verifica si ya se han mostrado todas las preguntas
        if (preguntaActual >= listaPreguntas.Count)
        {
            // Si se acabaron las preguntas oculta el panel de juego
            PanelJuego.IsVisible = false;

            // Y vuelve a mostrar el de bienvenida
            PanelBienvenida.IsVisible = true;

            // Muestra una ventana emergente con los resultados
            DisplayAlert("Resultado", $"✔️Aciertos: {aciertos}\n❌" +
                $"Fallos: {fallos}", "OK");
            return; // Sale del metodo
        }

        // 2️ Extrae la pregunta actual (país y capital correcta)
        var parActual = listaPreguntas[preguntaActual];
        string pais = parActual.pais;
        string capitalCorrecta = parActual.capital;

        // Guarda la capital correcta para compararla más adelante
        capitalCorrectaActual = capitalCorrecta;

        // Actualiza el texto de la pregunta en pantalla
        EtiquetaPregunta.Text = $"¿Cuál es la capital de {pais}?";

        // 3️ Construye una lista con todas las capitales disponibles
        List<string> todasLasCapitales = new List<string>();
        for (int i = 0; i < listaPreguntas.Count; i++)
        {
            todasLasCapitales.Add(listaPreguntas[i].capital);
        }

        // 4️ Mezcla las capitales manualmente para que el orden sea aleatorio
        List<string> capitalesMezcladas = new List<string>();
        while (todasLasCapitales.Count > 0)
        {
            int indice = generadorAleatorio.Next(todasLasCapitales.Count);
            capitalesMezcladas.Add(todasLasCapitales[indice]);
            //Y abajo eliminamos la capital ya añadida
            todasLasCapitales.RemoveAt(indice);
        }

        // 5️ Toma las primeras 3 capitales como opciones de respuesta
        List<string> opciones = new List<string>();
        for (int i = 0; i < 3 && i < capitalesMezcladas.Count; i++)
        {
            opciones.Add(capitalesMezcladas[i]);
        }

        // 6️ Verifica si la capital correcta está entre las opciones
        bool encontrada = false;
        for (int i = 0; i < opciones.Count; i++)
        {
            if (opciones[i] == capitalCorrecta)
            {
                encontrada = true;
                break;
            }
        }

        // 7️ Si no está, reemplaza una opción aleatoria por la correcta
        if (!encontrada)
        {
            int indiceReemplazo = generadorAleatorio.Next(opciones.Count);
            opciones[indiceReemplazo] = capitalCorrecta;
        }

        // 8️ Vuelve a mezclar las opciones para que no se sepa cuál es la correcta
        List<string> opcionesFinales = new List<string>();
        while (opciones.Count > 0)
        {
            int indice = generadorAleatorio.Next(opciones.Count);
            opcionesFinales.Add(opciones[indice]);
            opciones.RemoveAt(indice);
        }

        // 9️ Limpia los botones anteriores del panel
        PanelOpciones.Children.Clear();

        // 10 Crea un botón por cada opción final
        for (int i = 0; i < opcionesFinales.Count; i++)
        {
            Button boton = new Button
            {
                Text = opcionesFinales[i],               // Texto del botón
                BackgroundColor = Colors.DarkOliveGreen, // Color de fondo
                TextColor = Colors.Ivory,         // Color del texto
                CommandParameter = opcionesFinales[i]    // Guarda la opción seleccionada
            };

            // Asocia el evento de clic al botón
            boton.Clicked += AlSeleccionarOpcion;

            // Añade el botón al panel de opciones
            PanelOpciones.Children.Add(boton);
        }

        //  Actualiza la puntuación en pantalla
        EtiquetaPuntuacion.Text = $"Pregunta {preguntaActual + 1} de 10\n | ✔️Aciertos: {aciertos} | ❌Fallos: {fallos} |";
    }


    // Metodo al pulsar una opción
    private void AlSeleccionarOpcion(object sender, EventArgs e)
    {
        Button boton = sender as Button;
        if (boton != null && boton.CommandParameter is string seleccionada)
        {
            //Lanzara este metodo si el boton no es nulo y el parametro es un string
            VerificarRespuesta(seleccionada, capitalCorrectaActual);
        }
    }

    // Verifica si la respuesta seleccionada es correcta
    private void VerificarRespuesta(string seleccionada, string correcta)
    {
        if (seleccionada == correcta)
        {
            aciertos++; // Suma acierto
        }
        else
        {
            fallos++;   // Suma fallo
        }

        preguntaActual++; // Avanza a la siguiente pregunta
        MostrarSiguientePregunta(); // Metodo que muestra la siguiente pregunta
    }
}
