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
    public partial class BDAuthor : Form
    {
        Data _d;
        int _str;
        int _wrkINstr = 10;
        public BDAuthor(Index form, ref Data d)
        {
            InitializeComponent();
            
            _d = d;
            label1.Text = "Выберите человека";
            label2.Text = "Выберите ученую степень";
            label3.Text = "Введите индекс цитирования";
            label4.Text = "Впишите псевдоним";
            label5.Text = "1-я страница";
            button1.Text = "Добавить";
            button2.Text = "На следующую страницу";
            button3.Text = "На предыдущую страницу";
            string[] hmns = new string[_d.Humans.Count];
            for (int i = 0; i < hmns.Length; i++)
            {
                Human h = _d.Humans[i];
                hmns[i] = h.Fio+"|"+h.Birthday+"|"+h.Work.Name;
            }
            listBox1.Items.AddRange(hmns);

            string[] stpns = new string[_d.Stepens.Count];
            for(int i = 0; i < stpns.Length; i++)
            {
                stpns[i] = _d.Stepens[i].Name;
            }
            listBox2.Items.AddRange(stpns);

            button1.Click += btn1;
            button3.Click += PreStr;
            button2.Click += PostStr;
            button3.Enabled = false;
            dataGridView1.UserDeletingRow += dgv1UD;

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
            int WRKS = _d.Authors.Count;
            int fWandwINs = firstWrks + _wrkINstr;
            int lastwrk = fWandwINs > WRKS ? WRKS : fWandwINs;
            dataGridView1.Rows.Clear();

            for (; firstWrks < lastwrk; firstWrks++)
            {
                Author a = _d.Authors[firstWrks];
                Human h = a.Human;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = a.Psevdonim;
                row.Cells[1].Value = h.Fio + "|" + h.Birthday + "|" + h.Work.Name;
                row.Cells[2].Value = a.Stepen.Name;
                row.Cells[3].Value = a.Indx;
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
            else if (RC < _wrkINstr && _wrkINstr * _str + _wrkINstr <= _d.Authors.Count)
            {
                for (int i = RC; i < _wrkINstr; i++)
                {
                    Author a = _d.Authors[_wrkINstr * _str + RC];
                    Human h = a.Human;
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.Cells[0].Value = a.Psevdonim;
                    row.Cells[1].Value = h.Fio + "|" + h.Birthday + "|" + h.Work.Name;
                    row.Cells[2].Value = a.Stepen.Name;
                    row.Cells[3].Value = a.Indx;
                    dataGridView1.Rows.Add(row);
                }
            }
            else if (RC == 0 && _str > 0)
            {
                _str--;
                CreateTable();
            }

            button2.Enabled = _d.Authors.Count > (_str + 1) * _wrkINstr ? true : false;

        }
        public void AddObj()
        {
            Human hn = _d.Humans[listBox1.SelectedIndex];

            decimal indx = numericUpDown1.Value;
            Human hmn = _d.GetHuman(hn.Fio, hn.Birthday, hn.Work);
            Stepen stpn = _d.GetStepen(listBox2.SelectedItem.ToString());
            string psevdonim = textBox1.Text;

            if (hmn is null || stpn is null || indx > int.MaxValue)
                return;
            if (_d.AuthorAdd(hmn, stpn, (int)indx, psevdonim) == false)
                return;

            if (dataGridView1.Rows.Count <= _wrkINstr)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value = psevdonim;
                row.Cells[1].Value = hmn.Fio;
                row.Cells[2].Value = stpn.Name;
                row.Cells[3].Value = indx;
                dataGridView1.Rows.Add(row);
            }
        }

        private void dgv1UD(object? sender, DataGridViewRowCancelEventArgs e)
        {
            string psevdonim = e.Row.Cells[0].Value.ToString();
            Author author = _d.GetAuthor(psevdonim);
            if (author is null)
                return;
            _d.DeleteAuthor(author);
            StrIsFull();
        }

        private void btn1(object? sender, EventArgs e)
        {
            AddObj();
            int potentialLSTstr = (_d.Authors.Count / _wrkINstr);
            if (_str != potentialLSTstr && _d.Authors.Count % _wrkINstr != 0)
            {
                _str = potentialLSTstr;
                CreateTable();
                button3.Enabled = true;
            }
        }

    }
}
