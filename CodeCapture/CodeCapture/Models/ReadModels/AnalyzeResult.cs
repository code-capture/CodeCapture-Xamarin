using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models.ReadModels
{
    public class AnalyzeResult
    {
        public string version { get; set; }
        public List<ReadResult> readResults { get; set; }
    }
}
