using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private CollectionViewSource _myFileViewSource;
        private SortStrategy _sortStrategy;

        private SortStrategy SortStrategy
        {
            get { return _sortStrategy; }
            set
            {
                _sortStrategy = value;
                if (MyFileItemViewSource != null)
                {
                    MyFileItemViewSource.SortDescriptions.Clear();
                    switch (_sortStrategy)
                    {
                        case SortStrategy.Alpabetical:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Name",
                                ListSortDirection.Ascending));
                            break;
                        case SortStrategy.RAlphabetical:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Name",
                                ListSortDirection.Descending));
                            break;

                        case SortStrategy.Type:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Type",
                                ListSortDirection.Ascending));
                            break;
                        case SortStrategy.RType:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Type",
                                ListSortDirection.Descending));
                            break;
                        case SortStrategy.FileCount:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("FileCount",
                                ListSortDirection.Ascending));
                            break;
                        case SortStrategy.RFileCount:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("FileCount",
                                ListSortDirection.Descending));
                            break;
                        case SortStrategy.LastEdit:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("LastEdit",
                                ListSortDirection.Ascending));
                            break;
                        case SortStrategy.RLastEdit:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("LastEdit",
                                ListSortDirection.Descending));
                            break;
                        case SortStrategy.Size:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Size",
                                ListSortDirection.Ascending));
                            break;
                        case SortStrategy.RSize:
                            MyFileItemViewSource.SortDescriptions.Add(new SortDescription("Size",
                                ListSortDirection.Descending));
                            break;
                    }
                }
            }
        }

        private CollectionViewSource MyFileItemViewSource
        {
            get { return _myFileViewSource; }
            set
            {
                _myFileViewSource = value;
                FilterResults(SearchText.Text);
                UpdateColumns();    
                  
            } }

        private MyFileInfo _currentItem;
        public MainWindow()
        {
            InitializeComponent();
            _currentItem = new MyFileInfo(null);
            SortStrategy = SortStrategy.Alpabetical;
            SearchText.TextChanged += (sender, e) => FilterResults(SearchText.Text);
            NameHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Alpabetical) ? SortStrategy.RAlphabetical : SortStrategy.Alpabetical;
            PercentHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Size) ? SortStrategy.RSize : SortStrategy.Size;
            SizeHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Size) ? SortStrategy.RSize : SortStrategy.Size;
            LastEditHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.LastEdit) ? SortStrategy.RLastEdit : SortStrategy.LastEdit;
            FileCountHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.FileCount) ? SortStrategy.RFileCount : SortStrategy.FileCount;
            TypeHeader.Click += (sender, e) => SortStrategy = (SortStrategy == SortStrategy.Type) ? SortStrategy.RType : SortStrategy.Type;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyFileItemViewSource = ((CollectionViewSource)(FindResource("MyFileItemViewSource")));
            MyFileItemViewSource.Source = _currentItem.SubFiles;           
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
            }
        }

        private void searchButton_click(object sender, RoutedEventArgs e)
        {
            string path = TextDirectory.Text;
            if (Directory.Exists(path))
            {
                _currentItem = new MyFileInfo(TextDirectory.Text, _currentItem.Parent);
                MyFileItemViewSource.Source = _currentItem.SubFiles;
            }
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
    }

    public enum SortStrategy
    {
        Alpabetical,
        RAlphabetical,
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
