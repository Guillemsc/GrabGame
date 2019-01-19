using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void DelDialogFrame(DialogFrame ev);

public class DialogFrame
{
    public DialogFrame(TextMeshProUGUI text_to_use, string dialog)
    {
        this.text_to_use = text_to_use;
        this.full_dialog = dialog;

        GenerateTextPerLine();
    }

    private void GenerateTextPerLine()
    {
        lines = 0;
        text_per_line.Clear();

        if (text_to_use != null)
        {
            TMP_TextInfo info = text_to_use.GetTextInfo(full_dialog);

            lines = info.lineCount;

            int curr_dialog_pos = 0;
            for(int i = 0; i < lines; ++i)
            {
                TMP_LineInfo line_info = info.lineInfo[i];

                int total_line_words = line_info.characterCount;

                string curr_line = full_dialog.Substring(curr_dialog_pos, total_line_words);

                curr_dialog_pos += total_line_words;

                text_per_line.Add(curr_line);
            }
        }
    }

    public int GetLines()
    {
        return lines;
    }

    public string GetTextAtLine(int line_index)
    {
        string ret = "";

        if (text_per_line.Count > line_index)
            ret = text_per_line[line_index];

        return ret;
    }

    public TextMeshProUGUI GetTextToUse()
    {
        return text_to_use;
    }

    public void ClearText()
    {
        text_to_use.text = "";
    }

    public void SuscribeToOnFrameFinish(DelDialogFrame callback)
    {
        on_frame_finish += callback;
    }

    public void CallOnFrameFinish()
    {
        if (on_frame_finish != null)
            on_frame_finish(this);
    }

    private TextMeshProUGUI text_to_use = null;
    private string full_dialog = "";
    private int lines = 0;
    private List<string> text_per_line = new List<string>();

    private DelDialogFrame on_frame_finish = null;
}
