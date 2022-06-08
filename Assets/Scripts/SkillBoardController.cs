using UnityEngine;
using UnityEngine.UI;

public class SkillBoardController : MonoBehaviour
{
    [SerializeField]
    private int skillPoints;

    [SerializeField]
    private Skill[] learnedSkills;

    [Space]
    [SerializeField]
    private SkillBoard skillBoard;

    [Space]
    [SerializeField]
    private SkillBadge[] skillBadges;
    [SerializeField]
    private SkillTooltip skillTooltip;


    [Header("Skill Points")]
    [SerializeField]
    private Text skillPointsCountr;
    [SerializeField]
    private Button skillPointsButton;
    [SerializeField]
    private int addedSkillPoints = 1;


    [Header("Skill Buttons")]
    [SerializeField]
    private Button learnButton;
    [SerializeField]
    private Button forgetButton;
    [SerializeField]
    private Button forgetAllButton;

    [SerializeField]
    private Button background;

    //private _SkillBoard skillBoard;

    private SkillBadge selectedSkillBadge;

    void Start()
    {
        AddListener();

        skillBoard.SetLearnedSkills(learnedSkills);

        HideSkillTooltip();

        UpdateSkills();
        UpdateSkillPointsCountr();
        UpdateSkillButton();
        UpdateForgetAllButton();
    }

    private void OnDestroy()
    {
        RemoveListener();
    }


    private void AddListener() {
        foreach (var skillBadge in skillBadges)
        {
            skillBadge.onClick += HandleSkillClick;
        }

        skillPointsButton.onClick.AddListener(HandleSkillPointClick);

        learnButton.onClick.AddListener(HandleLearneSkillClick);
        forgetButton.onClick.AddListener(HandleForgetSkillClick);
        forgetAllButton.onClick.AddListener(HandleForgetAllSkillClick);

        background.onClick.AddListener(HandleBackgroundClick);
    }
    private void RemoveListener() {
        foreach (var skillBadge in skillBadges)
        {
            skillBadge.onClick -= HandleSkillClick;
        }

        skillPointsButton.onClick.RemoveListener(HandleSkillPointClick);

        learnButton.onClick.RemoveListener(HandleLearneSkillClick);
        forgetButton.onClick.RemoveListener(HandleForgetSkillClick);
        forgetAllButton.onClick.RemoveListener(HandleForgetAllSkillClick);

        background.onClick.RemoveListener(HandleBackgroundClick);
    }

    private void AddSkillPoints(int value) {
        skillPoints += value;
        UpdateSkillPointsCountr();
    }
    private void SubtractSkillPoints(int value) {
        skillPoints -= value;
        UpdateSkillPointsCountr();
    }


    private void HandleSkillPointClick()
    {
        AddSkillPoints(addedSkillPoints);
        UpdateSkillButton();
    }

    private void HandleSkillClick(SkillBadge skillBadge)
    {
        if (selectedSkillBadge != null)
        {
            if (selectedSkillBadge == skillBadge)
            {
                return;
            }

            selectedSkillBadge.Unselect();
        }

        skillBadge.Select();
        selectedSkillBadge = skillBadge;

        ShowSkillTooltip(selectedSkillBadge.GetSkill());
        UpdateSkillButton();
    }
    private void HandleBackgroundClick()
    {
        if (selectedSkillBadge == null)
        {
            return;
        }

        selectedSkillBadge.Unselect();
        selectedSkillBadge = null;

        HideSkillTooltip();
        UpdateSkillButton();
    }

    private void HandleLearneSkillClick()
    {
        var points = skillBoard.Learn(selectedSkillBadge.GetSkill());
        learnedSkills = skillBoard.GetLearnedSkills();

        SubtractSkillPoints(points);
        selectedSkillBadge.SetLearned(true);

        UpdateSkillButton();
        UpdateForgetAllButton();
    }
    private void HandleForgetSkillClick()
    {
        var points = skillBoard.Forget(selectedSkillBadge.GetSkill());
        learnedSkills = skillBoard.GetLearnedSkills();

        AddSkillPoints(points);
        selectedSkillBadge.SetLearned(false);

        UpdateSkillButton();
        UpdateForgetAllButton();
    }
    private void HandleForgetAllSkillClick()
    {
        var points = skillBoard.ForgetAll();
        learnedSkills = skillBoard.GetLearnedSkills();

        AddSkillPoints(points);
        UpdateSkills();

        UpdateSkillButton();
        UpdateForgetAllButton();
    }

    
    private void ShowSkillTooltip(Skill skill)
    {
        skillTooltip.SetContent(skill.title, skill.description, skill.price);
        skillTooltip.Show();
    }
    private void HideSkillTooltip()
    {
        skillTooltip.Hide();
    }


    private void UpdateSkills()
    {
        foreach (var skillBadge in skillBadges)
        {
            skillBadge.SetLearned(skillBoard.Learned(skillBadge.GetSkill()));
        }
    }

    private void UpdateSkillPointsCountr()
    {
        skillPointsCountr.text = skillPoints.ToString();
    }

    private void UpdateLearnButton(Skill skill)
    {
        if (skill == null)
        {
            learnButton.interactable = false;
        }
        else
        {
            learnButton.interactable = skillBoard.CanLearn(skill, skillPoints);
        }
    }

    private void UpdateForgetButton(Skill skill)
    {
        if (skill == null)
        {
            forgetButton.interactable = false;
        } else
        {
            forgetButton.interactable = skillBoard.CanForget(skill);
        }
    }

    private void UpdateSkillButton()
    {
        var skill = selectedSkillBadge?.GetSkill();

        UpdateLearnButton(skill);
        UpdateForgetButton(skill);
    }

    private void UpdateForgetAllButton()
    {
        forgetAllButton.interactable = skillBoard.CanForgetAll();
    }
}