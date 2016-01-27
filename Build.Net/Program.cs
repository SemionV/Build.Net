using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Build.Net.Framework;

namespace Build.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectPath = args[0];

            var registry = new OperationRegistry();
            var factory = new OperationFactory(registry);
            var runner = new OperationRunner<RunContext>(registry);
            var context = new RunContext();

            factory.LoadOperations(projectPath);

            runner.RunOperation(args[1], context);



            /*var collection = new ProjectCollection();
            collection.DefaultToolsVersion = "4.0";
            var project = collection.LoadProject(projectPath);
            var projectInstance = project.CreateProjectInstance();
            ConsoleLogger logger = new ConsoleLogger(LoggerVerbosity.Normal);
            
            var result = projectInstance.Build("Rebuild", new[] { logger });

            if (result)
            {
                var targetPath = projectInstance.GetPropertyValue("TargetPath");
                var targetDir = projectInstance.GetPropertyValue("TargetDir");

                Console.WriteLine("TargetPath: " + targetPath);

                ResolveEventHandler resolver = (object sender, ResolveEventArgs eventArgs) =>
                {
                    var dependency = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == eventArgs.Name);
                    if (dependency == null)
                    {
                        var length = eventArgs.Name.IndexOf(',');
                        if (length > 0)
                        {
                            dependency = Assembly.LoadFrom(Path.Combine(targetDir, eventArgs.Name.Substring(0, length) + ".dll"));
                        }
                    }

                    return dependency;
                };
                AppDomain.CurrentDomain.AssemblyResolve += resolver;
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolver;

                var assembly = Assembly.LoadFrom(targetPath);

                var projectType = typeof(Framework.Operation);
                var buildProjects = assembly.GetTypes().Where(x => projectType.IsAssignableFrom(x)).ToList();

                foreach (var buildProject in buildProjects)
                {
                    Console.WriteLine("Run: " + buildProject.FullName);
                    var task = Activator.CreateInstance(buildProject) as Framework.Operation;
                    task.Run(new RunContext());
                }
            }*/

            /*var pathToMsBuildBin = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions\4.0", "MSBuildToolsPath", String.Empty).ToString();

            var p = Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = Path.Combine(pathToMsBuildBin, "msbuild.exe"),
                RedirectStandardOutput = true,
                Arguments = projectPath + @" /t:Rebuild"
            });
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            Console.Write(output);*/
        }
    }
}
