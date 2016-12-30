using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TouchBoardServerComms;

namespace TouchBoardWinServer
{
    public partial class AuthClientsList : Form
    {
        public AuthClientsList()
        {
            InitializeComponent();
        }       

        private void RemoveSelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Do you want to remove the selected Client?", "Warning", MessageBoxButtons.YesNo);
            if (result1 == System.Windows.Forms.DialogResult.Yes)
            {
                var selItem = ClientsListBox.SelectedItem as AuthClient;
                ClientUtil.RemoveClient(selItem);
                AuthClientsList_Load(null, null);
                MessageBox.Show("Client removed. Restart the server to apply it.", "Success", MessageBoxButtons.OK);
            }
        }

        private void AuthClientsList_Load(object sender, EventArgs e)
        {
             var list = ClientUtil.GetClients();
            if (list == null)
                list = new List<AuthClient>();

            ClientsListBox.DataSource = list;
            ClientsListBox.Refresh();

        }
    }

   
}
