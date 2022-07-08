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
    public partial class ResultPercentGraphsPage : ContentPage
    {
        List<int> percentagesList = new List<int>();
        Dictionary<int, int> percentagesCounts = new Dictionary<int, int>();
        List<ChartEntry> chartEntries = new List<ChartEntry>();

        public ResultPercentGraphsPage()
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
                foreach (var reslt in json_array)
                {
                    var res = Convert.ToInt16(reslt["result_percent"]);
                    percentagesList.Add(res);
                }

                var sortedPercentages = percentagesList.OrderBy(res => res);

                foreach (var percent in sortedPercentages)
                {
                    if (!percentagesCounts.ContainsKey(percent))
                    {
                        var count = percentagesList.FindAll(item => item.Equals(percent)).Count;
                        percentagesCounts.Add(percent, count);
                    }
                }

                foreach (var item in percentagesCounts)
                {
                    if (item.Key == 0 || item.Key == 100)
                    {
                        chartEntries.Add(new ChartEntry(item.Value)
                        {
                            Label = item.Key.ToString(),
                            ValueLabel = item.Value.ToString()
                        });
                    }
                    else
                    {
                        chartEntries.Add(new ChartEntry(item.Value) { } );
                    }
                }

                chartViewBar.Chart = new BarChart() { Entries = chartEntries };
                chartViewLine.Chart = new LineChart() { Entries = chartEntries };
            }
        }
    }
}