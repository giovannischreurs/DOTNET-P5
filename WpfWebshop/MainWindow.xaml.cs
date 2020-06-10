using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using RestSharp;

namespace WpfWebshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            //open register screen
            Register registerScreen = new Register();
            registerScreen.ShowDialog();
        }

        private void Shop_Click(object sender, RoutedEventArgs e)
        {
            //open shop screen
            //Shop shopScreen = new Shop();
            //shopScreen.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("http://localhost:51326/");

            var request = new RestRequest("/api/users/login");
            request.AddJsonBody(new { username = TextBoxUsername.Text, password = TextBoxPasswordBox.Password });
            var response = client.Post(request);
            var content = response.Content;
            if(content == "true")
            {
                //open shop screen
                Shop shopScreen = new Shop(TextBoxUsername.Text);
                shopScreen.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid username/password combination.");
            }
        }
    }
}
