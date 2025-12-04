using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.U2D;
using UnityEngine;
using SFB;
using UnityEngine.UI;
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

    public AudioSource aud;
    public AudioClip win, click, fail;

    public GameObject LogInScreen;
    public GameObject CreatingIssueScreen;
    public GameObject IdentifyIssueScreen;
    public GameObject Map;
    public GameObject LogOutButton;
    public TextMeshProUGUI UserText;

    public TextMeshProUGUI displayLogInStatusText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI passwordText;

    public TextMeshProUGUI CreateIssueXCoord;
    public TextMeshProUGUI CreateIssueYCoord;

    public TextMeshProUGUI CreateIssueDescription;
    public TMP_Dropdown CreateIssueSituationType;
    public Image CreateIssueUploadedImage;

    public TextMeshProUGUI IdentifyIssueText;
    public Image IdentifyIssueImage;

    public GameObject AdminDeleteButton;
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
        aud.clip = click;
        aud.Play();
        state.OnConfirm();
        
    }
    public void OnClickMap()
    {
        state.OnClickMap();
    }
    public void OnLogOut()
    {
        aud.clip = click;
        aud.Play();
        state.OnLogOut();
    }
    public void OnAdminDelete()
    {
        state.OnAdminDelete();
    }
    public void OnUploadImage()
    {
        var extensions = new[] {
            new ExtensionFilter("PNG Image", "png")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a PNG", "", extensions, false);

        if (paths.Length > 0)
        {
            string filePath = paths[0];
            Debug.Log("Selected: " + filePath);
            byte[] data = System.IO.File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(data);

            CreateIssueUploadedImage.sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), new Vector2(.5f, .5f));
        }
        
    }
    public static void OnIdentifyIssue(Issue issue)
    {
        UIManager.instance.state.OnIdentifyIssue(issue);
    }

    public bool IsUserAdmin = false;

    public Dictionary<string, string> users = new Dictionary<string, string>()
    {
        ["User"] = "Password",
    };

    public Dictionary<string, string> ADMINusers = new Dictionary<string, string>()
    {
        ["User1"] = "Password1",
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
    public virtual void OnAdminDelete() { }
    public void OnLogOut() 
    {
        next_state = new LogIn(manager);
        IssueUI.IssueCurrentlyInCreation = new Issue();
        manager.UserText.text = "Not Logged In";
    }
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
        manager.LogOutButton.SetActive(false);
        manager.displayLogInStatusText.text = "Please Enter username and password.";
    }
    public override void OnAttemptLogIn()
    {
        foreach (string possible_user in manager.ADMINusers.Keys)
        {
            Debug.Log(possible_user);
            if (manager.usernameText.text.Contains(possible_user))
            {
                if (manager.passwordText.text.Contains(manager.ADMINusers[possible_user]))
                {
                    manager.IsUserAdmin = true;

                    next_state = new FindIssue(manager);
                    //manager.displayLogInStatusText.text = "Logging in...";
                    manager.UserText.text = "[Admin]\nUser: " + possible_user;
                    manager.aud.clip = manager.win;
                    manager.aud.Play();
                    return;
                }
            }
        }
        foreach (string possible_user in manager.users.Keys)
        {
            if (manager.usernameText.text.Contains(possible_user))
            {
                if (manager.passwordText.text.Contains(manager.users[possible_user]))
                {
                    manager.IsUserAdmin = false;

                    next_state = new FindIssue(manager);
                    //manager.displayLogInStatusText.text = "Logging in...";
                    manager.UserText.text = "User: " + possible_user;
                    manager.aud.clip = manager.win;
                    manager.aud.Play();
                    return;
                }
            }
        }
        manager.aud.clip = manager.fail;
        manager.aud.Play();
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
        manager.LogOutButton.SetActive(true);
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
        IssueUI.IssueCurrentlyInCreation.sprite = manager.CreateIssueUploadedImage.sprite;
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

        manager.AdminDeleteButton.SetActive(manager.IsUserAdmin);

        this.issue = issue;
        name = "IdentifyIssue";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(false);
        manager.CreatingIssueScreen.SetActive(false);
        manager.IdentifyIssueScreen.SetActive(true);

        manager.IdentifyIssueText.text = "Longitude: " + issue.position.x.ToString() + "\nLatitude: " + issue.position.y.ToString() + "\n\n" + issue.description + 
            "\nType: " + issue.situationtype.ToString();
      
        manager.IdentifyIssueImage.sprite = issue.sprite;
    }

    public override void OnConfirm()
    {
        next_state = new FindIssue(manager);    
    }
    public override void OnClickMap()
    {
        next_state = new FindIssue(manager);
    }
    public override void OnAdminDelete()
    {
        int index = IssueUI.IssueDatabase.IndexOf(issue);
        IssueUI.IssueDatabase.RemoveAt(index);
        IssueUI.InstantiateIssues();
        manager.aud.clip = manager.fail;
        manager.aud.Play();
        next_state = new FindIssue(manager);
    }
}