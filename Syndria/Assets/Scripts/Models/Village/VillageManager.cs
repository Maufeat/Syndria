using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Models.Village
{
    public class VillageManager : MonoBehaviour
    {
        public static VillageManager Instance { get; private set; }

        // Only on X-axis, Y is not a valid cell and only will be painted;
        public VillageTile[] cells;

        public bool blockGroundDrag = false;

        public GameObject villageObject;
        public Tilemap ground;
        public TilemapCollider2D groundCollider;
        public TileBase upperGroundTile;
        public TileBase lowerGroundTile;

        public float x_offset_multi = -6.45f;
        public float y_offset_multi = 2;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else { Destroy(gameObject); }
        }

        public void Init(int width)
        {
            cells = new VillageTile[99];

            villageObject = Instantiate(Resources.Load("Prefabs/Village")) as GameObject;
            groundCollider = villageObject.GetComponentInChildren<TilemapCollider2D>();
            ground = villageObject.GetComponentInChildren<Tilemap>();

            for (int x = 0; x < (width + 1); x++)
            {
                for (int y = 0; y > -5; y--)
                {
                    if (y == 0)
                    {
                        VillageTile newTile = new VillageTile(x, y);
                        cells[x] = newTile;
                        SetTile(ground, newTile.coordinate, upperGroundTile);
                    }
                    else
                    {
                        SetTile(ground, new Vector2Int(x, y), lowerGroundTile);
                    }
                }
            }

            // Fake Building
            GameObject fakeBuilding= new GameObject("fakeBuilding");
            //Add Components
            Building buildingScript = fakeBuilding.AddComponent<Building>();
            buildingScript.buildingData = Resources.Load<BuildingData>("Buildings/1");

            // Fake Building
            GameObject fakeBuilding2 = new GameObject("fakeBuilding");
            //Add Components
            Building buildingScript2 = fakeBuilding2.AddComponent<Building>();
            buildingScript2.buildingData = Resources.Load<BuildingData>("Buildings/2");

            // Fake Building
            GameObject fakeBuilding3 = new GameObject("fakeBuilding");
            //Add Components
            Building buildingScript3 = fakeBuilding3.AddComponent<Building>();
            buildingScript3.buildingData = Resources.Load<BuildingData>("Buildings/3");

            // Fake Building
            GameObject fakeBuilding4 = new GameObject("fakeBuilding");
            //Add Components
            Building buildingScript4 = fakeBuilding4.AddComponent<Building>();
            buildingScript4.buildingData = Resources.Load<BuildingData>("Buildings/1");

            cells[0].objectOnTile = buildingScript;
            buildingScript.RenderBuilding(cells[0].coordinate);

            cells[2].objectOnTile = buildingScript2;
            buildingScript2.RenderBuilding(cells[2].coordinate);

            cells[4].objectOnTile = buildingScript3;
            buildingScript3.RenderBuilding(cells[4].coordinate);

            cells[6].objectOnTile = buildingScript4;
            buildingScript4.RenderBuilding(cells[6].coordinate);
        }

        public void SetTile(Tilemap layer, Vector2Int pos, TileBase tile)
        {
            Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
            layer.SetTileFlags(location, TileFlags.None);
            layer.SetTile(location, tile);
        }
    }
}
