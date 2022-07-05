using LanGuide.Models;
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
    public partial class ResultsPage : ContentPage
    {
        List<Result> results = new List<Result>();
        public ResultsPage()
        {
            InitializeComponent();
            GetAPIdata();
        }

        public async Task GetAPIdata()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri("https://www.idt.mdh.se/personal/plt01/languide/?get=results"));
                var result = await response.Content.ReadAsStringAsync();
                var json_object = JObject.Parse(result);
                var json_array = JArray.Parse(json_object["data"].ToString());
                foreach (var user in json_array)
                {
                    Result rslt = new Result();
                    rslt.id_user = user["id_user"].ToString();
                    rslt.email = user["email"].ToString();
                    rslt.create_time = user["create_time"].ToString();
                    rslt.id_exercise = user["id_exercise"].ToString();
                    rslt.result_percent = user["result_percent"].ToString();
                    rslt.result_score = user["result_score"].ToString();
                    rslt.result_max = user["result_max"].ToString();
                    rslt.skill = user["skill"].ToString();
                    rslt.language = user["language"].ToString();
                    rslt.result_date = user["result_date"].ToString();
                    results.Add(rslt);
                }
            }
            resultListView.ItemsSource = results;
        }
    }
}