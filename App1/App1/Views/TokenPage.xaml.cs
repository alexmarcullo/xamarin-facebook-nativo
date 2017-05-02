using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TokenPage : ContentPage
    {
        public TokenPage(string token)
        {
            InitializeComponent();

            this.Content = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = token
                    }
                }
            };
        }
    }
}
