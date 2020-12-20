using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTest : Item
{
    int id = 1;
    public override bool OnUse(int qty = 1)
    {
        Debug.Log("DevTest::OnUse->QTY: " +qty);
        ClientSend.UseItem(id, qty);
        return false;
    }
}
