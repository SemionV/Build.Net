using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Build.Net.Tasks;

namespace Build.Net.Configs
{
    public class SmartData: Configuration
    {
        public string RepositoryPath { get { return @"c:\Work\VIP\SmartData\trunk"; } }
        public string SQLRepositoryPath { get { return Path.Combine(RepositoryPath, @"\Database"); } }

        private ComputableString webServiceRepositoryPath = new ComputableString(@"$(RepositoryPath)\WebServices");
        public ComputableString WebServiceRepositoryPath { get { return webServiceRepositoryPath; } set { webServiceRepositoryPath = value; } }

        public virtual void Configure()
        {
            WebServiceRepositoryPath = new ComputableString("defined: $(SmartData.SQLRepositoryPath)");
        }
    }
}
