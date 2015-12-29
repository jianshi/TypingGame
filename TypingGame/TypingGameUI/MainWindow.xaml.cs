using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            tm.TextChanged += new EventHandler(tm_TextChanged);
            tm.LineChanged += new LineChangedEventHandler(tm_LineChanged);
        }

        void tm_LineChanged(object sender, LineChangedEventArgs e)
        {
            // move the enterTextBox to new location, set view location
            Debug.Assert(e != null);
            int nOldIndex = e.nOldLineNumber;
            int nNewIndex = e.nNewLineNumber;

            if(nNewIndex < nOldIndex)
            {
                // move up one line
                LineNumberMoveUp(nNewIndex);
            }
            else if(nNewIndex > nOldIndex)
            {
                // move down one line
                LineNumberMoveDown(nNewIndex);
            }
            else
            {
                // same line, no change
                LineNumberShow(nNewIndex);
            }

            SetCurrentLineNumber(nNewIndex);
        }

        private void LineNumberMoveUp(int nLineNumber)
        {
            ShowHideTextEntry(nLineNumber + 2, false);
            LineNumberShow(nLineNumber);

            // move cursor to end of current line
        }

        private void LineNumberMoveDown(int nLineNumber)
        {
            ShowHideTextEntry(nLineNumber - 2, false);
            LineNumberShow(nLineNumber);
        }

        private void LineNumberShow(int nLineNumber)
        {
            for(int i = nLineNumber - 1; i <= nLineNumber + 1; i++)
            {
                ShowHideTextEntry(i, true);
            }
        }

        private void SetCurrentLineNumber(int nLineNumber)
        {
            // unsubscribe events from current line
            if(enterTextBox != null)
            {
                enterTextBox.PreviewKeyDown -= enterTextBox_PreviewKeyDown;
                enterTextBox.TextChanged -= enterTextBox_TextChanged;
            }

            if(nLineNumber >= 0 && nLineNumber < textInfoList.Count)
            {
                enterTextBox = textInfoList[nLineNumber].textEntry;
            }

            if(enterTextBox != null)
            {
                enterTextBox.PreviewKeyDown += enterTextBox_PreviewKeyDown;
                enterTextBox.TextChanged += enterTextBox_TextChanged;
                // set keyboard focus to this text input box
                FocusOnNewLine_AsyncThread();
            }
        }

        void tm_TextChanged(object sender, EventArgs e)
        {
            List<string> textList = tm.SplitText(80);
            mainTextBox.Items.Clear();
            foreach(string sLine in textList)
            {
                LineDisplayInfo lineDisplayInfo = new LineDisplayInfo();
                TextBlock textBlock = lineDisplayInfo.textDisplay;
                textBlock.Text = sLine;
                mainTextBox.Items.Add(textBlock);
                textInfoList.Add(lineDisplayInfo);
            }

            int nCurrentLine = tm.nCurrentLineNumber;
            LineNumberShow(nCurrentLine);
            SetCurrentLineNumber(nCurrentLine);
        }

        private void ShowHideTextEntry(int nLineNumber, bool bShow)
        {
            // show or hide the text entry at nLineNumber
            // the list view has mixture of text display and text entry, need to find out the index of this text entry
            // to show: insert into the list
            // to hide: remove from the list

            if((nLineNumber < 0) || (nLineNumber >= textInfoList.Count))
                return;

            LineDisplayInfo lineDisplayInfo = textInfoList[nLineNumber];
            if(lineDisplayInfo.bTextEntryVisible != bShow)
            {
                // count all text entries before this line, not including this line
                int nVisibleTextEntryBefore = 0;
                for (int i = 0; i < nLineNumber; i++)
                {
                    if (textInfoList[i].bTextEntryVisible)
                    {
                        nVisibleTextEntryBefore++;
                    }
                }

                // count all text displays including this line
                int nVisibleTextDisplayBefore = nLineNumber + 1;

                int nIndexInListView = nVisibleTextDisplayBefore + nVisibleTextEntryBefore;
                if(bShow)
                {
                    lineDisplayInfo.textEntry.Width = mainTextBox.ActualWidth - 30;
                    mainTextBox.Items.Insert(nIndexInListView, lineDisplayInfo.textEntry);
                }
                else
                {
                    RichTextBox richTextBox = mainTextBox.Items[nIndexInListView] as RichTextBox;
                    Debug.Assert(richTextBox != null);
                    mainTextBox.Items.RemoveAt(nIndexInListView);
                }
                lineDisplayInfo.bTextEntryVisible = bShow;
            }
        }

        TextManager tm = new TextManager();
        List<LineDisplayInfo> textInfoList = new List<LineDisplayInfo>();
        List<TextBlock> textDisplayList = new List<TextBlock>();
        List<RichTextBox> textEntryList = new List<RichTextBox>();

        private void NewTextButton_Click(object sender, RoutedEventArgs e)
        {
            using(StreamReader sr = new StreamReader(@"..\..\..\TextFiles\TextFile.txt"))
            {
                string text = sr.ReadToEnd();
                tm.SetText(text);
            }
        }

        private void enterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private bool m_bEditing = false;
        private void enterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.Assert(sender == enterTextBox);
            FlowDocument flowDoc = enterTextBox.Document;
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

                // last two characters are \r\n
                int position = totalTextRange.Text.Length - 2;

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
                if(position > 0)
                {
                    tm.AddCharacter(position);
                }
                else
                {
                    tm.RemoveCharacter(0);
                }
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
                    // Ignore arrow keys
                    e.Handled = true;
                    break;
                case Key.Return:
                    tm.AddReturn();
                    e.Handled = true;
                    break;
                //case Key.Back:
                //   tm.RemoveCharacter(0);
                //    e.Handled = true;
                //    break;
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

        private RichTextBox enterTextBox = null;

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if(enterTextBox != null)
            {
                enterTextBox.Focus();
            }
        }

        private void FocusOnNewLine_AsyncThread()
        {
            if(enterTextBox != null)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (ThreadStart)delegate { FocusOnNewLine(); }
                    );
            }
        }

        private void FocusOnNewLine()
        {
            if(enterTextBox != null)
            {
                enterTextBox.Focus();
                CenterAtNewLine();
            }
        }

        private void CenterAtNewLine()
        {
            // Get the border of the listview (first child of a listview)
            Decorator border = VisualTreeHelper.GetChild(mainTextBox, 0) as Decorator;
            if(border != null)
            {
                // Get scrollviewer
                ScrollViewer scrollViewer = border.Child as ScrollViewer;
                if(scrollViewer != null)
                {
                    // center the Scroll Viewer...
                    int nCurrentLine = tm.nCurrentLineNumber;
                    int nTotalLines = textInfoList.Count;
                    double percent = (double)nCurrentLine / nTotalLines;
                    double center = scrollViewer.ScrollableHeight * percent;
                    scrollViewer.ScrollToVerticalOffset(center);
                }
            }
        }
    }
}
