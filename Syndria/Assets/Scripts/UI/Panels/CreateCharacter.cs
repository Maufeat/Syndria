using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : UIPanel
{
    public int selectedCharacter = 1;

    public Button createBtn;

    public TMPro.TMP_InputField inputField;

    public GameObject charOne;
    public GameObject charTwo;
    public GameObject charThree;

    public GameObject statsHP;
    public GameObject statsAttack;
    public GameObject statsRange;
    public GameObject statsMovement;

    public SpriteRenderer charRenderer;

    public ParticleSystem particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        this.keepOnDispose = true;
        createBtn = GameObject.Find("CreateBtn").GetComponent<Button>();
        createBtn.GetComponent<Button>().onClick.AddListener(delegate {
            UIManager.instance.OpenLoadingBox("Creating Character...");
            ClientSend.SendCreateCharacter(selectedCharacter, inputField.text);

            /*LittleEndianWriter writer = new LittleEndianWriter();
            writer.WriteShort((short)PacketCmd.C2S_CreateCharacter);
            writer.WriteSizedString(inputField.text);
            writer.WriteInt(1); //Character ID, that the player chose
            NetworkManager.Instance.client.Send(writer.Data);*/

        });
        inputField = GameObject.Find("NameInput").GetComponent<TMPro.TMP_InputField>();
        
        charOne.GetComponent<Button>().onClick.AddListener(delegate
        {
            charOne.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
            charTwo.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            charThree.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            ChangeCharacter(charOne.GetComponentInChildren<PrepHeroItem>().hero);
        });
        charTwo.GetComponent<Button>().onClick.AddListener(delegate
        {
            charOne.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            charTwo.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
            charThree.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            ChangeCharacter(charTwo.GetComponentInChildren<PrepHeroItem>().hero);
        });
        charThree.GetComponent<Button>().onClick.AddListener(delegate
        {
            charOne.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            charTwo.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            charThree.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
            ChangeCharacter(charThree.GetComponentInChildren<PrepHeroItem>().hero);
        });
    }

    void ChangeCharacter(HeroData data)
    {
        selectedCharacter = data.ID;
        charRenderer.sprite = Resources.Load<Sprite>($"Characters/{data.ID}/sprite");
        //statsHP.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Heatlh.ToString();
        //statsAttack.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Attack.ToString();
        //statsRange.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.AttackRange.ToString();
        //statsMovement.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Movement.ToString();
        particleSystems.Play();
    }

    // Update is called once per frame
    new void Update()
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
