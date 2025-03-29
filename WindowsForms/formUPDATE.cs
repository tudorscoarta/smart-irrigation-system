using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCProiect
{
    public partial class formUPDATE : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Folder\UTCN\An 4-Sem 1\RC\Proiect\RCProiect\Database1.mdf"";Integrated Security=True";

        public formUPDATE()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormHelpUpdate formHelpUpdate = new FormHelpUpdate();
            formHelpUpdate.Show();
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

        private void button4_Click(object sender, EventArgs e)
        {
            string username = textBox3.Text;
            string password = textBox1.Text;
            bool isAdmin = checkBox1.Checked;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            // Update the user information in the Users table
            UpdateUserInfo(username, password, isAdmin);
        }

        private void UpdateUserInfo(string username, string password, bool isAdmin)
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
                            MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Exit the method if the user doesn't exist
                        }
                    }

                    // Proceed with the update if the user exists
                    string updateQuery;

                    if (string.IsNullOrEmpty(password))
                    {
                        // If the password is empty, update only isAdmin
                        updateQuery = "UPDATE Users SET isAdmin = @IsAdmin WHERE Username = @Username";
                    }
                    else
                    {
                        // If the password is provided, update both password and isAdmin
                        updateQuery = "UPDATE Users SET Password = @Password, isAdmin = @IsAdmin WHERE Username = @Username";
                    }

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        // Add password parameter only if it's not empty
                        if (!string.IsNullOrEmpty(password))
                        {
                            command.Parameters.AddWithValue("@Password", password);
                        }

                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear textboxes and checkbox after successful update
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            textBox3.Clear();
            textBox1.Clear();
            checkBox1.Checked = false;
        }

    }
}
