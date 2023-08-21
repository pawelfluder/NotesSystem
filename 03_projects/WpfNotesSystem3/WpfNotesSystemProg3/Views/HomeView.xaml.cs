using SwitchingViewsMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;
using WpfNotesSystem.Repetition;

namespace SwitchingViewsMVVM.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            DataContext = MyBorder.Container.Resolve<HomeViewModel>();
        }
    }
}
