using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Converter;
using System.Windows.Forms;
using NPOI;

namespace BDOLeadTranslationsConverter
{
    class Program
    {
        enum InputType { js, xls, resx, none}
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            
            List<InputBase> inputs = new List<InputBase>();

            for (int i = 0; i < args.Length; i++)
            {
                
                var filename = args[i];
                
                var fileInfo = new FileInfo(filename);
                var type = GetInputType(fileInfo);
                Console.WriteLine($"Converting {fileInfo.Name}");
                if (!File.Exists(filename) || type == InputType.none)
                {
                    Console.WriteLine("No idea what to do with {0}... skipping", fileInfo.Name);
                    continue;
                }

                if (!CheckKillExcel(fileInfo))
                {
                    Console.WriteLine("Can't convert with file open in Excel");
                    continue;
                }

                InputBase input = null;
                switch (type)
                {
                    case InputType.js:
                        input = new JsInput(fileInfo);
                        break;
                    case InputType.xls:
                        input = new XlsInput(fileInfo);
                        break;
                    case InputType.resx:
                        input = new ResxInput(fileInfo);
                        break;
                    case InputType.none:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                // Shouldn't happen but lets keep re-sharper happy
                if (input == null)
                {
                    Console.WriteLine("No idea what to do with {0}... skipping", fileInfo.Name);
                    continue;
                }
                inputs.Add(input);
                var models = input.GetModels();
                if (models.Count == 0)
                {
                    continue;
                }
                switch (type)
                {
                    case InputType.js:
                        new XlsOutput(fileInfo).Convert(models);
                        new ResxOutput(fileInfo).Convert(models);
                        break;
                    case InputType.xls:
                        new JsOutput(fileInfo).Convert(models);
                        new ResxOutput(fileInfo).Convert(models);
                        break;
                    case InputType.resx:
                        new XlsOutput(fileInfo).Convert(models);
                        new JsOutput(fileInfo).Convert(models);
                        break;
                    case InputType.none:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static InputType GetInputType(FileInfo fileInfo)
        {
            if (fileInfo.Extension.ToLower() == ".js")
            {
                return InputType.js;
            }
            else if (fileInfo.Extension.ToLower() == ".xls")
            {
                return InputType.xls;
            }
            else if (fileInfo.Extension.ToLower() == ".resx")
            {
                return InputType.resx;
            }

            return InputType.none;
        }

        static bool CheckKillExcel(FileInfo fileInfo)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL") select p;

            var found = false;
            var excelFileName = Path.ChangeExtension(fileInfo.Name, ".xls");

            var processArray = processes as Process[] ?? processes.ToArray();
            foreach (var process in processArray)
            {
                if (process.MainWindowTitle.Contains("Excel") && process.MainWindowTitle.Contains(excelFileName))
                {
                    found = true;
                }
            }

            if (!found)
            {
                return true;
            }

            var result = MessageBox.Show($"{excelFileName} is open in Excel!\r\n Close?", "Error", MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.No)
            {
                return false;
            }

            foreach (var process in processArray)
            {
                if (process.MainWindowTitle.Contains("Excel") && process.MainWindowTitle.Contains(excelFileName))
                {
                    process.Kill();
                }
            }

            return true;
        }
    }
}
