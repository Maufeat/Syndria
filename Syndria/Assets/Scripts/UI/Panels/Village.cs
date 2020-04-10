using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Village : UIPanel
{
    // Start is called before the first frame update
    void Start()
    {
        keepOnDispose = true;
        StartCoroutine(NetworkManager.Instance.DownloadImage(NetworkManager.Instance.fbPic, GameObject.Find("ProfilePicture").GetComponent<Image>()));
        //GameObject.Find("NameLbl").GetComponent<TMPro.TextMeshProUGUI>().text = NetworkManager.Instance.me.Username;
        GameObject.Find("SocialNameLbl").GetComponent<TMPro.TextMeshProUGUI>().text = "aka " + NetworkManager.Instance.fbName;
        //GameObject.Find("LevelLbl").GetComponent<TMPro.TextMeshProUGUI>().text = "Lv. " + NetworkManager.Instance.me.level.ToString();


        //var discordBtn = GameObject.Find("DiscordBtn").GetComponent<Button>();
        //discordBtn.GetComponent<Button>().onClick.AddListener(delegate { StaticTestScript.OpenDiscordUrl(); });
        
        var fightBtn = GameObject.Find("Button (2)").GetComponent<Button>();
        fightBtn.GetComponent<Button>().onClick.AddListener(delegate {
            UIManager.instance.OpenPanel("UIHeroes");
        });
    }
}
