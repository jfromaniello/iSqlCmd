using System;
using System.Linq;

namespace iSqlCmd
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var login = LoginDetailsProvider.Get(args);
            if (login == null) return;
            var executor = new SqlExecutor(Console.Out, login);
            using(login)
            {
                while (true)
                {
                    var query = GetSqlLines();
                    if(query == ":clear")
                    {
                        Console.Clear();
                        continue;
                    }
                    query = query.StartsWith(":") ? ShortcutTranslator.Translate(query) : query;
                    executor.Execute(query);
                    Console.WriteLine();
                }    
            }
        }

        private static string GetSqlLines()
        {
            var lines = Enumerable.Range(1, int.MaxValue)
                      .Select(i =>
                                {
                                    Console.Write("{0}> ", i);
                                    return Console.ReadLine();
                                })
                      .TakeUntil(s => s == null || s.EndsWith(";") || s.StartsWith(":"))
                      .ToArray();
            return string.Join(Environment.NewLine, lines);
        }

    }
}
