namespace DATABASE1
{
    public partial class Index : Form
    {
        Data _d;
        public Index()
        {
            InitializeComponent();

            button1.Click += btn1;
            button2.Click += btn2;
            button3.Click += btn3;
            button4.Click += btn4;
            button5.Click += btn5;
            button6.Click += btn6;
            button1.Text = "БД Работы";
            button2.Text = "БД Издательства";
            button3.Text = "БД Люди";
            button4.Text = "БД Авторы";
            button5.Text = "БД Научные работы";
            button6.Text = "БД Степени";
            _d = new Data();
        }

        private void btn1(object? sender, EventArgs e)
        {
            BDWork newForm = new BDWork(this, ref _d);
            newForm.ShowDialog();
        }
        private void btn2(object? sender, EventArgs e)
        {
            BDIzdatelstvo newForm = new BDIzdatelstvo(this, ref _d);
            newForm.ShowDialog();
        }
        private void btn3(object? sender, EventArgs e)
        {
            BDHuman newForm = new BDHuman(this, ref _d);
            newForm.ShowDialog();

        }
        private void btn4(object? sender, EventArgs e)
        {
            BDAuthor newForm = new BDAuthor(this, ref _d);
            newForm.ShowDialog();
        }
        private void btn5(object? sender, EventArgs e)
        {
            BDScienceWork newForm = new BDScienceWork(this, ref _d);
            newForm.ShowDialog();
        }
        private void btn6(object? sender, EventArgs e)
        {
            BDStepen newForm = new BDStepen(this, ref _d);
            newForm.ShowDialog();
        }
    }
}