using RestSharp;
using System;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace WpfWebshop
{
    /// <summary>
    /// Interaction logic for Shop.xaml
    /// </summary>
    public partial class Shop : Window
    {
        private RestClient client = new RestClient("http://localhost:51326/");
        private String username;

        public Shop(String username)
        {
            InitializeComponent();

            this.username = username;

            updateMoney();
            fillProducts();
            fillInventory();
        }

        private void fillInventory()
        {
            InventoryListBox.Items.Clear();
            foreach (var inventory in getInventory())
            {
                if ((int)inventory["amount"] <= 0) continue; //only 0+ amount
                InventoryListBox.Items.Add(inventory["name"] + ": " + inventory["amount"]);
            }
        }

        private void fillProducts()
        {
            ProductsListBox.Items.Clear();
            foreach (var item in getProducts())
            {
                if ((int)item["amount"] <= 0) continue; //only 0+ amount
                ProductsListBox.Items.Add(item["name"] + ": " + item["price"]);
            }
        }

        private JArray getProducts()
        {
            var request = new RestRequest("/api/products");
            var response = client.Get(request);

            return JArray.Parse(response.Content);
        }

        private JObject getProduct(String name)
        {
            var request = new RestRequest("/api/products/" + name);
            var response = client.Get(request);
            JObject jObject = JObject.Parse(response.Content);

            return jObject;
        }

        private JArray getInventory()
        {
            var request = new RestRequest("/api/inventory/" + username);
            var response = client.Get(request);

            return JArray.Parse(response.Content);
        }

        private double getMoney()
        {
            var request = new RestRequest("/api/users/" + this.username);
            var response = client.Get(request);
            JToken jToken = JToken.Parse(response.Content);

            return (double)jToken["money"];
        }

        private void updateMoney()
        {
            MoneyLabel.Content = "Money left: €" + getMoney();
        }

        private void buyFruit(string productName, double cost)
        {
            var request = new RestRequest("/api/products/decrease"); //decrease product amount
            request.AddJsonBody(new { productName = productName });
            client.Post(request);

            request = new RestRequest("/api/inventory/increase"); //increase inventory amount 
            request.AddJsonBody(new { username = this.username, productName = productName });
            client.Post(request);

            request = new RestRequest("/api/inventory/add"); //add to inventory 
            request.AddJsonBody(new { username = this.username, productName = productName });
            client.Post(request);

            request = new RestRequest("/api/users/decrease"); //decrease money
            request.AddJsonBody(new { username = this.username, cost = cost });
            client.Post(request);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //buy product button
            if (ProductsListBox.SelectedItem != null)
            {
                String[] selectedItemText = Regex.Split(ProductsListBox.SelectedItem.ToString(), ":");
                String selectedItemName = selectedItemText[0].TrimEnd();
                Double selectedItemPrice = Convert.ToDouble(selectedItemText[1].TrimStart());
                if (getMoney() > selectedItemPrice)//has money
                {
                    buyFruit(selectedItemName, selectedItemPrice);
                    updateMoney();
                    fillProducts();
                    fillInventory();
                    MessageBox.Show("You have bought a " + selectedItemName);
                }
                else//no money
                {
                    MessageBox.Show("Cannot buy; Insufficient balance. (€" + selectedItemPrice + ")");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //refresh button
            fillProducts();
            fillInventory();
        }
    }
}
