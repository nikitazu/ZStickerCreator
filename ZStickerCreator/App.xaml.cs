using System.Linq;
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
            var persister = new Core.Persist.XmlDataPersister();
            var win = new MainWindow();
            win.DataContext = new MainWindowViewModel(
                new UI.Commands.LoadPackCommand(persister),
                new UI.Commands.SavePackCommand(persister),
                () =>
                {
                    using (var s = GetResourceStream(new System.Uri("pack://application:,,,/Resources/DataSource/ExamplePack.xml")).Stream)
                    {
                        return persister.Load(s).Select(x => new UI.Main.StickerItemViewModel
                        {
                            Emoji = x.Emoji,
                            Title = x.Title,
                        }).ToList();
                    }
                }
            );
            win.Show();
        }
    }
}
