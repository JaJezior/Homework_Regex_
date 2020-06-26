using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Homework_RegEx
{
    public class Program
    {
        static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            string allText=null;
            while (allText == null)
            {
                string filePath = GetFilePath();
                try
                {
                    allText = LoadFile(filePath);
                }
                catch (FileNotFoundException fnf)
                {
                    Console.WriteLine($"Plik {filePath} nie istnieje.", fnf);
                }
                catch (ArgumentException arg)
                {
                    Console.WriteLine("Ścieżka nie może być pusta.", arg);
                }
            }

            MatchCollection matchCollection = MatchWithDomainRegex(allText);

            Dictionary<string, int> domainDictionary = GetDictionaryFromMatches(matchCollection);

            SaveResultsToFile(domainDictionary);
        }

        public void SaveResultsToFile(Dictionary<string, int> domainDictionary)
        {
            var textToFile = new StringBuilder();

            foreach (var domain in domainDictionary)
            {
                textToFile.Append($"'{domain.Key}' : {domain.Value}");
                textToFile.AppendLine();
            }

            Console.WriteLine($"{textToFile}");

            Console.WriteLine("Podaj ścieżkę docelową pliku z wynikami:");
            var saveFilePath = Console.ReadLine();
            File.WriteAllText(saveFilePath, textToFile.ToString());
        }

        public Dictionary<string, int> GetDictionaryFromMatches(MatchCollection matchCollection)
        {
            var domainDictionary = new Dictionary<string, int>();

            foreach (Match match in matchCollection)
            {
                if (domainDictionary.ContainsKey(match.Value))
                {
                    domainDictionary[match.Value]++;
                }
                else
                {
                    domainDictionary.Add(match.Value, 1);
                }
            }

            return domainDictionary;
        }

        public MatchCollection MatchWithDomainRegex(string allText)
        {
            return Regex.Matches(allText, @"(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]");
        }

        public string LoadFile(string filePath)
        {
            string allText = File.ReadAllText(filePath);

            return allText;
        }

        public string GetFilePath()
        {
            Console.WriteLine("Wprowadź ścieżkę pliku:");
            string filePath = Console.ReadLine();
            return filePath;
        }
    }
}
