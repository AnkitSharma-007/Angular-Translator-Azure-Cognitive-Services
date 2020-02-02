using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTranslator.Models
{
    public class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }
}
