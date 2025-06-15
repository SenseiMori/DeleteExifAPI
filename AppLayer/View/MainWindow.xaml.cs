using System;
using System.Reflection;
using System.Windows;
using AppCore.Model.Interfaces;


namespace AppCore
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(IMainViewModel mainViewModel)
        {
            
            InitializeComponent();
            DataContext = mainViewModel;
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
