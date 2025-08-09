using CoreLib.Services;
using System.Threading.Tasks;

namespace FileSample
{
    public partial class Form1 : Form
    {
        JsonFileManager<ImprovementRequest> ImprovementService;
        public Form1()
        {
            InitializeComponent();
            ImprovementService = new JsonFileManager<ImprovementRequest>("improvementRequests.json");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await ImprovementService.Save(new List<ImprovementRequest>
            {
                new ImprovementRequest(textBox1.Text, textBox2.Text),
                new ImprovementRequest("user2", "Add dark mode support")
            }, textBox1.Text);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string content = await ImprovementService.Load(textBox1.Text);
            textBox3.Text = content;
        }
    }
}
