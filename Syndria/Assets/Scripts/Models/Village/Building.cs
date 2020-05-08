using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.Village
{
    public class Building : MonoBehaviour
    {
        public BuildingData buildingData;

        public Vector2Int pos;

        public SpriteRenderer buildingRenderer;

        /*
            if(spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            hero.location = new Vector2(x, y);
            transform.position = new Vector3(hero.location.x * x_offset_multi + x_offset_fixed, hero.location.y * y_offset_multi + y_offset_fixed, 5);
            spriteRenderer.sortingOrder = (99 - Convert.ToInt32(hero.location.y));
        */

        public void RenderBuilding(Vector2Int cellPos)
        {
            pos = cellPos;

            if (GetComponent<SpriteRenderer>() == null)
                buildingRenderer = gameObject.AddComponent<SpriteRenderer>();
            else
                buildingRenderer = GetComponent<SpriteRenderer>();

            buildingRenderer.sprite = buildingData.Sprite;
            transform.localScale = new Vector3(2, 2, 1);
        }

        public void Update()
        {
            var spriteHeight = buildingRenderer.bounds.size.x / 2 + pos.x;
            var spriteWidth = buildingRenderer.bounds.size.y / 2 + pos.y;
            transform.position = new Vector3(spriteHeight * VillageManager.Instance.x_offset_multi + buildingData.offset_x, spriteWidth * VillageManager.Instance.y_offset_multi + buildingData.offset_y, 5);
        }
    }
}
