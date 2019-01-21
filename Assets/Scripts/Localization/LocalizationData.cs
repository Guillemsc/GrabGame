using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationData
{
    public LocalizationData(string data_name)
    {
        this.data_name = data_name;
    }

    public void AddKeyData(string language, string key, string data)
    {
        int data_index = GetDataIndexFromLanguage(language);

        Dictionary<string, string> curr_data = null;

        if (data_index >= 0)
        {
            curr_data = keyed_data[data_index];
        }
        else
        {
            int new_index = keyed_data.Count;

            language_to_data_index[language] = new_index;

            curr_data = new Dictionary<string, string>();

            keyed_data.Add(curr_data);
        }

        if(curr_data != null)
        {
            curr_data[key] = data;
        }
    }

    public string GetKeyData(string language, string key)
    {
        string ret = "";

        int data_index = GetDataIndexFromLanguage(language);

        if(data_index >= 0)
        {
            Dictionary<string, string> curr_data = keyed_data[data_index];

            if (curr_data.ContainsKey(key))
            {
                ret = curr_data[key];
            }
            else
                ret = "KEY_NOT_FOUND";
        }
        else
        {
            ret = "LANGUAGE_NOT_FOUND";
        }

        return ret;
    }

    private int GetDataIndexFromLanguage(string language)
    {
        int ret = -1;

        for (int i = 0; i < language_to_data_index.Count; ++i)
        {
            if(language_to_data_index.ContainsKey(language))
            {
                ret = language_to_data_index[language];
            }
        }

        return ret;
    }

    private string data_name = "";

    private Dictionary<string, int> language_to_data_index = new Dictionary<string, int>();
    private List<Dictionary<string, string>> keyed_data = new List<Dictionary<string, string>>();
}
