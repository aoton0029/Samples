using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibWinforms.Core.Histories;

namespace ProjectSample.Core
{
    public class AppContext
    {
        private static Lazy<AppContext> _instance = new Lazy<AppContext>(() => new AppContext());
        public static AppContext Instance => _instance.Value;

        public HistoryService historyService { get; set; } = new HistoryService();

        public AppContext()
        {

        }


    }
}
