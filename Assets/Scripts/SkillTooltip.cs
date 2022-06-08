using UnityEngine;
using UnityEngine.UI;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text descriptionText;

    [SerializeField]
    private Text priceText;

    public void Show ()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetContent(string title, string description, int price)
    {
        titleText.text = title;
        descriptionText.text = description;
        priceText.text = price.ToString();
    }
}
