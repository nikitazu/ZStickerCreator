using System.Windows;

namespace ZStickerCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((MainWindowViewModel)e.NewValue).PreviewImage = PreviewImage;
        }
    }
}
