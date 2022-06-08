using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill board/Skill")]
public class Skill : ScriptableObject
{
    [Space]
    public string title;
    [TextArea]
    public string description;
    public Sprite icon;

    [Space]
    public int price; 
}
