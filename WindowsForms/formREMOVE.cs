using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RCProiect
{
    public partial class formREMOVE : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Folder\UTCN\An 4-Sem 1\RC\Proiect\RCProiect\Database1.mdf"";Integrated Security=True";

        public formREMOVE()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string usernameToRemove = textBox3.Text;

            if (string.IsNullOrEmpty(usernameToRemove))
            {
                MessageBox.Show("Please enter a username to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Remove the user from the Users table
            RemoveUser(usernameToRemove);
        }

        private void RemoveUser(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the user exists in the database
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Username", username);
                        int existingUserCount = (int)checkUserCommand.ExecuteScalar();

                        if (existingUserCount == 0)
                        {
                            MessageBox.Show("User not found. No changes were made.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Exit the method if the user doesn't exist
                        }
                    }

                    // Proceed with the removal if the user exists
                    string removeQuery = "DELETE FROM Users WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(removeQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear the textbox after successful removal
                            textBox3.Clear();
                        }
                        else
                        {
                            MessageBox.Show("User removal failed. No changes were made.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
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

        private void button6_Click_1(object sender, EventArgs e)
        {
            FormHelpRemove formHelpRemove = new FormHelpRemove();
            formHelpRemove.Show();
            this.Hide();
        }
    }
}
