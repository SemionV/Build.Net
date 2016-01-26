using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Build.Net.Framework
{
    public class OperationFactory
    {
        public OperationRegistry Registry { get; private set; }

        public OperationFactory(OperationRegistry registry)
        {
            Registry = registry;
        }

        public virtual void LoadOperations(string projectPath)
        {
            var assembly = BuildAndLoadProject(projectPath);
            RegisterOperations(assembly);
        }

        public virtual BuildProject LoadProject(string projectPath)
        {
            var assembly = BuildAndLoadProject(projectPath);
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        protected virtual void RegisterOperations(Assembly assembly)
        {
            var baseOperationType = typeof(Operation<>);
            var operationTypes = assembly.GetTypes().Where(x => IsSubclassOfRawGeneric(baseOperationType, x)).ToList();

            foreach (var operationType in operationTypes)
            {
                object key;
                var metaAttribute = operationType.GetCustomAttribute<OperationAttribute>();
                if(metaAttribute != null && !String.IsNullOrEmpty(metaAttribute.Name))
                {
                    key = metaAttribute.Name;
                }
                else
                {
                    key = Guid.NewGuid().ToString();
                }

                RegisterOperation(key, operationType);
            }
        }

        protected virtual void RegisterOperation(object key, Type operationType)
        {
            Registry.RegisterOperation(operationType, key);
        }

        protected virtual Assembly BuildAndLoadProject(string projectPath)
        {
            var collection = new ProjectCollection();
            collection.DefaultToolsVersion = "4.0";
            var project = collection.LoadProject(projectPath);
            var projectInstance = project.CreateProjectInstance();
            ConsoleLogger logger = new ConsoleLogger(LoggerVerbosity.Normal);

            if(projectInstance.Build("Rebuild", new[] { logger }))
            {
                var targetPath = projectInstance.GetPropertyValue("TargetPath");
                var targetDir = projectInstance.GetPropertyValue("TargetDir");

                return LoadProjectAssembly(targetPath, targetDir);
            }
            else
            {
                throw new Exception("Compilation of the project is failed");
            }
        }

        protected virtual Assembly LoadProjectAssembly(string assemblyPath, string projectBinPath)
        {
            ResolveEventHandler resolver = (object sender, ResolveEventArgs eventArgs) =>
            {
                var dependency = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == eventArgs.Name);
                if (dependency == null)
                {
                    var length = eventArgs.Name.IndexOf(',');
                    if (length > 0)
                    {
                        dependency = Assembly.LoadFrom(Path.Combine(projectBinPath, eventArgs.Name.Substring(0, length) + ".dll"));
                    }
                }

                return dependency;
            };
            AppDomain.CurrentDomain.AssemblyResolve += resolver;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolver;

            return Assembly.LoadFrom(assemblyPath);
        } 
    }
}
