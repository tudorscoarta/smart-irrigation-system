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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            formADD formADD = new formADD();
            formADD.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            formREMOVE formREMOVE = new formREMOVE();
            formREMOVE.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            formUPDATE formUPDATE = new formUPDATE();
            formUPDATE.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            FormOptions formoptions = new FormOptions();
            formoptions.Show();
            this.Hide();
        }
    }
}
