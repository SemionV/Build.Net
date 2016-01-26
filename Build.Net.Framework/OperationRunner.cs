using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class OperationRunner<T> where T : RunContext
    {
        public OperationRegistry OperationRegistry { get; private set; }

        public OperationRunner(OperationRegistry registry)
        {
            OperationRegistry = registry;
        }

        public virtual OperationRunner<T> RunOperation(object key, T context)
        {
            return RunOperation<Operation<T>>(key, context, null);
        }

        public virtual OperationRunner<T> RunOperation<TOperation>(object key, T context, Action<TOperation> configure) where TOperation : Operation<T>
        {
            Type operationType = OperationRegistry.GetOperationType(key);
            var operation = CreateOperationInstance<TOperation>(operationType);

            if(configure != null)
            {
                configure(operation);
            }

            operation.Run(context);

            return this;
        }

        public virtual TOperation CreateOperationInstance<TOperation>(Type operationType) where TOperation : Operation<T>
        {
            return Activator.CreateInstance(operationType) as TOperation;
        }
    }
}
