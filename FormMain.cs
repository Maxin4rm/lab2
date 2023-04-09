using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.classes;

namespace WindowsFormsApp1
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        public static List<Transport> vehicles = new List<Transport>();

        private void DisplayVehicles()
        {
            while (vehiclesGV.Rows.Count > 0)
                vehiclesGV.Rows.RemoveAt(0);
            int i = 1;
            foreach (Transport transport in vehicles)
            {
                vehiclesGV.Rows.Add(new object[] {
                    i,
                    transport.GetType().Name,
                    transport.Model,
                    transport.MaxSpeed
                });
            }
        }
        private void AddButton_Click(object sender, EventArgs e)
        {

            Color a = Color.Red;

            FormAdd addForm = new FormAdd();
            FormAdd.mode = FormAdd.ADD_MODE;
            addForm.ShowDialog();
            DisplayVehicles();
            
        }
        
        private void ViewButton_Click(object sender, EventArgs e)
        {
            
            FormAdd addForm = new FormAdd();
            FormAdd.mode = FormAdd.VIEW_MODE;
            FormAdd.currentVehicle = vehicles[vehiclesGV.CurrentRow.Index];
            addForm.ShowDialog();
        }
        
        private void EditButton_Click(object sender, EventArgs e)
        {
            
            FormAdd addForm = new FormAdd();
            FormAdd.mode = FormAdd.EDIT_MODE;
            FormAdd.currentVehicle = vehicles[vehiclesGV.CurrentRow.Index];
            FormAdd.editIndex = vehiclesGV.CurrentRow.Index;
            addForm.ShowDialog();
            DisplayVehicles();
            
        }

        private void vehiclesGV_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ViewButton.Enabled = EditButton.Enabled = DeleteButton.Enabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Transport temp = vehicles[vehiclesGV.CurrentRow.Index];
            vehicles.RemoveAt(vehiclesGV.CurrentRow.Index);
            temp.driver = null;
            /*for (int i = 0; i < temp.passengers.Count; i++)
            {
                temp.passengers[i] = null;
            }
            temp.passengers = null;*/
            temp = null;
            vehiclesGV.Rows.RemoveAt(vehiclesGV.CurrentRow.Index);
            if (vehiclesGV.Rows.Count == 0)
            {
                ViewButton.Enabled = EditButton.Enabled = DeleteButton.Enabled = false;
            }
        }
    }
}
