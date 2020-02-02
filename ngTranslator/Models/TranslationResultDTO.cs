using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ngTranslator.Models
{
    public class TranslationResultDTO
    {
        public string DetectedLanguage { get; set; }
        public string TranslationOutput { get; set; }
    }
}
