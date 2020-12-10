using System;

namespace S2IoC
{
    internal interface IAbstractTypeMapping
    {
        bool FindMapping(Type abstractType, out Type implementationType);
    }
}
