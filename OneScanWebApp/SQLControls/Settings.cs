using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLControls
{
    public class Settings
    {
        public static void SetConnectionString(string conn)
        {
            Properties.Settings.Default.Database = conn;
            Properties.Settings.Default.Save();
        }
    }
}
