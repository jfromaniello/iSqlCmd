using System;

namespace iSqlCmd
{
    public static class ShortcutTranslator
    {
        public static string Translate(string shortcut)
        {
            if(shortcut == ":show dbs")
            {
                return "select substring(name, 1, 60) as [Database] from sys.databases order by name;";
            }
            if (shortcut.StartsWith(":show tables in"))
            {
                var regex = new System.Text.RegularExpressions.Regex("show tables in (.*)");
                var dbName = regex.Match(shortcut).Groups[1].Value;
                return string.Format("SELECT substring([TABLE_SCHEMA] + '.' + [TABLE_NAME], 1, 60) as [Table]  from {0}.information_schema.tables;", dbName);
            }
            if(shortcut.StartsWith(":show schemas in"))
            {
                var regex = new System.Text.RegularExpressions.Regex("show schemas in (.*)");
                var dbName = regex.Match(shortcut).Groups[1].Value;
                return string.Format("SELECT substring(SCHEMA_NAME, 1, 60) FROM claim.information_schema.schemata;", dbName);
            }
            return shortcut;
        }
    }
}