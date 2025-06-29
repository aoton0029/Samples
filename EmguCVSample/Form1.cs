namespace EmguCVSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            using (FormTrain frm = new FormTrain())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnAuthorize_Click(object sender, EventArgs e)
        {
            using (FormAuthorize frm = new FormAuthorize())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }
    }
}
