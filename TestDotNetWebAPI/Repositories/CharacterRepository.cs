using Neo4j.Driver;
using TestDotNetWebAPI.Model;

namespace TestDotNetWebAPI.Repositories
{
    public class CharacterRepository
    {
        private readonly IDriver _driver;
        private readonly QueryConfig _queryConfig;

        public CharacterRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task CreateCharacterAsync(Characters character)
        {
            var query = @"
            CREATE (c:Characters {
                name: $name, 
                height: $height, 
                mass: $mass, 
                skin_color: $skinColors, 
                hair_color: $hairColors, 
                eye_color: $eyeColors, 
                birth_year: $birthYear, 
                gender: $gender, 
                homeworld: $homeworld, 
                species: $species
            })
        ";

            var session = _driver.AsyncSession(o => o.WithDatabase("neo4j"));
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    await tx.RunAsync(query, new
                    {
                        name = character.Name,
                        height = character.Height,
                        mass = character.Mass,
                        skinColors=character.SkinColors,
                        hairColors=character.HairColors,
                        eyeColors=character.EyeColors,
                        birthYear=character.BirthYear,
                        gender=character.Gender,
                        homeworld=character.Homeworld,
                        species=character.Species
                    });
            });
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task UpdateCharacterAsync(Characters character)
        {
            var query = @"
            MATCH (c:Characters {name: $deletename})
            SET c.name= $name, 
                c.height= $height, 
                c.mass= $mass, 
                c.skin_color= $skinColors, 
                c.hair_color= $hairColors, 
                c.eye_color= $eyeColors, 
                c.birth_year= $birthYear, 
                c.gender= $gender, 
                c.homeworld= $homeworld, 
                c.species= $species
        ";

            var session = _driver.AsyncSession(o => o.WithDatabase("neo4j"));
            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    await tx.RunAsync(query, new
                    {
                        deletename = character.Name,
                        name = character.Name,
                        height = character.Height,
                        mass = character.Mass,
                        skinColors = character.SkinColors,
                        hairColors = character.HairColors,
                        eyeColors = character.EyeColors,
                        birthYear = character.BirthYear,
                        gender = character.Gender,
                        homeworld = character.Homeworld,
                        species = character.Species
                    });
                });
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task DeleteCharacterAsync(string name)
        {
            var query = @"
            MATCH (c:Characters {name: $name})
            DETACH DELETE c
            ";
            var session = _driver.AsyncSession(o => o.WithDatabase("neo4j"));

            try
            {
                await session.ExecuteWriteAsync(async tx =>
                {
                    await tx.RunAsync(query, new { name });
                });
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }

        }

        public async Task<Characters> GetCharacterAsync(string name)
        {
            var query = @"
            MATCH (c:Characters {name: $name})
            RETURN c
        ";

            var session = _driver.AsyncSession(o => o.WithDatabase("neo4j"));
            try
            {
                var character = await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new { name });

                    if (await cursor.FetchAsync())
                    {
                        var characterNode = cursor.Current["c"].As<INode>();

                        return new Characters
                        {
                            Name = characterNode["name"].As<string>(),
                            Height = characterNode["height"].As<string>(),
                            Mass = characterNode["mass"].As<string>(),
                            SkinColors = characterNode["skin_color"].As<string>(),
                            HairColors = characterNode["hair_color"].As<string>(),
                            EyeColors = characterNode["eye_color"].As<string>(),
                            BirthYear = characterNode["birth_year"].As<string>(),
                            Gender = characterNode["gender"].As<string>(),
                            Homeworld = characterNode["homeworld"].As<string>(),
                            Species = characterNode["species"].As<string>()
                        };
                    }
                    else
                    {
                        return null;
                    }
                });

                return character;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }

}
