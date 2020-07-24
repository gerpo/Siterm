using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using Serilog;

namespace Siterm.Support.Services
{
    public class RtfToFlowConverter
    {
        private readonly FlowDocument _flowDocument;
        private readonly TextRange _textRange;
        
        public RtfToFlowConverter()
        {
            _flowDocument = new FlowDocument();
            _textRange = new TextRange(_flowDocument.ContentStart, _flowDocument.ContentEnd);
        }

        public FlowDocument CreateFlowDocument(string rtfPath)
        {
            try
            {
                using var fileStream = File.Open(rtfPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                _textRange.Load(fileStream, DataFormats.Rtf);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Error converting rtf file to FlowDocument.");
            }

            return _flowDocument;
        }
    }
}