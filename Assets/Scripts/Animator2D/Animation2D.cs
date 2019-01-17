using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation2D
{
    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }

    public void SetOffset(Vector2 offset)
    {
        this.offset = offset;
    }

    public Vector2 GetOffset()
    {
        return offset;
    }

    public void SetSprites(int count)
    {
        for(int i = sprites.Count - 1; i < count; ++i)
        {
            sprites.Add(null);
        }

        if(count < sprites.Count)
        {
            sprites.RemoveRange(count, sprites.Count - count);
        }
    }

    public void RemoveAt(int index)
    {
        if (sprites.Count > index)
        {
            sprites.RemoveAt(index);
        }
    }

    public void AddSprite(int index, Sprite sprite)
    {
        if(sprites.Count > index)
        {
            sprites[index] = sprite;
        }
    }

    public List<Sprite> GetSprites()
    {
        return sprites;
    }

    public Sprite GetSprite(int index)
    {
        Sprite ret = null;

        if (sprites.Count > index)
        {
            ret = sprites[index];
        }

        return ret;
    }

    public int GetSpritesCount()
    {
        return sprites.Count;
    }

    [SerializeField] [HideInInspector]
    public bool editor_foolded = false;

    [SerializeField][HideInInspector]
    private string name;

    [SerializeField] [HideInInspector]
    private List<Sprite> sprites = new List<Sprite>();

    [SerializeField] [HideInInspector]
    private Vector2 offset = Vector2.zero;
}
