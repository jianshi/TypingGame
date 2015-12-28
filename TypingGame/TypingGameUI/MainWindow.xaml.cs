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
            List<string> textList = new List<string>(){"abc", "def", "ghk"};
            mainTextBox.Items.Clear();
            foreach (string sLine in textList)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = sLine;
                mainTextBox.Items.Add(textBlock);
            }
            enterTextBox.Width = mainTextBox.ActualWidth;
//            rtbFlowDoc.PageWidth = mainTextBox.Width;
            mainTextBox.Items.Insert(1, enterTextBox);
        }

        private void NewTextButton_Click(object sender, RoutedEventArgs e)
        {
            NewText("my text\n2nd line\n 3rd line");
        }

        private void enterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private bool m_bEditing = false;
        private void enterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int offset = -2;
            Debug.WriteLine(e.ToString());
            if(!m_bEditing && flowDoc != null && flowDoc.Blocks != null && flowDoc.Blocks.FirstBlock != null)
            {
                m_bEditing = true;
                flowDoc.Blocks.FirstBlock.Foreground = Brushes.DarkGreen;
                var start = enterTextBox.Document.ContentStart;
                var end = enterTextBox.Document.ContentEnd;
                var lastCharStartPos = end.GetPositionAtOffset(offset - 1);
                var lastCharEndPos = end.GetPositionAtOffset(offset);
                var lastCharRange = new TextRange(lastCharStartPos, lastCharEndPos);
                var totalTextRange = new TextRange(start, end);
                int position = totalTextRange.Text.Length;

                if( (position - position / 2 * 2) == 0)
                {
                    // red color, bold
                    lastCharRange.ApplyPropertyValue(TextElement.ForegroundProperty,
                        new SolidColorBrush(Colors.Red));
                    lastCharRange.ApplyPropertyValue(TextElement.FontWeightProperty,
                        FontWeights.Bold);
                }
                else
                {
                    // black color, normal weight
                    lastCharRange.ApplyPropertyValue(TextElement.ForegroundProperty,
                        new SolidColorBrush(Colors.Black));
                    lastCharRange.ApplyPropertyValue(TextElement.FontWeightProperty,
                        FontWeights.Normal);
                }
                m_bEditing = false;
            }
        }
    }
}
