﻿using LanGuide.Models;
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
        List<string> skillList = new List<string>();
        List<string> languageList = new List<string>();

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
                    if (rslt.skill != String.Empty)
                        skillList.Add(rslt.skill);
                    if (rslt.language != String.Empty)
                        languageList.Add(rslt.language);
                }
            }
            var sortedResults = results.OrderBy(result => Convert.ToInt16(result.id_user)).ThenBy(res => res.result_date);
            resultListView.ItemsSource = sortedResults;

            var lanPickerList = languageList.Distinct().OrderBy(lang => lang).ToList();
            var skillPickerList = skillList.Distinct().OrderBy(skill => skill).ToList();
            languagePicker.ItemsSource = lanPickerList;
            skillPicker.ItemsSource = skillPickerList;
        }
        public void userSearch_Changed(object sender, TextChangedEventArgs e)
        {
            var searchResults = results.Where(result => result.id_user.Equals(e.NewTextValue));
            resultListView.ItemsSource = searchResults;
        }
        public void emailSearch_Changed(object sender, TextChangedEventArgs e)
        {
            var searchResults = results.Where(result => result.email.Equals(e.NewTextValue));
            var orderedResults = searchResults.OrderBy(result => result.email).ThenBy(res => Convert.ToInt16(res.id_user));
            resultListView.ItemsSource = orderedResults;
        }
        public void exerciseSearch_Changed(object sender, TextChangedEventArgs e)
        {
            var searchResults = results.Where(result => result.id_exercise.StartsWith(e.NewTextValue));
            var orderedResults = searchResults.OrderBy(result => result.id_exercise).ThenBy(res => Convert.ToInt16(res.id_user));
            resultListView.ItemsSource = orderedResults;
        }
        public async void languagePicker_Clicked(object sender, EventArgs e)
        {
            
            if (languagePicker.SelectedIndex != -1)
            {
                string pickedLanguage = languagePicker.SelectedItem.ToString();
                var pickedResults = results.Where(result => result.language == pickedLanguage);
                var orderedResults = pickedResults.OrderBy(result => result.language).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Picker selection can't be empty.", "OK");
            }
        }

        public async void languageCharts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguageGraphsPage());
        }

        public async void skillPicker_Clicked(object sender, EventArgs e)
        {
            if (skillPicker.SelectedIndex != -1)
            {
                string pickedSkill = skillPicker.SelectedItem.ToString();
                var pickedResults = results.Where(result => result.skill == pickedSkill);
                var orderedResults = pickedResults.OrderBy(result => result.skill).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Picker selection can't be empty.", "OK");
            }
        }

        public async void skillCharts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SkillGraphsPage());
        }

        public async void resPercentButton_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToInt16(resPercentMinEntry.Text) < Convert.ToInt16(resPercentMaxEntry.Text))
            {
                var filteredResults = results.Where(result => Convert.ToInt16(result.result_percent) >= Convert.ToInt16(resPercentMinEntry.Text) && Convert.ToInt16(result.result_percent) <= Convert.ToInt16(resPercentMaxEntry.Text));
                var orderedResults = filteredResults.OrderBy(result => Convert.ToInt16(result.result_percent)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Range not valid. 'Min' field value must be smaller than 'Max' field value.", "OK");
            }
        }
        public async void resPercentCharts_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResultPercentGraphsPage());
        }

        public async void resScoreButton_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToInt16(resScoreMinEntry.Text) < Convert.ToInt16(resScoreMaxEntry.Text))
            {
                var filteredResults = results.Where(result => Convert.ToInt16(result.result_score) >= Convert.ToInt16(resScoreMinEntry.Text) && Convert.ToInt16(result.result_score) <= Convert.ToInt16(resScoreMaxEntry.Text));
                var orderedResults = filteredResults.OrderBy(result => Convert.ToInt16(result.result_score)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Range not valid. 'Min' field value must be smaller than 'Max' field value.", "OK");
            }
        }
        public async void resMaxButton_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToInt16(resMaxMinEntry.Text) < Convert.ToInt16(resMaxMaxEntry.Text))
            {
                var filteredResults = results.Where(result => Convert.ToInt16(result.result_max) >= Convert.ToInt16(resMaxMinEntry.Text) && Convert.ToInt16(result.result_max) <= Convert.ToInt16(resMaxMaxEntry.Text));
                var orderedResults = filteredResults.OrderBy(result => Convert.ToInt16(result.result_max)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Range not valid. 'Min' field value must be smaller than 'Max' field value.", "OK");
            }
        }

        public async void dateCalc_Clicked(object sender, EventArgs e)
        {
            if (startDatePicker.Date < endDatePicker.Date)
            {
                var filteredResults = results.Where(result => Convert.ToDateTime(result.result_date) >= startDatePicker.Date && Convert.ToDateTime(result.result_date) <= endDatePicker.Date);
                var orderedResults = filteredResults.OrderBy(result => Convert.ToDateTime(result.result_date));
                resultListView.ItemsSource = orderedResults;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Range not valid. Start date must come earlier than end date.", "OK");
            }
        }
        public void resetListButton_Clicked(object sender, EventArgs e)
        {
            var sortedResults = results.OrderBy(result => Convert.ToInt16(result.id_user)).ThenBy(res => res.result_date);
            resultListView.ItemsSource = sortedResults;
        }
        private void sortID_Tapped(object sender, EventArgs e)
        {
            if (sortIDAscending)
            {
                var sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.id_user)).ThenBy(res => res.result_date);
                resultListView.ItemsSource = sortedResults;
                sortIDAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => Convert.ToInt16(result.id_user)).ThenBy(res => res.result_date);
                resultListView.ItemsSource = sortedResults;
                sortIDAscending = true;
            }
        }

        private void sortEmail_Tapped(object sender, EventArgs e)
        {
            if (sortEmailAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.email).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortEmailAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.email).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortEmailAscending = true;
            }
        }
        private void sortTime_Tapped(object sender, EventArgs e)
        {
            if (sortTimeAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.create_time);
                resultListView.ItemsSource = sortedResults;
                sortTimeAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.create_time);
                resultListView.ItemsSource = sortedResults;
                sortTimeAscending = true;
            }
        }
        private void sortExercise_Tapped(object sender, EventArgs e)
        {
            if (sortExerciseAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.id_exercise).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortExerciseAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.id_exercise).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortExerciseAscending = true;
            }
        }
        private void sortResultPercent_Tapped(object sender, EventArgs e)
        {
            if (sortResultPercentAscending)
            {
                var sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_percent)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultPercentAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_percent)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultPercentAscending = true;
            }
        }
        private void sortResultScore_Tapped(object sender, EventArgs e)
        {
            if (sortResultScoreAscending)
            {
                var sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_score)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultScoreAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_score)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultScoreAscending = true;
            }
        }
        private void sortResultMax_Tapped(object sender, EventArgs e)
        {
            if (sortResultMaxAscending)
            {
                var sortedResults = results.OrderByDescending(result => Convert.ToInt16(result.result_max)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultMaxAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => Convert.ToInt16(result.result_max)).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortResultMaxAscending = true;
            }
        }
        private void sortSkill_Tapped(object sender, EventArgs e)
        {
            if (sortSkillAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.skill).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortSkillAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.skill).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortSkillAscending = true;
            }
        }
        private void sortLanguage_Tapped(object sender, EventArgs e)
        {
            if (sortLanguageAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.language).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortLanguageAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.language).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortLanguageAscending = true;
            }
        }

        private void sortDate_Tapped(object sender, EventArgs e)
        {
            if (sortDateAscending)
            {
                var sortedResults = results.OrderByDescending(result => result.result_date).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortDateAscending = false;
            }
            else
            {
                var sortedResults = results.OrderBy(result => result.result_date).ThenBy(res => Convert.ToInt16(res.id_user));
                resultListView.ItemsSource = sortedResults;
                sortDateAscending = true;
            }
        }
    }
}