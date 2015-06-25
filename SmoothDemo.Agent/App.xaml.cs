using Microsoft.Owin.Hosting;
using SmoothDemo.Agent.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmoothDemo.Agent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel MainViewModel { get; set; }
        IDisposable localHubHost { get; set; }
        public App()
        {
            MainViewModel = new MainViewModel();
            MainViewModel.Init();

            try
            {
                MainViewModel.StatusMessage = "Starting local hub.";
                localHubHost = WebApp.Start<Startup>(ConfigurationManager.AppSettings["LocalHubURL"]);

                if (localHubHost == null)
                {
                    MainViewModel.StatusMessage = "Hub start failed.";
                }
                else
                {
                    MainViewModel.StatusMessage = "Hub  started on " + ConfigurationManager.AppSettings["LocalHubURL"] + " signalr/hubs";
                }
            }
            catch (Exception ex)
            {

                MainViewModel.StatusMessage = ex.Message;
            }
        }

        

        protected override void OnExit(ExitEventArgs e)
        {
            if (localHubHost != null)
            {
                localHubHost.Dispose();
                localHubHost = null;
            }

            base.OnExit(e);
        }
    }
}
