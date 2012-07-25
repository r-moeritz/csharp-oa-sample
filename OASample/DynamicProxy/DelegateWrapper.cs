using System;
using Castle.DynamicProxy;

namespace OASample.DynamicProxy
{
    static class DelegateWrapper
    {
        public static T WrapAs<T>(Delegate impl) where T : class
        {
            var generator = new ProxyGenerator();
            var interceptor = new MethodInterceptor(impl);
            var proxy = generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
            return proxy;
        }
    }
}