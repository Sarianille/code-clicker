using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public List<Achievement> unachievedAchievements;
    public List<Achievement> achievedAchievements;
    public Clicker clicker;
    private List<string> buildingAchievementNames;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupAchievements();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAchievements();
    }

    private void SetupAchievements()
    {
        AddBuildingNames();

        unachievedAchievements = new List<Achievement>
        {
            new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/Click1").gameObject, (object o) => clicker.clicks >= 1),
            new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/Click100").gameObject, (object o) => clicker.clicks >= 100),
            new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/Click10000").gameObject, (object o) => clicker.clicks >= 10000),
            new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/Click100000").gameObject, (object o) => clicker.clicks >= 100000)
        };

        AddBuildingAchievements();

        achievedAchievements = new List<Achievement>();
    }

    private void AddBuildingNames()
    {
        buildingAchievementNames = new List<string>
        {
            "Key1", "Key5", "Key25", "Key50",
            "URandom1", "URandom5", "URandom25", "URandom50",
            "CodeMonkey1", "CodeMonkey5", "CodeMonkey25", "CodeMonkey50",
            "GiftedChild1", "GiftedChild5", "GiftedChild25", "GiftedChild50",
            "MFFStudent1", "MFFStudent5", "MFFStudent25", "MFFStudent50",
            "TeamMember1", "TeamMember5", "TeamMember25", "TeamMember50",
            "ContractorTeam1", "ContractorTeam5", "ContractorTeam25", "ContractorTeam50",
            "Company1", "Company5", "Company25", "Company50",
            "AI1", "AI5", "AI25", "AI50"
        };
    }

    private void AddBuildingAchievements()
    {
        foreach (var building in clicker.buildings)
        {
            unachievedAchievements.Add(new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/" + buildingAchievementNames[counter]).gameObject, (object o) => building.Amount >= 1));
            unachievedAchievements.Add(new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/" + buildingAchievementNames[counter + 1]).gameObject, (object o) => building.Amount >= 5));
            unachievedAchievements.Add(new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/" + buildingAchievementNames[counter + 2]).gameObject, (object o) => building.Amount >= 25));
            unachievedAchievements.Add(new Achievement(clicker.transform.Find("Achievements Scroll/Viewport/Content/" + buildingAchievementNames[counter + 3]).gameObject, (object o) => building.Amount >= 50));

            counter += 4;
        }
    }

    private void UpdateAchievements()
    {
        if (unachievedAchievements.Count > 0)
        {
            foreach (var achievement in unachievedAchievements)
            {
                if (achievement.IsAchieved())
                {
                    clicker.notification.ShowMessage("Achievement unlocked.");
                    achievedAchievements.Add(achievement);
                }
            }
        }

        if (achievedAchievements.Count > 0)
        {
            foreach (var achievement in achievedAchievements)
            {
                unachievedAchievements.Remove(achievement);
            }
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