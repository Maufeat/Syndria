using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : UIPanel
{
    public Button createBtn;
    public TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        this.keepOnDispose = true;
        createBtn = GameObject.Find("CreateBtn").GetComponent<Button>();
        createBtn.GetComponent<Button>().onClick.AddListener(delegate {
            UIManager.instance.OpenLoadingBox("Creating Character...");

            /*LittleEndianWriter writer = new LittleEndianWriter();
            writer.WriteShort((short)PacketCmd.C2S_CreateCharacter);
            writer.WriteSizedString(inputField.text);
            writer.WriteInt(1); //Character ID, that the player chose
            NetworkManager.Instance.client.Send(writer.Data);*/

        });
        inputField = GameObject.Find("NameInput").GetComponent<TMPro.TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            createBtn.interactable = true;
        } else
        {
            createBtn.interactable = false;
        }
    }
}
