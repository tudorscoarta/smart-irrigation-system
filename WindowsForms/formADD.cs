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
    public partial class formADD : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Folder\UTCN\An 4-Sem 1\RC\Proiect\RCProiect\Database1.mdf"";Integrated Security=True";
        public formADD()
        {
            InitializeComponent();
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

        private void button6_Click(object sender, EventArgs e)
        {
            FormHelpAdd formHelpAdd = new FormHelpAdd();
            formHelpAdd.Show();
            this.Hide();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string username = textBox3.Text;
            string password = textBox1.Text;
            bool isAdmin = checkBox1.Checked;

            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Update user in the database
            if (UpdateUser(username, password, isAdmin))
            {
                //MessageBox.Show("User updated successfully.");
                ClearFields(); // Optional: Clear the fields after successful update
            }
            else
            {
                MessageBox.Show("Error updating user. Please try again.");
            }
        }

        private bool UpdateUser(string username, string password, bool isAdmin)
        {
            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the user already exists
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Username", username);
                        int existingUserCount = (int)checkUserCommand.ExecuteScalar();

                        if (existingUserCount > 0)
                        {
                            MessageBox.Show("User already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false; // Exit the method if the user already exists
                        }
                    }

                    // Use parameterized query to prevent SQL injection
                    string query = "INSERT INTO Users (Username, Password, isAdmin) VALUES (@Username, @Password, @isAdmin)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@isAdmin", isAdmin);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User added successfully.");
                            success = true;
                        }
                        else
                        {
                            MessageBox.Show("Error adding user. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return success;
        }




        private void ClearFields()
        {
            textBox3.Clear();
            textBox1.Clear();
            checkBox1.Checked = false;
        }

        private void UpdateAdminStatus(string username, string password, bool isAdmin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if the user already exists in the database
                    string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@Username", username);
                        int existingUserCount = (int)checkUserCommand.ExecuteScalar();

                        if (existingUserCount > 0)
                        {
                            MessageBox.Show("User already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Exit the method if the user already exists
                        }
                    }

                    // Proceed with the update if the user doesn't exist
                    string updateQuery = "UPDATE Users SET Password = @Password, isAdmin = @IsAdmin WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("User not found. No changes were made.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
