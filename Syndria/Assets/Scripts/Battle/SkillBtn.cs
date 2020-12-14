using Assets.Scripts.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    public Button btn;
    public TMPro.TextMeshProUGUI spellName;
    public Image spellTexture;
    public Image locked;

    public SpellData spell;

    public void ChangeBtn(SpellData _data)
    {
        if(_data == null)
        {
            locked.gameObject.SetActive(true);
            spellTexture.gameObject.transform.parent.gameObject.SetActive(false);
            spellName.gameObject.SetActive(false);
            btn.interactable = false;
            return;
        } else
        {
            locked.gameObject.SetActive(false);
            spellTexture.gameObject.transform.parent.gameObject.SetActive(true);
            spellName.gameObject.SetActive(true);
            btn.interactable = true;
        }
        spell = _data;
        spellName.text = _data.Name;
        spellTexture.sprite = _data.sprite;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() =>
        {
            BattleManager.Instance.selectedHero.WantToAttack(spell);
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
