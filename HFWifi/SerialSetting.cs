using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HFWifi
{
    public partial class SerialSetting : Form
    {
        public SerialSetting()
        {
            InitializeComponent();
        }
        public int Baud
        {
            get { return int.Parse(this.Baudrate.SelectedItem.ToString()); }
        }
        public string Port
        {
            get { return this.SerialNum.SelectedItem.ToString(); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
