using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models.ReadModels
{
    public class ReadResult
    {
        public int page { get; set; }
        public string language { get; set; }
        public double angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
        public List<Line> lines { get; set; }
    }
}
