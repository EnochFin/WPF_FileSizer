using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionViewSource myFileItemViewSource;
        public MyFileInfo currentItem;
        public MainWindow()
        {
            InitializeComponent();
            currentItem = new MyFileInfo(null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myFileItemViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("myFileItemViewSource")));
            myFileItemViewSource.Source = currentItem.SubFiles;
        }

        private void goToButton_Click(object sender, RoutedEventArgs e)
        {
            string path = textDirectory.Text;
            if (Directory.Exists(path))
            {
                currentItem = new MyFileInfo(textDirectory.Text, currentItem.Parent);
                myFileItemViewSource.Source = currentItem.SubFiles;
            }
        }

        private void myFileItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as MyFileInfo;
            if (item != null)
            {
                if (item.Type == FileType.Directory)
                {
                    currentItem = item;
                    myFileItemViewSource.Source = currentItem.SubFiles;
                }
                else
                {
                    Process.Start(item.FullPath);
                }
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            currentItem = currentItem.Parent;
            myFileItemViewSource.Source = currentItem.SubFiles;

        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            var result = currentItem.SubFiles.Where(fileinfo => fileinfo.Name.Contains(searchText.Text));
            myFileItemViewSource.Source = result;
        }

        private void alphaSortButton_Click(object sender, RoutedEventArgs e)
        {

            var result = currentItem.SubFiles.OrderBy(fileinfo => fileinfo.Name);
            myFileItemViewSource.Source = result;

        }

        private void SizeSortButton_Click(object sender, RoutedEventArgs e)
        {

            var result = currentItem.SubFiles.OrderBy(fileinfo => fileinfo.Size);
            myFileItemViewSource.Source = result;

        }

        private void FileCountButton_Click(object sender, RoutedEventArgs e)
        {

            var result = currentItem.SubFiles.OrderBy(fileinfo => fileinfo.FileCount);
            myFileItemViewSource.Source = result;

        }

        private void LastEditButton_Click(object sender, RoutedEventArgs e)
        {
            var result = currentItem.SubFiles.OrderBy(fileinfo => fileinfo.LastEdit);
            myFileItemViewSource.Source = result;
        }
    }
}
