using AutoProjectWPF.ViewModel;
using System.Windows;
using System.Windows.Media;

namespace AutoProjectWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();     
        }

        private void BackColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Resources["panel"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(BackColor.SelectedItem.ToString()));
        }
    }
}
