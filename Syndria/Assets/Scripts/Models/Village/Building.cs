using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Models.Village
{
    public class Building : MonoBehaviour
    {
        public BuildingData buildingData;

        public Vector2Int pos;

        public SpriteRenderer buildingRenderer;
        public BoxCollider2D buildingCollider;

        public TMPro.TextMeshPro buildingName;

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

            if (GetComponent<BoxCollider2D>() == null)
                buildingCollider = gameObject.AddComponent<BoxCollider2D>();
            else
                buildingCollider = GetComponent<BoxCollider2D>();

            buildingRenderer.sprite = buildingData.Sprite;
            transform.localScale = new Vector3(2, 2, 1);

            buildingName = (Instantiate(Resources.Load("Prefabs/UI/Misc/3DVillageText")) as GameObject).GetComponent<TMPro.TextMeshPro>();
            buildingName.transform.SetParent(transform);
        }

        public void Update()
        {
            var spriteHeight = buildingRenderer.bounds.size.x / 2 + pos.x;
            var spriteWidth = buildingRenderer.bounds.size.y / 2 + pos.y;
            if (GetComponent<BoxCollider2D>() != null)
                buildingCollider.size = buildingRenderer.bounds.size / 2;
            buildingName.text = buildingData.Name;
            buildingName.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            buildingName.transform.position = new Vector3(this.transform.position.x, buildingData.text_offset_y,6);
            transform.position = new Vector3(spriteHeight * VillageManager.Instance.x_offset_multi + buildingData.offset_x, spriteWidth * VillageManager.Instance.y_offset_multi + buildingData.offset_y, 5);
        }

        private void OnMouseUp()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return;
            }

            var b = UIManager.Instance.OpenPanel("VillagePopup");
            b.GetComponent<VillagePopUp>().data = buildingData;

            switch (buildingData.ID)
            {
                case 1:
                    break;
                default:
                    break;
            }
        }
    }
}
