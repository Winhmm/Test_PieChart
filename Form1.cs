using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEMO_PieChart
{
    public partial class Form1 : Form
    {
        // Tạo đối tượng DataContext để truy cập database
        CarDataDataContext db = new CarDataDataContext();
        public Form1()
        {
            InitializeComponent();
            LoadPieChart();
            LoadColumnChart();
            LoadBrandStats();
        }
        private void LoadPieChart()
        {
            // Lấy dữ liệu tổng số lượng bán theo hãng xe bằng LINQ
            var query = from b in db.BUYs
                        join c in db.CARs on b.CAR_ID equals c.CAR_ID
                        group new { b, c } by c.CAR_BRAND into g
                        select new
                        {
                            Brand = g.Key,
                            Revenue = g.Sum(x => x.b.BUY_QUANTITY * x.c.CAR_PRICE)
                        };

            // Gắn dữ liệu vào LiveCharts
            SeriesCollection series = new SeriesCollection();
            foreach (var item in query)
            {
                series.Add(new PieSeries
                {
                    Title = item.Brand,
                    Values = new ChartValues<double> { (double)item.Revenue },
                    DataLabels = true,
                    FontSize = 9,
                    LabelPoint = chartPoint => $"{chartPoint.SeriesView.Title}: {chartPoint.Y:N0} VNĐ"
                });
            }
            pieChart1.Series = series;
            pieChart1.LegendLocation = LegendLocation.Right;
        }

        private void LoadColumnChart()
        {
            // Lấy dữ liệu bằng LINQ
            var query = from b in db.BUYs
                        join c in db.CARs on b.CAR_ID equals c.CAR_ID
                        group b by c.CAR_BRAND into g
                        select new
                        {
                            Brand = g.Key,
                            TotalSold = g.Sum(x => x.BUY_QUANTITY)
                        };
            cartesianChart1.Hoverable = true;
            cartesianChart1.DisableAnimations = false;
            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                     Title = "Number Of Cars Sold",
                     Values = new ChartValues<double>(query.Select(x => (double)x.TotalSold)),
                     DataLabels = true, // Hiện trên đầu cột
                     LabelPoint = chartPoint => $"{chartPoint.Y} car"
                }
            };

            //  tooltip
            cartesianChart1.DataTooltip = new DefaultTooltip
            {
                // Edit for tooltip
                SelectionMode = TooltipSelectionMode.OnlySender,
                Background = System.Windows.Media.Brushes.LightYellow,
                Foreground = System.Windows.Media.Brushes.Black,
                FontSize = 13,
                Padding = new System.Windows.Thickness(8)
            };
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Brand",
                Labels = query.Select(x => x.Brand).ToArray()
            });

            cartesianChart1.AxisY.Clear();
            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sales Volume",
                LabelFormatter = value => value.ToString("N0")
            });
            cartesianChart1.LegendLocation = LegendLocation.Top;
            cartesianChart1.DisableAnimations = false; // Slide up
        }
        // Hàm tính trung vị
        private double GetMedian(List<double> numbers)
        {
            if (numbers.Count == 0) return 0;

            var sorted = numbers.OrderBy(n => n).ToList();
            int count = sorted.Count;
            if (count % 2 == 1)
                return sorted[count / 2];
            else
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
        }
        private void LoadBrandStats()
        {
            // Lấy danh sách hãng xe
            var brandStats = (from b in db.BUYs
                              join c in db.CARs on b.CAR_ID equals c.CAR_ID
                              group new { b, c } by c.CAR_BRAND into g
                              select new
                              {
                                  Brand = g.Key,
                                  TotalSold = g.Sum(x => x.b.BUY_QUANTITY),
                                  AveragePrice = g.Average(x => x.c.CAR_PRICE),
                                  MedianPrice = GetMedian(
                                      g.Select(x => (double)x.c.CAR_PRICE).ToList())
                              })
                              .OrderByDescending(x => x.TotalSold)
                              .ToList();

            dgvBrandStats.DataSource = brandStats;

            // Highlight hãng bán nhiều nhất
            if (dgvBrandStats.Rows.Count > 0)
            {
                dgvBrandStats.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvBrandStats.Rows[0].DefaultCellStyle.Font = new Font(dgvBrandStats.Font, FontStyle.Bold);
            }
        }

    }
}
