namespace Trivial;

public partial class pais : ContentPage
{
    private int preguntaActual = 0;
    private int aciertos = 0;
    private int fallos = 0;
    private string paisCorrectoActual = "";

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

    private Random generadorAleatorio = new();

    public pais()
    {
        InitializeComponent();
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    private void OnStartGameClicked(object sender, EventArgs e)
    {
        aciertos = 0;
        fallos = 0;
        preguntaActual = 0;

        PanelJuego.IsVisible = true;
        PanelBienvenida.IsVisible = false;

        MostrarSiguientePregunta();
    }

    private void MostrarSiguientePregunta()
    {
        if (preguntaActual >= listaPreguntas.Count)
        {
            PanelJuego.IsVisible = false;
            PanelBienvenida.IsVisible = true;

            DisplayAlert("Resultado", $"✔️Aciertos: {aciertos}\n❌Fallos: {fallos}", "OK");
            return;
        }

        var parActual = listaPreguntas[preguntaActual];
        string paisCorrecto = parActual.pais;
        string capital = parActual.capital;

        paisCorrectoActual = paisCorrecto;

        EtiquetaPregunta.Text = $"¿A qué país pertenece la capital {capital}?";

        List<string> todosLosPaises = new List<string>();
        for (int i = 0; i < listaPreguntas.Count; i++)
        {
            todosLosPaises.Add(listaPreguntas[i].pais);
        }

        List<string> paisesMezclados = new List<string>();
        while (todosLosPaises.Count > 0)
        {
            int indice = generadorAleatorio.Next(todosLosPaises.Count);
            paisesMezclados.Add(todosLosPaises[indice]);
            todosLosPaises.RemoveAt(indice);
        }

        List<string> opciones = new List<string>();
        for (int i = 0; i < 3 && i < paisesMezclados.Count; i++)
        {
            opciones.Add(paisesMezclados[i]);
        }

        bool encontrada = false;
        for (int i = 0; i < opciones.Count; i++)
        {
            if (opciones[i] == paisCorrecto)
            {
                encontrada = true;
                break;
            }
        }

        if (!encontrada)
        {
            int indiceReemplazo = generadorAleatorio.Next(opciones.Count);
            opciones[indiceReemplazo] = paisCorrecto;
        }

        List<string> opcionesFinales = new List<string>();
        while (opciones.Count > 0)
        {
            int indice = generadorAleatorio.Next(opciones.Count);
            opcionesFinales.Add(opciones[indice]);
            opciones.RemoveAt(indice);
        }

        PanelOpciones.Children.Clear();

        for (int i = 0; i < opcionesFinales.Count; i++)
        {
            Button boton = new Button
            {
                Text = opcionesFinales[i],
                BackgroundColor = Colors.DarkOliveGreen,
                TextColor = Colors.Ivory,
                CommandParameter = opcionesFinales[i]
            };

            boton.Clicked += AlSeleccionarOpcion;
            PanelOpciones.Children.Add(boton);
        }

        EtiquetaPuntuacion.Text = $"Pregunta {preguntaActual + 1} de 10\n | ✔️Aciertos: {aciertos} | ❌Fallos: {fallos} |";
    }

    private void AlSeleccionarOpcion(object sender, EventArgs e)
    {
        Button boton = sender as Button;
        if (boton != null && boton.CommandParameter is string seleccionada)
        {
            VerificarRespuesta(seleccionada, paisCorrectoActual);
        }
    }

    private void VerificarRespuesta(string seleccionada, string correcta)
    {
        if (seleccionada == correcta)
        {
            aciertos++;
        }
        else
        {
            fallos++;
        }

        preguntaActual++;
        MostrarSiguientePregunta();
    }
}
