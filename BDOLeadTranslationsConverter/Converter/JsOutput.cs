using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace BDOLeadTranslationsConverter.Converter
{
    class JsOutput : OutputBase
    {
        public JsOutput(FileInfo fileInfo) : base(fileInfo)
        {
        }
        public string ToJson(TranslationModel model)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine($"\"key\":  \"{model.key}\",");
            foreach (var translation in model.Translations)
            {
                stringBuilder.AppendLine($"\"{translation.Key}\":    \"{translation.Value.Replace("\"", "\\\"")}\",");
            }

            stringBuilder.AppendLine("},");
            return stringBuilder.ToString();
        }
        public override void Convert(List<TranslationModel> models)
        {
            var contentStringBuilder = new StringBuilder();
            foreach (var translationModel in models)
            {
                contentStringBuilder.AppendLine(ToJson(translationModel));
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
