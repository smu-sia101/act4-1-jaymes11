using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route("generate")]
    public class PetNameController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> PetNames = new()
        {
            { "dog", new List<string> { "Buddy", "Max", "Charlie", "Rocky", "Rex" } },
            { "cat", new List<string> { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" } },
            { "bird", new List<string> { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" } }
        };

        [HttpPost]
        public IActionResult GeneratePetName(string AnimalType, bool TwoPart = false)
        {
            if (AnimalType == null)
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string animalType = AnimalType.ToString().ToLower();
            bool? twoPart = TwoPart != null ? (bool?)TwoPart : null;

            if (!PetNames.ContainsKey(animalType))
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (twoPart.HasValue && twoPart.Value.GetType() != typeof(bool))
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            var random = new Random();
            var nameList = PetNames[animalType];
            string generatedName = nameList[random.Next(nameList.Count)];

            if (twoPart == true)
            {
                generatedName += nameList[random.Next(nameList.Count)];
            }

            return Ok(new { name = generatedName });
        }
    }
}
