// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

using Fluid.Inject;

Console.WriteLine("FLUiD Inject Console Example");

IContainer container = new Container();

container.Add<MyObject>();
container.Add<MyService>();
container.Add<MyBaseService>();

var my_base_service = container.Get<MyBaseService>();

var my_object_factory = container.Get<MyObject.Factory>();
var my_object = my_object_factory(1, "ABC");

var my_service = container.Get<MyService>();
my_service.Add(1, "ABC");

public class MyBaseService {}

public class MyObject
{
    private readonly MyBaseService _base_service;

    public delegate MyObject Factory(int index, string name);

    public int Index { get; set; }
    public string Name { get; set; }

    public MyObject(int index, string name, MyBaseService base_service)
    {
        _base_service = base_service;
        Index = index;
        Name = name;
    }
}

public class MyService
{
    private readonly MyObject.Factory _my_object_factory;

    private readonly List<MyObject> _items = new();

    public MyService(MyObject.Factory my_object_factory)
    {
        _my_object_factory = my_object_factory;
    }

    public void Add(int index, string name)
    {
        _items.Add(_my_object_factory(index, name));
    }
}
