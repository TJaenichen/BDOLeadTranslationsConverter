using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BDOLeadTranslationsConverter.Models;

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

            var translations = Directory.GetFiles(FileInfo.DirectoryName,
                Path.GetFileNameWithoutExtension(FileInfo.Name) + "*" + ".xres");

            foreach (var translation in translations)
            {
                Console.WriteLine(translation);
            }

            return models;
        }
    }
}
 