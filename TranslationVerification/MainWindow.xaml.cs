using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;
using System.Diagnostics;

namespace TranslationVerification
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Lista plików w źródle
        /// </summary>
        private string[]? sourceFiles;
        /// <summary>
        /// Lista plików w tłumaczeniu
        /// </summary>
        private string[]? translFiles;

        /// <summary>
        /// Lista kluczy źródłowego pliku
        /// </summary>
        private List<string>? sourceKeys = new();

        /// <summary>
        /// Lista kluczy tłumaczonego pliku
        /// </summary>
        private List<string?> translKeys = new();

        private List<string?> equalsKeys = new();

        private List<string>? keys = new();

        
        public MainWindow()
        {
            InitializeComponent();
            RegistryOperator.SetRegistryKeyOnStart();
            Hint.Text = HintOperator.RandomizeHint();
            RegistryOperator.CheckPath();
            TXB_SourcePath.Text = GlobalData.sourcePath;
            TXB_TranslatePath.Text = GlobalData.translPath;
        }

        /// <summary>
        /// Reakcja na naciśnięcie przycisku 'zatwierdź' przy źródle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_ApplySource_Click(object sender, RoutedEventArgs e)
        {
            string sp = TXB_SourcePath.Text;
            if (CheckBothPath(sp)) PrepareFiles(sp);
        }

        /// <summary>
        /// Reakcja na naciśnięcie przycisku 'zatwierdź' przy tłumaczeniu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_ApplyTranslate_Click(object sender, RoutedEventArgs e)
        {
            string st = TXB_TranslatePath.Text;
            if (CheckBothPath(st)) PrepareFiles(st, false);
        }

        /// <summary>
        /// Sprawdza poprawność ścieżki do pliku
        /// </summary>
        /// <param name="path">Ścieżka do pliku</param>
        /// <returns>TRUE jeśli jest prawidłowa</returns>
        private bool CheckBothPath(string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show($"Wprowadzona ścieżka: \"{path}\" jest nieprawidłowa.", $"Nieprawidłowa ścieżka.", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Przygotowanie listy plików
        /// </summary>
        /// <param name="path">Ścieżka</param>
        /// <param name="isSource">TRUE jeśli źródło</param>
        private void PrepareFiles(string path, bool isSource = true)
        {
            if (isSource)
            {
                sourceFiles = Directory.GetFiles(path);
                SourceFilesList.Items.Clear();
                foreach(string file in sourceFiles)
                {
                    SourceFilesList.Items.Add(Path.GetFileName(file));
                }
                GlobalData.sourcePath = path;
            } 
            else
            {
                translFiles = Directory.GetFiles(path);
                TranslFilesList.Items.Clear();
                foreach (string file in translFiles)
                {
                    TranslFilesList.Items.Add(Path.GetFileName(file));
                }
                GlobalData.translPath = path;
            }
        }

        private void SourceFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? item;
            try
            {
                item = SourceFilesList.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                item = null;
            }

            if (item != null && GlobalData.sourcePath != null)
            {
                KeysManager kM = new(new(GlobalData.sourcePath + "\\" + item));
                SourceFileName.Content = kM.GetFileName();
                sourceKeys = kM.GetKeys();
                SourceFileLines.Content = kM.GetAmoutOfLines();
                SourceFileKeys.Content = kM.GetAmountOfKeys();
                SourceFileWage.Content = kM.GetFileSize();
            }
        }

        private void TranslFilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? item;
            try
            {
                item = TranslFilesList.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                item = null;
            }
            
            if (item != null && GlobalData.translPath != null)
            {
                KeysManager kM = new(new(GlobalData.translPath + "\\" + item));
                TranslFileName.Content = kM.GetFileName();
                translKeys = kM.GetKeys();
                TranslFileLines.Content = kM.GetAmoutOfLines();
                TranslFileKeys.Content = kM.GetAmountOfKeys();
                TranslFileWage.Content = kM.GetFileSize();
            }
        }

        /// <summary>
        /// Wypełnienie tabelki z porównywaniem kluczy obu plików
        /// </summary>
        /// <param name="sourceArray">Źródłowa tablica</param>
        /// <param name="translArray">Tłumaczona tablica</param>
        private void EqualKeys()
        {
            if (CheckSourceTransl.Items.Count != 0) CheckSourceTransl.Items.Clear();
            if (SourceMoreKeys.Items.Count != 0) SourceMoreKeys.Items.Clear();
            if (TranslMoreKeys.Items.Count != 0) TranslMoreKeys.Items.Clear();

            List<string> _sourceKeys = sourceKeys;
            List<string> _translKeys = translKeys;

            foreach (var skey in _sourceKeys)
            {
                if (!_translKeys.Contains(skey))
                    SourceMoreKeys.Items.Add($"{_sourceKeys.IndexOf(skey) + 1} {skey}");
            }

            foreach (var tkey in _translKeys)
            {
                if (!_sourceKeys.Contains(tkey))
                    TranslMoreKeys.Items.Add($"{_translKeys.IndexOf(tkey) + 1} {tkey}");
            }

            int countList = (_sourceKeys.Count > _translKeys.Count) ? _sourceKeys.Count : _translKeys.Count;
            for (int i = 0; i < countList; i++)
            {
                string a = (i < _sourceKeys.Count) ? $"{_sourceKeys[i]}" : $"SOURCE OOL";
                string b = (i < _translKeys.Count) ? $"{_translKeys[i]}" : $"TRANSL OOL";
                string c;

                if (a != b)
                    c = $"!! {i + 1}: {a} *** {b}";
                else
                    c = $"{i + 1}: {a} *** {b}";

                CheckSourceTransl.Items.Add(c);
                equalsKeys.Add(c);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SourceFileName.Content.ToString() != null && SourceFileName.Content.ToString() != "")
            {
                if ((SourceFileName.Content.ToString() == TranslFileName.Content.ToString()))
                    EqualKeys();
                else
                    MessageBox.Show($"Próbujesz porównać do siebie 2 różne pliki:\n{SourceFileName.Content} >> {TranslFileName.Content}", "Wybrano różne pliki", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show($"Nie wybrano pliku lub plików", "Brak plików do porównania", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TXB_SearchKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(equalsKeys.Count > 0 || equalsKeys != null)
            {
                CheckSourceTransl.Items.Clear();
                if (TXB_SearchKey.Text == "")
                {
                    foreach (var key in equalsKeys)
                    {
                        CheckSourceTransl.Items.Add(key);
                    }
                }
                var skeys = equalsKeys.FindAll(x => x.Contains(TXB_SearchKey.Text));
                foreach (var item in skeys)
                {
                    CheckSourceTransl.Items.Add(item);
                }
            }
        }

        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = (sender as ListBox).Tag.ToString().Contains("S") ? Path.Combine(GlobalData.sourcePath, SourceFilesList.SelectedItem.ToString()) : Path.Combine(GlobalData.translPath, TranslFilesList.SelectedItem.ToString())
            };
            process.Start();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) => Hint.Text = HintOperator.RandomizeHint();
    }
}