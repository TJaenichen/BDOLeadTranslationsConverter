using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;

namespace BDOLeadTranslationsConverter.Converter
{
    class XlsOutput : OutputBase
    {
        public XlsOutput(FileInfo fileInfo) : base(fileInfo)
        {
        }

        public override void Convert(List<TranslationModel> models)
        {
            var contentStringBuilder = new StringBuilder();
            foreach (var translationModel in models)
            {
                contentStringBuilder.AppendLine(translationModel.ToJson());
            }

            var json = Configuration.Configuration.JS_FILE_TEMPLATE.Replace(
                Configuration.Configuration.JS_TRANSLATION_REPLACEMENT_TAG, contentStringBuilder.ToString());

            var jsonFileName = Path.ChangeExtension(FileInfo.FullName, ".js");

            if (File.Exists(jsonFileName))
            {
                File.Delete(jsonFileName);
            }
            File.WriteAllText(jsonFileName, json);
        }
    }
}
