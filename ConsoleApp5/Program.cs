﻿//Możesz napisać program gdzie dla 30 studentów losuje 9 razy randomowo tak i nie i 9 los decyduje o zaliczeniu czy zdane czy nie

using System.Collections.Concurrent;
using System.Security.Cryptography;

class Program
{
    private string[] label = new[] {"Eleonora Zadrożna",
        "Waldemar Kubecki", "Dominik Pierzchała", "Kuba Wilmański","Julita Mężyk","Bożena Sokolińska","Grażyna Kempińska",
        "Fryderyk Yavorskyi", "Justyna Kowalkowska", "Urszula Tomaszek", "Zuzanna Walter", "Amelia Reczek", "Czesław Marczyk",
        "Mateusz Wolańczyk", "Jan Niesłuchowski", "Kacper Damięcki", "Tymoteusz Kulczycki", "Daniel Beczek", "Łucja Rachwał",
        "Kuba Witulski", "Mirosław Dryja", "Leonard Radomski", "Dariusz Łowicki", "Cyprian Siatka", "Sylwester Pisula", 
        "Klaudiusz Kyrychenko", "Apolonia Piaseczna", "Eugeniusz Przybycień", "Artur Borkowski", "Julianna Wachowska", };
    
    private ThreadLocal<Random> rnd = new (() => new Random(Guid.NewGuid().GetHashCode()));
    private ConcurrentDictionary<string, (double sum, string result)> promotion = new ();

    async Task Run()
    {
        await Task.Run(() => GetPromotion());
        PrintData();
        void GetPromotion()
        {
            Parallel.For(0, label.Length, i =>
            {
            double[] numbers = new double[10];
                for (int j = 0; j < numbers.Length; j++)
                {
                 numbers[j] = GetSecureRandomDouble();   
                }
                double sum = numbers.Sum();
                string result = sum >= 51 ? "Zdał" : "Nie zdał";
                promotion.TryAdd(label[i], (sum, result));
            });
        }
        
        void PrintData()
        {
                while (promotion.Count < label.Length)
                {
                    Thread.Sleep(10);
                }

                foreach (var entry in promotion.OrderBy(n=>n.Key))
                {
                    Console.WriteLine($"{entry.Key} Suma punktów: {entry.Value.sum:F2} => {entry.Value.result}");
                    double grade = entry.Value.sum switch
                    {
                        >= 90 and <= 100 => 5,
                            >= 80 and < 90 => 4.5,
                            >= 70 and < 80 => 4,
                            >= 60 and < 70 => 3.5,
                            >= 51 and < 60 => 3,
                        _ => 2
                    };
                    Console.WriteLine($"Ocena: {grade}");
                    Console.WriteLine(new string('-', 60)); 
                } 
        } 
        static double GetSecureRandomDouble()
        {
            byte[] bytes = new byte[4];
            RandomNumberGenerator.Fill(bytes);
            uint randomUint = BitConverter.ToUInt32(bytes, 0);
            return (randomUint / (double)uint.MaxValue) * 10;
        }

    }

    public static async Task Main(string[] args)
    {
        Program p = new Program();
        await p.Run();
    }

}
