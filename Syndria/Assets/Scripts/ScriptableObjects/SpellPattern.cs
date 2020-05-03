using UnityEngine;

[System.Serializable]
public class SpellPattern : ScriptableObject
{
    public int id;
    public int _width = 1;
    public int _height = 1;

    [SerializeField]
    int[] _pattern;

    public void SetData(int x, int y, int val)
    {
        if ((x >= 0) && (y >= 0) && (x < _width) && (y < _height))
            _pattern[x + y * _width] = val;
    }

    public int GetData(int x, int y)
    {
        if ((x >= 0) && (y >= 0) && (x < _width) && (y < _height))
            return _pattern[x + y * _width];

        return 0;
    }

    public bool IsCenter(int x, int y)
    {
        return x == _width / 2 && y == _height / 2;
    }

    public void Resize(int width, int height)
    {
        width = Mathf.Max(width, 1);
        height = Mathf.Max(height, 1);

        int[] newData = new int[width * height];
        //for (int x = 0; (x < width) && (x < _width); x++)
           // for (int y = 0; (y < height) && (y < _height); x++)
                //newData[x + y * width] = _pattern[x + y * _width];

        _width = width;
        _height = height;
        _pattern = newData;
    }
}
