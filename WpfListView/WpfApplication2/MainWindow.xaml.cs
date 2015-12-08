using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
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
using Microsoft.Win32;

namespace WpfApplication2
{
    /* PERSONAL FEATURE INDEX: Carver
       Cached file structure -- file strutcure is manually recursed and stored in ram for high filter performance -- located in MyFileItem.cs
       UnathorizedAccessException coverage -- attempting to view blocked file will not crash and is indicated  -- located in MyFileItem.cs and converters and classes that use it
       Custom Button Templates -- custom appearence of buttons -- located in FileSizerFiles.xaml
       Custom Images based of file type -- images for each type of file tracked by MyFileItem -- located in MainWindow.xaml
    */


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionViewSource _myFileViewSource;
        private SortStrategy _sortStrategy;



        public bool CanBack
        {
            get { return (bool)GetValue(CanBackProperty); }
            set { SetValue(CanBackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanBack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanBackProperty =
            DependencyProperty.Register("CanBack", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));



        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));


        private SortStrategy SortStrategy
        {
            get { return _sortStrategy; }
            set
            {
                _sortStrategy = value;
                if (MyFileItemViewSource != null)
                {
                    MyFileItemViewSource.SortDescriptions.Clear();
                    string sortOn = _sortStrategy.ToString();
                    bool descending = sortOn.StartsWith("R");
                    if (descending) sortOn = sortOn.Substring(1);
                    MyFileItemViewSource.SortDescriptions.Add(new SortDescription(sortOn, (descending) ? ListSortDirection.Descending : ListSortDirection.Ascending));
                }
            }
        }

        private CollectionViewSource MyFileItemViewSource
        {
            get { return _myFileViewSource; }
            set
            {
                _myFileViewSource = value;
            } }

        private MyFileInfo _currentItem;
        private MyDirInfo _currentDir;
        public MainWindow()
        {
            InitializeComponent();
            _currentItem = new MyFileInfo(null);
            SortStrategy = SortStrategy.Name;
            SearchText.TextChanged += (sender, e) => FilterResults(SearchText.Text);
            NameHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Name) ? SortStrategy.RName : SortStrategy.Name;
            PercentHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Size) ? SortStrategy.RSize : SortStrategy.Size;
            SizeHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Size) ? SortStrategy.RSize : SortStrategy.Size;
            LastEditHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.LastEdit) ? SortStrategy.RLastEdit : SortStrategy.LastEdit;
            FileCountHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.FileCount) ? SortStrategy.RFileCount : SortStrategy.FileCount;
            TypeHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Type) ? SortStrategy.RType : SortStrategy.Type;
            ((INotifyCollectionChanged)MyFileItemListView.Items).CollectionChanged += (s, args) => {
                UpdateColumns();
                CanBack = _currentItem.Parent != null;
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyFileItemViewSource = ((CollectionViewSource)(FindResource("MyFileItemViewSource")));
            MyFileItemViewSource.Source = _currentItem.SubFiles;

            _currentDir = new MyDirInfo("C:/");
            FileTreeView.ItemsSource = _currentDir.SubFiles;
        }

        private void myFileItemListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = MyFileItemListView.SelectedItem as MyFileInfo;
            if (item != null)
            {
                if (item.Type == FileType.Directory)
                {
                    _currentItem = item;
                    MyFileItemViewSource.Source = _currentItem.SubFiles;
                    SearchText.Clear();
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
                MyFileItemViewSource.Source = _currentItem.SubFiles;
                SearchText.Clear();
            }

        }

        private async void searchButton_click(object sender, RoutedEventArgs e)
        {
            string path = TextDirectory.Text;
            if (Directory.Exists(path))
            {
                IsBusy = true;
                _currentItem = await Task.Run(() =>
                {
                    return new MyFileInfo(path, null);
                });
                MyFileItemViewSource.Source = _currentItem.SubFiles;
                IsBusy = false;
            }
        }

        private async Task UpdateCurrentItem()
        {
            
        }

        private void FilterResults(string filterTerm)
        {
            if (_currentItem.SubFiles != null)
            {
                var result = _currentItem.SubFiles.Where(fileinfo => fileinfo.Name.ToUpper().Contains(filterTerm.ToUpper()));
                _myFileViewSource.Source = result;
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

        private void FileTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyDirInfo selected = FileTreeView.SelectedItem as MyDirInfo;
            
            if (selected != null)
            {
                _currentDir = selected;
                _myFileViewSource.Source = new MyFileInfo(selected.FullPath, null).SubFiles;
            }
            else
            {
                Console.WriteLine("Cant cast");
            }
        }
    }

    public enum SortStrategy
    {
        Name,
        RName,
        Size,
        RSize,
        FileCount,
        RFileCount,
        LastEdit,
        RLastEdit,
        Type,
        RType
    }


}
