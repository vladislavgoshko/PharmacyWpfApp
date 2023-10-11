using BLLPharmacy;
using ClassLibraryPharmacy.DBModels;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfAppPharmacy
{
    /// <summary>
    /// Логика взаимодействия для AddDrugWindow.xaml
    /// </summary>
    public partial class AddDrugWindow : Window
    {
        private Button add_InputInstruction_Button = new Button()
        {
            Content = "+",
            Background = Brushes.White,
            FontWeight = FontWeights.Bold,
            Height = 27,
            Width = 625,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        private Drug? _drug { get; set; }
        public AddDrugWindow(Drug drug)
        {
            InitializeComponent();
            _drug = drug;
            this.drugName_TextBox.Text = drug.DrugName;
            this.mainForm_TextBox.Text = drug.MainForm;
            this.dosage_TextBox.Text = drug.Dosage;
            this.packing_TextBox.Text = drug.Packing.ToString();
            this.pharmaForm_TextBox.Text = drug.PharmaForm;
            this.curtForm_TextBox.Text = drug.CurtForm;
            this.manufacturer_TextBox.Text = drug.Manufacturer;
            this.producer_TextBox.Text = drug.Producer;
            this.mnn_TextBox.Text = drug.Mnn;
            this.ftg_TextBox.Text = drug.Ftg;

            this.drugName_TextBox.Opacity = 1;
            this.mainForm_TextBox.Opacity = 1;
            this.dosage_TextBox.Opacity = 1;
            this.packing_TextBox.Opacity = 1;
            this.pharmaForm_TextBox.Opacity = 1;
            this.curtForm_TextBox.Opacity = 1;
            this.manufacturer_TextBox.Opacity = 1;
            this.producer_TextBox.Opacity = 1;
            this.mnn_TextBox.Opacity = 1;
            this.ftg_TextBox.Opacity = 1;

            foreach (var ins in new Service().getDrugInstructionsByDrugID(drug.DrugId))
            {
                instructions_StackPanel.Children.Add(new InputInstructionUserControl(this, ins));
            }
            add_InputInstruction_Button.Click += addInputInstruction_Button_Click;
            instructions_StackPanel.Children.Add(add_InputInstruction_Button);
        }
        public AddDrugWindow()
        {
            InitializeComponent();
            add_InputInstruction_Button.Click += addInputInstruction_Button_Click;
            instructions_StackPanel.Children.Add(add_InputInstruction_Button);
        }
        private void textBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBox.Text.Replace(" ", "")))
            {
                textBox.Text = "";
                textBox.Opacity = 0;
            }
        }
        private void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            textBox.Opacity = 1;
        }
        private void addInputInstruction_Button_Click(object sender, RoutedEventArgs e)
        {
            instructions_StackPanel.Children.Remove(add_InputInstruction_Button);
            instructions_StackPanel.Children.Add(new InputInstructionUserControl(this));
            instructions_StackPanel.Children.Add(add_InputInstruction_Button);
        }
        public void removeInputInstruction(InputInstructionUserControl inputInstructionUserControl)
        {
            instructions_StackPanel.Children.Remove(inputInstructionUserControl);
        }
        private void packing_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) { return; }
            textBox.Text = textBox.Text.Replace(" ", "");
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private async void saveDrug_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (_drug == null)
            {
                _drug = new Drug();
            }
            if (this.drugName_TextBox.Text.IsNullOrEmpty() ||
                this.mainForm_TextBox.Text.IsNullOrEmpty() ||
                this.dosage_TextBox.Text.IsNullOrEmpty() ||
                this.packing_TextBox.Text.IsNullOrEmpty() ||
                this.pharmaForm_TextBox.Text.IsNullOrEmpty() ||
                this.curtForm_TextBox.Text.IsNullOrEmpty() ||
                this.manufacturer_TextBox.Text.IsNullOrEmpty() ||
                this.producer_TextBox.Text.IsNullOrEmpty() ||
                this.mnn_TextBox.Text.IsNullOrEmpty() ||
                this.ftg_TextBox.Text.IsNullOrEmpty())
            {
                return;
            }

            _drug.DrugName = this.drugName_TextBox.Text;
            _drug.MainForm = this.mainForm_TextBox.Text;
            _drug.Dosage = this.dosage_TextBox.Text;
            _drug.Packing = int.Parse(this.packing_TextBox.Text);
            _drug.PharmaForm = this.pharmaForm_TextBox.Text;
            _drug.CurtForm = this.curtForm_TextBox.Text;
            _drug.Manufacturer = this.manufacturer_TextBox.Text;
            _drug.Producer = this.producer_TextBox.Text;
            _drug.Mnn = this.mnn_TextBox.Text;
            _drug.Ftg = this.ftg_TextBox.Text;
            if (_drug.DrugId == 0)
            {
                _drug = new Service().CreateDrug(_drug);
            }
            else
                await Task.Run(() => new Service().UpdateDrug(_drug));

            foreach (var inputIns in instructions_StackPanel.Children)
            {
                if (inputIns.GetType() == typeof(InputInstructionUserControl))
                {
                    var input = (InputInstructionUserControl)inputIns;
                    var instruction = input._instruction;
                    if (instruction == null)
                        instruction = new Instruction();
                    if (input.header_TextBox.Text.IsNullOrEmpty() || input.description_TextBox.Text.IsNullOrEmpty())
                    {
                        return;
                    }
                }
            }
            foreach (var inputIns in instructions_StackPanel.Children)
            {
                if (inputIns.GetType() == typeof(InputInstructionUserControl))
                {
                    var input = (InputInstructionUserControl)inputIns;
                    var instruction = input._instruction;
                    if (instruction == null)
                    {
                        instruction = new Instruction();
                    }
                    instruction.InstructionId = -1;
                    instruction.InstructionName = input.header_TextBox.Text;
                    instruction.InstructionDescription = input.description_TextBox.Text;
                    new Service().UpdateOrCreateInstruction(_drug, instruction);
                }
            }

            Close();
        }
    }
}
