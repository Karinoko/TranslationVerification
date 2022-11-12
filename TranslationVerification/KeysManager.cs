using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TranslationVerification
{
    internal class KeysManager
    {
        /// <summary>
        /// Plik źródłowy
        /// </summary>
        private FileInfo sourceFile;
        /// <summary>
        /// Linie z danego pliku
        /// </summary>
        private List<string> fileLines = new();
        /// <summary>
        /// Klucze z danego pliku
        /// </summary>
        private List<string> fileKeys = new();


        private string pattern = @"(^([\w\d-]+|\s*[\w\d-]+)\s+(\""|\[))|(Software\b\n|Feature\b)|(^(\s+Item|Item)\s?\{\s?Name)";
        private int keysAmount = 0;

        public KeysManager(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile;
            fileLines = GetLinesFromFile();
            GetKeysFromLines();
        }

        /// <summary>
        /// Pobiera linie z pliku
        /// </summary>
        /// <returns>Lista linii</returns>
        private List<string> GetLinesFromFile() => File.ReadAllLines(sourceFile.FullName).ToList();
        
        /// <summary>
        /// Pobiera klucze z linii danego pliku
        /// </summary>
        private void GetKeysFromLines()
        {
            foreach (string line in fileLines)
            {
                Match match = Regex.Match(line, pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    fileKeys.Add(match.Value.Replace('"', ' ').Replace('{', ' ').Replace('[', ' ').Trim());
                    keysAmount++;
                }
                else
                    fileKeys.Add("NK | C | NL");
            }
        }

        private List<U> FindDuplicates<T, U>(List<T> list, Func<T, U> keySelector)
        {
            return list.GroupBy(keySelector)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();
        }

        public List<string> GetKeysDuplicates() => FindDuplicates(fileKeys, x => x);

        /// <summary>
        /// Przekazuje linie
        /// </summary>
        /// <returns>Lista linii (string)</returns>
        public List<string> GetLines() => fileLines;

        /// <summary>
        /// Przekazuje klucze
        /// </summary>
        /// <returns>Lista kluczy (string)</returns>
        public List<string> GetKeys() => fileKeys;

        /// <summary>
        /// Zwraca ilość kluczy
        /// </summary>
        /// <returns>INT ilość kluczy</returns>
        public int GetAmountOfKeys() => keysAmount;

        /// <summary>
        /// Zwraca ilość linii
        /// </summary>
        /// <returns>INT ilość linii</returns>
        public int GetAmoutOfLines() => fileLines.Count;

        /// <summary>
        /// Zrwaca nazwę samego pliku
        /// </summary>
        /// <returns>Nazwa pliku</returns>
        public string GetFileName() => sourceFile.Name;

        /// <summary>
        /// Zwraca rozmiar pliku
        /// </summary>
        /// <returns>FLOAT rozmiar pliku</returns>
        public float GetFileSize() => sourceFile.Length / 1024 + 1;
    }
}
