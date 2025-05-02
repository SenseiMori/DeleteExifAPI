using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using TextRemoveExif.Services;
using TextRemoveExif.ViewModel;

namespace TextRemoveExif
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            MenuAlignment();

        }

        public static void MenuAlignment ()
        {
            var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            Action setAlignmentValue = () => {
                if (SystemParameters.MenuDropAlignment && menuDropAlignmentField != null) menuDropAlignmentField.SetValue(null, false);
            };
            setAlignmentValue();
            SystemParameters.StaticPropertyChanged += (sender, e) => { setAlignmentValue(); };


        }

    }
}
