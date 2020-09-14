using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models.CompileModels
{
    class Output
    {
        public string output { get; set; }
        public int statusCode { get; set; }
        public string memory { get; set; }
        public string cpuTime { get; set; }
    }
}
