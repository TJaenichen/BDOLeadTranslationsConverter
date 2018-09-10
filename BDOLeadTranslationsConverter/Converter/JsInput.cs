using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDOLeadTranslationsConverter.Models;
using Newtonsoft.Json.Linq;

namespace BDOLeadTranslationsConverter.Converter
{
    class JsInput : InputBase
    {
        public JsInput(FileInfo fileInfo) : base(fileInfo)
        {
        }
        public override List<TranslationModel> GetModels()
        {
            var models = new List<TranslationModel>();

            using (StreamReader streamReader = new StreamReader(FileInfo.FullName))
            {
                var line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    var found = false;
                    foreach (var startToken in Configuration.Configuration.JS_START_TOKEN)
                    {
                        if (line.Contains(startToken))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                    line = streamReader.ReadLine();
                }

                var content = streamReader.ReadToEnd();
                foreach (var startToken in Configuration.Configuration.JS_START_TOKEN)
                {
                    content = content.Replace(startToken, "");
                }
                
                content = content.Trim();
                if (content.Length < 2)
                {
                    return null;
                }

                if (content.Substring(content.Length - 3) != Configuration.Configuration.JS_END_TOKEN)
                {
                    return null;
                }

                content = "[" + content.Replace(Configuration.Configuration.JS_END_TOKEN, "") + "]";
                var objs = JArray.Parse(content);

                foreach (var obj in objs)
                {
                    var curModel = new TranslationModel();
                    foreach (var childToken in obj)
                    {
                        var property = childToken as JProperty;
                        if (property == null)
                        {

                        }

                        if (property.Name.ToLower() == "key")
                        {
                            curModel.key = property.Value.ToString();
                        }
                        else
                        {
                            curModel.Translations.Add(property.Name, property.Value.ToString());
                        }
                    }
                    models.Add(curModel);
                }
            }
            return models;
        }
    }
}
