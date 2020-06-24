using System.Windows;

namespace ZStickerCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var win = new MainWindow();
            win.DataContext = new MainWindowViewModel();
            win.Show();
        }
    }
}
