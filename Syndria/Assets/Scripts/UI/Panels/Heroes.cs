using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heroes : UIPanel
{
    void Start()
    {
        foreach(var hero in Client.instance.me.heroes)
        {

        }
    }
}
