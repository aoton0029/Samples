using CoreLibWinforms.Core.Permissions;
using CoreLibWinforms.Forms;

namespace PermissionSample
{
    public partial class Form1 : Form
    {
        private PermissionService service = new PermissionService();

        public Form1()
        {
            InitializeComponent();
            reload();
        }

        private void reload()
        {
            service.Load();

            btnISI.Visible = service.IsControlVisible("00000645", "PK", this.Name, btnISI.Name);
            btnPK.Visible = service.IsControlVisible("00000645", "PK", this.Name, btnPK.Name);
            btnKenkyu.Visible = service.IsControlVisible("00000645", "PK", this.Name, btnKenkyu.Name);
        }

        private void btnPerm_Click(object sender, EventArgs e)
        {
            using(FormPermission frm = new FormPermission())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if(frm.ShowDialog() == DialogResult.OK)
                {
                    // Handle the OK result if needed
                    reload();
                }
            }
        }

        private void btnPK_Click(object sender, EventArgs e)
        {

        }

        private void btnISI_Click(object sender, EventArgs e)
        {

        }

        private void btnKenkyu_Click(object sender, EventArgs e)
        {

        }
    }
}
