using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public class IsolatedExecutor : MarshalByRefObject
    {
        public object Execute(ExecutionParameters parameters, string code)
        {
            CSharpCodeProvider c = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();

            cp.ReferencedAssemblies.AddRange(parameters.AssemblyPaths);

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            CompilerResults cr = c.CompileAssemblyFromSource(cp, code);
            if (cr.Errors.Count > 0)
            {
                throw new Executor.CompilationErrorException(cr.Errors, code);
            }

            Assembly a = cr.CompiledAssembly;
            object codeObject = a.CreateInstance(parameters.MainObjectType);

            Type t = codeObject.GetType();
            MethodInfo mi = t.GetMethod(parameters.MethodName);
            object s;
            var task = Task.Run(() => s = mi.Invoke(codeObject, parameters.MethodParameters));

            bool successfulCompilation;
            try
            {
                successfulCompilation = task.Wait(TimeSpan.FromMilliseconds(Executor.MaxExecutionTimeOut));
            }
            catch (Exception e)
            {
                throw new Executor.ExecutionExceptionException(e, code);
            }
            if (successfulCompilation)
                return task.Result;
            else
            {
                if (task.Status == TaskStatus.Faulted)
                {
                    throw task.Exception;
                }
                else
                {
                    throw new Executor.ExecutionExceptionException(new Executor.ExecutionTimeOutException(Executor.MaxExecutionTimeOut), code);
                }
            }
        }
    }
}
