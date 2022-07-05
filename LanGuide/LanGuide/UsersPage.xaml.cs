using LanGuide.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LanGuide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        List<User> users = new List<User>();
        public UsersPage()
        {
            InitializeComponent();
            GetAPIdata();
        }

        public async Task GetAPIdata()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri("https://www.idt.mdh.se/personal/plt01/languide/?get=users"));
                var result = await response.Content.ReadAsStringAsync();
                var json_object = JObject.Parse(result);
                var json_array = JArray.Parse(json_object["data"].ToString());
                foreach (var user in json_array)
                {
                    User usr = new User();
                    usr.id_user = user["id_user"].ToString();
                    usr.email = user["email"].ToString();
                    usr.create_time = user["create_time"].ToString();
                    users.Add(usr);
                }
            }
            userListView.ItemsSource = users;
        }
    }
}