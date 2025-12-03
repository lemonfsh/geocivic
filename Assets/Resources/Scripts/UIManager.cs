using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.U2D;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UIstate state;
    void Start()
    {
        instance = this;
        state = new FindIssue(this);
        if (IssueUI.issuePrefab == null)
            IssueUI.issuePrefab = Resources.Load<GameObject>("Scenes/IssueUI");
    }




    private void Update()
    {
        if (state.next_state != null)
        {
            if (state.name != state.next_state.name)
            {
                Debug.Log("changed state to: " + state.next_state.name);
                state = state.next_state;
            }
        }
        state.OnUpdate();
    }


    public GameObject LogInScreen;
    public GameObject CreatingIssueScreen;
    public GameObject IdentifyIssueScreen;
    public GameObject Map;

    public TextMeshProUGUI displayLogInStatusText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI passwordText;

    public TextMeshProUGUI CreateIssueXCoord;
    public TextMeshProUGUI CreateIssueYCoord;

    public TextMeshProUGUI CreateIssueDescription;
    public TMP_Dropdown CreateIssueSituationType;

    public TextMeshProUGUI FindIssueText;
    

    public void OnAttemptLogIn()
    {
        state.OnAttemptLogIn();
    }

    public void OnCancelCreateIssue()
    {
        state.OnCancelCreateIssue();
    }
    public void OnConfirm()
    {
        state.OnConfirm();
    }
    public void OnClickMap()
    {
        state.OnClickMap();
    }
    public static void OnIdentifyIssue(Issue issue)
    {
        UIManager.instance.state.OnIdentifyIssue(issue);
    }

    public Dictionary<string, string> users = new Dictionary<string, string>()
    {
        ["User"] = "Password",
    };

    public Dictionary<string, string> ADMINusers = new Dictionary<string, string>()
    {
        ["User1"] = "Passwordr",
    };
}

public abstract class UIstate
{
    public string name = "null";
    public UIstate next_state = null;
    public UIManager manager;
    public virtual void OnUpdate() {}
    public UIstate(UIManager manager) 
    { 
        this.manager = manager;
    }

    public virtual void OnAttemptLogIn() { }
    public virtual void OnCancelCreateIssue() { }
    public virtual void OnConfirm() { }
    public virtual void OnClickMap() { }
    public virtual void OnIdentifyIssue(Issue issue) { }

}
public class LogIn : UIstate
{
    public LogIn(UIManager manager) : base(manager)
    {
        name = "LogIn";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(true);
        manager.CreatingIssueScreen.SetActive(false);
        manager.IdentifyIssueScreen.SetActive(false);
    }
    public override void OnAttemptLogIn()
    {
        foreach (string possible_user in manager.users.Keys)
        {
            Debug.Log(possible_user);
            if (manager.usernameText.text.Contains(possible_user))
            {
                Debug.Log(manager.users[possible_user]);
                if (manager.passwordText.text.Contains(manager.users[possible_user]))
                {
                    next_state = new FindIssue(manager);
                    manager.displayLogInStatusText.text = "Logging in...";
                    return;
                }
            }
        }
        manager.displayLogInStatusText.text = "Could not log in";
    }
}
//the state where the user clicks on a place on the map to create an issue
public class FindIssue : UIstate
{
    public FindIssue(UIManager manager) : base(manager)
    {
        name = "FindIssue";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(false);
        manager.CreatingIssueScreen.SetActive(false);
        manager.IdentifyIssueScreen.SetActive(false);
    }
    public override void OnClickMap()
    {
        next_state = new CreateIssue(manager);
        Vector3 mousePos = Input.mousePosition;

        manager.CreateIssueXCoord.text = "Longitude: " + mousePos.x.ToString();
        manager.CreateIssueYCoord.text = "Latitude: " + mousePos.y.ToString();


        IssueUI.IssueCurrentlyInCreation = new Issue();
        IssueUI.IssueCurrentlyInCreation.position = new Vector2(mousePos.x, mousePos.y);
    }
    public override void OnIdentifyIssue(Issue issue)
    {

        next_state = new IdentifyIssue(manager, issue);
    }
}

public class CreateIssue : UIstate
{
    public CreateIssue(UIManager manager) : base(manager)
    {
        name = "CreateIssue";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(false);
        manager.CreatingIssueScreen.SetActive(true);
        manager.IdentifyIssueScreen.SetActive(false);
    }
    public override void OnUpdate()
    {
    }
    public override void OnCancelCreateIssue()
    {
        next_state = new FindIssue(manager);
    }
    public override void OnConfirm()
    {
        IssueUI.IssueCurrentlyInCreation.description = manager.CreateIssueDescription.text;
        IssueUI.IssueCurrentlyInCreation.situationtype = (Issue.SituationType)manager.CreateIssueSituationType.value;
        IssueUI.CreateIssue();

        IssueUI.InstantiateIssues();
        next_state = new FindIssue(manager);
        Debug.Log("what");
    }
}

public class IdentifyIssue : UIstate
{
    public Issue issue;
    public IdentifyIssue(UIManager manager, Issue issue) : base(manager)
    {
        this.issue = issue;
        name = "IdentifyIssue";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(false);
        manager.CreatingIssueScreen.SetActive(false);
        manager.IdentifyIssueScreen.SetActive(true);

        manager.FindIssueText.text = "Longitude: " + issue.position.x.ToString() + "\nLatitude: " + issue.position.y.ToString() + "\n\n" + issue.description + 
            "\nType: " + issue.situationtype.ToString();
    }

    public override void OnConfirm()
    {
        next_state = new FindIssue(manager);    
    }
    public override void OnClickMap()
    {
        next_state = new FindIssue(manager);
    }
}