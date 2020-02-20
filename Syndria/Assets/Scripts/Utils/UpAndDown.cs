using UnityEngine;

class UpAndDown : MonoBehaviour
{
    public float xyz = 0;
    public float multip = 1;
    public bool up = true;

    public float startY = 0;

    private void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (!up)
        {
            xyz -= Time.deltaTime;
        } else
        {
            xyz += Time.deltaTime;
        }
        if (xyz >= 1 * multip) up = false;
        if (xyz < 0) up = true; 

        transform.position = new Vector3(transform.position.x, startY + xyz, 5);
    }
}
