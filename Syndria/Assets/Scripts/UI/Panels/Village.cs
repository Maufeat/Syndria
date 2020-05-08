using Assets.Scripts.Models.Village;
using System.Collections;
using System.Collections.Generic;
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

        VillageManager.Instance.Init(10);

        //SNAP
        camVertExtent = Camera.main.orthographicSize;
        halfWidth = camVertExtent * Screen.width / Screen.height;
        Camera.main.transform.position = new Vector3(VillageManager.Instance.groundCollider.bounds.min.x - halfWidth + 0.01f, 0, transform.position.z);
    }

    public void LateUpdate()
    {
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
            Vector3 newPosition = camera_position - deltaPosition * 0.02f;
            newPosition.z = 0;
            newPosition.y = 0;
            Camera.main.transform.position = newPosition;
        }

        float clampedX = Mathf.Clamp(Camera.main.transform.position.x, VillageManager.Instance.groundCollider.bounds.min.x + halfWidth + 0.01f, VillageManager.Instance.groundCollider.bounds.max.x - halfWidth - 0.01f);
        Camera.main.transform.position = new Vector3(clampedX, 0, 0);
    }
}
