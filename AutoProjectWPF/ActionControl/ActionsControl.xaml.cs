using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActionControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ActionsControl : UserControl
    {
        public ActionsControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ActionsProperty =
           DependencyProperty.Register("Actions", typeof(IEnumerable<ICommand>), typeof(ActionsControl));

        public IEnumerable<ICommand> Actions
        {
            get
            {
                return (IEnumerable<ICommand>)GetValue(ActionsProperty);
            }
            set
            {
                SetValue(ActionsProperty, value);
            }
        }
    }
}
