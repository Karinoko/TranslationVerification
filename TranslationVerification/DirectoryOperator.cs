using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TranslationVerification
{
    internal sealed class DirectoryOperator
    {
        static public void FindSourceDirectory()
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                DriveInfo di = new(drive);

                if (!di.IsReady)
                    continue;

                List<string> files = Directory.EnumerateFiles(di.Name, "*.tyd", new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true, ReturnSpecialDirectories = false }).ToList();

                GlobalData.sourcePath = Path.GetDirectoryName(new FileInfo(files.First(x => x.Contains(@"Software Inc\Localization\English"))).FullName);
                GlobalData.translPath = Path.GetDirectoryName(new FileInfo(files.First(x => x.Contains(@"362620\2778995379"))).FullName);
            }
        }
    }
}
