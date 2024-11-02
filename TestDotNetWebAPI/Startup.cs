using Neo4j.Driver;
using TestDotNetWebAPI.Repositories;

namespace TestDotNetWebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton(GraphDatabase.Driver(
            Environment.GetEnvironmentVariable("NEO4J_URI") ?? "neo4j+s://xxxx.databases.neo4j.io",
            AuthTokens.Basic(
                Environment.GetEnvironmentVariable("NEO4J_USER") ?? "xxxx",
                Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? "xxxxxx"
            )
        ));
        services.AddSingleton<CharacterRepository>();
        services.AddSingleton<PlanetsRepository>();
        // Add Swagger services
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        // Enable middleware to serve generated Swagger as a JSON endpoint
        app.UseSwagger();

        app.UseSwaggerUI();

    }
}