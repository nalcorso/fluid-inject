// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

using Fluid.Inject;
using fluid.inject.console;

Console.WriteLine("FLUiD Inject - Console Example");

// 1. Create a Container. This is the root of the dependency injection tree.
IContainer container = new Container();

// Example - Resolve a dependency manually.
container.Add<MyBaseService>();
var my_base_service = container.Get<MyBaseService>();

// Example - Resolve and use a factory method.
container.Add<MyObject>();
var my_object_factory = container.Get<MyObject.Factory>();
_ = my_object_factory(1, "ABC");

// Example - Resolve a dependency that uses a factory method.
container.Add<MyService>();
var my_service = container.Get<MyService>();
my_service.Add(1, "ABC");

// Example - Resolve a named dependency.
container.Add<TestCommand1>().As<ICommand>().AsTransient().WithName("Test1Command");
container.Add<TestCommand2>().As<ICommand>().AsTransient().WithName("Test2Command");

var test_command1 = container.Get<ICommand>("Test1Command");
test_command1.Execute();

var test_command2 = container.Get<ICommand>("Test2Command");
test_command2.Execute();

// Example - Resolve an open generic
container.Add(typeof(GenericService<>))
    .As(typeof(IGenericService<>))
    .AsTransient();

var my_generic1 = container.Get<GenericService<int>>();
my_generic1.Execute();

var my_generic2 = container.Get<IGenericService<string>>();
my_generic2.Execute();


// The following features have not been implemented in the API yet.

// Example - Lazy Resolution of a Service (should warn on singleton probably).
//var lazy_service = container.Get<ILazy<MyBaseService>>();

// Example - Resolve All instances of a Service
//var all_commands = container.Get<IEnumerable<ICommand>>();

// Example - Add assembly as Module. Calls the assembly's Register / Unregister method.
//var assembly = typeof(MyService).Assembly;
//container.AddModule(assembly);
//container.RemoveModule(assembly);

// Example - Scan assemblies for all dependencies matching predicate.
//var assembly = typeof(MyService).Assembly;
//container.AddFrom(assembly).Where(t => t.FullName.EndsWith("ViewModel")).AsTransient();

// Example - Scan assembly for all services matching an interface
//var assembly = typeof(MyService).Assembly;
//container.AddFrom(assembly).As<IMyService>();
