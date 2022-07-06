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

        bool sortIDAscending = true;
        bool sortEmailAscending = true;
        bool sortTimeAscending = true;
        bool sortExerciseAscending = true;
        bool sortResultPercentAscending = true;
        bool sortResultScoreAscending = true;
        bool sortResultMaxAscending = true;
        bool sortSkillAscending = true;
        bool sortLanguageAscending = true;
        bool sortDateAscending = true;
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

        private void sortID_Tapped(object sender, EventArgs e)
        {
            if (sortIDAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortIDAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => Convert.ToInt16(result.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortIDAscending = true;
            }
        }

        private void sortEmail_Tapped(object sender, EventArgs e)
        {
            if (sortEmailAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => result.email).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortEmailAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => result.email).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortEmailAscending = true;
            }
        }
        private void sortTime_Tapped(object sender, EventArgs e)
        {
            if (sortTimeAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => result.create_time).ToList();
                resultListView.ItemsSource = sortedResults;
                sortTimeAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => result.create_time).ToList();
                resultListView.ItemsSource = sortedResults;
                sortTimeAscending = true;
            }
        }
        private void sortExercise_Tapped(object sender, EventArgs e)
        {
            if (sortExerciseAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.id_exercise)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortExerciseAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => Convert.ToInt16(result.id_exercise)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortExerciseAscending = true;
            }
        }
        private void sortResultPercent_Tapped(object sender, EventArgs e)
        {
            if (sortResultPercentAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_percent)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultPercentAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_percent)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultPercentAscending = true;
            }
        }
        private void sortResultScore_Tapped(object sender, EventArgs e)
        {
            if (sortResultScoreAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_score)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultScoreAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_score)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultScoreAscending = true;
            }
        }
        private void sortResultMax_Tapped(object sender, EventArgs e)
        {
            if (sortResultMaxAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_max)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultMaxAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_max)).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortResultMaxAscending = true;
            }
        }
        private void sortSkill_Tapped(object sender, EventArgs e)
        {
            if (sortSkillAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => result.skill).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortSkillAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => result.skill).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortSkillAscending = true;
            }
        }
        private void sortLanguage_Tapped(object sender, EventArgs e)
        {
            if (sortLanguageAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => result.language).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortLanguageAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => result.language).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortLanguageAscending = true;
            }
        }

        private void sortDate_Tapped(object sender, EventArgs e)
        {
            if (sortDateAscending)
            {
                List<Result> sortedResults = results.OrderByDescending(result => result.result_date).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortDateAscending = false;
            }
            else
            {
                List<Result> sortedResults = results.OrderBy(result => result.result_date).ThenBy(res => Convert.ToInt16(res.id_user)).ToList();
                resultListView.ItemsSource = sortedResults;
                sortDateAscending = true;
            }
        }
    }
}