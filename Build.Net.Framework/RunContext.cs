using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class RunContext
    {
        public OperationRunner Runner { get; set; }

        public RunContext(OperationRunner runner)
        {
            Runner = runner;
        }
    }
}
