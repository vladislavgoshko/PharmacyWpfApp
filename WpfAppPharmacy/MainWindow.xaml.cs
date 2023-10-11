using BLLPharmacy;
using ClassLibraryPharmacy.DBModels;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfAppPharmacy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new Service().getDrugsByName("connectDB");

        }

        private async void search_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            string searchText = search_TextBox.Text.Replace(" ", "");

            if (string.IsNullOrEmpty(searchText))
            {
                searchResult_ListBox.ItemsSource = null;
                webBrowser.NavigateToString(@"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head></head></body></html>");
                return;
            }

            var drugs = await Task.Run(() => new Service().getDrugsByName(searchText));

            // Обновление интерфейса на основном потоке (UI потоке)
            Dispatcher.Invoke(() =>
            {
                searchResult_ListBox.ItemsSource = drugs;
            });
        }

        private async void searchResult_ListBox_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            Drug? selectedItem = searchResult_ListBox.SelectedItem as Drug;

            if (selectedItem == null)
            {
                return;
            }
            var instructions = await Task.Run(() => new Service().getDrugInstructionsByDrugID(selectedItem.DrugId));

            if (instructions.Count == 0)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    webBrowser.NavigateToString(@"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head>
                        <style>
                        * {
                            font-family: -apple-system, system-ui, BlinkMacSystemFont, ""“Segoe UI”"", Oxygen, Ubuntu, Cantarell, ""“Fira Sans”"", ""“Droid Sans”"", ""“Helvetica Neue”"", ""“Proxima Nova”"", sans-serif !important;
                            }    
                        </style>
                        </head><body><h2>Инструкция отсутствует</h2></body></html>");
                });
                return;
            }

            var result = instructions.Select(x => $"<h2>{x.InstructionName}</h2>{x.InstructionDescription}")
                .Aggregate((x, y) => x + "" + y).Replace("<img src=\"", "<img src=\"https://apteka.103.by");

            await Dispatcher.InvokeAsync(() =>
            {
                webBrowser.NavigateToString(@$"<!DOCTYPE html ><html><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'><head>
                    <style>
                        * {{
                            font-family: -apple-system, system-ui, BlinkMacSystemFont, ""“Segoe UI”"", Oxygen, Ubuntu, Cantarell, ""“Fira Sans”"", ""“Droid Sans”"", ""“Helvetica Neue”"", ""“Proxima Nova”"", sans-serif !important;
                            }}    
                        .drugInstruction__drugInfoCaption {{
                            font-size: 16px;
                            font-weight: bold;
                            line-height: 24px;
                            color: #000;
                        }}
                        .drugInstruction__drugInfoName {{ 
                            font-size: 16px;
                            line-height: 24px;
                            color: rgba(0,0,0,.64);
                        }}
                        </style>
                    </head>
                    <body>
                    <h1>{selectedItem.DrugName}: инструкция по применению</h1>
                    <span class=""drugInstruction__drugInfoCaption"">Форма выпуска:</span>
                    <span class=""drugInstruction__drugInfoName"">{selectedItem.MainForm}</span></br>
                    <span class=""drugInstruction__drugInfoCaption"">МНН:</span>
                    <span class=""drugInstruction__drugInfoName"">{selectedItem.Mnn}</span></br>
                    <span class=""drugInstruction__drugInfoCaption"">ФТГ:</span>
                    <span class=""drugInstruction__drugInfoName"">{selectedItem.Ftg}</span>
                    {result}</body></html>");
            });

        }

        private void search_TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (search_TextBox.Text.Replace(" ", "").IsNullOrEmpty())
                search_TextBox.Opacity = 0;
        }

        private void search_TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            search_TextBox.Opacity = 1;
        }

        private async void showAnalogs_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            Drug? selectedItem = searchResult_ListBox.SelectedItem as Drug;
            if (selectedItem == null)
            {
                MessageBox.Show("Вы не выбрали лекарство", "Ошибка");
                return;
            }

            var analogs = await Task.Run(() => new Service().getDrugsByMNN(selectedItem.Mnn));

            Dispatcher.Invoke(() =>
            {
                searchResult_ListBox.ItemsSource = analogs;
            });
        }

        private void addDrug_Button_Click(object sender, RoutedEventArgs e)
        {
            AddDrugWindow addDrugWindow = new AddDrugWindow();
            Closed += (s, args) => addDrugWindow.Close();
            addDrugWindow.Show();
        }

        private void changeDrug_Button_Click(object sender, RoutedEventArgs e)
        {
            Drug? selectedItem = searchResult_ListBox.SelectedItem as Drug;
            if (selectedItem == null)
            {
                MessageBox.Show("Вы не выбрали лекарство", "Ошибка");
                return;
            }
            AddDrugWindow addDrugWindow = new AddDrugWindow(selectedItem);
            Closed += (s, args) => addDrugWindow.Close();
            addDrugWindow.Show();
        }

        private async void deleteDrug_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            Drug? selectedItem = searchResult_ListBox.SelectedItem as Drug;
            if (selectedItem == null)
            {
                MessageBox.Show("Вы не выбрали лекарство", "Ошибка");
                return;
            }
            var newSearchList = searchResult_ListBox.ItemsSource as List<Drug>;
            if (newSearchList != null)
            {
                searchResult_ListBox.ItemsSource = null;
                newSearchList.Remove(selectedItem);
                searchResult_ListBox.ItemsSource = newSearchList;
            }

            await Task.Run(() => new Service().deleteDrug(selectedItem));
        }
    }
}
