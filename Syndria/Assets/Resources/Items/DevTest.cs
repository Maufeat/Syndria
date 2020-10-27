using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTest : Item
{
    public override bool OnUse(int qty = 1)
    {
        Debug.Log("DevTest::OnUse->QTY");
        return false;
    }
}
