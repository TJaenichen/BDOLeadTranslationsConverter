using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BDOLeadTranslationsConverter.Models;
using BDOLeadTranslationsConverter.XsdSchema;

namespace BDOLeadTranslationsConverter.Converter
{
    class ResxOutput : OutputBase
    {
        public ResxOutput(FileInfo fileInfo) : base(fileInfo)
        {

        }

        string GetLanguageFileName(FileInfo originalFileInfo, string languageName)
        {
            if (languageName == Configuration.Configuration.RESX_DEFAULT_LANGUAGE)
            {
                return originalFileInfo.FullName;
            }

            return Path.Combine(originalFileInfo.DirectoryName,
                Path.GetFileNameWithoutExtension(originalFileInfo.Name) + "." + languageName + ".resx");
        }

        public override void Convert(List<TranslationModel> models)
        {
            var languages = GetLanguages(models);

            Dictionary<string, Root> outputs = new Dictionary<string, Root>();

            foreach (var language in languages)
            {
                var serializer = new XmlSerializer(typeof(Root));
                var root = new Root {Data = new List<Data>()};
                foreach (var translationModel in models)
                {
                    var entry = translationModel.Translations.FirstOrDefault(x => x.Key == language);
                    if (string.IsNullOrEmpty(entry.Key))
                    {
                        continue;
                    }
                    root.Data.Add(new Data{Name = translationModel.key, Value = entry.Value});
                }
                TextWriter writer = new StreamWriter(GetLanguageFileName(FileInfo, language));
                serializer.Serialize(writer, root);
                writer.Close();
            }
        }
    }
}
