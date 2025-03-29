using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCProiect
{
    public partial class FormHelpRemove : Form
    {
        public FormHelpRemove()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formREMOVE formREMOVE = new formREMOVE();
            formREMOVE.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Create a list to store forms to close
            List<Form> formsToClose = new List<Form>();

            // Add all open forms to the list
            foreach (Form form in Application.OpenForms)
            {
                formsToClose.Add(form);
            }

            // Close all forms from the list
            foreach (Form form in formsToClose)
            {
                form.Close();
            }

            // Exit the application
            Application.Exit();
        }
    }
}
