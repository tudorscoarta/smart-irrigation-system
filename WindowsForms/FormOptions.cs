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
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
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

        private void button6_Click(object sender, EventArgs e)
        {
            FormAdminHelp formAdminHelp = new FormAdminHelp();
            formAdminHelp.Show();
            this.Hide();
        }
    }
}
