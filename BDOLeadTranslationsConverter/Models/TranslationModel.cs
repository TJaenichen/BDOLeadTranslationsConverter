using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDOLeadTranslationsConverter.Models
{
    class TranslationModel
    {
        public string key { get; set; }
        public Dictionary<string, string> Translations { get; set; }

        public TranslationModel()
        {
            Translations = new Dictionary<string, string>();
        }
    }
}
