using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;

namespace BDOLeadTranslationsConverter.Converter
{
    class InputBase
    {
        protected FileInfo FileInfo;
        public InputBase(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }
        public virtual List<TranslationModel> GetModels()
        {
            return null;
        }

    }
}
