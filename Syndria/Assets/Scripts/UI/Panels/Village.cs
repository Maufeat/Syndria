﻿using Assets.Scripts.Models.Village;
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

    // Camera Drag
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    void Start()
    {
        keepOnDispose = true;

        StartCoroutine(NetworkManager.Instance.DownloadImage(NetworkManager.Instance.fbPic, GameObject.Find("ProfilePicture").GetComponent<Image>()));

        VillageManager.Instance.Init(10);

        GameObject.Find("NameLbl").GetComponent<TextMeshProUGUI>().text = Client.Instance.me.Username;

        GameObject.Find("ProfilePicture").GetComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Instance.OpenPanel("UIAccountSettings");
        });

        //SNAP
        camVertExtent = Camera.main.orthographicSize;
        halfWidth = camVertExtent * Screen.width / Screen.height;
        Camera.main.transform.position = new Vector3(VillageManager.Instance.groundCollider.bounds.min.x - halfWidth + 0.01f, 0, transform.position.z);
    }

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
            var deltaPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y) - hit_position;
            Vector3 newPosition = camera_position - deltaPosition * 0.01f;
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
        } catch (Exception e)
        {
            base.Close();
        }
    }
}
