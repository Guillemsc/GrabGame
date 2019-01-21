using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
    

public class LocalizationManager : Singleton<LocalizationManager>
{
    public enum LocalizationLanguage
    {
        ENGLISH,
        SPANISH,
    }

    LocalizationManager()
    {
        InitInstance(this);
    }

    private void Awake()
    {
        LoadLocalizationFiles();

        SetCurrLanguage(starting_language);
    }

    public void AddLocalizedText(LocalizationText text)
    {
        texts.Add(text);
    }

    private void LoadLocalizationFiles()
    {
        if(localization_files_names != null)
        {
            for (int i = 0; i < localization_files_names.Length; ++i)
            {
                string curr_localization_file_name = localization_files_names[i];

                string full_path = Application.dataPath + "/" + localization_files_asset_path + "/" + curr_localization_file_name;

                ExcelData data = ExcelLoader.Load(full_path); 
                
                if(data != null)
                {
                    int sheets_count = data.GetSheetsCount();

                    for(int sh = 0; sh < sheets_count; ++sh)
                    {
                        ExcelSheet sheet = data.GetSheetAt(sh);

                        LocalizationData loc_data = new LocalizationData(sheet.GetName());

                        int rows_count = sheet.GetRows();
                        int columns_count = sheet.GetColumns();

                        List<string> languages = new List<string>();

                        for(int r = 0; r < rows_count; ++r)
                        {
                            string curr_key = "";

                            for(int c = 0; c < columns_count; ++c)
                            {
                                if(r == 0)
                                {
                                    if(c > 0)
                                    {
                                        languages.Add(sheet.GetDataAt(r, c));
                                    }
                                }
                                else
                                {
                                    if(c == 0)
                                    {
                                        curr_key = sheet.GetDataAt(r, c);
                                    }
                                    else
                                    {
                                        if(languages.Count > c-1)
                                        {
                                            string curr_data = sheet.GetDataAt(r, c);
                                            string curr_language = languages[c - 1];

                                            if (!string.IsNullOrEmpty(curr_data) && !string.IsNullOrEmpty(curr_language))
                                                loc_data.AddKeyData(curr_language, curr_key, curr_data);
                                        }
                                    }
                                }
                            }
                        }

                        all_data[sheet.GetName()] = loc_data;
                    }
                }
            }
        }
    }

    public void SetCurrLanguage(LocalizationLanguage language)
    {
        curr_language = language;
    }

    public string GetLocalizedText(string group, string key)
    {
        string ret = "";

        if (all_data.ContainsKey(group))
        {
            LocalizationData loc_data = all_data[group];

            ret = loc_data.GetKeyData(curr_language.ToString(), key);
        }
        else
            ret = "GROUP_NOT_FOUND";

        return ret;
    }

    public void UpdateLocalizedTexts()
    {
        for(int i = 0; i < texts.Count; ++i)
        {
            texts[i].UpdateText();
        }
    }

    [SerializeField]
    private LocalizationLanguage starting_language = new LocalizationLanguage();

    [SerializeField]
    private string localization_files_asset_path = "";

    [SerializeField]
    private string[] localization_files_names = null;

    private Dictionary<string, LocalizationData> all_data = new Dictionary<string, LocalizationData>();
    private LocalizationLanguage curr_language = new LocalizationLanguage();

    private List<LocalizationText> texts = new List<LocalizationText>();
}
