using System;
using System.IO;
using ConsoleApplication13;
using NUnit.Framework;
using SharpTestsEx;

namespace iSqlCmd.Tests
{
    [TestFixture]
    public class SqlExecutorTests
    {
        [Test]   
        public void ShouldContainTitles()
        {
            var result = Execute("select * from album;", new LoginDetails { DatabaseName = "chinook" })
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            result[0].Should().Be.EqualTo(string.Format("{0} {1} {2}",
                                                   "AlbumId".PadRight(int.MaxValue.ToString().Length),
                                                   "Title".PadRight(160),
                                                   "ArtistId".PadRight(int.MaxValue.ToString().Length)));
        }

        [Test]
        public void ShouldContainsALine()
        {
            var result = Execute("select * from album;", new LoginDetails { DatabaseName = "chinook" })
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            result[1].Should().Be.EqualTo(new string('-', result[0].Length));
        }

        [Test]
        public void ThirdShouldContainData()
        {
            var result = Execute("select * from album;", new LoginDetails { DatabaseName = "chinook" })
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            result[2]
                .Should().Be.EqualTo("         1 For Those About To Rock We Salute You                                                                                                                                     1");
        }

        [Test]
        public void WhenQueryHasAnError_ThenShowException()
        {
            var result = Execute("update album set title = 'das' where id = 1;", new LoginDetails { DatabaseName = "chinook" })
                                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            result[0]
                .Should().Be.EqualTo("Invalid column name 'id'.");
        }

        public string Execute(string query, LoginDetails loginDetails)
        {
            string result;
            using (var ms = new MemoryStream(2048))
            using (var tw = new StreamWriter(ms))
            {
                var executor = new SqlExecutor(tw, loginDetails);
                executor.Execute(query);
                tw.Flush();
                ms.Position = 0;
                using (var tr = new StreamReader(ms))
                {
                    result = tr.ReadToEnd();
                }
            }
            return result;
        }
    }
}
