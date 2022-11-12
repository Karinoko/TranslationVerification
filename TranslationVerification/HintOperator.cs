using System;

namespace TranslationVerification
{
    internal sealed class HintOperator
    {
        /// <summary>
        /// Losuje wskazówkę z listy wskazówek
        /// </summary>
        /// <returns>String wskazówki</returns>
        static public string RandomizeHint() => GlobalData.hints[new Random().Next(GlobalData.hints.Count)];
    }
}
