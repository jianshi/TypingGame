using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TypingGameUI
{
    class TextManager
    {
        public void SetText(string text)
        {
            Debug.Assert(text != null);
            m_sText = text;
        }

        public List<string> SplitText(int nCharPerLine)
        {
            m_textList = SplitText(m_sText, nCharPerLine);
            return m_textList;
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
    }
}
