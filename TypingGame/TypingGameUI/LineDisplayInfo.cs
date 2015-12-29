using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TypingGameUI
{
    class LineDisplayInfo
    {
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
    }
}
