using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models.ReadModels
{
    public class Line
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public List<Word> words { get; set; }
    }
}
