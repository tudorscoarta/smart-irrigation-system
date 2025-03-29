using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RCProiect
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Folder\UTCN\An 4-Sem 1\RC\Proiect\RCProiect\Database1.mdf"";Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            foreach (Form form in Application.OpenForms)
            {
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox2.Text;
            string password = textBox1.Text;

            // Validate username and password against the database
            if (AuthenticateUser(username, password))
            {
                // If login is successful, check if the user is an admin
                bool isAdmin = CheckAdminStatus(username);

                // Hide or close Form1
                this.Hide(); // or this.Close(); if you want to close Form1

                // Open either Form2 or Form3 based on admin status
                if (isAdmin)
                {
                    FormOptions formoptions = new FormOptions();
                    formoptions.Show();
                }
                else
                {
                    Form2 form2 = new Form2();
                    form2.Show();
                }
            }
            // No need for an else block, as you don't want to display the login message
        }

        private bool CheckAdminStatus(string username)
        {
            // Add code to check if the user is an admin in the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT isAdmin FROM Users WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        // Assuming isAdmin column is of type BIT in the database
                        bool isAdmin = (bool)command.ExecuteScalar();

                        return isAdmin;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return false;
                }
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection opened successfully!");

                    // No need to hash the password for comparison, as the database stores plain text passwords
                    // Remove the HashPassword method call
                    string hashedPassword = password;

                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        int count = (int)command.ExecuteScalar();

                        Console.WriteLine("Count: " + count);

                        if (count > 0)
                        {
                            // Authentication successful
                            return true;
                        }
                        else
                        {
                            // Authentication failed
                            MessageBox.Show("Invalid username or password. Please try again.");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    Console.WriteLine("XD");
                    return false;
                }
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }
    }
}
