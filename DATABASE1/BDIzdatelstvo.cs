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
    public partial class BDIzdatelstvo : Form
    {
        Data _d;
        int _str;
        int _wrkINstr = 10;
        public BDIzdatelstvo(Form form, ref Data d)
        {
            InitializeComponent();

            dataGridView1.Columns[0].Width = dataGridView1.Width - 43;
            label1.Text = "Введите название работы";
            label2.Text = "1-я страница";
            button1.Text = "Добавить";
            button2.Text = "Удалить по названию";
            button3.Text = "На предыдущую страницу";
            button4.Text = "На следующую страницу";
            button1.Click += btn1;
            textBox1.KeyUp += TextBox1_KeyUp;
            button2.Click += btn2;
            button3.Click += PreStr;
            button4.Click += PostStr;
            button3.Enabled = false;
            dataGridView1.UserDeletingRow += dgv1UD;

            _d = d;
            _str = 0;
            CreateTable();
        }

        private void TextBox1_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn1(sender, e);
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
            int WRKS = _d.Isdtvos.Count;
            int fWandwINs = firstWrks + _wrkINstr;
            int lastwrk = fWandwINs > WRKS ? WRKS : fWandwINs;
            dataGridView1.Rows.Clear();

            for (; firstWrks < lastwrk; firstWrks++)
            {
                dataGridView1.Rows.Add(_d.Isdtvos[firstWrks].Name);
            }
            if (lastwrk == WRKS)
                button4.Enabled = false;
            else
                button4.Enabled = true;
            label2.Text = $"{_str + 1}-я страница";
        }
        public void StrIsFull()
        {
            int RC = dataGridView1.Rows.Count - 2;
            if (RC > _wrkINstr)
            {
                _str++;
                CreateTable();
            }
            else if (RC < _wrkINstr && _wrkINstr * _str + _wrkINstr <= _d.Isdtvos.Count)
            {
                for (int i = RC; i < _wrkINstr; i++)
                {
                    dataGridView1.Rows.Add(_d.Isdtvos[_wrkINstr * _str + RC].Name);
                }
            }
            else if (RC == 0 && _str > 0)
            {
                _str--;
                CreateTable();
            }
            button4.Enabled = _d.Isdtvos.Count > (_str + 1) * _wrkINstr ? true : false;

        }
        public void AddObj()
        {
            string izdtvoName = textBox1.Text;
            if (izdtvoName.Length > 3)
            {
                if (_d.IzdtvoAdd(izdtvoName) == false)
                {
                    MessageBox.Show("Такое издательство уже зарегестрировано!");
                    return;
                }
                if (dataGridView1.Rows.Count <= _wrkINstr)
                    dataGridView1.Rows.Add(izdtvoName);
            }
        }

        private void dgv1UD(object? sender, DataGridViewRowCancelEventArgs e)
        {
            _d.DeleteIzdtvo(e.Row.Cells[0].Value.ToString());
            StrIsFull();
        }

        private void btn1(object? sender, EventArgs e)
        {
            AddObj();
            int potentialLSTstr = (_d.Isdtvos.Count / _wrkINstr);
            if (_str != potentialLSTstr && _d.Isdtvos.Count % _wrkINstr != 0)
            {
                _str = potentialLSTstr;
                CreateTable();
                button3.Enabled = true;
            }
        }

        private void btn2(object? sender, EventArgs e)
        {
            string nameOfIzdtvo = textBox1.Text;
            if (nameOfIzdtvo.Length > 3)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (nameOfIzdtvo == row.Cells[0].Value.ToString())
                    {
                        dataGridView1.Rows.Remove(row);
                        _d.DeleteIzdtvo(nameOfIzdtvo);
                        textBox1.Text = "";
                        dataGridView1.Refresh();
                        StrIsFull();
                        return;
                    }
                }
            }
        }
    }
}
