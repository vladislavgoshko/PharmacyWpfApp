using ClassLibraryPharmacy.DBModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAppPharmacy
{
    /// <summary>
    /// Логика взаимодействия для InputInstructionUserControl.xaml
    /// </summary>
    public partial class InputInstructionUserControl : UserControl
    {
        private AddDrugWindow _addDrugWindow { get; }
        public Instruction? _instruction { get; set; }
        public InputInstructionUserControl(AddDrugWindow addDrugWindow)
        {
            InitializeComponent();
            _addDrugWindow = addDrugWindow;
        }
        public InputInstructionUserControl(AddDrugWindow addDrugWindow, Instruction instruction)
        {
            InitializeComponent();
            _instruction = instruction;
            _addDrugWindow = addDrugWindow;
            header_TextBox.Text = instruction.InstructionName;
            description_TextBox.Text = instruction.InstructionDescription;

            header_TextBox.Opacity = 1;
            description_TextBox.Opacity = 1;
        }
        private void textBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBox.Text.Replace(" ", "")))
                textBox.Opacity = 0;
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

        private void deleteDrug_Button_Click(object sender, RoutedEventArgs e)
        {
            _addDrugWindow.removeInputInstruction(this);
        }
    }
}
