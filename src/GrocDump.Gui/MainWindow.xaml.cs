using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GrocDump.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Elevator.Elevate();
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel && sender is TextBox textBox)
            {
                var filter = (textBox.Text + e.Text).ToLower();
                mainViewModel.FilterProcesses(filter);
            }
        }
    }
}