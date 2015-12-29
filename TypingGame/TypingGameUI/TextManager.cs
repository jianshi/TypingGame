using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TypingGameUI
{
    // A delegate type for hooking up change notifications.
    public delegate void LineChangedEventHandler(object sender, LineChangedEventArgs e);

    class TextManager
    {
        #region public events

        public event EventHandler TextChanged;

        protected virtual void OnTextChanged(EventArgs e)
        {
            if(TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        public event LineChangedEventHandler LineChanged;

        protected virtual void OnLineChanged(LineChangedEventArgs e)
        {
            if(LineChanged != null)
            {
                LineChanged(this, e);
            }
        }
        #endregion

        public void SetText(string text)
        {
            Debug.Assert(text != null);
            m_sText = text;
            m_currentLineNumber = 0;
            OnTextChanged(new EventArgs());
        }

        public List<string> SplitText(int nCharPerLine)
        {
            m_nCharPerLine = nCharPerLine;
            m_textList = SplitText(m_sText, nCharPerLine);
            return m_textList;
        }

        public int nCurrentLineNumber
        {
            get
            {
                return m_currentLineNumber;
            }
        }

        public void AddCharacter(int columnNumber)
        {
            if(columnNumber >= m_nCharPerLine - 1)
            {
                MoveToNext();
            }
        }

        public void RemoveCharacter(int columnNumber)
        {
            if(columnNumber <= 0)
            {
                MoveToPrev();
            }
        }

        public void AddReturn()
        {
            MoveToNext();
        }

        private void MoveToPrev()
        {
            if(m_currentLineNumber > 0)
            {
                LineChangedEventArgs lineChangedEventArgs = new LineChangedEventArgs();
                lineChangedEventArgs.nOldLineNumber = m_currentLineNumber;
                lineChangedEventArgs.nNewLineNumber = --m_currentLineNumber;
                OnLineChanged(lineChangedEventArgs);
            }
        }

        private void MoveToNext()
        {
            if(m_currentLineNumber < m_textList.Count - 1)
            {
                LineChangedEventArgs lineChangedEventArgs = new LineChangedEventArgs();
                lineChangedEventArgs.nOldLineNumber = m_currentLineNumber;
                lineChangedEventArgs.nNewLineNumber = ++m_currentLineNumber;
                OnLineChanged(lineChangedEventArgs);
            }
        }

        private List<string> SplitText(string text, int nCharPerLine)
        {
            // for now assuming all text is fixed width
            List<string> lines = new List<string>();

            // scan text, generate new line with enough characters or a new LF/CR
            string[] paragraphs = text.Split(m_separator_LF_CR, StringSplitOptions.RemoveEmptyEntries);
            foreach (string paragraph in paragraphs)
            {
                lines.AddRange(SplitIntoLines(paragraph, nCharPerLine));
            }

            return lines;
        }

        private List<string> SplitIntoLines(string text, int nCharPerLine)
        {
            // split single paragraph into lines with fixed width
            List<string> lines = new List<string>();

            Debug.Assert(nCharPerLine > 0);
            int nLength = text.Length;
            int nLines = (nLength - 1)/nCharPerLine + 1;
            Debug.Assert(nLength >= (nLines - 1)*nCharPerLine + 1);
            Debug.Assert(nLength <= nLines*nCharPerLine);

            for (int i = 0; i < nLines - 1; i++)
            {
                lines.Add(text.Substring(i*nCharPerLine, nCharPerLine));
            }
            if(nLines > 0)
            {
                lines.Add(text.Substring((nLines - 1) * nCharPerLine));
            }

            return lines;
        }

        private string[] m_separator_LF_CR = new string[] { "\r\n", "\n", "\r" };
        private string m_sText;
        private List<string> m_textList;
        private int m_currentLineNumber = 0;
        private int m_nCharPerLine = 80;
    }
}
