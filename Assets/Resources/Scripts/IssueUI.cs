using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class IssueUI : MonoBehaviour
{
    public Issue issue;
    public static GameObject issuePrefab;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

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

        for (int i = 0; i < IssueDatabase.Count; i++)
        {
            GameObject g = Instantiate(issuePrefab);
            IssueUI as_issue = g.GetComponent<IssueUI>();

            as_issue.issue = IssueDatabase[i];
            RectTransform as_rect = g.GetComponent<RectTransform>();
            as_rect.anchoredPosition = as_issue.issue.position;

            g.transform.parent = UIManager.instance.transform;
            g.transform.SetSiblingIndex(1);
        }
    }

    public static List<GameObject> InstantiatedIssues = new List<GameObject>();
    public static Issue IssueCurrentlyInCreation = null;
    public static void CreateIssue()
    {
        IssueDatabase.Add(IssueCurrentlyInCreation);
    }


    public static List<Issue> IssueDatabase = new List<Issue>() { };
}

public class Issue
{
    public Image image = null;
    public Vector2 position = Vector2.zero;
    public string description = "none";
    public SituationType situationtype = SituationType.LowPriority;
    public enum SituationType
    {
        LowPriority,
        MediumPriority,
        HighPriority
    }



}