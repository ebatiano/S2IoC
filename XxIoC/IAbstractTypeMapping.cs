using System;

namespace XxIoC
{
    internal interface IAbstractTypeMapping
    {
        bool FindMapping(Type abstractType, out Type implementationType);
    }
}
