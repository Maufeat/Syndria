using Assets.Scripts.Models.Village;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Village : UIPanel
{    
    private float camVertExtent;
    private float halfWidth;

    public Image ProfilePicture;

    // Camera Drag
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start()
    {
        keepOnDispose = true;

        //StartCoroutine(NetworkManager.Instance.DownloadImage(NetworkManager.Instance.fbPic, GameObject.Find("ProfilePicture").GetComponent<Image>()));

        GameObject.Find("NameLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.nickname;
        GameObject.Find("LevelLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.level.ToString();
        GameObject.Find("XPLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.exp.ToString() + " / " + Client.Instance.me.experienceTable[Client.Instance.me.level];
        GameObject.Find("Diamonds").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.diamonds.ToString();
        GameObject.Find("Gold").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.gold.ToString();
        ProfilePicture.sprite = Resources.Load<Sprite>($"Characters/{Client.Instance.me.profile_picture_id}/thumb");

        GameObject.Find("ProfilePicture").GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel("UIAccountSettings");
        });

        //SNAP
        /*
        VillageManager.Instance.Init(10);

        camVertExtent = Camera.main.orthographicSize;
        halfWidth = camVertExtent * Screen.width / Screen.height;
        Camera.main.transform.position = new Vector3(VillageManager.Instance.groundCollider.bounds.min.x - halfWidth + 0.01f, 0, transform.position.z);*/
    }

    /*
    public void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return;
        }

        if (VillageManager.Instance.blockGroundDrag)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = Camera.main.transform.position;
        }
       
        if (Input.GetMouseButton(0))
        {
            var deltaPosition = Input.mousePosition - hit_position;
            Vector3 newPosition = camera_position - deltaPosition * 0.02f;
            newPosition.z = 0;
            newPosition.y = 0;
            Camera.main.transform.position = newPosition;
        }

        float clampedX = Mathf.Clamp(Camera.main.transform.position.x, VillageManager.Instance.groundCollider.bounds.min.x + halfWidth + 0.01f, VillageManager.Instance.groundCollider.bounds.max.x - halfWidth - 0.01f);
        Camera.main.transform.position = new Vector3(clampedX, 0, 0);
    }

    public override void Close()
    {
        try
        {
            Debug.Log("Close Village");
            foreach (var building in VillageManager.Instance.buildings)
            {
                Destroy(building.gameObject);
            }
            Destroy(VillageManager.Instance.villageObject);
            Camera.main.transform.position = new Vector3(0, 0, 0);
            base.Close(); 
        catch (Exception e)
        {
            base.Close();
        }
    }*/

    public void onDiscordButton()
    {
        Application.OpenURL("https://discord.gg/6wFphds");
    }
}
