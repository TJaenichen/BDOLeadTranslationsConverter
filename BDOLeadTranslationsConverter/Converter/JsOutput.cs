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
        public override void Convert(List<TranslationModel> models)
        {
            var languages = GetLanguages(models);

            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Translations");
            var headerRow = sheet1.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("key");
            for (var i = 0; i < languages.Count; i++)
            {
                var language = languages[i];
                headerRow.CreateCell(i + 1).SetCellValue(language);
            }

            for (int i = 0; i < models.Count; i++)
            {
                var model = models[i];

                var row = sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(model.key);

                for (int j = 0; j < languages.Count; j++)
                {
                    var language = languages[j];
                    if (model.Translations.ContainsKey(language))
                    {
                        row.CreateCell(j + 1).SetCellValue(model.Translations[language]);
                    }
                }
            }

            var exportFileName = Path.ChangeExtension(FileInfo.FullName, ".xls");

            if (File.Exists(exportFileName))
            {
                File.Delete(exportFileName);
            }

            FileStream sw = File.Create(exportFileName);

            workbook.Write(sw);

            sw.Close();
        }
    }
}
