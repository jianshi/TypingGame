using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TypingGameUI
{
    class LineDisplayInfo
    {
        private const double m_leftPadding = 8;
        public LineDisplayInfo()
        {
            m_textDisplay.FontFamily = m_fontFamily;
            m_textDisplay.FontSize = m_fontSize;
            m_textDisplay.Padding = new Thickness(m_leftPadding, 0, 0, 0);
            m_textDisplay.Background = Brushes.Wheat;
        }

        public TextBlock textDisplay
        {
            get { return m_textDisplay; }
        }

        public RichTextBox textEntry
        {
            get
            {
                if(m_textEntry == null)
                {
                    m_textEntry = new RichTextBox();
                    m_textEntry.FontFamily = m_fontFamily;
                    m_textEntry.FontSize = m_fontSize;
                    m_textEntry.BorderBrush = Brushes.Transparent;
                    m_textEntry.Background = Brushes.AliceBlue;
                }
                return m_textEntry;
            }
        }

        public bool bTextEntryVisible
        {
            get { return m_bTextEntryVisible; }
            set { m_bTextEntryVisible = value; }
        }

        /// <summary>
        /// To reuse the text entry control
        /// </summary>
        /// <param name="lineDisplayInfoForRecycle"></param>
        public void RecycleTextEntry(LineDisplayInfo lineDisplayInfoForRecycle)
        {
            RichTextBox richTextBox = lineDisplayInfoForRecycle.m_textEntry;
            lineDisplayInfoForRecycle.m_textEntry = null;
            if(this.m_textEntry == null)
            {
                if(richTextBox != null && richTextBox.Document != null && richTextBox.Document.Blocks != null)
                {
                    richTextBox.Document.Blocks.Clear();
                }
                this.m_textEntry = richTextBox;
            }
        }

        private bool m_bTextEntryVisible = false;
        private TextBlock m_textDisplay = new TextBlock();
        private RichTextBox m_textEntry = null;
        private FontFamily m_fontFamily = new FontFamily("Courier New");
        private const double m_fontSize = 20;
    }
}
