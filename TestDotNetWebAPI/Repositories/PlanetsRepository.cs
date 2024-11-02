using Neo4j.Driver;
using TestDotNetWebAPI.Model;

namespace TestDotNetWebAPI.Repositories
{
    public class PlanetsRepository
    {
        private readonly IDriver _driver;

        public PlanetsRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Planets> GetPlanets(string name)
        {
            var query = @"
            MATCH (p:Planets {name: $name})
            RETURN c
        ";
            var session = _driver.AsyncSession(o => o.WithDatabase("neo4j"));
            try
            {
                var plantes = await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new { name });

                    if (await cursor.FetchAsync())
                    {
                        var planetsNode = cursor.Current["p"].As<INode>();
                        return new Planets
                        {
                            Name = planetsNode["name"].As<string>(),
                            RotationPeriod = planetsNode["rotation_period"].As<string>(),
                            OrbitalPeriod = planetsNode["orbital_period"].As<string>(),
                            Diameter = planetsNode["diameter"].As<string>(),
                            Climate = planetsNode["climate"].As<string>(),
                            Gravity = planetsNode["gravity"].As<string>(),
                            Terrain = planetsNode["terrain"].As<string>(),
                            SurfaceWater = planetsNode["surface_water"].As<string>(),
                            Population = planetsNode["population"].As<string>(),
                        };
                    }
                    else
                    {
                        return null;
                    }
                });
                return plantes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { await session.CloseAsync(); }

            
        }
    }
}
