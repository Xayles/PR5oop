using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SerializeDemo
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Address Addr { get; set; }
        public List<string> Hobbies { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int Building { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var person = new Person
            {
                FirstName = "Іван",
                LastName = "Петров",
                Age = 30,
                Addr = new Address { City = "Київ", Street = "Садова", Building = 12 },
                Hobbies = new List<string> { "Читання", "Програмування", "Футбол" }
            };

            string jsonPath = "person.json";
            string xmlPath = "person.xml";

            SerializeToJson(person, jsonPath);
            var personFromJson = DeserializeFromJson<Person>(jsonPath);

            SerializeToXml(person, xmlPath);
            var personFromXml = DeserializeFromXml<Person>(xmlPath);

            Console.WriteLine("=== Об'єкт з JSON ===");
            PrintPerson(personFromJson);

            Console.WriteLine("\n=== Об'єкт з XML ===");
            PrintPerson(personFromXml);

            Console.WriteLine("\nФайли збережено у поточній папці програми:");
            Console.WriteLine(Path.GetFullPath(jsonPath));
            Console.WriteLine(Path.GetFullPath(xmlPath));
            Console.WriteLine("\nПерегляньте їх у будь-якому текстовому редакторі або браузері.");
            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }

        static void SerializeToJson<T>(T obj, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
            string json = JsonSerializer.Serialize(obj, options);
            File.WriteAllText(filePath, json);
        }

        static T DeserializeFromJson<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(json, options);
        }

        static void SerializeToXml<T>(T obj, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stream = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(stream, obj);
        }

        static T DeserializeFromXml<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stream = new FileStream(filePath, FileMode.Open);
            return (T)serializer.Deserialize(stream);
        }

        static void PrintPerson(Person p)
        {
            if (p == null)
            {
                Console.WriteLine("null");
                return;
            }
            Console.WriteLine($"{p.FirstName} {p.LastName}, Age: {p.Age}");
            if (p.Addr != null)
                Console.WriteLine($"Address: {p.Addr.City}, {p.Addr.Street} {p.Addr.Building}");
            if (p.Hobbies != null)
                Console.WriteLine("Hobbies: " + string.Join(", ", p.Hobbies));
        }
    }
}