using CoreLib.Cmds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockerDesktopSample
{
    public partial class UcPageContainer : UserControl
    {
        private readonly DockerWslExecutor _dockerWslExecutor;

        public UcPageContainer(DockerWslExecutor executor)
        {
            InitializeComponent();
            _dockerWslExecutor = executor;
        }

        private async void btnReload_Click(object sender, EventArgs e)
        {
            var containers = await _dockerWslExecutor.GetContainersAsync(true);
        }
    }
}
