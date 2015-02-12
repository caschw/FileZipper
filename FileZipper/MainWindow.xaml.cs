using System.Windows;
using FileZipper.ViewModels;

namespace FileZipper
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

        private void FileNameDropBox_Drop(object sender, DragEventArgs e)
        {
            var filePaths = (string[])e.Data.GetData( DataFormats.FileDrop );
            var vm = LayoutRoot.DataContext as FileZipperViewModel;
            vm.OnFileDrop( filePaths );
        }
    }
}
