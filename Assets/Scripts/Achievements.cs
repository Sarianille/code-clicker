using System;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public List<Achievement> unachievedAchievements;
    private List<string> achievementNames;
    public Clicker clicker;

    private int counter = 0;
    private ulong[] clickValues = { 1, 100, 10000, 100000 };
    private ulong[] buildingValues = { 1, 5, 25, 50 };

    void Start()
    {
        SetupAchievements();
        InvokeRepeating(nameof(UpdateAchievements), 0, 1);
    }

    private void SetupAchievements()
    {
        unachievedAchievements = new List<Achievement>();

        AddAchievementNames();
        AddClickingAchievements();
        AddBuildingAchievements();
    }

    private void AddAchievementNames()
    {
        achievementNames = new List<string>
        {
            "Click",
            "Key",
            "URandom",
            "CodeMonkey",
            "GiftedChild",
            "MFFStudent",
            "TeamMember",
            "ContractorTeam",
            "Company",
            "AI"
        };
    }

    private void AddClickingAchievements()
    {
        foreach (var clickValue in clickValues)
        {
            unachievedAchievements.Add(new Achievement(this.transform.Find($"Achievements Scroll/Viewport/Content/" + achievementNames[counter] + clickValue.ToString()).gameObject, (object o) => clicker.clicks >= clickValue));
        }

        counter++;
    }

    private void AddBuildingAchievements()
    {
        foreach (var building in clicker.buildings)
        {
            foreach (var buildingValue in buildingValues)
            {
                unachievedAchievements.Add(new Achievement(this.transform.Find("Achievements Scroll/Viewport/Content/" + achievementNames[counter] + buildingValue.ToString()).gameObject, (object o) => building.GetAmount() >= buildingValue));
            }

            counter++;
        }
    }

    private void UpdateAchievements()
    {
        List<Achievement> achievedAchievements = null;

        if (unachievedAchievements.Count > 0)
        {
            foreach (var achievement in unachievedAchievements)
            {
                if (achievement.IsAchieved())
                {
                    clicker.notification.ShowMessage("Achievement unlocked.");

                    if (achievedAchievements is null)
                    {
                        achievedAchievements = new List<Achievement>();
                    }

                    achievedAchievements.Add(achievement);
                }
            }
        }

        if (achievedAchievements is null)
        {
            return;
        }

        foreach (var achievement in achievedAchievements)
        {
            unachievedAchievements.Remove(achievement);
        }
    }
}

public class Achievement
{
    public GameObject achievement;
    public Predicate<object> requirement;
    public bool achieved;

    public Achievement(GameObject achievement, Predicate<object> requirement)
    {
        this.achievement = achievement;
        this.requirement = requirement;
        achieved = false;
    }

    public bool IsAchieved()
    {
        if (!achieved && RequirementMet())
        {
            achievement.SetActive(true);
            achieved = true;
        }

        return achieved;
    }

    public bool RequirementMet()
    {
        return requirement.Invoke(null);
    }
}