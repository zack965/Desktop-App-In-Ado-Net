using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace first_ado
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }
        ADO d = new ADO();

        private void LogIn_Load(object sender, EventArgs e)
        {
            d.Connecter();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
