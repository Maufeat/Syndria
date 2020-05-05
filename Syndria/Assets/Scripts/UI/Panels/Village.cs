using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Village : UIPanel
{
    public GameObject grid;
    public Tilemap ground;
    public TilemapCollider2D groundCollider;

    public TileBase test1;
    public TileBase test2;

    private float camVertExtent;
    private float halfWidth;

    private Vector2 startPos;
    private Vector2 cameraStartPos;
    private bool slides;
    private float lastDiffSmoothing = 0f;

    void Start()
    {
        keepOnDispose = true;
        //StartCoroutine(NetworkManager.Instance.DownloadImage(NetworkManager.Instance.fbPic, GameObject.Find("ProfilePicture").GetComponent<Image>()));
        //GameObject.Find("NameLbl").GetComponent<TMPro.TextMeshProUGUI>().text = NetworkManager.Instance.me.Username;
        //GameObject.Find("SocialNameLbl").GetComponent<TMPro.TextMeshProUGUI>().text = "aka " + NetworkManager.Instance.fbName;
        //GameObject.Find("LevelLbl").GetComponent<TMPro.TextMeshProUGUI>().text = "Lv. " + NetworkManager.Instance.me.level.ToString();
        
        //var discordBtn = GameObject.Find("DiscordBtn").GetComponent<Button>();
        //discordBtn.GetComponent<Button>().onClick.AddListener(delegate { StaticTestScript.OpenDiscordUrl(); });
        
        var fightBtn = GameObject.Find("Button (2)").GetComponent<Button>();
        fightBtn.GetComponent<Button>().onClick.AddListener(delegate {
            UIManager.instance.OpenPanel("UIHeroes");
        });
        
        camVertExtent = Camera.main.orthographicSize;
        halfWidth = camVertExtent * Screen.width / Screen.height;
        grid.transform.position = new Vector3(groundCollider.bounds.min.x - halfWidth, -5, transform.position.z);
    
        for (int x = 0; x < 111; x++)
        {
            for (int y = 0; y > -5; y--)
            {
                Tile newTile = new Tile(x, y);
                if (y == 0)
                {
                    SetTile(ground, newTile.coordinate, test1);
                } else
                {
                    SetTile(ground, newTile.coordinate, test2);
                }
            }
        }
    }

    public void SetTile(Tilemap layer, Vector2Int pos, TileBase tile)
    {
        Vector3Int location = new Vector3Int(pos.x, pos.y, 0);
        layer.SetTileFlags(location, TileFlags.None);
        layer.SetTile(location, tile);
    }

    public void Update()
    {
        /*if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var deltaPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 newPosition = deltaPosition;
            newPosition.z = -5;
            newPosition.y = 0;

            float clampedX = Mathf.Clamp(newPosition.x, groundCollider.bounds.min.x + halfWidth + 0.01f, groundCollider.bounds.max.x - halfWidth - 0.01f);
            grid.transform.position = new Vector3(clampedX, 0, grid.transform.position.z);
        }*/
    }

}
