using Microsoft.AspNetCore.Mvc;
using TestDotNetWebAPI.Model;
using TestDotNetWebAPI.Repositories;

namespace TestDotNetWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterRepository _repository;

        public CharacterController(CharacterRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] Characters character)
        {
            await _repository.CreateCharacterAsync(character);
            return CreatedAtAction(nameof(GetCharacter), new { name = character.Name }, character);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter([FromBody] Characters character)
        {
            await _repository.UpdateCharacterAsync(character);
            return CreatedAtAction(nameof(GetCharacter), new { name = character.Name }, character);

        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCharacter(string name)
        {
            await _repository.DeleteCharacterAsync(name);
            return Ok();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCharacter(string name)
        {
            var character = await _repository.GetCharacterAsync(name);
            if (character == null)
            {
                return NotFound();
            }
            return Ok(character);
        }

    }

}
