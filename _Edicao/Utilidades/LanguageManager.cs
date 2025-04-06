using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

class LanguageManager
{
    private Dictionary<int, string> langStrings = new();
    private string currentLanguage = "ptbr";
    private string languagesFolder;

    public LanguageManager(string programFolder)
    {
        languagesFolder = Path.Combine(programFolder, "Languages");
        LoadLanguage(currentLanguage);
    }

    public void LoadLanguage(string languageCode)
    {
        string languageFile = Path.Combine(languagesFolder, $"{languageCode}.language");

        if (!File.Exists(languageFile))
        {
            Console.WriteLine($"Arquivo de linguagem {languageFile} não encontrado.");
            return;
        }

        langStrings.Clear();

        try
        {
            XDocument doc = XDocument.Load(languageFile);

            foreach (var element in doc.Descendants("language"))
            {
                if (int.TryParse(element.Attribute("id")?.Value, out int key))
                {
                    string text = element.Attribute("text")?.Value ?? "";
                    langStrings[key] = text;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar o arquivo de linguagem: {ex.Message}");
        }

        currentLanguage = languageCode;
    }

    public string GetLang(int index)
    {
        return langStrings.TryGetValue(index, out string value) ? value : $"[Texto {index} não encontrado]";
    }

    public List<string> GetAvailableLanguages()
    {
        return Directory.Exists(languagesFolder)
            ? Directory.GetFiles(languagesFolder, "*.language")
                      .Select(Path.GetFileNameWithoutExtension)
                      .ToList()
            : new List<string>();
    }

    public string this[int index] => GetLang(index); // Permite acesso como langManager[index]
}
