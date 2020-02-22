using System;
using UnityEngine;

class FieldHero : MonoBehaviour
{
    public PlayerHero player;

    public float mod1 = -7.35f;
    public float mod2 = -2.45f;
    public float mod3 = 1.85f;
    public float mod4 = 1.5f;
    
    public void SetPosition(int x, int y)
    {
        player.hero.location = new Vector2Int(x, y);
        transform.position = new Vector3(player.hero.location.x * mod3 + mod1, player.hero.location.y * mod4 + mod2, 5);
        GetComponent<SpriteRenderer>().sortingOrder = (99 - Convert.ToInt32(player.hero.location.y));
    }
}
