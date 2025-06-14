using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceNet10;

[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MarkdownExporterAttribute.GitHub]
[KeepBenchmarkFiles]
[MemoryDiagnoser]
public class SystemTextJsonTests
{
    private const string Json = """
                                {
                                    "Name": "Steven Giesel",
                                    "Age": 34,
                                    "IsEmployed": true,
                                    "Skills": ["C#", "Blazor", "ASP.NET Core"],
                                    "Address": {
                                        "Street": "1725 Slough Avenue",
                                        "Suite": "Suite 200",
                                        "City": "Scranton",
                                        "State": "PA",
                                        "ZipCode": "18505"
                                    },
                                    "PhoneNumbers": [
                                        {
                                            "Type": "Home",
                                            "Number": "123-456-7890"
                                        },
                                        {
                                            "Type": "Work",
                                            "Number": "098-765-4321"
                                        }
                                    ]
                                }
                                """;
    private readonly Person person = new()
    {
        Name = "Steven Giesel",
        Age = 34,
        IsEmployed = true,
        Skills = ["C#", "Blazor", "ASP.NET Core"],
        Address = new Address
        {
            Street = "1725 Slough Avenue",
            Suite = "Suite 200",
            City = "Scranton",
            State = "PA",
            ZipCode = "18505"
        },
        PhoneNumbers =
        [
            new PhoneNumber { Type = "Home", Number = "123-456-7890" },
            new PhoneNumber { Type = "Work", Number = "098-765-4321" }
        ]
    };

    [Benchmark]
    public Person DeserializePerson()
    {
        return System.Text.Json.JsonSerializer.Deserialize<Person>(Json)!;
    }

    [Benchmark]
    public string SerializePerson()
    {
        return System.Text.Json.JsonSerializer.Serialize(person);
    }

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool IsEmployed { get; set; }
        public List<string> Skills { get; set; } = [];
        public Address Address { get; set; } = new();
        public List<PhoneNumber> PhoneNumbers { get; set; } = [];
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string Suite { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class PhoneNumber
    {
        public string Type { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
    }
}