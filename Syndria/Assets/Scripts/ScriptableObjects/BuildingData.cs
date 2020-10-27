using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Building/Add Building")]
public class BuildingData: ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public float offset_x;
    public float offset_y;
    public float text_offset_y;
}
