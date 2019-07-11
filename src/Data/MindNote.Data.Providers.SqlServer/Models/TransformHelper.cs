using MindNote.Data.Raws;
using System;
using System.Linq;

namespace MindNote.Data.Providers.SqlServer.Models
{
    public static class TransformHelper
    {
        public static string KeywordsToString(string[] keywords)
        {
            if (keywords == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Join(";", keywords.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public static string[] KeywordsToArray(string keywords)
        {
            if (keywords == null)
            {
                return Array.Empty<string>();
            }
            else
            {
                return keywords.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }
    }
}
