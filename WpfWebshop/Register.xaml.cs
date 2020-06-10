using RestSharp;
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
using System.Windows.Shapes;

namespace WpfWebshop
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //register user
            char[] charArray = TextBoxUsername.Text.ToCharArray();
            Array.Reverse(charArray);

            PasswordLabel.Content = "Password:\t" + new string(charArray);

            var client = new RestClient("http://localhost:51326/");
            var request = new RestRequest("/api/users/register");
            request.AddJsonBody(new { username = TextBoxUsername.Text, password = new string(charArray) });
            var response = client.Post(request);
            var content = response.Content;
            if (content == "true")
            {
                this.Close();
                new Shop(TextBoxUsername.Text).Show();
            }
            else
            {
                MessageBox.Show("User already exists, choose another combination.");
            }        
        }
    }
}
