using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
            SearchText.TextChanged += (sender, e) => FilterResults(SearchText.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _myFileItemViewSource = ((CollectionViewSource)(FindResource("MyFileItemViewSource")));
            _myFileItemViewSource.Source = _currentItem.SubFiles;           
        }

        private void myFileItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = MyFileItemListView.SelectedItem as MyFileInfo;
            if (item != null)
            {
                if (item.Type == FileType.Directory)
                {
                    _currentItem = item;
                    UpdateListViewSource(_currentItem.SubFiles);
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
                UpdateListViewSource(_currentItem.SubFiles);
            }
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            string path = TextDirectory.Text;
            if (Directory.Exists(path))
            {
                _currentItem = new MyFileInfo(TextDirectory.Text, _currentItem.Parent);
                UpdateListViewSource(_currentItem.SubFiles);
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

        private void UpdateColumns()
        {
            foreach (GridViewColumn column in ((GridView) MyFileItemListView.View).Columns)
            {
                if (column != PercentageColumn)
                UpdateColumnWidth(column);
            }
        }


        private void UpdateColumnWidth(GridViewColumn column)
        {
            if (double.IsNaN(column.Width))
            {
                column.Width = column.ActualWidth;
            }

            column.Width = double.NaN;
        }

        private void UpdateListViewSource(IList<MyFileInfo> source)
        {
            _myFileItemViewSource.Source = source;
            UpdateColumns();
        }
    }
}
