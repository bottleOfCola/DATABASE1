using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace DATABASE1
{
    public partial class BDHuman : Form
    {
        Data _d;
        int _str;
        int _wrkINstr = 10;
        public BDHuman(Index form, ref Data d)
        {
            InitializeComponent();
            _d = d;
            label1.Text = "Введите ФИО человека";
            label2.Text = "Выберите дату рождения";
            label3.Text = "Выберите место работы";
            label4.Text = "1-я страница";
            button1.Text = "Добавить";
            button3.Text = "На предыдущую страницу";
            button2.Text = "На следующую страницу";
            string[] wrks = new string[_d.Works.Count];
            for (int i = 0; i < _d.Works.Count; i++)
            {
                wrks[i] = _d.Works[i].Name;
            }
            listBox1.Items.AddRange(wrks);

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
            int WRKS = _d.Humans.Count;
            int fWandwINs = firstWrks + _wrkINstr;
            int lastwrk = fWandwINs > WRKS ? WRKS : fWandwINs;
            dataGridView1.Rows.Clear();

            for (; firstWrks < lastwrk; firstWrks++)
            {
                Human h = _d.Humans[firstWrks];
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = h.Fio;
                row.Cells[1].Value = h.Birthday;
                row.Cells[2].Value = h.Work.Name;
                dataGridView1.Rows.Add(row);
            }
            if (lastwrk == WRKS)
                button2.Enabled = false;
            else
                button2.Enabled = true;
            label4.Text = $"{_str + 1}-я страница";
        }
        public void StrIsFull()
        {
            int RC = dataGridView1.Rows.Count - 2;
            if (RC > _wrkINstr)
            {
                _str++;
                CreateTable();
            }
            else if (RC < _wrkINstr && _wrkINstr * _str + _wrkINstr <= _d.Humans.Count)
            {
                for (int i = RC; i < _wrkINstr; i++)
                {
                    Human h = _d.Humans[_wrkINstr * _str + RC];
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.Cells[0].Value = h.Fio;
                    row.Cells[1].Value = h.Birthday;
                    row.Cells[2].Value = h.Work.Name;
                    dataGridView1.Rows.Add(row);
                }
            }
            else if (RC == 0 && _str > 0)
            {
                _str--;
                CreateTable();
            }
            button2.Enabled = _d.Humans.Count > (_str + 1) * _wrkINstr ? true : false;

        }
        public void AddObj()
        {
            Work wrk = _d.GetWork(listBox1.SelectedItem.ToString());
            if (wrk is null)
                return;
            if (_d.HumanAdd(textBox1.Text, dateTimePicker1.Value, wrk) == false)
                return;

            if (dataGridView1.Rows.Count <= _wrkINstr)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = textBox1.Text;
                row.Cells[1].Value = dateTimePicker1.Value.GetDateTimeFormats()[0];
                row.Cells[2].Value = listBox1.Items[listBox1.SelectedIndex].ToString();
                dataGridView1.Rows.Add(row);
            }
        }

        private void dgv1UD(object? sender, DataGridViewRowCancelEventArgs e)
        {
            string fio = e.Row.Cells[0].Value.ToString();
            string dt = e.Row.Cells[1].Value.ToString();
            Work work = _d.GetWork(e.Row.Cells[2].Value.ToString());
            Human h = _d.GetHuman(fio,dt, work);
            if (h is null)
                return;
            _d.DeleteHuman(h);
            StrIsFull();
        }

        private void btn1(object? sender, EventArgs e)
        {
            AddObj();
            int potentialLSTstr = (_d.Humans.Count / _wrkINstr);
            if (_str != potentialLSTstr && _d.Humans.Count % _wrkINstr != 0)
            {
                _str = potentialLSTstr;
                CreateTable();
                button3.Enabled = true;
            }
        }
    }
}
