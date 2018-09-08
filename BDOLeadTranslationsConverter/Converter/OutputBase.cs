using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;

namespace BDOLeadTranslationsConverter.Converter
{
    class OutputBase
    {
        protected FileInfo FileInfo;
        public OutputBase(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
        public virtual void Convert(List<TranslationModel> models)
        {

        }
        protected List<String> GetLanguages(List<TranslationModel> models)
        {
            var retVal = new List<string>();
            foreach (var translationModel in models)
            {
                foreach (var translationsKey in translationModel.Translations.Keys)
                {
                    if (!retVal.Contains(translationsKey))
                    {
                        retVal.Add(translationsKey);
                    }
                }
            }

            return retVal;
        }
    }
}
