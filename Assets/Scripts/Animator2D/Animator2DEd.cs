using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 

using UnityEditor;

[CustomEditor(typeof(Animator2D))]
public class Animator2DEd : Editor
{
    public override void OnInspectorGUI()
    {
        Animator2D myScript = (Animator2D)target;

        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Add Animation"))
            {
                myScript.AddAnimation();
            }
        }

        List<Animation2D> animations = myScript.GetAnimations();

        for(int i = 0; i < animations.Count; ++i)
        {
            Animation2D curr_animation = animations[i];

            string curr_name = curr_animation.GetName();

            string text = curr_name + ": [" + i + "]";

            curr_animation.editor_foolded = EditorGUILayout.Foldout(curr_animation.editor_foolded, text);

            if (curr_animation.editor_foolded)
            {
                if (!Application.isPlaying)
                {
                    string new_name = GUILayout.TextField(curr_name);

                    curr_animation.SetName(new_name);
                }

                List<Sprite> curr_sprites = curr_animation.GetSprites();

                if (!Application.isPlaying)
                {
                    int new_sprites_count = EditorGUILayout.IntField("Frames:", curr_sprites.Count);

                    if (new_sprites_count != curr_sprites.Count)
                        curr_animation.SetSprites(new_sprites_count);
                }
                else
                    GUILayout.Label("Frames: " + curr_sprites.Count);

                if(!Application.isPlaying)
                {
                    Vector2 new_offset = EditorGUILayout.Vector2Field("Offset", curr_animation.GetOffset());

                    curr_animation.SetOffset(new_offset);
                }

                curr_sprites = curr_animation.GetSprites();

                for (int y = 0; y < curr_sprites.Count; ++y)
                {
                    Sprite curr_sprite = curr_sprites[y];

                    string sprite_name = "Sprite [" + y + "]";

                    if (curr_sprite != null)
                        sprite_name = curr_sprite.name;

                    if (!Application.isPlaying)
                    {
                        Sprite new_sprite = (Sprite)EditorGUILayout.ObjectField(sprite_name, curr_sprite, typeof(Sprite), false);

                        curr_animation.AddSprite(y, new_sprite);
                    }
                    else
                        GUILayout.Label(sprite_name);
                }
            }
        }

        DrawDefaultInspector();
    }
}

#endif
