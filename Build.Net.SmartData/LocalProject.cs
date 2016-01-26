using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Build.Net.Framework;

namespace Build.Net.SmartData
{
    [Operation(Name = "Local")]
    public class LocalOperation: Operation<RunContext>
    {
        public override bool Run(RunContext context)
        {
            Console.WriteLine("Build Local");
            return base.Run(context);
        }
    }

    [Operation(Name = "Dev")]
    public class DevOperation : Operation<RunContext>
    {
        public override bool Run(RunContext context)
        {
            Console.WriteLine("Build Dev");
            return base.Run(context);
        }
    }

    [Operation(Name = "QA")]
    public class QAOperation : Operation<RunContext>
    {
        public override bool Run(RunContext context)
        {
            Console.WriteLine("Build QA");
            return base.Run(context);
        }
    }

    [Operation(Name = "Configure")]
    public class ConfigureOperation : Operation<RunContext>
    {
        public override bool Run(RunContext context)
        {
            Console.WriteLine("Run Configure");
            return base.Run(context);
        }
    }

    [Operation(Name = "Default")]
    public class DefaultOperation : Operation<RunContext>
    {
        public override bool Run(RunContext context)
        {
            Console.WriteLine("Run Default");

            context.Runner
                .RunOperation<ConfigureOperation>("Configure", context, o => { })
                .RunOperation("Local", context)
                .RunOperation("Dev", context);

            return base.Run(context);
        }
    }
}
