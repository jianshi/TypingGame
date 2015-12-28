using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypingGameUI
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

        public void NewText(string text)
        {
            mainTextBox.Document.Blocks.Clear();
            mainTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private void mainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private void mainTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private void NewTextButton_Click(object sender, RoutedEventArgs e)
        {
            NewText("my text\n2nd line\n 3rd line");
        }
    }
}
