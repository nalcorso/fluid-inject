// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

using Fluid.Inject;

Console.WriteLine("FLUiD Inject Console Example");

IContainer container = new Container();

// Simplest example of adding a service
container.AddSingleton(new GuidService1());

// Ability to Register Instances or lazily create them
//builder.AddSingleton(instance);
//builder.AddSingleton<Interface>(instance);
//builder.AddSingleton<Implementation>();
//builder.AddSingleton<Interface, Implementation>();

// Ability to Register By InstanceType (Singleton & Transient)
container.AddSingleton<GuidService2>();
container.AddTransient<GuidService3>();

// Ability to Register Type with Interface Type
container.AddTransient<IGuidGenerator, GuidService4>();

//builder.AddTransient<ISharedServiceInterface, ServiceWithInterface1>();
//builder.AddTransient<ISharedServiceInterface, ServiceWithInterface2>();

// Ability to Register Types with Dependencies
container.AddTransient<ISharedServiceInterface, ServiceWithDependency>();

// Ability to Receive the Container as a Dependency
container.AddTransient<ServiceWithContainerDependency>();

// Ability to Register Named Services
//builder.AddNamesTransient<ICommand, SomeCommand1>("SomeCommand1");
//builder.AddNamedTransient<ICommand, SomeCommand2>("SomeCommand2");
//builder.AddNamedSingleton<ICommand, SomeCommand3>("SomeCommand3");
//builder.AddNamedSingleton<ICommand, SomeCommand4>("SomeCommand4");

// Ability to Register All Types in Assembly that implement an Interface
//var assembly = Assembly.GetExecutingAssembly();
//builder.AddAssemblyTransiants<ICommand>(assembly);
//builder.AddAssemblySingletons<IService>(assembly);

// Ability to Register All Types in Assembly that match a Pattern
//builder.AddAssemblyTransiants(assembly, t => t.Name.EndsWith("ViewModel") && t.IsPublic);

// Ability to Register All Modules in Assembly
//builder.AddAssemblyPlugins(assembly);

var guid_service1_instance1 = container.Get<GuidService1>();
var guid_service1_instance2 = container.Get<GuidService1>();
Console.WriteLine("The following 2 Guid's should match:");
Console.WriteLine("    Guid from guid_service1_instance1: {0}", guid_service1_instance1.ServiceId);
Console.WriteLine("    Guid from guid_service1_instance2: {0}", guid_service1_instance2.ServiceId);
Console.WriteLine();

var guid_service2_instance1 = container.Get<GuidService2>();
var guid_service2_instance2 = container.Get<GuidService2>();
Console.WriteLine("The following 2 Guid's should match:");
Console.WriteLine("    Guid from guid_service2_instance1: {0}", guid_service2_instance1.ServiceId);
Console.WriteLine("    Guid from guid_service2_instance2: {0}", guid_service2_instance2.ServiceId);
Console.WriteLine();

var guid_service3_instance1 = container.Get<GuidService3>();
var guid_service3_instance2 = container.Get<GuidService3>();
var guid_service3_instance3 = container.Get<GuidService3>();
Console.WriteLine("The following 3 Guid's should differ:");
Console.WriteLine("    Guid from guid_service2_instance1: {0}", guid_service3_instance1.ServiceId);
Console.WriteLine("    Guid from guid_service2_instance2: {0}", guid_service3_instance2.ServiceId);
Console.WriteLine("    Guid from guid_service2_instance3: {0}", guid_service3_instance3.ServiceId);
Console.WriteLine();

var service_with_interface = container.Get<ISharedServiceInterface>();
Console.WriteLine("The following should print from Service1, Service2 or ServiceWithDependency:");
Console.Write("    ");
service_with_interface.Print();
Console.WriteLine();

//var service1 = container.Get<IService1>();
//var service2 = container.Get<IService2>();

//var viewmodel = container.Get<MainWindowViewModel>();

//var foo_command = container.Get<ICommand, Command1>("foo.command");
//var bar_command = container.Get<ICommand, Command2>("bar.command");

//var factory_item_factory = container.Get<FactoryItem.Factory>();
//var factory_item = factory_item_factory(123, "Foo");

//IEnumerable<ICommand> commands = container.Get<ICommand>();

//internal interface IService1 {}

//internal interface IService2 {}

//internal interface IService3 {}

//internal interface ICommand {}

//public class SampleService1 : IService1 {}

//public class SampleService2 : IService2 {}

//public class SampleService3 : IService3 {}

public class GuidService1
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}

public class GuidService2
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}

public class GuidService3
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}

public interface IGuidGenerator
{
    public Guid ServiceId { get; }
}

public class GuidService4 : IGuidGenerator
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}

public interface ISharedServiceInterface
{
    public void Print();
}

public class ServiceWithInterface1 : ISharedServiceInterface
{
    public void Print()
    {
        Console.WriteLine("Printing from service: ServiceWithInterface1");
    }
}

public class ServiceWithInterface2 : ISharedServiceInterface
{
    public void Print()
    {
        Console.WriteLine("Printing from service: ServiceWithInterface2");
    }
}

public class ServiceWithDependency : ISharedServiceInterface
{
    private readonly IGuidGenerator _guid_generator;

    public ServiceWithDependency(IGuidGenerator guid_generator)
    {
        _guid_generator = guid_generator;
    }

    public void Print()
    {
        Console.WriteLine("Printing from service: ServiceWithDependency");
    }
}

public class ServiceWithContainerDependency
{
    public IContainer Container { get; }

    public ServiceWithContainerDependency(IContainer container)
    {
        Container = container;
    }

    public void Print()
    {
        Console.WriteLine("Printing from service: ServiceWithContainerDependency");
    }
}

//public class Command1 : ICommand {}

//public class Command2 : ICommand {}

//public interface IPlugin
//{
//    public void Load(ContainerBuilder builder);
//}

//public class PluginCommand : ICommand {}

//public class Plugin1 : IPlugin
//{
//    public void Load(ContainerBuilder builder)
//    {
//builder.AddTransiant<ICommand, PluginCommand>("baz.command");
//    }
//}

//public class MainWindowViewModel {}

/*public class FactoryItem
{
    public delegate FactoryItem Factory(int index, string name);

    public int Index { get; init; }
    public string Name { get; init; }

    public FactoryItem(int index, string name)
    {
        Index = index;
        Name = name;
    }
}*/

/*public class FactoryItemCollection
{
    private readonly FactoryItem.Factory _factory_item_factory;

    private readonly List<FactoryItem> _items = new();

    public FactoryItemCollection(FactoryItem.Factory factory_item_factory)
    {
        _factory_item_factory = factory_item_factory;
    }

    public void Add(int index, string name)
    {
        _items.Add(_factory_item_factory(index, name));
    }
}*/
