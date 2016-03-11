using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
namespace BTLT1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String connectionString = "";
        public MainWindow()
        {
            connectionString = @"Data Source=(localdb)\socnau;Initial Catalog=QLDongHo;Integrated Security=True";
            InitializeComponent();
            binddatagrid();
            //loadcb();
            txtmasp.Text = mactauto();
        }

        private void binddatagrid()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "Select * from [SANPHAM]";
            command.Connection = connection;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            gl.ItemsSource = dt.DefaultView;
            connection.Close();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            //sqliteConnection

        }
        //public void loadcb()
        //{
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand();
        //    command.CommandText = "Select * from [SANPHAM]";
        //    command.Connection = connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    DataSet dataset = new DataSet();
        //    //manv,ten
        //}
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có muốn xóa", "Xóa đi", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
             {
                 if (gl.SelectedItems.Count > 0)
                 {
                     DataRowView drv = (DataRowView)gl.SelectedItem;
                     String id = drv.Row[0].ToString();
                     SqlConnection connection = new SqlConnection(connectionString);
                     connection.Open();
                     SqlCommand command = new SqlCommand("delete from SANPHAM where MaSP = @idMaSP", connection);
                     command.Parameters.AddWithValue("@idMaSP", id);
                     command.ExecuteNonQuery();
                     string sql = "select * from SANPHAM";
                     SqlCommand cm2 = new SqlCommand(sql, connection);
                     SqlDataAdapter adapter = new SqlDataAdapter(cm2);
                     DataTable dt = new DataTable();
                     adapter.Fill(dt);
                     gl.ItemsSource = dt.DefaultView;
                     MessageBox.Show("Xóa thành công");
                     connection.Close();
                 }
             }
            else if (result == MessageBoxResult.No)
            {
                //...
            }             
        }
        public string mactauto()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            DataTable dt = new DataTable(); 

            string sql = @"select * from SANPHAM";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            adapter.Fill(dt);

            string ma = "";
            if (dt.Rows.Count <= 0)
            {
                ma = "DH001";
            }
            else
            {
                ma = "DH";
                int k = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1][0].ToString().Substring(2, 3));
                k += 1;
                if (k < 10)
                {
                    ma = ma + "00";
                }
                else if (k < 100)
                {
                    ma = ma + "0";
                }
                ma = ma + k.ToString();
            }
            return ma;

        }
        //chỉ được nhập số thêm PreviewTextInput="NumberValidationTextBox" vào trong textbox cần làm 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "insert into SANPHAM(MaSP,Ten,Ngaysx,Pin,Trongluong,Color,Chatlieu,Timebaohanh,Dongia,Xuatxu) values(@MaSP,@Ten,@Ngaysx,@Pin,@Trongluong,@Color,@Chatlieu,@Timebaohanh,@Dongia,@Xuatxu)";
                command.Parameters.AddWithValue("@MaSP", txtmasp.Text);
                command.Parameters.AddWithValue("@Ten", txtten.Text);
                command.Parameters.AddWithValue("@Ngaysx", dtngaysx.SelectedDate.ToString());
                command.Parameters.AddWithValue("@Pin", txtpin.Text);
                command.Parameters.AddWithValue("@Trongluong", txttrongluong.Text);
                command.Parameters.AddWithValue("@Color", cbocolor.SelectedValue.ToString());
                command.Parameters.AddWithValue("@Chatlieu", txtchatlieu.Text);
                command.Parameters.AddWithValue("@Timebaohanh", dttimebaohanh.SelectedDate.ToString());
                command.Parameters.AddWithValue("@Dongia", txtdongia.Text);
                command.Parameters.AddWithValue("@Xuatxu", txtxuatxu.Text);
                command.Connection = connection;
                MessageBox.Show("Thêm thành công");
                command.ExecuteNonQuery();
                string sql = "select * from SANPHAM";
                SqlCommand cm = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gl.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch { MessageBox.Show("Xót thông tin dữ liệu"); }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "update SANPHAM set ten = N'"+txtten.Text+"',Ngaysx = '" + dtngaysx.SelectedDate.ToString()+"',Pin = " + txtpin.Text + ", Trongluong = " + txttrongluong.Text + ", Color = N'"+cbocolor.SelectionBoxItemStringFormat.ToString()+"',Chatlieu = N'" + txtchatlieu.Text + "', Timebaohanh = '" + dttimebaohanh.SelectedDate.ToString() + "', Dongia = " + txtdongia.Text + ", Xuatxu = N'"+txtxuatxu.Text+ "' where MaSP = " + txtmasp.Text +"";
                command.Connection = connection;
                MessageBox.Show("Sửa thành công");
                command.ExecuteNonQuery();
                string sql = "select * from SANPHAM";
                SqlCommand cm2 = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cm2);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gl.ItemsSource = dt.DefaultView;
                connection.Close();
        }
    }
}
