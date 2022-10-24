using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace todo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Webview : ContentPage
    {
        public Webview()
        {
            InitializeComponent();

            var browser = new WebView();
            browser.Source = "http://10.0.2.2:8000/";
            Content = browser;
        }
    }
}