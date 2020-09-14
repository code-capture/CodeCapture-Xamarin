using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCapture.Models.CompileModels
{
    class Input
    {
        public string script { get; set; }
        public string language { get; set; }
        public string stdin { get; set; }
        public string versionIndex { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}
