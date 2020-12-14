using System;

namespace XxIoC
{
    public interface IObjectFactory
    {
        object Create(Type type, params object[] parameters);
        TObject Create<TObject>(params object[] parameters);
    }
}
