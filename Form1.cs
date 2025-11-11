using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
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
        private DataBase2DataContext db = new DataBase2DataContext();

        public Form1()
        {
            InitializeComponent();
            // *** THAY ĐỔI: Xóa các hàm Load...() ra khỏi đây ***
            // Chúng ta sẽ chuyển nó vào Form1_Load để đảm bảo ComboBox
            // được tạo xong xuôi trước khi tải dữ liệu.
        }

        // *** MỚI: Hàm xử lý sự kiện khi chọn ComboBox ***
        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy tháng đã chọn (0 = Tất cả, 1 = Tháng 1, 2 = Tháng 2, ...)
            int selectedMonth = cbMonth.SelectedIndex;

            // Gọi hàm cập nhật tất cả biểu đồ và lưới dữ liệu
            UpdateAllCharts(selectedMonth);
        }

        // *** MỚI: Hàm helper để tải lại toàn bộ dữ liệu ***
        private void UpdateAllCharts(int selectedMonth)
        {
            db = new DataBase2DataContext();

            LoadPieChart(selectedMonth);
            LoadColumnChart(selectedMonth);
            LoadBrandStats(selectedMonth);
            LoadTotal(selectedMonth);
        }

        // *** THAY ĐỔI: Thêm tham số (int selectedMonth) ***
        private void LoadPieChart(int selectedMonth)
        {
            // *** LỌC DỮ LIỆU THEO THÁNG ***
            // Giả sử bảng BUYs có cột BUY_DATE (kiểu DateTime)
            var buysQuery = db.BUYs.AsQueryable();
            if (selectedMonth > 0) // 0 = "Tất cả"
            {
                // Lọc theo tháng. 
                // Nếu bạn muốn lọc theo cả năm hiện tại, hãy thêm:
                // .Where(x => x.BUY_DATE.Year == DateTime.Now.Year)
                buysQuery = buysQuery.Where(x => x.BUY_DATE.HasValue && x.BUY_DATE.Value.Month == selectedMonth);
            }
            // *** KẾT THÚC LỌC ***

            // Dùng 'buysQuery' đã được lọc thay vì 'db.BUYs'
            var query = from b in buysQuery
                        join c in db.CARs on b.CAR_ID equals c.CAR_ID
                        group new { b, c } by c.CAR_BRAND into g
                        select new
                        {
                            Brand = g.Key,
                            // Thêm (decimal?) và ?? 0 để tránh lỗi nếu tháng đó không có doanh thu
                            Revenue = (decimal?)g.Sum(x => x.b.BUY_QUANTITY * x.c.CAR_PRICE) ?? 0m
                        };


            SeriesCollection series = new SeriesCollection();

            foreach (var item in query)
            {
                series.Add(new PieSeries
                {
                    Title = item.Brand,
                    Values = new ChartValues<double> { (double)item.Revenue }, // Chuyển từ decimal sang double
                    DataLabels = true,
                    FontSize = 9,
                    LabelPoint = chartPoint => $"{chartPoint.SeriesView.Title}: {chartPoint.Y:N0} VNĐ"
                });
            }
            pieChart1.Series = series;
            pieChart1.LegendLocation = LegendLocation.Right;
        }

        // *** THAY ĐỔI: Thêm tham số (int selectedMonth) ***
        private void LoadColumnChart(int selectedMonth)
        {
            // *** LỌC DỮ LIỆU THEO THÁNG ***
            var buysQuery = db.BUYs.AsQueryable();
            if (selectedMonth > 0)
            {
                buysQuery = buysQuery.Where(x => x.BUY_DATE.HasValue && x.BUY_DATE.Value.Month == selectedMonth);
            }
            // *** KẾT THÚC LỌC ***

            // Dùng 'buysQuery' đã được lọc
            var query = (from b in buysQuery
                         join c in db.CARs on b.CAR_ID equals c.CAR_ID
                         group b by c.CAR_BRAND into g
                         select new
                         {
                             Brand = g.Key,
                             TotalSold = (double?)g.Sum(x => x.BUY_QUANTITY) ?? 0.0
                         }).ToList();

            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Number Of Cars Sold",
                    Values = new ChartValues<double>(query.Select(x => x.TotalSold)),
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y} car"
                }
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
        }

        // Hàm tính trung vị (không đổi)
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

        // *** THAY ĐỔI: Thêm tham số (int selectedMonth) ***
        private void LoadBrandStats(int selectedMonth)
        {
            // *** LỌC DỮ LIỆU THEO THÁNG ***
            var buysQuery = db.BUYs.AsQueryable();
            if (selectedMonth > 0)
            {
                buysQuery = buysQuery.Where(x => x.BUY_DATE.HasValue && x.BUY_DATE.Value.Month == selectedMonth);
            }
            // *** KẾT THÚC LỌC ***

            // Dùng 'buysQuery' đã được lọc
            var brandStats = (from b in buysQuery
                              join c in db.CARs on b.CAR_ID equals c.CAR_ID
                              group new { b, c } by c.CAR_BRAND into g
                              // Thêm các xử lý an toàn (?? 0) phòng trường hợp group rỗng
                              let totalSold = (int?)g.Sum(x => x.b.BUY_QUANTITY) ?? 0
                              let avgPrice = (double?)g.Average(x => x.c.CAR_PRICE) ?? 0.0
                              let medianPriceList = g.Select(x => (double)x.c.CAR_PRICE).ToList()
                              select new
                              {
                                  Brand = g.Key,
                                  TotalSold = totalSold,
                                  AveragePrice = avgPrice,
                                  MedianPrice = medianPriceList.Any() ? GetMedian(medianPriceList) : 0.0
                              })
                      .OrderByDescending(x => x.TotalSold)
                      .ToList();

            dgvBrandStats.DataSource = brandStats;

            if (dgvBrandStats.Rows.Count > 0)
            {
                dgvBrandStats.Rows[0].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvBrandStats.Rows[0].DefaultCellStyle.Font = new Font(dgvBrandStats.Font, FontStyle.Bold);
            }
        }

        // *** THAY ĐỔI: Thêm tham số (int selectedMonth) ***
        private void LoadTotal(int selectedMonth)
        {
            // *** LỌC DỮ LIỆU THEO THÁNG ***
            var buysQuery = db.BUYs.AsQueryable();
            if (selectedMonth > 0)
            {
                buysQuery = buysQuery.Where(x => x.BUY_DATE.HasValue && x.BUY_DATE.Value.Month == selectedMonth);
            }
            // *** KẾT THÚC LỌC ***

            // Dùng (int?) và ?? 0 để tránh lỗi
            var totalCars = (from b in buysQuery // Dùng 'buysQuery' đã được lọc
                             join c in db.CARs on b.CAR_ID equals c.CAR_ID
                             select (int?)b.BUY_QUANTITY).Sum() ?? 0;


            var totalList = new List<dynamic>
            {
                new
                {
                    Label = "Tổng số xe bán được",
                    TotalSold = totalCars
                }
            };

            dgvTotal.DataSource = totalList;
            if (dgvTotal.Columns.Contains("Label")) // Kiểm tra trước khi thiết lập
            {
                dgvTotal.Columns["Label"].HeaderText = "Mô tả";
            }
            if (dgvTotal.Columns.Contains("TotalSold")) // Kiểm tra trước khi thiết lập
            {
                dgvTotal.Columns["TotalSold"].HeaderText = "Số xe bán được";
            }
            dgvTotal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvTotal.Rows.Count > 0)
            {
                dgvTotal.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                dgvTotal.Rows[0].DefaultCellStyle.Font = new Font(dgvTotal.Font, FontStyle.Bold);
            }
        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        // *** THAY ĐỔI: Sửa lại Form1_Load ***
        private void Form1_Load(object sender, EventArgs e)
        {
            cbMonth.Items.Add("Tất cả");
            for (int i = 1; i <= 12; i++)
            {
                cbMonth.Items.Add("Tháng " + i);
            }

            // *** MỚI: Thêm sự kiện ***
            // Thêm hàm xử lý sự kiện
            cbMonth.SelectedIndexChanged += new EventHandler(cbMonth_SelectedIndexChanged);


            // Đặt giá trị ban đầu (sẽ tự động chạy code lần đầu)
            cbMonth.SelectedIndex = 0;
        }

    }
}