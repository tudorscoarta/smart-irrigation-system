using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCProiect
{
    public partial class Form2 : Form
    {
        private bool isToggleOn = true;
        private SerialPort myport;
        private bool isArduinoOn = true; // Toggle state
        public Form2()
        {
            InitializeComponent();
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = "com7";
            myport.Open();  // open the port once during form initialization
            UpdateArduinoState(); // Send initial command based on the toggle state
            myport.Close(); // Close the port immediately to avoid sending any commands during form creation
            myport.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
            //toggleButton = Controls.OfType<Button>().FirstOrDefault(b => b.Name == "ToggleButton");
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string receivedData = myport.ReadLine();
                Console.WriteLine("Arduino response: " + receivedData);


            }
            catch (TimeoutException)
            {
                Console.WriteLine("TimeoutException: No response from Arduino.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading Arduino response: " + ex.Message);
            }
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            isToggleOn = !isToggleOn;
            isArduinoOn = !isArduinoOn;

            if (isArduinoOn)
            {
                toggleButton.Text = "OFF";
                pictureBox5.Image = Image.FromFile(@"..\..\resurse\power button.png");
                ChangeButtonsStateAndColor();

            }
            else
            {
                toggleButton.Text = "ON";
                pictureBox5.Image = Image.FromFile(@"..\..\resurse\power off.png");
                ChangeButtonsStateAndColor();
            }


            // Update Arduino state after changing the toggle state
            UpdateArduinoState();
            UpdateButtonState();
        }

        private void ChangeButtonsStateAndColor()
        {
            button1.Enabled = isArduinoOn;
            button3.Enabled = isArduinoOn;
            button4.Enabled = isArduinoOn;
            button5.Enabled = isArduinoOn;
            button7.Enabled = isArduinoOn;
            textBox1.Enabled = isArduinoOn;

            if (isArduinoOn)
            {
                button1.Cursor = Cursors.Hand;
                button3.Cursor = Cursors.Hand;
                button4.Cursor = Cursors.Hand;
                button5.Cursor = Cursors.Hand;
                button7.Cursor = Cursors.Hand;

                button1.BackColor = Color.LightBlue;
                button3.BackColor = Color.LightBlue;
                button4.BackColor = Color.LightBlue;
                button5.BackColor = Color.LightBlue;
                button7.BackColor = Color.LightBlue;

            }
            else
            {
                button1.Cursor = Cursors.Default;
                button3.Cursor = Cursors.Default;
                button4.Cursor = Cursors.Default;
                button5.Cursor = Cursors.Default;
                button7.Cursor = Cursors.Default;

                button1.BackColor = Color.LightGray;
                button3.BackColor = Color.LightGray;
                button4.BackColor = Color.LightGray;
                button5.BackColor = Color.LightGray;
                button7.BackColor = Color.LightGray;
            }
        }

        private void UpdateButtonState()
        {
            Button toggleButton = Controls.OfType<Button>().FirstOrDefault();
            if (toggleButton != null)
            {
                //Check the Arduino state
                if (!myport.IsOpen)
                {
                    myport.Open();
                }
                string response = " ";
                isArduinoOn = response.Trim().Equals("Process ON", StringComparison.OrdinalIgnoreCase) ||
                              response.Trim().Equals("Process OFF", StringComparison.OrdinalIgnoreCase);

                // Update the button based on the correct state
                if (isArduinoOn)
                {
                    toggleButton.Text = "Turn Off";
                    toggleButton.BackColor = Color.LightBlue;
                }
                else
                {
                    toggleButton.Text = "Turn On";
                    toggleButton.BackColor = Color.LightGray;
                }
            }
        }



        private void UpdateArduinoState()
        {
            try
            {
                if (!myport.IsOpen)
                {
                    myport.Open();
                }

                if (isArduinoOn)
                {
                    myport.WriteLine("O");
                    myport.WriteLine("\n");// Send command to turn on the process
                    Console.WriteLine("Sent 'O' ");
                    isArduinoOn = true;
                }
                else
                {
                    myport.WriteLine("F"); // Send command to turn off the process
                    myport.WriteLine("\n");
                    Console.WriteLine("Sent 'F' ");
                    isArduinoOn = false;
                }

                // No need to close the port here, leave it open for subsequent commands

                // Update button text and color based on the toggle state
               

                //MessageBox.Show("Arduino process is now " + (isArduinoOn ? "on." : "off."));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Inside button7_Click (assuming you want to send 'T' command before 'A')

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                // Read humidity value from textBox1
                int humidityPercentage;
                if (int.TryParse(textBox1.Text, out humidityPercentage))
                {
                    // Convert the humidity value to the desired range (0% to 100% to 600 to 1010)
                    int convertedValue = (int)((humidityPercentage / 100.0) * (600 - 1010) + 1010);


                    //Open the serial port
                    if (!myport.IsOpen)
                    {
                        myport.Open();
                    }

                    // Send the converted value to the Arduino
                    myport.WriteLine("T" + convertedValue);
                    myport.WriteLine("\n");
                    Console.WriteLine("Sent humidity command: T" + convertedValue);

                    // Send 'A' for automatic mode
                    myport.WriteLine("A");
                    myport.WriteLine("\n");
                    Console.WriteLine("Sent 'A' ");

                    // Close the serial port
                    myport.Close();
                }
                else
                {
                    MessageBox.Show("Invalid humidity value. Please enter a valid percentage.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
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



        private void button8_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!myport.IsOpen)
                {
                    myport.Open();
                }

                //Send 'M' for manual mode

                myport.WriteLine("M");
                myport.WriteLine("\n");
                Console.WriteLine("Sent 'M' ");
                //Close the serial port
                myport.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!myport.IsOpen)
                {
                    myport.Open();
                }

                // Send 'A' for automatic mode
                myport.WriteLine("A");
                myport.WriteLine("\n");
                Console.WriteLine("Sent 'A' ");

                // Close the serial port
                myport.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!myport.IsOpen)
                {
                    myport.Open();
                }

                // Send 'A' for automatic mode
                myport.WriteLine("P");
                myport.WriteLine("\n");
                Console.WriteLine("Sent 'P' ");

                // Close the serial port
                myport.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!myport.IsOpen)
                {
                    myport.Open();
                }

                // Send 'A' for automatic mode
                myport.WriteLine("S");
                myport.WriteLine("\n");
                Console.WriteLine("Sent 'S' ");

                // Close the serial port
                myport.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2Help form2help = new Form2Help();
            form2help.Show();
            this.Hide();
        }
    }
}
