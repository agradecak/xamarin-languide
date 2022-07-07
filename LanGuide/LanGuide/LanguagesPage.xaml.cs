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
    public partial class LanguagesPage : ContentPage
    {
        List<string> languagesList = new List<string>();
        bool sortLangAscending = true;
        public LanguagesPage()
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
                foreach (var res in json_array)
                {
                    var lang = res["language"].ToString();
                    if (lang != String.Empty)
                    {
                        languagesList.Add(lang);
                    }
                }
            }
            var distinctLangs = languagesList.Distinct().OrderBy(lang => lang);
            languagesListView.ItemsSource = distinctLangs;
        }
        private void sortLang_Tapped(object sender, EventArgs e)
        {
            if (sortLangAscending)
            {
                var sortedResults = languagesList.Distinct().OrderByDescending(lang => lang);
                languagesListView.ItemsSource = sortedResults;
                sortLangAscending = false;
            }
            else
            {
                var sortedResults = languagesList.Distinct().OrderBy(lang => lang);
                languagesListView.ItemsSource = sortedResults;
                sortLangAscending = true;
            }
        }
    }
}