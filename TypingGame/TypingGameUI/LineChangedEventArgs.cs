using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypingGameUI
{
    public class LineChangedEventArgs : EventArgs
    {
        public int nOldLineNumber { get; set; }
        public int nNewLineNumber { get; set; }
    }
}
