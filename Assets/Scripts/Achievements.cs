using System;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    private List<Achievement> unachievedAchievements;
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

    /// <summary>
    /// Adds the core of the names. The rest of the name is the value of the requirement and do not have to be added.
    /// </summary>
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

    /// <summary>
    /// Finds the clicking achievement gameobject and adds it to the list of unachieved achievements with the correct requirement predicate.
    /// </summary>
    private void AddClickingAchievements()
    {
        foreach (var clickValue in clickValues)
        {
            unachievedAchievements.Add(new Achievement(this.transform.Find($"Achievements Scroll/Viewport/Content/" + achievementNames[counter] + clickValue.ToString()).gameObject, (object o) => clicker.clicks >= clickValue));
        }

        counter++;
    }

    /// <summary>
    /// Finds the building achievement gameobject and adds it to the list of unachieved achievements with the correct requirement predicate.
    /// </summary>
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

    /// <summary>
    /// Check if any achievements have been achieved. If so, remove them from the list of unachieved achievements.
    /// </summary>
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

    /// <summary>
    ///  If the achievement has not been achieved and the requirement is met, show the achievement and set it to achieved.
    /// </summary>
    /// <returns>Whether the achievmment is achieved.</returns>
    public bool IsAchieved()
    {
        if (!achieved && RequirementMet())
        {
            achievement.SetActive(true);
            achieved = true;
        }

        return achieved;
    }

    /// <summary>
    /// Invoke the requirement predicate.
    /// </summary>
    /// <returns>Whether the requirement is fullfilled.</returns>
    public bool RequirementMet()
    {
        return requirement.Invoke(null);
    }
}