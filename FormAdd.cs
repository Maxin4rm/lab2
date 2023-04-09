using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.classes;
using WindowsFormsApp1.Factories;

namespace WindowsFormsApp1
{
    public partial class FormAdd : Form
    {
        public FormAdd()
        {
            InitializeComponent();
        }

        
        private Label passengersLabel = new Label();
        public static int mode = ADD_MODE;
        public const int ADD_MODE = 0;
        public const int VIEW_MODE = 1;
        public const int EDIT_MODE = 2;
        public static Transport currentVehicle;
        public static int editIndex;

        private void CreateLabel(Label label, string name, float fontSize, FontStyle fontStyle)
        {
            labelsPanel.Controls.Add(label);
            label.Dock = DockStyle.Top;
            label.Font = new Font("Arial", fontSize, fontStyle, GraphicsUnit.Point, ((byte)(204)));
            label.Location = new Point(0, 0);
            label.Name = name + "label";
            label.Size = new Size(223, 48);
            label.Margin = new Padding(0, 5, 0, 5);
            label.Text = name;
            label.TextAlign = ContentAlignment.TopRight;
            labelsPanel.Controls.SetChildIndex(label, 0);
        }

        private void CreateTextBox(TextBox textBox, string name, int count)
        {
            fieldsPanel.Controls.Add(textBox);
            textBox.Location = new Point(0, 46 + 33 * count + (count > 0 ? 15 * count : 0));
            textBox.Name = name + "TextBox";
            textBox.Size = new Size(407, 36);
            textBox.MaxLength = 15;
            textBox.Font = new Font("Arial", 12.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            textBox.KeyUp += new KeyEventHandler(this.Control_KeyUp);
            textBox.KeyPress += new KeyPressEventHandler(this.TextBox_KeyPress);
            fieldsPanel.Controls.SetChildIndex(textBox, 0);
        }

        private void CreateNumericUpDown(NumericUpDown numericUpDown, string name, int maxValue, int count)
        {
            fieldsPanel.Controls.Add(numericUpDown);
            numericUpDown.Location = new Point(0, 46 + 33 * count + (count > 0 ? 15 * count : 0));
            numericUpDown.Font = new Font("Arial", 12.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            numericUpDown.Name = name + "NumericUpDown";
            numericUpDown.Size = new Size(407, 36);
            numericUpDown.Maximum = maxValue;
            numericUpDown.Minimum = (maxValue == 2023) ? 1900 : 0;
            numericUpDown.Value = (maxValue == 2023) ? 2023 : 0;
            numericUpDown.KeyUp += new KeyEventHandler(this.Control_KeyUp);
            fieldsPanel.Controls.SetChildIndex(numericUpDown, 0);
        }

        private void CreateBool(CheckBox checkBox, string name, int count)
        {
            fieldsPanel.Controls.Add(checkBox);
            checkBox.Location = new Point(0, 46 + 33 * count + (count > 0 ? 15 * count : 0));
            checkBox.Font = new Font("Arial", 12.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            checkBox.Name = name + "NumericUpDown";
            checkBox.Size = new Size(407, 36);
            checkBox.KeyUp += new KeyEventHandler(this.Control_KeyUp);
            fieldsPanel.Controls.SetChildIndex(checkBox, 0);
        }

        private void CreateComboBox(ComboBox comboBox, string name, string[] items, int count)
        {
            fieldsPanel.Controls.Add(comboBox);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            comboBox.FormattingEnabled = true;
            comboBox.Items.AddRange(items);
            comboBox.Location = new Point(0, 46 + 33 * count + (count > 0 ? 15 * count : 0));
            comboBox.Name = name + "ComboBox";
            comboBox.Size = new Size(407, 36);
            comboBox.TabIndex = 0;
            comboBox.SelectedIndex = 0;
            comboBox.KeyUp += new KeyEventHandler(this.Control_KeyUp);
            fieldsPanel.Controls.SetChildIndex(comboBox, 0);
        }

        private void CreateControl(FieldInfo field, ref int count)
        {
            if (field.FieldType == typeof(String))
            {
                CreateTextBox(new TextBox(), field.Name, count++);
                return;
            }
            if (field.FieldType == typeof(Color))
            {
                //CreateTextBox(new TextBox(), field.Name, count++);
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Red", "Green", "Blue" }, count++);
                return;
            }
            if (field.FieldType == typeof(int))
            {
                CreateNumericUpDown(new NumericUpDown(), field.Name, Int32.MaxValue, count++);
                return;
            }
            if (field.FieldType == typeof(bool))
            {
                //CreateBool(new CheckBox(), field.Name, count++);
                CreateComboBox(new ComboBox(), field.Name, new string[] { "true", "false"}, count++);
                return;
            }
            if (field.FieldType == typeof(Bus.BusType))
            {
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Passenger", "Special"}, count++);
                return;
            }
            if (field.FieldType == typeof(FreightTrain.WagonType))
            {
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Tank", "Hopper", "Boxcar", "GondolaCar" }, count++);
                return;
            }
            if (field.FieldType == typeof(PassengerCar.BodyShape))
            {
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Hatchback", "Pickup", "Limousine", "Coupe", "Sedan", "Minivan" }, count++);
                return;
            }
            if (field.FieldType == typeof(PassengerTrain.DistanceType))
            {
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Suburban", "Local", "LongDistance" }, count++);
                return;
            }
            if (field.FieldType == typeof(Person.Sex))
            {
                CreateComboBox(new ComboBox(), field.Name, new string[] { "Male", "Female" }, count++);
                return;
            }
        }

        private void CreateSpecificControls(ref int count)
        {
            FieldInfo[] thisFields = GetThisFields();
            foreach (var field in thisFields)
            {
                object[] MyAttribute = field.GetCustomAttributes(typeof(NameAttribute), false);
                CreateLabel(new Label(), ((NameAttribute)(MyAttribute[0])).Name, 12.0F, FontStyle.Regular);
                CreateControl(field, ref count);
            }
        }

        private FieldInfo[] GetThisFields()
        {
            Type type = Type.GetType("WindowsFormsApp1.classes." + typeComboBox.Text);
            var obj = Activator.CreateInstance(type);
            var allFields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            FieldInfo[] thisFields = new FieldInfo[allFields.Length - 1];
            for (int i = 0; i < thisFields.Length; i++)
                thisFields[i] = allFields[i];
            return thisFields;
        }
        private void CreateNewLayout()
        {
            int count = 0;
            CreateSpecificControls(ref count);
            CreateLabel(new Label(), "Driver", 12.0F, FontStyle.Bold);
            count++;
            CreateLabel(new Label(), "Driver's firstname", 10.8F, FontStyle.Regular);
            CreateTextBox(new TextBox(), "firstname", count++);
            CreateLabel(new Label(), "Driver's lastname", 10.8F, FontStyle.Regular);
            CreateTextBox(new TextBox(), "lastname", count++);
            CreateLabel(new Label(), "Driver's sex", 10.8F, FontStyle.Regular);
            CreateComboBox(new ComboBox(), "sex", new string[] { "Male", "Female" }, count++);
            CreateLabel(new Label(), "Driver's age", 10.8F, FontStyle.Regular);
            CreateNumericUpDown(new NumericUpDown(), "age", 150, count++);
            addButton.Visible = true;
            this.CenterToScreen();
        }

        private bool nonNumberEntered = false;
        private bool backspaceEntered = false;

        private bool IsInputCorrect()
        {
            for (int i = 0; i < fieldsPanel.Controls.Count; i++)
            {
                var control = fieldsPanel.Controls[i];
                Type controlType = control.GetType();
                if (controlType == typeof(TextBox) && ((control as TextBox).Text.Length == 0 || (control as TextBox).Text.Length > 15))
                    return false;
                if (controlType == typeof(NumericUpDown) && ((control as NumericUpDown).Value > Int32.MaxValue))
                    return false;
                if (controlType == typeof(ComboBox) && ((control as ComboBox).SelectedIndex == -1))
                    return false;
            }
            return true;
        }

        Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>(){
            { typeof(Bus), typeof(Factories.BusFactory) },
            { typeof(Truck), typeof(Factories.TruckFactory) },
            { typeof(PassengerCar), typeof(Factories.PassengerCarFactory) },
            { typeof(PassengerTrain), typeof(Factories.PassengerTrainFactory) },
            { typeof(FreightTrain), typeof(Factories.FreightTrainFactory) }
        };

        private void CreateObject(bool add)
        {
            Type type = Type.GetType("WindowsFormsApp1.classes." + typeComboBox.SelectedItem.ToString());
            Factories.TransportFactory transportFactory = (Factories.TransportFactory)Activator.CreateInstance(dictionary[type]);
            List<string> fields = new List<string>();
            int count = fieldsPanel.Controls.Count;
            
            for (int i = fieldsPanel.Controls.Count - 2; i > -1; i--)
            {
                fields.Add(fieldsPanel.Controls[i].Text);
            }
            Transport transport = transportFactory.createTransport(fields);
            if (add)
            {
                FormMain.vehicles.Add(transport);
            }
            else
            {
                FormMain.vehicles[editIndex] = transport;
            }
        }

        private void FullfillFields(bool enable)
        {
            string itemName = currentVehicle.GetType().ToString();
            itemName = itemName.Substring(itemName.LastIndexOf('.') + 1);
            foreach (var item in typeComboBox.Items)
            {
                if ((string)item == itemName)
                {
                    typeComboBox.SelectedItem = item;
                }
            }
            FillDriverFields(enable);            
            foreach (var field in currentVehicle.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                string componentName;
                if (field.FieldType == typeof(String))
                {
                    componentName = field.Name + "TextBox";
                    var textBox = GetComponentByName(componentName);
                    (textBox as TextBox).Text = (string)currentVehicle.GetType().GetField(field.Name).GetValue(currentVehicle);
                    (textBox as TextBox).Enabled = enable;

                }
                else if (field.FieldType == typeof(int))
                {
                    componentName = field.Name + "NumericUpDown";
                    var numericUpDown = GetComponentByName(componentName);
                    (numericUpDown as NumericUpDown).Value = (int)currentVehicle.GetType().GetField(field.Name).GetValue(currentVehicle);
                    (numericUpDown as NumericUpDown).Enabled = enable;
                }
                else if (field.FieldType == typeof(Bus.BusType) || field.FieldType == typeof(FreightTrain.WagonType) || field.FieldType == typeof(PassengerTrain.DistanceType) || field.FieldType == typeof(PassengerCar.BodyShape) || field.FieldType == typeof(Person.Sex))
                {
                    componentName = field.Name + "ComboBox";
                    var comboBox = GetComponentByName(componentName);
                    foreach (var item in (comboBox as ComboBox).Items)
                    {
                        if (item == currentVehicle.GetType().GetField(field.Name).GetValue(currentVehicle))
                        {
                            (comboBox as ComboBox).SelectedItem = item;
                        }
                    }
                    (comboBox as ComboBox).Enabled = enable;
                }
            }
        }

        private void FillDriverFields(bool enable)
        {
            var firstNameTextBox = (TextBox)GetComponentByName("firstnameTextBox");
            var lastNameTextBox = (TextBox)GetComponentByName("lastnameTextBox");
            var ageNumericUpDown = (NumericUpDown)GetComponentByName("ageNumericUpDown");
            var sexComboBox = (ComboBox)GetComponentByName("sexComboBox");
            firstNameTextBox.Text = currentVehicle.driver.firstname;
            firstNameTextBox.Enabled = enable;
            lastNameTextBox.Text = currentVehicle.driver.lastname;
            lastNameTextBox.Enabled = enable;
            ageNumericUpDown.Value = currentVehicle.driver.age;
            ageNumericUpDown.Enabled = enable;
            foreach (var item in sexComboBox.Items)
                if ((string)item == currentVehicle.driver.sex.ToString())
                    sexComboBox.SelectedItem = item;
            sexComboBox.Enabled = enable;
        }

        private Component GetComponentByName(string name)
        {
            for (int i = 0; i < fieldsPanel.Controls.Count; i++)
            {
                if (name == fieldsPanel.Controls[i].Name)
                    return fieldsPanel.Controls[i];
            }
            return null;
        }

        private void DeleteOldLayout() 
        {
            int count = labelsPanel.Controls.Count;
            for (int i = 0; i < count && labelsPanel.Controls.Count > 1; i++)
            {
                Control c = labelsPanel.Controls[0];
                if (c != typeLabel)
                {
                    labelsPanel.Controls.Remove(c);
                    c.Dispose();
                }
            }
            count = fieldsPanel.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                Control c = fieldsPanel.Controls[0];
                if (c != typeComboBox)
                {
                    fieldsPanel.Controls.Remove(c);
                    c.Dispose();
                }
            }
        }

        private void typeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            labelsPanel.Visible = fieldsPanel.Visible = false;
            DeleteOldLayout();
            CreateNewLayout();
            labelsPanel.Visible = fieldsPanel.Visible = true;
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            switch (mode)
            {
                case EDIT_MODE:
                    {
                        captionLabel.Text = "Edit mode";
                        addButton.Text = "Confirm changes";
                        addButton.Enabled = true;
                        FullfillFields(true);
                        break;
                    }
                case VIEW_MODE:
                    {
                        captionLabel.Text = "View mode";
                        addButton.Text = "Back";
                        addButton.Enabled = true;
                        typeComboBox.Enabled = false;
                        FullfillFields(false);
                        break;
                    }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            switch (mode)
            {
                case ADD_MODE:
                    {
                        CreateObject(true);
                        break;
                    }
                case EDIT_MODE:
                    {
                        CreateObject(false);
                        break;
                    }
            }
            Close();
        }

        private void Control_KeyUp(object sender, KeyEventArgs e)
        {
            if (mode != VIEW_MODE)
                addButton.Enabled = IsInputCorrect();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var regex = new Regex(@"[^a-zA-Z\s\b]");
            if (regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void FormAdd_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
