using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsForm
{
    public partial class Form1 : Form
    {

        EmployeeService employeeService;
        List<Employee> employeeList;

        public Form1()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
            employeeService.createConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var employeeList = employeeService.GetEmployeeList();

            comboBox1.DataSource = employeeList;
            comboBox1.DisplayMember = "Name";
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            Employee newEmployee = new Employee();
            newEmployee.name = textBox2.Text;

            int departmentId;
            if (!int.TryParse(textBox3.Text, out departmentId))
            {
                MessageBox.Show("Introduceți un număr valid pentru departmentID.");
                return;
            }
            newEmployee.departmentID = departmentId;
            if (checkBox1.Checked) 
            {
                checkBox1.Checked = true;
                int managerId = GetManagerIdForDepartment(departmentId);
                if (managerId == -1)
                {
                    MessageBox.Show("Nu s-a găsit managerID pentru departmentID-ul introdus.");
                    return;
                }
                newEmployee.managerID = managerId;
            }
            

            newEmployee.email = textBox5.Text;

            bool insertionResult = employeeService.InsertEmployee(newEmployee);

            if (insertionResult)
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;

                MessageBox.Show("Inserare angajatului s-a realizat cu succes!");
            }
            else
            {
                MessageBox.Show("Inserare angajatului a eșuat!");
            }
            RefreshEmployeeList();
        }


     
        private void RefreshEmployeeList()
        {
            employeeList = employeeService.GetEmployeeList();
            comboBox1.DataSource = employeeList;
            comboBox1.DisplayMember = "Name";
        }

       public int GetManagerIdForDepartment(int departmentId)
        {
            string connectionString = "server=localhost;user=root;password=ADDcamera321!;database=departmentmanagement";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT managerId FROM Department WHERE departmentId = @DepartmentId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Eroare: " + ex.Message);
                        return -1;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }




        private void btnUpdate_Click(object sender, EventArgs e)
        {
            {
                int employeeIdToUpdate;
                if (!int.TryParse(textBox6.Text, out employeeIdToUpdate))
                {
                    MessageBox.Show("Introduceți un ID valid pentru actualizare.");
                    return;
                }

                Employee updatedEmployee = new Employee();

                if (!string.IsNullOrEmpty(textBox7.Text))
                {
                    updatedEmployee.name = textBox7.Text;
                }
                if (!string.IsNullOrEmpty(textBox10.Text))
                {
                    updatedEmployee.email = textBox10.Text;
                }

                int departmentId;
                if (!string.IsNullOrEmpty(textBox8.Text) && int.TryParse(textBox8.Text, out departmentId))
                {

                    int managerId = GetManagerIdForDepartment(departmentId);

                    
                    bool wasManager = managerId != 0;

                    
                    DialogResult result = MessageBox.Show("Doriți să fie manager?", "Confirmare", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        
                        if (managerId != -1)
                        {
                            updatedEmployee.departmentID = departmentId;
                            updatedEmployee.managerID = managerId;
                        }
                        else
                        {
                            MessageBox.Show("DepartmentID-ul introdus nu există în baza de date.");
                            return;
                        }
                    }
                    else
                    {
                        
                        updatedEmployee.departmentID = departmentId;
                        updatedEmployee.managerID = 0;
                        MessageBox.Show("Angajatul nu va fi manager. ManagerID-ul setat la 0.");
                    }
                }
                bool updateResult = employeeService.UpdateEmployee(employeeIdToUpdate, updatedEmployee, this);

                if (updateResult)
                {
                    MessageBox.Show("Actualizare reușită în baza de date!");
                }
                else
                {
                    MessageBox.Show("Eroare la actualizare sau ID inexistent!");
                }


                textBox6.Text = "";
                textBox7.Text = "";
                textBox10.Text = "";
                textBox8.Text = "";

                RefreshEmployeeList();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int employeeIdToDelete;
            if (!int.TryParse(textBox11.Text, out employeeIdToDelete))
            {
                MessageBox.Show("Introduceți un ID valid pentru ștergere.");
                return;
            }

            bool deletionResult = employeeService.DeleteEmployee(employeeIdToDelete);

            if (deletionResult)
            {
                MessageBox.Show("Ștergere angajatului s-a realizat cu succes!");
            }
            else
            {
                MessageBox.Show("Ștergerea a eșuat - ID inexistent!");
            }

            textBox11.Text = "";
            RefreshEmployeeList();
        }
    }
}