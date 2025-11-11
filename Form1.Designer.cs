namespace DEMO_PieChart
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pieChart1 = new LiveCharts.WinForms.PieChart();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.dgvBrandStats = new System.Windows.Forms.DataGridView();
            this.dgvTotal = new System.Windows.Forms.DataGridView();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrandStats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // pieChart1
            // 
            this.pieChart1.Location = new System.Drawing.Point(94, 82);
            this.pieChart1.Name = "pieChart1";
            this.pieChart1.Size = new System.Drawing.Size(772, 604);
            this.pieChart1.TabIndex = 0;
            this.pieChart1.Text = "pieChart1";
            this.pieChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.pieChart1_ChildChanged);
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(890, 426);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(582, 281);
            this.cartesianChart1.TabIndex = 1;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // dgvBrandStats
            // 
            this.dgvBrandStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBrandStats.Location = new System.Drawing.Point(974, 82);
            this.dgvBrandStats.Name = "dgvBrandStats";
            this.dgvBrandStats.RowHeadersWidth = 51;
            this.dgvBrandStats.RowTemplate.Height = 24;
            this.dgvBrandStats.Size = new System.Drawing.Size(418, 150);
            this.dgvBrandStats.TabIndex = 2;
            // 
            // dgvTotal
            // 
            this.dgvTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTotal.Location = new System.Drawing.Point(974, 238);
            this.dgvTotal.Name = "dgvTotal";
            this.dgvTotal.RowHeadersWidth = 51;
            this.dgvTotal.RowTemplate.Height = 24;
            this.dgvTotal.Size = new System.Drawing.Size(418, 54);
            this.dgvTotal.TabIndex = 3;
            // 
            // cbMonth
            // 
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Location = new System.Drawing.Point(47, 52);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(121, 24);
            this.cbMonth.TabIndex = 4;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1553, 719);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.dgvTotal);
            this.Controls.Add(this.dgvBrandStats);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.pieChart1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrandStats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTotal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.PieChart pieChart1;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private System.Windows.Forms.DataGridView dgvBrandStats;
        private System.Windows.Forms.DataGridView dgvTotal;
        private System.Windows.Forms.ComboBox cbMonth;
    }
}

