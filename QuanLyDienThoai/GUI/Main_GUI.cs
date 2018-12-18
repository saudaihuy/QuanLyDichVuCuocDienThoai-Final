using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDienThoai.GUI
{
    public partial class Main_GUI : Form
    {
        string name_admin;
        Timer t = new Timer();
        BUS.SimBUS sim = new BUS.SimBUS();
        public Main_GUI()
        {
            InitializeComponent();
        }
        public Main_GUI(string _name_admin)
        {
            name_admin = _name_admin;
            InitializeComponent();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;
            string time = "";
            if (hh < 10)
                time += "0" + hh;
            else
                time += hh;

            time += ":";
            if (mm < 10)
                time += "0" + mm;
            else
                time += mm;

            time += ":";
            if (ss < 10)
                time += "0" + ss;
            else
                time += ss;

            lbl_time.Text = time;
        }

        private void Main_GUI_Load(object sender, EventArgs e)
        {
            lbl_name_admin.Text = name_admin;
            t.Interval = 1000;
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
            add_user_control();
        }

        private void exit_winform_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void titlebar_service_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            pnl_all_service.SelectedPage = pnl_all_service.Pages[e.Item.Id] as INavigationPage;            
        }

        // Add user control của các dịch vụ vào navigationFrame1
        private void add_user_control()
        {
            pnl_service_customer.Controls.Add(new Customer_GUI.Customer_GUI());
            
            pnl_service_sim.Controls.Add(new Sim_GUI.Sim_GUI());
            pnl_service_account.Controls.Add(new Account_GUI.Account_GUI());
            pnl_service_contracts.Controls.Add(new Contract_GUI.Contract_GUI());
            pnl_service_orders.Controls.Add(new Bill_GUI.Bill_GUI());
            pnl_use_service.Controls.Add(new Detail_GUI.Detail_GUI());
            pnl_service_cal_time.Controls.Add(new Fare_GUI.Fare_GUI());
        }
        private void Print_MessageBox(string StringMessage, string title)
        {
            MessageBox.Show(StringMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        Random random = new Random();
        private void btn_Random_ItemClick(object sender, EventArgs e)
        {
            DateTime t1 = DateTime.Now.AddYears(-2);

            SaveFileDialog savefile = new SaveFileDialog();

            savefile.FileName = ".txt";

            savefile.Filter = "Text files (*.txt)|*.txt";

            var array = sim.GetAll_Array();
            if ( array != null)
            {
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(savefile.FileName))
                    {
                        sw.WriteLine("IDSIM\tTGBD\tTGKT");
                        int itemRows = 0;
                        while (itemRows < 1000)
                        {
                            t1 = t1.Add(RandomTimeSpan()).AddDays(RandomInteger(0,3));
                            DateTime t2 = t1.Add(RandomTimeSpan());
                            itemRows++;
                            var temp = RandomInteger(0,array.Length);
                            if (t1 > DateTime.Now || t2 > DateTime.Now)
                                break;
                            sw.WriteLine(array[temp] + "\t" + t1.ToString("dd/MM/yyyy HH:mm:ss") + "\t" + t2.ToString("dd/MM/yyyy HH:mm:ss"));                           
                        }
                    }
                    Print_MessageBox("Tạo log phát sinh ngẫu nhiên thành công !", "Thông báo tạo log ngẫu nhiên");
                    Process.Start(savefile.FileName);
                }
            }
            else
            {
                Print_MessageBox("Tạo log phát sinh ngẫu nhiên thất bại ! Không có bất kì dữ liệu SIM nào tồn tại !", "Thông báo tạo log ngẫu nhiên");
            }
        }
        private TimeSpan RandomTimeSpan()
        {
            TimeSpan start = TimeSpan.FromHours(0);
            TimeSpan end = TimeSpan.FromHours(24);
            int maxMinutes = (int)((end - start).TotalMinutes);
            int minutes = random.Next(maxMinutes);
            return start.Add(TimeSpan.FromMinutes(minutes));
        }
        private int RandomInteger(int min, int max)
        {
            return random.Next(min, max);
        }

        private void link_website_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost:52913/ACCOUNTs/Login");
        }
    }
}
