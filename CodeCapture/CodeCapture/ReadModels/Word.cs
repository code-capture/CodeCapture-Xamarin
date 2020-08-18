using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.ReadModels
{
    public class Word
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
    }
}
