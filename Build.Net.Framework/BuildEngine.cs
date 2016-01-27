using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace Build.Net.Framework
{
    public class BuildEngine
    {
        public BuildProject BuildProject { get; private set; }

        public BuildEngine Run(object operationKey)
        {
            if (operationKey == null)
            {
                operationKey = GetDefaultOperationKey();
            }

            var context = CreateContext();

            RunOperation<RunContext, Operation<RunContext>>(operationKey, context);

            return this;
        }

        public BuildProject LoadBuildProject(string path)
        {
            
        }

        public BuildProject LoadBuildProject(Assembly assembly)
        {
            
        }

        protected virtual Assembly BuildAndLoadProject(string projectPath)
        {
            var collection = new ProjectCollection();
            collection.DefaultToolsVersion = "4.0";
            var project = collection.LoadProject(projectPath);
            var projectInstance = project.CreateProjectInstance();
            ConsoleLogger logger = new ConsoleLogger(LoggerVerbosity.Normal);

            if (projectInstance.Build("Rebuild", new[] { logger }))
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

        protected object GetDefaultOperationKey()
        {
            return "Default";
        }

        protected RunContext CreateContext()
        {
            return Activator.CreateInstance(BuildProject.Context) as RunContext;
        }

        protected TOperation CreateOperation<TContext, TOperation>(object key)
            where TContext : RunContext
            where TOperation : Operation<TContext>
        {
            var type = GetOperationType(key);
            return Activator.CreateInstance(type) as TOperation;
        }

        protected Type GetOperationType(object key)
        {
            Type operationType;
            if (BuildProject.Operations.TryGetValue(key, out operationType))
            {
                return operationType;
            }
            else
            {
                throw new Exception(String.Format("Operation {0} is not registered", key));
            }
        }

        public void RunOperation<TContext, TOperation>(object key, TContext context)
            where TContext : RunContext
            where TOperation : Operation<TContext>
        {
            var operation = CreateOperation<TContext, TOperation>(key);
            operation.Run(context);
        }
    }
}
