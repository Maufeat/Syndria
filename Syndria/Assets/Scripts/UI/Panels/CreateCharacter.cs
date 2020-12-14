using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : UIPanel
{
    public int selectedCharacter = 1;

    public Button createBtn;

    public TMPro.TMP_InputField inputField;

    public GameObject charContaienr;
    public GameObject charRenderer;

    public GameObject statsHP;
    public GameObject statsAttack;
    public GameObject statsRange;
    public GameObject statsMovement;


    public ParticleSystem particleSystems;

    // Start is called before the first frame update
    void Start()
    {
        this.keepOnDispose = true;
        createBtn = GameObject.Find("CreateBtn").GetComponent<Button>();
        createBtn.GetComponent<Button>().onClick.AddListener(delegate {
            UIManager.Instance.OpenLoadingBox("Creating Character...");
            ClientSend.SendCreateCharacter(selectedCharacter, inputField.text);

        });
        inputField = GameObject.Find("NameInput").GetComponent<TMPro.TMP_InputField>();

        SetAvailableCharacters(Client.Instance.availableCreateCharacter);
    }

    public void SetAvailableCharacters(string[] heroIDs)
    {
        int[] ids = new int[heroIDs.Length];
        for (int i = 0; i <= heroIDs.Length; i++) {
            ids[i] = Convert.ToInt32(heroIDs[i]);
        }
        SetAvailableCharacters(ids);
    }

    public void SetAvailableCharacters(int[] heroIDs)
    {
        int i = 0;
        foreach(int id in heroIDs)
        {
            var hero = new PlayerHero();
            hero.template = Resources.Load<HeroTemplate>($"Characters/{id}/data");
            if (hero.template != null) {
                var characterItem = Instantiate(Resources.Load("Prefabs/UI/Misc/CreateCharacterItem") as GameObject, charContaienr.transform);
                var prepItem = characterItem.GetComponentInChildren<PrepHeroItem>();
                prepItem.hero = hero;
                
                characterItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = prepItem.hero.template.name;
                characterItem.GetComponent<Button>().onClick.AddListener(delegate
                {
                    ChangeCharacter(prepItem.hero.template);
                });
                prepItem.GetComponentInChildren<PrepHeroItem>().SetupImages();
                if(i == 0)
                    ChangeCharacter(prepItem.hero.template);
            }
            i++;
        }
    }

    void ChangeCharacter(HeroTemplate data)
    {
        if(charRenderer.transform.childCount > 0)
            Destroy(charRenderer.transform.GetChild(0).gameObject);
        var instance = Instantiate(data.overwriteGameObject, charRenderer.transform);
        instance.transform.localPosition = new Vector3(0, 0, 0);
        instance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Characters/{data.id}/sprite");
        selectedCharacter = data.id;
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
