using AspirePostgisSample.API;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<MyDbContext>("postgisdb", configureDbContextOptions: (configure =>
{
    configure.UseNpgsql(opt =>
    {
        opt.UseNetTopologySuite();
        opt.EnableRetryOnFailure();
    });

}));


builder.AddServiceDefaults();
var app = builder.Build();

//we use delay to postgres db up
await Task.Delay(10000);
using var scope = app.Services.CreateScope();
var dbcontext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
await dbcontext.Database.MigrateAsync();

app.MapDefaultEndpoints();

app.Map("/nearest-london", (MyDbContext context) =>
{
    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    var londonLocation = geometryFactory.CreatePoint(new Coordinate(-0.1276, 51.5074));

    var nearbyCities = context
    .Cities
    .OrderBy(city => city.Location.Distance(londonLocation))
    .Select(s=> new
    {
        s.Id,
        s.Name,
    })
    .ToListAsync();
    return nearbyCities;
});

app.Run();
