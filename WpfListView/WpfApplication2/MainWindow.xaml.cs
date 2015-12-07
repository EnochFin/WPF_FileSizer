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
        private CollectionViewSource _myFileItemViewSource;
        private MyFileInfo _currentItem;
        public MainWindow()
        {
            InitializeComponent();
            _currentItem = new MyFileInfo(null);
            searchText.TextChanged += (sender, e) => FilterResults(searchText.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _myFileItemViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("myFileItemViewSource")));
            _myFileItemViewSource.Source = _currentItem.SubFiles;           
        }

        private void myFileItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = myFileItemListView.SelectedItem as MyFileInfo;
            if (item != null)
            {
                if (item.Type == FileType.Directory)
                {
                    _currentItem = item;
                    _myFileItemViewSource.Source = _currentItem.SubFiles;
                }
                else
                {
                    Process.Start(item.FullPath);
                }
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentItem.Parent != null)
            {
                _currentItem = _currentItem.Parent;
                _myFileItemViewSource.Source = _currentItem.SubFiles;
            }
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            string path = textDirectory.Text;
            if (Directory.Exists(path))
            {
                _currentItem = new MyFileInfo(textDirectory.Text, _currentItem.Parent);
                _myFileItemViewSource.Source = _currentItem.SubFiles;
            }
        }

        private void alphaSortButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _currentItem.SubFiles.OrderBy(fileinfo => fileinfo.Name);
            _myFileItemViewSource.Source = result;
        }

        private void SizeSortButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _currentItem.SubFiles.OrderBy(fileinfo => fileinfo.Size);
            _myFileItemViewSource.Source = result;
        }

        private void FileCountButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _currentItem.SubFiles.OrderBy(fileinfo => fileinfo.FileCount);
            _myFileItemViewSource.Source = result;
        }

        private void LastEditButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _currentItem.SubFiles.OrderBy(fileinfo => fileinfo.LastEdit);
            _myFileItemViewSource.Source = result;
        }

        private void FilterResults(string filterTerm)
        {
            if (_currentItem.SubFiles != null)
            {
                var result = _currentItem.SubFiles.Where(fileinfo => fileinfo.Name.ToUpper().Contains(filterTerm.ToUpper()));
                _myFileItemViewSource.Source = result;
            }
        }
    }
}
