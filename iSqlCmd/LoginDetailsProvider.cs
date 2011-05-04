using System;
using NDesk.Options;

namespace ConsoleApplication13
{
    public class LoginDetailsProvider
    {
        public static LoginDetails Get(string[] args)
        {
            bool help = false;
            var loginDetails = new LoginDetails ();

            var optionSet = new OptionSet
                                {
                                    {"?", "show syntax summary", h => help = true},
                                    {"U", "login id (default: null/trusted connection)", user => loginDetails.UserName = user},
                                    {"P", "password (default: null/trusted connection)",password => loginDetails.Password = password},
                                    {"S", "server (default: .\\sqlexpress", server => loginDetails.Server = server},
                                    {"d", "database name (default: master)", db => loginDetails.DatabaseName = db},
                                    {"E", "trusted connection (default: true / if not user name is supplied)", db => loginDetails.DatabaseName = db }
                                };
            optionSet.Parse(args);

            if(help)
            {
                optionSet.WriteOptionDescriptions(Console.Out);
                return null;
            }
            return loginDetails;
        }
    }
}