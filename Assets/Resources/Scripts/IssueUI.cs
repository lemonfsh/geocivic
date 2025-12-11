using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class IssueUI : MonoBehaviour
{
    public Issue issue;
    public static GameObject issuePrefab;
    public void OnClick()
    {
        UIManager.OnIdentifyIssue(issue);
    }

    public static void InstantiateIssues()
    {
        foreach (GameObject g in InstantiatedIssues)
        {
            GameObject.Destroy(g);
        }
        InstantiatedIssues.Clear();

        for (int i = 0; i < Map.Issues.Count; i++)
        {
            GameObject g = Instantiate(issuePrefab);
            IssueUI as_issue = g.GetComponent<IssueUI>();
            InstantiatedIssues.Add(g);


            as_issue.issue = Map.Issues[i];
            RectTransform as_rect = g.GetComponent<RectTransform>();
            as_rect.anchoredPosition = as_issue.issue.position;

            g.transform.parent = UIManager.instance.transform;
            g.transform.SetSiblingIndex(1);
        }
    }

    public static List<GameObject> InstantiatedIssues = new List<GameObject>();
    public static Issue IssueCurrentlyInCreation = new Issue();
    public static void CreateIssue()
    {
        Map.Add(IssueCurrentlyInCreation);
    }
    public static Map Map = new Map();
}

public class Map
{
    public List<Issue> Issues = new List<Issue>();
    public void Add(Issue issue)
    {
        Issues.Add(issue);
    }
}

public class Issue
{
    public Sprite image = null;
    public Vector2 position = Vector2.zero;
    public string description = "";
    public SituationType situationtype = SituationType.Minor;
    public enum SituationType
    {
        Urgent,
        Major,
        Minor,
    }
}