using System;

using System.Windows;

namespace SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        [STAThread]
        public void Main(string[] args)
        {
            var app = new App();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var window = new MainWindow();
            window.DataContext = new MainWindowVm();
            window.Show();
        }
    }
}