using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmoothDemo.UniversalClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = App.MainViewModel;
        }

    

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void popupClose_Tapped(object sender, TappedRoutedEventArgs e)
        {
            popup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            // App.MainViewModel.RequestActionList();
            App.MainViewModel.Init();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.SendCommand("next");
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.SendCommand("previous");
        }

        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.SendCommand("skip");
        }

        private void restart_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.SendCommand("restart");
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void SwitchB_Click(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.ToggleSwitch(2);
        }

        private void SwitchA_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
