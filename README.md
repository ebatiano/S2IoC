# XxIoC

**XxIoC** is a lightweight IoC solution providing Service Provider ans Objects Factory for your .NET projects.

## Initialization
To create new service container, `ScopeBuilder` class is used. Then you just need to register your services and instance types.
````
ScopeBuilder sb = new ScopeBuilder();

sb
  .WithInstance(new TestImplementation()).As<ITest>().As<ITestEx>() 	// Register instance as specific services.
  .WithType<FontService>().As<IFontService>().AsSingleton() 	// Register type as specific service(s).
  .WithType<MyFont>().As<IFont>(); 	// Register type as implementation of some abstract type.

IServicesProvider provider = sb.Build(); // Build scope and get services provider.
````

## Use Cases
### Resolving services
To get any service from provider, use `GetService` methods:
````
var service1 = (IService)provider.GetService(typeof(IService)); // returns object
var service2 = provider.GetService<IService>(); // returns exact type
````
There is also possibility to try to get service without exception on failure:
````
if(provider.TryResolveInstance(typeof(IService), out object instance))
{
	this.myService = (IService)instance;
}

if(provider.TryResolveInstance<IService>(out var service))
{
	this.myService = service;
}
````
### Constructing objects with service and parameters injection
`IObjectsFactory` let's us create instances of objects while injecting proper services and parameters to constructor.
First we need a `IObjectsFactory` from container, so at some point in code there will be at last one explicit service resolution. Later on `IObjectsFactory` will be injected into constructor like any other services.
````
IObjectsFactory factory = provider.GetService<IServicesFactory>();

class MyClass
{
	private readonly IObjectsFactory factory;
	public MyClass(IObjectsFactory factory)
	{
		this.factory = factory;
	}
}
````
Then we use `IObjectsFactory` to instantiate types:
````
var newObject = factory.Create<SomeClassWithInjectedServices>();
````
All required services will be injected to the constructor of the class. Optional services or parameters are also possible. So when you write default value to some constructor argument, factory won't throw an exception if the service or parameter is not defined:
````
public MyClass(IFontService fontService = null)
{
	this.fontService = fontService ?? new FontService();
}
````
#### Additional parameters while constructing objects.
You can pass additional parameters to `Create` method and they will be injected like services into constructor.
````
class MyClass
{
	public class Parameters
	{
		public bool Check;
		public int Value;
	}
	public MyClass(IFontService fontService, Parameters parameters)
	{
		// Do some initialization
	}
}

...

var myClass = factory.Create<MyClass>(new MyClass.Parameters
{
	Check = true,
	Value = 10
});
````
Built in types can also be passed, but remember, that parameter type matching algorithm will match first parameter of desired type, so if you write something like:
````
public MyClass(int firstValue, int secondValue)
{
}

[...]

var myClass = factory.Create<MyClass>(5,10);
````

`firstValue` and `secondValue` will have value of 5, because 5 is first `int` passed as parameter. That's why it is advised to create construction parameters class like in previous example.

## Creating new scope
When you want to create a new scope which extends current one, just use `WithParent` method on new `ScopeBuilder`:
````
var sb = new ScopeBuilder()
		.WithParent(provider);
				
// Register your scope services and types.
sb.WithType<MyType>().As<IType>().AsSingleton();
````
## Destroying scope
To destroy scope and dispose all services registered in that scope, use `IServicesProvider.Dispose` method.

## NuGet
This lib is avaliable as NuGet package as `XxIoC`.
````
nuget install XxIoC
````
