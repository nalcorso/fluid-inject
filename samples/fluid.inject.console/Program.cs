// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

using Fluid.Inject;
using fluid.inject.console;

Console.WriteLine("FLUiD Inject - Console Example");

// 1. Create a Container. This is the root of the dependency injection tree.
IContainer container = new Container();

container.Add<MyObject>();
container.Add<MyService>();
container.Add<MyBaseService>();

// Example - Resolve a dependency manually.
var my_base_service = container.Get<MyBaseService>();

// Example - Resolve and use a factory method.
var my_object_factory = container.Get<MyObject.Factory>();
var my_object = my_object_factory(1, "ABC");

// Example - Resolve a dependency that uses a factory method.
var my_service = container.Get<MyService>();
my_service.Add(1, "ABC");

// The following features have not been implemented in the API yet.

// Example - Resolve a named dependency.
//var my_named_service = container.Get<MyService>("named");

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
