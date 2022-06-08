using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBadge : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Skill skill;

    [Space]
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Image selectImage;

    [Space]
    [SerializeField]
    private Color defaultColor;

    [SerializeField]
    private Color learnedColor;

    public delegate void EventHandler(SkillBadge skillBadge);
    public event EventHandler onClick;

    void Start()
    {
        SetSelect(false);
        SetLearned(false);

        iconImage.sprite = skill.icon;
    }

    public Skill GetSkill()
    {
        return skill;
    }

    public void Select()
    {
        SetSelect(true);
    }


    public void Unselect()
    {
        SetSelect(false);
    }

    public void SetSelect(bool value)
    {
        selectImage.gameObject.SetActive(value);
    }

    public void SetLearned(bool value)
    {
        backgroundImage.color = value ? learnedColor : defaultColor;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }
}
