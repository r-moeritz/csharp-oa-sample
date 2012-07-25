using System;
using Castle.DynamicProxy;

namespace OASample.DynamicProxy
{
    class MethodInterceptor : IInterceptor
    {
        private readonly Delegate _impl;

        public MethodInterceptor(Delegate impl)
        {
            _impl = impl;
        }

        public void Intercept(IInvocation invocation)
        {
            var result = _impl.DynamicInvoke(invocation.Arguments);
            invocation.ReturnValue = result;
        }
    }
}
