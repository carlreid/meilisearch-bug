using Meilisearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunMe
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        List<string>? Tags { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new MeilisearchClient("http://localhost:7700");

            for (var timesRan = 0; timesRan < 100; timesRan++)
            {
                Console.WriteLine($"Attempt: {timesRan}");
                Console.WriteLine("Checking indexes...");
                var indexes = await client.GetAllIndexes();
                var indexId = "books";
                if (indexes.Select(x => x.Uid).Contains(indexId))
                {
                    Console.WriteLine($"Found {indexId} index, deleting it...");
                    await client.DeleteIndex(indexId);
                    await Task.Delay(1000);
                }

                Console.WriteLine($"Creating new {indexId} index");
                var newIndex = await client.CreateIndex(indexId);

                for (int batch = 0; batch < 10; batch++)
                {
                    var index = await client.GetIndex(indexId);
                    for (int i = 0; i < 1000; i++)
                    {
                        var documents = Enumerable.Range(0, 200)
                           .Select(j => new Book { Id = $"{i}_{j}", Title = $"Le Book {i}_{j}" }).ToArray();
                        var result = await index.AddDocuments<Book>(documents);
                        await Task.Delay(100);
                    }
                }
                Console.WriteLine("Finished writing books");

                var targetIndex = await client.GetIndex(indexId);
                var stats = await targetIndex.GetStats();
                Console.WriteLine("Got stats fine... lets try again.");
            }

        }
    }
}
