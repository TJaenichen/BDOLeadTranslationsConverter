using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using BDOLeadTranslationsConverter.Models;
using BDOLeadTranslationsConverter.XsdSchema;
using NPOI.SS.Formula.Functions;

namespace BDOLeadTranslationsConverter.Converter
{
    class ResxInput : InputBase
    {
        protected static List<FileInfo> AllResx = new List<FileInfo>();
        
        public ResxInput(FileInfo fileInfo) : base(fileInfo)
        {
            AllResx.Add(fileInfo);
        }

        public override List<TranslationModel> GetModels()
        {
            var models = new List<TranslationModel>();

            if (FileInfo.Name.Count(x => x == '.') > 1)
            {
                return models;
            }
            
            var translationFiles = Directory.GetFiles(FileInfo.DirectoryName,
                Path.GetFileNameWithoutExtension(FileInfo.Name) + "*" + ".resx");
           
            foreach (var translationFile in translationFiles)
            {
                var language = GetLanguageFromResx(translationFile);

                var serializer = new XmlSerializer(typeof(Root));
                var streamReader = new StreamReader(translationFile);

                var xmlReader = new XmlTextReader(streamReader);

                var iAm = serializer.Deserialize(xmlReader)as Root;

                foreach (var translation in iAm.Data)
                {
                    var existing = models.FirstOrDefault(x => x.key == translation.Name);
                    if (existing == null)
                    {
                        var newTranslationModel = new TranslationModel
                        {
                            key = translation.Name,
                            Translations = new Dictionary<string, string>
                            {
                                {
                                    language, translation.Value
                                }
                            }
                        };
                        models.Add(newTranslationModel);
                    }
                    else
                    {
                        existing.Translations.Add(language, translation.Value);
                    }
                }
            }

            return models;
        }

        private string GetLanguageFromResx(string translationFile)
        {
            var parts = translationFile.Split('.');
            if (parts.Length == 2)
            {
                return Configuration.Configuration.RESX_DEFAULT_LANGUAGE;
            }
            return parts[1];
        }
    }
}
 