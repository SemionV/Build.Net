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

        public virtual OperationRunner RunOperation<TContext>(object key, TContext context) where TContext: RunContext
        {
            return RunOperation<TContext, Operation<TContext>>(key, context, null);
        }

        public virtual OperationRunner RunOperation<TContext, TOperation>(object key, TContext context, Action<TOperation> configure) where TContext: RunContext where TOperation : Operation<TContext>
        {
            Type operationType = OperationRegistry.GetOperationType(key);
            var operation = CreateOperationInstance<TContext, TOperation>(operationType);

            if(configure != null)
            {
                configure(operation);
            }

            operation.Run(context, this);

            return this;
        }

        public virtual TOperation CreateOperationInstance<TContext, TOperation>(Type operationType)
            where TContext : RunContext
            where TOperation : Operation<TContext>
        {
            return Activator.CreateInstance(operationType) as TOperation;
        }
    }
}
