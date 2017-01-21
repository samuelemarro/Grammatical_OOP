using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetterPECLE_v3
{
    public sealed class IsolatedEnvironment : IDisposable
    {
        private AppDomain _domain;
        private IsolatedExecutor _executor;

        public IsolatedEnvironment()
        {
            _domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(),
               null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(IsolatedExecutor);

            _executor = (IsolatedExecutor)_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public IsolatedExecutor Value
        {
            get
            {
                return _executor;
            }
        }

        public void Dispose()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);

                _domain = null;
            }
        }
    }
}
