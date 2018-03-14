using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grammatical_OOP
{
    [Serializable]
    public struct ExecutionParameters
    {
        public string BluePrintCode { get; set; }

        public string MainObjectType { get; set; }

        public string MethodName { get; set; }

        public string[] AssemblyPaths { get; set; }

        public object[] MethodParameters { get; set; }

        public ExecutionParameters(string bluePrintCode, string mainObjectType, string methodName)
        {
            BluePrintCode = bluePrintCode;
            MainObjectType = mainObjectType;
            MethodName = methodName;

            List<string> assemblyPaths = new List<string>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    assemblyPaths.Add(GetAssemblyDirectory(a));
                }
                catch (NotSupportedException)
                {
                    //Dynamic assemblies are ignored
                }
            }

            AssemblyPaths = assemblyPaths.ToArray();
            MethodParameters = new object[0];
        }

        public ExecutionParameters(string bluePrintCode, string mainObjectType, string methodName, object[] methodParameters)
        {
            BluePrintCode = bluePrintCode;
            MainObjectType = mainObjectType;
            MethodName = methodName;

            List<string> assemblyPaths = new List<string>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    assemblyPaths.Add(GetAssemblyDirectory(a));
                }
                catch (NotSupportedException)
                {
                    //Dynamic assemblies are ignored
                }
            }

            AssemblyPaths = assemblyPaths.ToArray();
            MethodParameters = methodParameters;
        }

        public ExecutionParameters(string bluePrintCode, string mainObjectType, string methodName, string[] assemblyPaths)
        {
            BluePrintCode = bluePrintCode;
            MainObjectType = mainObjectType;
            MethodName = methodName;
            

            AssemblyPaths = assemblyPaths;
            MethodParameters = new object[0];
        }

        public ExecutionParameters(string bluePrintCode, string mainObjectType, string methodName, string[] assemblyPaths, object[] methodParameters)
        {
            BluePrintCode = bluePrintCode;
            MainObjectType = mainObjectType;
            MethodName = methodName;
            AssemblyPaths = assemblyPaths;
            MethodParameters = methodParameters;
        }

        private static string GetAssemblyDirectory(Assembly a)
        {
            string path = a.CodeBase;
            path = (new Uri(path)).AbsolutePath;
            path = Uri.UnescapeDataString(path);
            path = Path.GetFullPath(path);
            return path;
        }

    }
}
