using NUnit.Framework;
using SharpTestsEx;

namespace iSqlCmd.Tests
{
    [TestFixture]
    public class ShortcutTranslatorTests
    {
        [Test]
        public void WhenTheShortIsShowDb_ThenReturnTheCorrectCommand()
        {
            var shortcut = ":show dbs";
            var query = ShortcutTranslator.Translate(shortcut);
            query.Should().Be.EqualTo("select substring(name, 1, 60) as [Database] from sys.databases order by name;");
        }

        [Test]
        public void WhenTheShortIsShowTables_ThenReturnTheCorrectCommand()
        {
            var shortcut = ":show tables in chinook";
            var query = ShortcutTranslator.Translate(shortcut);
            query.Should().Be.EqualTo("SELECT substring([TABLE_SCHEMA] + '.' + [TABLE_NAME], 1, 60) as [Table]  from chinook.information_schema.tables;");
        }


        [Test]
        public void WhenTheShortIsShowSchemas_ThenReturnTheCorrectCommand()
        {
            var shortcut = ":show schemas in chinook";
            var query = ShortcutTranslator.Translate(shortcut);
            query.Should().Be.EqualTo("SELECT substring(SCHEMA_NAME, 1, 60) FROM claim.information_schema.schemata;");
        }

        //select SCHEMA_NAME from claim.information_schema.schemata;
    }
}