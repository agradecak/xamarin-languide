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
        
        bool sortIDAscending = true;
        bool sortEmailAscending = true;
        bool sortTimeAscending = true;
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
            usersListView.ItemsSource = users;
        }

        private void sortID_Tapped(object sender, EventArgs e)
        {
            if (sortIDAscending)
            {
                List<User> sortedUsers = users.OrderByDescending(user => Convert.ToInt16(user.id_user)).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortIDAscending = false;
            }
            else
            {
                List<User> sortedUsers = users.OrderBy(user => Convert.ToInt16(user.id_user)).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortIDAscending = true;
            }
        }

        private void sortEmail_Tapped(object sender, EventArgs e)
        {
            if (sortEmailAscending)
            {
                List<User> sortedUsers = users.OrderByDescending(user => user.email).ThenBy(usr => Convert.ToInt16(usr.id_user)).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortEmailAscending = false;
            }
            else
            {
                List<User> sortedUsers = users.OrderBy(user => user.email).ThenBy(usr => Convert.ToInt16(usr.id_user)).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortEmailAscending = true;
            }
        }
        private void sortTime_Tapped(object sender, EventArgs e)
        {
            if (sortTimeAscending)
            {
                List<User> sortedUsers = users.OrderByDescending(user => user.create_time).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortTimeAscending = false;
            }
            else
            {
                List<User> sortedUsers = users.OrderBy(user => user.create_time).ToList();
                usersListView.ItemsSource = sortedUsers;
                sortTimeAscending = true;
            }
        }
    }
}