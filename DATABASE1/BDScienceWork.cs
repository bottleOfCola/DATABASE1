using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DATABASE1
{
    public partial class BDScienceWork : Form
    {
        Data _d;
        int _str;
        int _wrkINstr = 10;
        public BDScienceWork(Index indx, ref Data d)
        {
            InitializeComponent();
            _d = d;
            label1.Text = "Выберите человека";
            label2.Text = "Выберите ученую степень";
            label3.Text = "Введите код УДК";
            label4.Text = "Впишите псевдоним";
            label6.Text = "Введите индекс цитирования";
            label7.Text = "Введите число страниц";
            label8.Text = "1-я страница";
            button1.Text = "Добавить";
            button2.Text = "На следующую страницу";
            button3.Text = "На предыдущую страницу";

            string[] authors = new string[_d.Authors.Count];
            for(int i = 0; i < authors.Length; i++)
            {
                authors[i] = _d.Authors[i].Psevdonim;
            }
            listBox1.Items.AddRange(authors);

            string[] izdatelstva = new string[_d.Isdtvos.Count];
            for (int i = 0; i < izdatelstva.Length; i++)
            {
                izdatelstva[i] = _d.Isdtvos[i].Name;
            }
            listBox2.Items.AddRange(izdatelstva);

            button1.Click += btn1;
            button3.Click += PreStr;
            button2.Click += PostStr;
            button3.Enabled = false;
            dataGridView1.UserDeletingRow += dgv1UD;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;

            _str = 0;
            CreateTable();
        }

        private void PostStr(object? sender, EventArgs e)
        {
            _str++;
            CreateTable();
            button3.Enabled = _str == 0 ? false : true;
        }

        private void PreStr(object? sender, EventArgs e)
        {
            _str--;
            CreateTable();
            button3.Enabled = _str == 0 ? false : true;
        }

        public void CreateTable()
        {
            int firstWrks = _wrkINstr * _str;
            int WRKS = _d.ScienceWorks.Count;
            int fWandwINs = firstWrks + _wrkINstr;
            int lastwrk = fWandwINs > WRKS ? WRKS : fWandwINs;
            dataGridView1.Rows.Clear();

            for (; firstWrks < lastwrk; firstWrks++)
            {
                ScienceWork sw = _d.ScienceWorks[firstWrks];
                Author a = sw.Author;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = a.Psevdonim;
                row.Cells[1].Value = sw.Stranici;
                row.Cells[2].Value = sw.Udk;
                row.Cells[3].Value = sw.Date;
                row.Cells[4].Value = sw.WorkType;
                row.Cells[5].Value = sw.Indx;
                row.Cells[6].Value = sw.Isdtvo.Name;
                dataGridView1.Rows.Add(row);
            }
            if (lastwrk == WRKS)
                button2.Enabled = false;
            else
                button2.Enabled = true;
            label5.Text = $"{_str + 1}-я страница";
        }
        public void StrIsFull()
        {
            int RC = dataGridView1.Rows.Count - 2;
            if (RC > _wrkINstr)
            {
                _str++;
                CreateTable();
            }
            else if (RC < _wrkINstr && _wrkINstr * _str + _wrkINstr <= _d.ScienceWorks.Count)
            {
                for (int i = RC; i < _wrkINstr; i++)
                {
                    ScienceWork sw = _d.ScienceWorks[_wrkINstr * _str + RC];
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.Cells[0].Value = sw.Author.Psevdonim;
                    row.Cells[1].Value = sw.Stranici;
                    row.Cells[2].Value = sw.Udk;
                    row.Cells[3].Value = sw.Date;
                    row.Cells[4].Value = sw.WorkType;
                    row.Cells[5].Value = sw.Indx;
                    row.Cells[6].Value = sw.Isdtvo.Name;
                    dataGridView1.Rows.Add(row);
                }
            }
            else if (RC == 0 && _str > 0)
            {
                _str--;
                CreateTable();
            }

            button2.Enabled = _d.ScienceWorks.Count > (_str + 1) * _wrkINstr ? true : false;

        }
        public void AddObj()
        {
            Author author = _d.Authors[listBox1.SelectedIndex];
            DateTime authorsBth = DateTime.Parse(author.Human.Birthday);
            string psevdonim = author.Psevdonim;
            decimal dstr = (int)numericUpDown1.Value;
            int str;
            string udk = textBox1.Text;
            DateTime dt = dateTimePicker1.Value;
            string wType = textBox2.Text;
            decimal dindx = numericUpDown2.Value;
            int indx;
            Izdatelstvo iz = _d.GetIzdtvo(listBox2.Items[listBox2.SelectedIndex].ToString());
            if (dt < authorsBth || dt > DateTime.Now)
                return;
            string date = dt.GetDateTimeFormats()[0];

            if (dstr > int.MaxValue || dstr < 0)
                return;
            str = (int)dstr;

            if (dindx > int.MaxValue || dindx < 0)
                return;
            indx = (int)dindx;

            if (author is null || iz is null)
                return;

            if (_d.ScienceWorkAdd(str, udk, date, indx, wType, iz, author) == false)
                return;

            if (dataGridView1.Rows.Count <= _wrkINstr)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = psevdonim;
                row.Cells[1].Value = str;
                row.Cells[2].Value = udk;
                row.Cells[3].Value = date;
                row.Cells[4].Value = wType;
                row.Cells[5].Value = indx;
                row.Cells[6].Value = iz.Name;
                dataGridView1.Rows.Add(row);
            }
        }

        private void dgv1UD(object? sender, DataGridViewRowCancelEventArgs e)
        {
            ScienceWork sw = _d.GetScienceWork(e.Row.Index);
            if (sw is null)
                return;
            _d.DeleteScienceWork(sw);
            StrIsFull();
        }

        private void btn1(object? sender, EventArgs e)
        {
            AddObj();
            int potentialLSTstr = (_d.ScienceWorks.Count / _wrkINstr);
            if (_str != potentialLSTstr && _d.ScienceWorks.Count % _wrkINstr != 0)
            {
                _str = potentialLSTstr;
                CreateTable();
                button3.Enabled = true;
            }
        }
    }
}
