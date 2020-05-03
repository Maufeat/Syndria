using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public Button btn;
    public TMPro.TextMeshProUGUI spellName;
    public Image spellTexture;

    public SpellData data;

    public void ChangeBtn(SpellData _data)
    {
        data = _data;
        spellName.text = _data.Name;
        spellTexture.sprite = _data.sprite;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            BattleManager.Instance.selectedHero.WantToAttack(data);
        });
    }

    void Start()
    {
        if(btn == null)
            btn = GetComponent<Button>();
        if(spellName == null)
            spellName = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

}
