using System;

namespace S2IoC
{
    public interface IObjectFactory
    {
        object Create(Type type, params object[] parameters);
        TObject Create<TObject>(params object[] parameters);
    }
}
