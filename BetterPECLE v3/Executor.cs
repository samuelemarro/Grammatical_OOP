using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public static class Executor
    {
        public static int MaxExecutionTimeOut
        {
            get
            {
                return maxExecutionTimeOut;
            }
            set
            {
                maxExecutionTimeOut = value;
            }
        }

        private static int maxExecutionTimeOut = 5000;

        public static int MaxExecutions
        {
            get
            {
                return maxExecutions;
            }
            set
            {
                maxExecutions = value;
            }
        }

        private static int maxExecutions = 20;

        private static IsolatedEnvironment environment = new IsolatedEnvironment();

        private static int executions = 0;
        
        public static object Execute(ExecutionParameters parameters, string code)
        {
            string newCode = parameters.BluePrintCode.Replace("PECLECODE", code);
            executions++;
            if (executions > maxExecutions)
            {
                environment.Dispose();
                environment = new IsolatedEnvironment();
                executions = 0;
            }

            return environment.Value.Execute(parameters, newCode);
        }

        [Serializable]
        public class CompilationErrorException : Exception, ISerializable
        {
            public CompilerErrorCollection Errors { get; }
            public string Code { get; }
            public CompilationErrorException(CompilerErrorCollection errors, string code) : base("One or more errors during compilation.")
            {
                Errors = errors;
                Code = code;
            }

            protected CompilationErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                Errors = (CompilerErrorCollection)info.GetValue("Errors", typeof(CompilerErrorCollection));
                Code = info.GetString("Code");
            }

            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Errors", Errors);
                info.AddValue("Code", Code);

                base.GetObjectData(info, context);
            }

        }

        [Serializable]
        public class ExecutionExceptionException : Exception, ISerializable
        {
            public string Code { get; }

            public ExecutionExceptionException(Exception executionException, string code) : base("Exception during execution: \"" + executionException.Message + "\".", executionException)
            {
                Code = code;
            }
            protected ExecutionExceptionException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                Code = info.GetString("Code");
            }

            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Code", Code);

                base.GetObjectData(info, context);
            }

        }

        [Serializable]
        public class ExecutionTimeOutException : Exception
        {
            public ExecutionTimeOutException(int maxCompilationTimeOut) : base("The program took more than " + maxCompilationTimeOut + " milliseconds to run and was stopped.") { }
            public ExecutionTimeOutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }

    }
}
