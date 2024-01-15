using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsForm
{
    public partial class Form2 : Form
    {

        DepartmentService departmentService;
        List<Department> departmentList;
        public Form2()
        {
            InitializeComponent();
            departmentService = new DepartmentService();
            departmentService.createConnection();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var departmentList = departmentService.GetDepartmentList();
            comboBox1.DataSource = departmentList;
            comboBox1.DisplayMember = "Description";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            Department newDepartment = new Department();
            newDepartment.description = textBox2.Text;

            int managerId;
            if (!int.TryParse(textBox3.Text, out managerId))
            {
                MessageBox.Show("Introduceți un număr valid pentru managerID.");
                return;
            }
            newDepartment.managerID = managerId;

            int parentId;
            if (!int.TryParse(textBox5.Text, out parentId))
            {
                MessageBox.Show("Introduceți un număr valid pentru parentID.");
                return;
            }
            newDepartment.parentID = parentId;

            bool insertionResult = departmentService.InsertDepartment(newDepartment);

            if (insertionResult)
            {
                MessageBox.Show("Inserare departamentului s-a realizat cu succes!");
            }
            else
            {
                MessageBox.Show("Inserarea departamentului a eșuat!");
            }

            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";

            RefreshDepartmentList();
        }
        private void RefreshDepartmentList()
        {
            departmentList = departmentService.GetDepartmentList();
            comboBox1.DataSource = departmentList;
            comboBox1.DisplayMember = "Description";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int departmentIdToDelete;
            if (!int.TryParse(textBox11.Text, out departmentIdToDelete))
            {
                MessageBox.Show("Introduceți un ID valid pentru ștergere.");
                return;
            }

            bool deletionResult = departmentService.DeleteDepartment(departmentIdToDelete);

            if (deletionResult)
            {
                MessageBox.Show("Ștergerea departamentului s-a realizat cu succes!");
            }
            else
            {
                MessageBox.Show("Ștergerea a eșuat - ID inexistent!");
            }

            textBox11.Text = "";

            RefreshDepartmentList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int departmentIdToUpdate;
            if (!int.TryParse(textBox6.Text, out departmentIdToUpdate))
            {
                MessageBox.Show("Introduceți un ID valid pentru actualizare.");
                return;
            }

            Department updatedDepartment = new Department();



            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                updatedDepartment.description = textBox7.Text;
            }
            int managerId;
            if (!string.IsNullOrEmpty(textBox8.Text) && int.TryParse(textBox8.Text, out managerId))
            {
                updatedDepartment.managerID = managerId;
            }
            int parentId;
            if (!string.IsNullOrEmpty(textBox10.Text) && int.TryParse(textBox10.Text, out parentId))
            {
                updatedDepartment.parentID = parentId;
            }

            bool updateResult = departmentService.UpdateDepartment(departmentIdToUpdate, updatedDepartment);

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
            textBox8.Text = "";
            textBox10.Text = "";

            RefreshDepartmentList();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
