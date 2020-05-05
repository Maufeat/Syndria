using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyAnimEnd : MonoBehaviour
{
    public int AnimationDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(AnimationDuration);
        Debug.Log("DESTORY");
        Destroy(this.transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
