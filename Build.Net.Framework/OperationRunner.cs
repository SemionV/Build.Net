using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class OperationRunner
    {
        public OperationRegistry OperationRegistry { get; private set; }

        public OperationRunner(OperationRegistry registry)
        {
            OperationRegistry = registry;
        }

        public virtual OperationRunner RunOperation(object key, RunContext context)
        {
            return RunOperation<Operation>(key, context, null);
        }

        public virtual OperationRunner RunOperation<T>(object key, RunContext context, Action<T> configure) where T : Operation
        {
            Type operationType = OperationRegistry.GetOperationType(key);
            var operation = CreateOperationInstance<T>(operationType);

            if(configure != null)
            {
                configure(operation);
            }

            operation.Run(context);

            return this;
        }

        public virtual T CreateOperationInstance<T>(Type operationType) where T : Operation
        {
            return Activator.CreateInstance(operationType) as T;
        }
    }
}
