using TestUseCaseForDIContainer;
using DotNetDiContainer;

var services = new ServiceCollection();
//Scope
//
services.AddScoped<IIdGenerator, IdGenerator>();


//Transient
//
//services.AddTransient<IIdGenerator, IdGenerator>();
//services.AddTransient(provider => new IdGenerator(provider.GetService<ConsoleWriter>()!));

//Singleton
//
services.AddSingleton<IConsoleWriter, ConsoleWriter>();
//services.AddSingleton<ConsoleWriter>();

//services.AddService(new ServiceDescriptor
//{
//    ImplementationType = typeof(ConsoleWriter),
//    ServiceType = typeof(IConsoleWriter),
//    Lifetime = ServiceLifetime.Singleton
//});

//services.AddSingleton(new IdGenerator(new ConsoleWriter()));
var serviceProvider = services.BuildServiceProvider();

//////Scoped Service
using (var scope1 = serviceProvider.CreateScope())
{
    var service1 = scope1.GetService<IIdGenerator>();
    var service2 = scope1.GetService<IIdGenerator>();
    service1.PrintId();
    service2.PrintId();
}
using (var scope1 = serviceProvider.CreateScope())
{
    var service1 = scope1.GetService<IIdGenerator>();
    var service2 = scope1.GetService<IIdGenerator>();
    service1.PrintId();
    service2.PrintId();
}
//var service3 = serviceProvider.GetService<IIdGenerator>();
//var service4 = serviceProvider.GetService<IIdGenerator>();
//Console.WriteLine(service.Id);
//Console.WriteLine(service2.Id);
//service3.PrintId();
//service4.PrintId();