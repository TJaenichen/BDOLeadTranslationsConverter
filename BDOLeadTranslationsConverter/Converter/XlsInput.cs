using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;

namespace BDOLeadTranslationsConverter.Converter
{
    class XlsInput : InputBase
    {
        public XlsInput(FileInfo fileInfo) : base(fileInfo)
        {
        }
        public override List<TranslationModel> GetModels()
        {
            var models = new List<TranslationModel>();

            using (StreamReader streamReader = new StreamReader(FileInfo.FullName))
            {

                var workbook = new HSSFWorkbook(new POIFSFileSystem(streamReader.BaseStream));
                var sheet = workbook.GetSheet("Translations");
                var headerRow = sheet.GetRow(0);

                var languages = new Dictionary<int, string>();

                for (var i = 1; i < headerRow.Cells.Count; i++)
                {
                    var headerRowCell = headerRow.Cells[i];
                    if (headerRowCell.StringCellValue != "key")
                    {
                        languages.Add(i, headerRowCell.StringCellValue);
                    }
                }

                for (var i = 1; i < sheet.LastRowNum + 1; i++)
                {
                    var row = sheet.GetRow(i);
                    var model = new TranslationModel();
                    model.key = row.GetCell(0).StringCellValue;
                    for (var j = 1; j < languages.Count + 1; j++)
                    {
                        model.Translations.Add(languages[j], row.GetCell(j).StringCellValue);
                    }
                    models.Add(model);
                }
            }

            return models;
        }

    }
}
