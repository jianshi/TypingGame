using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        TextManager tm = new TextManager();

        public void NewText(string text)
        {
            tm.SetText(text);
            List<string> textList = tm.SplitText(40);
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
            using(StreamReader sr = new StreamReader(@"..\..\..\TextFiles\TextFile.txt"))
            {
                string text = sr.ReadToEnd();
                NewText(text);
            }
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

        private void enterTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.ToString());
            switch(e.Key)
            {
                case Key.Down:
                case Key.Up:
                case Key.Left:
                case Key.Right:
                    e.Handled = true;
                    break;
                case Key.Return:
                case Key.Back:
                    // handle possible line down and line up
                    e.Handled = true;
                    break;
                default:
                    break;
            }
        }

        private void enterTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine(e.ToString());
            string systemText = e.SystemText;
            string text = e.Text;
            string controlText = e.ControlText;
        }
    }
}
