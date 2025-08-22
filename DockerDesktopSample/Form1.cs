namespace DockerDesktopSample
{
    public partial class Form1 : Form
    {
        public static Color SelectedColor = Color.FromArgb(27, 81, 117);

        CoreLib.Cmds.DockerWslExecutor dockerWslExecutor = new CoreLib.Cmds.DockerWslExecutor();
        UcPageContainer ucPageContainer;
        UcPageImages ucPageImages;

        public Form1()
        {
            InitializeComponent();
            ucPageContainer = new UcPageContainer(dockerWslExecutor);
            ucPageImages = new UcPageImages();

            changePages(ucPageContainer);
        }

        private void changePages(UserControl page)
        {
            pnlMain.Controls.Clear();
            page.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(page);
        }

        private void OnClickBtn(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Name)
                {
                    case "btnContainer":
                        changePages(ucPageContainer);
                        break;
                    case "btnImages":
                        changePages(ucPageImages);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
