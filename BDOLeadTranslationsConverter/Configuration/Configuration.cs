using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDOLeadTranslationsConverter.Configuration
{
    static class Configuration
    {
        public static string RESX_DEFAULT_LANGUAGE = "en";
        public static string[] JS_START_TOKEN = new string[]{"translations = translations.concat([",
            "overriddenTranslations = overriddenTranslations.concat(["};
        public static string JS_END_TOKEN = "]);";
        public static string JS_TRANSLATION_REPLACEMENT_TAG = "###TRANSLATION_OBJECTS###";
        public static string JS_FILE_TEMPLATE = @"if (translations == null) {
    var translations = [];
}

translations = translations.concat([
###TRANSLATION_OBJECTS###
]);";
    }
}
