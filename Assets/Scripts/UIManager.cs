using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.U2D;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public UIstate state;
    void Start()
    {
        state = new LogIn(this);
    }
    private void Update()
    {
        if (state.next_state != null)
        {
            if (state.name != state.next_state.name)
            {
                Debug.Log("changed state..");
                state = state.next_state;
            }
        }
        state.on_update();
    }


    public GameObject LogInScreen;
    public GameObject CreatingIssueScreen;
    public GameObject Map;

    public TextMeshProUGUI displayLogInStatusText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI passwordText;

    public TextMeshProUGUI xCoord;
    public TextMeshProUGUI yCoord;


    public void OnAttemptLogIn()
    {
        state.OnAttemptLogIn();
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
    public virtual void on_update() {}
    public UIstate(UIManager manager) 
    { 
        this.manager = manager;
    }

    public virtual void OnAttemptLogIn() { }
}
public class LogIn : UIstate
{
    public LogIn(UIManager manager) : base(manager)
    {
        name = "LogIn";

        manager.Map.SetActive(true);
        manager.LogInScreen.SetActive(true);
        manager.CreatingIssueScreen.SetActive(false);
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
    }
    public override void on_update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            next_state = new CreateIssue(manager);
            Vector3 mousePos = Input.mousePosition;

            manager.xCoord.text = mousePos.x.ToString();
            manager.yCoord.text = mousePos.y.ToString();
        }
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
    }
    public override void on_update()
    {
    }
}