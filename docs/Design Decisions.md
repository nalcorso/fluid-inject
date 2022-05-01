



## Container Builder

When adding a Singleton to the container it makes sense to not allow the user to add an instance if they have previously created a Typed singleton.

```C#
var builder = new ContainerBuilder();

// Add a Singleton that will be create on first use
builder.AddSingleton<MyService>();

// Exception: The type MyService has already been registered as a singleton
builder.AddSingleton<MyService>(new MyService());
```

While it is technically feasable to add the instance to the container in the above example, it is not recommended as the behaviour will vary depending
on the prior usage.

We can reconsider this if a use case arises.