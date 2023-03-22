using System.Windows.Controls;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using XmlJsonConverter.Core;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace XmlJsonConverter
{
    /// <summary>
    /// Logika interakcji dla klasy ConverterPage.xaml
    /// </summary>
    public partial class ConverterPage : Page
    {
        public static ErrorService errorService = new ErrorService();
        public static MessageService messageService = new MessageService();

        private ConverterPageViewModel viewModel = new ConverterPageViewModel(errorService,messageService);
        public ConverterPage()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                path.Text = Path.GetFullPath(openFileDialog.FileName);
                viewModel.filePath = path.Text;
                
            }
        }
    }
}