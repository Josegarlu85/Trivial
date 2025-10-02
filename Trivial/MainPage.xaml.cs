namespace Trivial
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void capitalTrivial(object sender, EventArgs e)
        {
            // Lógica para capitales
            await Shell.Current.GoToAsync("///capital");
        }

        private async void paisTrivial(object sender, EventArgs e)
        {
            // Lógica para países
            await Shell.Current.GoToAsync("///pais");
        }
    }
}
