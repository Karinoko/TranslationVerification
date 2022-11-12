using System.Collections.Generic;

namespace TranslationVerification
{
    internal sealed class GlobalData
    {
        /// <summary>
        /// Ścieżka do źródła
        /// </summary>
        static public string sourcePath = @"D:\Steam\steamapps\common\Software Inc\Localization\English";
        /// <summary>
        /// Ścieżka do tłumaczenia
        /// </summary>
        static public string translPath = @"D:\Steam\steamapps\workshop\content\362620\2778995379";

        static public readonly List<string> hints = new()
        {
            "Upewnij się, że ścieżki są prawidłowe.",
            "Program nie pozwoli porównać ze sobą różnych plików.",
            "Możesz otworzyć pliki w obu listach klikając 2-krotnie.",
            "Program operuje głównie na plikach *.tyd należących do gry Software.Inc, inne pliki mogą zostać otwarte i porównane, ale nie ma gwarancji, że prawidłowo zostaną rozpoznane ich klucze.",
            "Klikając podwójnie w \"Wskazówka\" pokaże się losowa wskazówka.",
            "Program dalej się rozwija, więc jeśli coś nie działa zgłoś, zostanie naprawione."
        };
    }
}
