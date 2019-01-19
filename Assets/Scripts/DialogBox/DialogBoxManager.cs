using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBoxManager : Singleton<DialogBoxManager>
{
    DialogBoxManager()
    {
        InitInstance(this);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCurrentDialog();
    }

    public void AddDialogText(TextMeshProUGUI text_to_use, string text, DelDialogFrame on_finish_callback = null)
    {
        if(text != "" && text_to_use != null)
        {
            text_to_use.text = "";

            DialogFrame df = new DialogFrame(text_to_use, text);

            if (on_finish_callback != null)
                df.SuscribeToOnFrameFinish(on_finish_callback);

            dialog_frames_to_add.Add(df);
        }
    }

    public void PushCurrentDialog()
    {       
       for (int i = 0; i < dialog_frames_to_show.Count; ++i)
           dialog_frames_to_show[i].ClearText();

       dialog_frames_to_show.Clear();
       dialog_frames_to_show.AddRange(dialog_frames_to_add);
       dialog_frames_to_add.Clear();

       printing_dialog = true;
       waiting_for_next_box = false;

       curr_frame_printing = 0;
       curr_line_printing = 0;
       curr_word_printing = 0;
    }

    public void PushNextBox()
    {
        if (waiting_for_next_box)
        {
            waiting_for_next_box = false;

            if (curr_frame_printing < dialog_frames_to_show.Count)
            {
                DialogFrame curr_frame = dialog_frames_to_show[curr_frame_printing];

                curr_frame.ClearText(); ;
            }
            else
            {
                if (dialog_frames_to_show.Count > 0)
                {
                    DialogFrame curr_frame = dialog_frames_to_show[dialog_frames_to_show.Count - 1];

                    curr_frame.ClearText();

                    printing_dialog = false;
                }
            }
        }
    }

    private void UpdateCurrentDialog()
    {
        if(printing_dialog)
        {
            if(curr_frame_printing < dialog_frames_to_show.Count)
            {
                DialogFrame curr_frame = dialog_frames_to_show[curr_frame_printing];

                if (!waiting_for_next_box)
                {
                    if (curr_frame.GetLines() > curr_line_printing)
                    {
                        string curr_line_text = curr_frame.GetTextAtLine(curr_line_printing);

                        if (curr_line_text.Length > curr_word_printing)
                        {
                            if (printing_leter_timer.ReadTime() > printing_leter_time || curr_word_printing == 0)
                            {
                                printing_leter_timer.Start();

                                curr_frame.GetTextToUse().text += curr_line_text[curr_word_printing];

                                ++curr_word_printing;
                            }
                        }
                        else
                        {
                            curr_word_printing = 0;
                            ++curr_line_printing;

                            curr_frame.GetTextToUse().text += "\n";

                            if (curr_line_printing > max_lines_per_box - 1)
                                waiting_for_next_box = true;
                        }
                    }
                    else
                    {
                        curr_line_printing = 0;
                        curr_word_printing = 0;

                        curr_frame.CallOnFrameFinish();

                        ++curr_frame_printing;
                    }
                }
            }
            else
            {
                waiting_for_next_box = true;
            }
        }
    }

    [SerializeField]
    TextMeshProUGUI tmp_text = null;

    [SerializeField]
    private int max_lines_per_box = 0;

    private List<DialogFrame> dialog_frames_to_add = new List<DialogFrame>();
    private List<DialogFrame> dialog_frames_to_show = new List<DialogFrame>();
    private int curr_frame_printing = 0;
    private int curr_line_printing = 0;
    private int curr_word_printing = 0;

    private bool printing_dialog = false;
    private bool waiting_for_next_box = false;

    private Timer printing_leter_timer = new Timer();
    private float printing_leter_time = 0.05f; 
}
