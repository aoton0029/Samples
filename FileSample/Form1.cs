using CoreLib.Merges;
using CoreLib.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileSample
{
    public partial class Form1 : Form
    {
        JsonFileService<ImprovementRequest> _service;
        public Form1()
        {
            InitializeComponent();
            _service = new JsonFileService<ImprovementRequest>();
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var (result, model) = await _service.ReadModelAsync("—v–].json");
                txtLoaded.Text = model?.ToString() ?? "No data loaded.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await _service.SaveModelWithSessionAsync(
                    "—v–].json"
                    , new ImprovementRequest(txtUserId.Text, txtDesc.Text)
                    , txtUserId.Text
                    , ConflictResolutionStrategy.AutoMerge);

                if (!result.Success || result.ConflictInfo != null)
                {
                    Debug.Print(result.Model?.ToString());
                    Debug.Print(result.ConflictInfo?.ToString());
                }
                else
                {
                    Debug.Print("Save successful.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
