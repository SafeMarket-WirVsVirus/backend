using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ReservationSystem
{
    public class HumanReadableKeyGeneratorService
    {
        public ImmutableArray<string> CodeWords { get; }


        public HumanReadableKeyGeneratorService(IConfiguration configuration)
        {
            var myJsonString = File.ReadAllText(Path.Combine(
                configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "CodeWords.json"));
            var result = JsonSerializer.Deserialize<CodeWords>(myJsonString);
            CodeWords = result.foods.ToImmutableArray();
        }


        public string GetHumanReadableText()
        {
            var randomIndex = (new Random()).Next(0, CodeWords.Length);
            return CodeWords[randomIndex];
        }
    }

    public class CodeWords
    {
        public string[] foods { get; set; }
    }
}