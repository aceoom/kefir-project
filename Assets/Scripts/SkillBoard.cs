using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class SkillBoard : MonoBehaviour
{
    [SerializeField]
    private Skill baseSkill;

    [SerializeField]
    private Skill[] skills;

    [SerializeField]
    private SkillRelationship[] relationships;


    private HashSet<Skill> learnedSkills = new HashSet<Skill>();
    private Dictionary<Skill, List<Skill>> adjacencyList = new Dictionary<Skill, List<Skill>>();


    private void Start()
    {
        foreach (var relationship in relationships) {
            if (!adjacencyList.ContainsKey(relationship.left))
            {
                adjacencyList.Add(relationship.left, new List<Skill>());
            }

            if (!adjacencyList.ContainsKey(relationship.right))
            {
                adjacencyList.Add(relationship.right, new List<Skill>());
            }

            adjacencyList[relationship.left].Add(relationship.right);
            adjacencyList[relationship.right].Add(relationship.left);
        }
    }

    public void SetLearnedSkills (Skill[] skills)
    {
        learnedSkills = new HashSet<Skill>();
        foreach (var skill in skills)
        {
            if (skill == baseSkill) continue;
            if (learnedSkills.Contains(skill)) continue;
            learnedSkills.Add(skill);
        }
    }

    public Skill[] GetLearnedSkills()
    {
        return learnedSkills.ToArray();
    }


    public void SetLearnedSkill(Skill skill)
    {
        learnedSkills.Add(skill);
    }

    public void SetForgetSkill(Skill skill)
    {
        learnedSkills.Remove(skill);
    }

    public int Learn(Skill skill)
    {
        if (IsBaseSkill(skill)) return 0;

        learnedSkills.Add(skill);

        return skill.price;
    }

    public int Forget(Skill skill)
    {
        if (IsBaseSkill(skill)) return 0;

        learnedSkills.Remove(skill);

        return skill.price;
    }

    public int ForgetAll()
    {
        var points = 0;
        foreach(var learnedSkill in learnedSkills)
        {
            points += learnedSkill.price;
        }

        learnedSkills = new HashSet<Skill>();

        return points;
    }


    public bool Learned (Skill skill)
    {
        if (IsBaseSkill(skill)) return true;

        return learnedSkills.Contains(skill);
    }

    public bool CanLearn (Skill skill, int points)
    {
        if (IsBaseSkill(skill)) return false;
        if (Learned(skill)) return false;
        if (IsNotEnoughPoints(skill, points)) return false;

        return IsLearnedNeighboring(skill);
    }

    public bool CanForget(Skill skill)
    {
        if (IsBaseSkill(skill)) return false;
        if (!Learned(skill)) return false;

        return IsBasedNeighboring(skill);
    }

    public bool CanForgetAll()
    {
        return learnedSkills.Count > 0;
    }

    private bool IsBaseSkill(Skill skill)
    {
        return baseSkill.Equals(skill);
    }

    private bool IsNotEnoughPoints(Skill skill, int points)
    {
        return skill.price > points;
    }

    private bool IsLearnedNeighboring(Skill skill)
    {
        if (!adjacencyList.ContainsKey(skill))
        {
            return false;
        }

        foreach(var neighboringSkill in adjacencyList[skill])
        {
            if (Learned(neighboringSkill))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsBasedNeighboring(Skill skill)
    {
        if (!adjacencyList.ContainsKey(skill))
        {
            return false;
        }

        foreach (var neighboringSkill in adjacencyList[skill])
        {
            if (!Learned(neighboringSkill)) continue;
            if (IsBaseSkill(neighboringSkill)) continue;

            Debug.Log($"Start: {neighboringSkill.name}");
            if (!HasBasedNeighboring(skill, neighboringSkill)) return false;
            Debug.Log($"End: {neighboringSkill.name}");

        }

        return true;
    }

    private bool HasBasedNeighboring(Skill left, Skill right)
    {
        var visited = new HashSet<Skill> { left, right };
        var stack = new Stack<Skill> { };

        foreach (var nextSkill in adjacencyList[right])
        {
            Debug.Log($"Add: {nextSkill.name}");
            stack.Push(nextSkill);
        }

        while (stack.Count > 0)
        {
            var currentSkill = stack.Pop();

            Debug.Log($"{currentSkill.name} visited:{visited.Contains(currentSkill)} learned:{Learned(currentSkill)} base:{IsBaseSkill(currentSkill)}");

            if (visited.Contains(currentSkill)) continue;
            visited.Add(currentSkill);

            if (!Learned(currentSkill)) continue;
            if (IsBaseSkill(currentSkill)) return true;

            foreach (var nextSkill in adjacencyList[currentSkill])
            {
                Debug.Log($"Add: {nextSkill.name}");
                stack.Push(nextSkill);
            }
        }

        return false;
    }
}

[Serializable]
public class SkillRelationship
{
    public Skill left;
    public Skill right;
}