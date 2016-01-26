using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class OperationRegistry
    {
        public Dictionary<object, Type> Registry { get; private set; }

        public OperationRegistry()
        {
            Registry = new Dictionary<object, Type>();
        }

        public void RegisterOperation(Type operationType, object key)
        {
            Registry[key] = operationType;
        }

        public Type GetOperationType(object key)
        {
            Type operationType;
            if (Registry.TryGetValue(key, out operationType))
            {
                return operationType;
            }
            else
            {
                throw new Exception(String.Format("Operation {0} is not registered", key));
            }
        }
    }
}
