using Microcharts;
using Newtonsoft.Json.Linq;
using SkiaSharp;
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
    public partial class SkillGraphsPage : ContentPage
    {
        List<string> skillsList = new List<string>();
        Dictionary<string, int> skillsCounts = new Dictionary<string, int>();
        Random random = new Random();
        List<ChartEntry> chartEntries = new List<ChartEntry>();

        public SkillGraphsPage()
        {
            InitializeComponent();
            GetDataAndCreateCharts();
        }

        public async Task GetDataAndCreateCharts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri("https://www.idt.mdh.se/personal/plt01/languide/?get=results"));
                var result = await response.Content.ReadAsStringAsync();
                var json_object = JObject.Parse(result);
                var json_array = JArray.Parse(json_object["data"].ToString());
                foreach (var res in json_array)
                {
                    var skill = res["skill"].ToString();
                    if (skill != String.Empty)
                    {
                        skillsList.Add(skill);
                    }
                }

                var sortedSkills = skillsList.OrderBy(lang => lang);

                foreach (var skill in sortedSkills)
                {
                    if (!skillsCounts.ContainsKey(skill))
                    {
                        var count = skillsList.FindAll(item => item.Equals(skill)).Count;
                        skillsCounts.Add(skill, count);
                    }
                }

                foreach (var item in skillsCounts)
                {
                    var color = String.Format("#{0:X6}", random.Next(0x1000000));

                    chartEntries.Add(new ChartEntry(item.Value)
                    {
                        Label = item.Key.ToString(),
                        Color = SKColor.Parse(color),
                        ValueLabel = item.Value.ToString()
                    });
                }

                chartViewBar.Chart = new BarChart() { Entries = chartEntries };

                chartViewPie.Chart = new PieChart() { Entries = chartEntries };

                chartViewLine.Chart = new LineChart() { Entries = chartEntries };
            }
        }
    }
}