using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationText : MonoBehaviour
{
    private void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if(text != null)
        {
            text.text = LocalizationManager.Instance.GetLocalizedText(group, key);
        }
    }

    [SerializeField]
    private string group = "";

    [SerializeField]
    private string key = "";

    private TextMeshProUGUI text = null;
}
