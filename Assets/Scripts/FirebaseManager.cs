using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Net;
using System.IO;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{

    /// <summary>
    /// This Manager handles all functions related to connecting to the firebase database
    /// 
    /// 'Key' :- this parameter is the ID of the projects or user IDs both from the database
    /// 
    /// Functions -
    /// Listen :- Listens on the database for changes/updates in the database
    /// Unsub :- Stops listening on the databse
    /// </summary>
  
    protected Firebase.Auth.FirebaseAuth auth;
    protected Firebase.Auth.FirebaseAuth otherAuth;
    protected DatabaseReference reference;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableDisabled;

    private UIManager UIMgr;
    private ProfileManager ProfileMgr;
    private RememberList RememberListMgr;

    //STATIC 
    public static bool isLoading;

    private void Awake()
    {
        // Getting the objects in the scene for all the managers 
        UIMgr = FindObjectOfType(typeof(UIManager))as UIManager;
        ProfileMgr = FindObjectOfType(typeof(ProfileManager)) as ProfileManager;
        RememberListMgr = FindObjectOfType(typeof(RememberList)) as RememberList;

    }

    public virtual void Start()
    {
        // Checking dependencies for firebase (Don't need to worry)
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {

                InitializeFirebase();
                UIMgr.SignInButton.interactable = true; //Enable the sign in after the initialization for the database
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                UIMgr.CreateToast("(" + dependencyStatus.ToString() + ")" + " - Contact Support");
            }
        });

        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            //No connection

            UIMgr.CreateToast("No internet connection");
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found

            UIMgr.CreateToast("Internet connection not found");
        }
        else
        {
            
        }
    }

    /// <summary>
    /// Function to authenticate with Firebase and the Unity Project
    /// </summary>
    protected void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        app.SetEditorDatabaseUrl("https://dyc-app.firebaseio.com/");
        if(app.Options.DatabaseUrl != null)
        {
            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        }
        Debug.Log(":)");
    }

    public void FirebaseSignIn()
    {
        isLoading = true; // To disable use of the app for the app to load 
        auth.SignInWithEmailAndPasswordAsync(UIMgr.InputEmail.text, UIMgr.InputPassword.text).ContinueWith(task => { //This is the function in the API to sign in for users from the database (read documentation)
            if (task.IsCanceled)
            {
                isLoading = false;
                UIMgr.CreateToast("SignInWithEmailAndPasswordAsync was canceled. Try again later");
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                isLoading = false;
                UIMgr.CreateToast("Incorrect password or email, please try again.");
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            // Sign in is succcessful 
            if(task.IsCompleted)
            {
                PassSignIn();
            }

        });
    }

    /// <summary>
    /// This function passes information from the database to the user grabbing user information for the roles for 
    /// the UI to adjust to the user role
    /// </summary>
    void PassSignIn()
    {
        if(auth.CurrentUser.UserId != null)
        {
            reference = FirebaseDatabase.DefaultInstance.RootReference; //Setting the database reference to the root

            FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
            {
                if(task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else
                {
                    DataSnapshot snapshot = task.Result;
                    User user = new User();
                    string json = "";
                    json = snapshot.GetRawJsonValue();
                    user = JsonUtility.FromJson<User>(json);

                    if(user.position == "Secretary") // if the position is secretary
                    {
                        UIMgr.UserIsSecretary = true; // enable the UI for the secretary
                    }

                    UIMgr.MenuHeader.text = "Welcome, " + user.username.ToString();
                    UIMgr.GoToMenu();
                    isLoading = false;
                    UIMgr.CheckForSecretary(); // This tells the UI to check if the boolean 'UserIsSecretary' is active and adjust the UI Elements 
                }
            });
        }
    }

    /// <summary>
    /// Signs out of the firebase database
    /// </summary>
    public void FirebaseSignOut()
    {
        auth.SignOut();

        /// Clears all the UI Elements to delete UI elements still enabled
        ClearAllActivePPnBListItems();
        ClearAllAllPPnBListItems();
        ClearAllMembersListItems();
        ClearAllMyPPnBListItems();
        ClearAllPendingPPnBListItems();
        ClearAllProjectRegMembersListItems();
        ClearAllProjectRegUserLists();
        ClearAllRememberLists();
        ClearAllSecEvaFormListItems();
    }

    /// <summary>
    /// This region is for methods that are not connect to the firebase database
    /// </summary>
    #region Non-Firebase Methods

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    #endregion

    #region List Public Methods (PP&B)

    //Documentation done 
    #region RememberList

    public void UnsubRememberList()
    {
        FirebaseDatabase.DefaultInstance.GetReference("rememberlist").ValueChanged -= HandleRememberList;
    }

    public void ListenRememberList()
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference("rememberlist").ValueChanged += HandleRememberList;
    }

    /// <summary>
    /// Function that loops through the database to handle the UI input and output for the firebase database rememberlist UI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e2"></param>
    void HandleRememberList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException()); //Something went wrong (CONSOLE)
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllRememberLists(); //Clear

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                AddRememberListItem(ChildSnapshot.Key.ToString()); // Adds a UI list item to the remember list 
            }

            isLoading = false;
        }
    }

    /// <summary>
    /// Function that handles the UI from the firebase database of existing items of the remember list in the database
    /// </summary>
    /// <param name="key"></param>
    void AddRememberListItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.REMEMBER_LIST).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;// Get the data snapshot from the return data in the database 
                RememberItem RL = new RememberItem(); // Initialize a new rememberlist item
                string json = "";
                json = snapshot.GetRawJsonValue(); // Convert the data snapshot to a json value return
                RL = JsonUtility.FromJson<RememberItem>(json); // json template gets return data into the rememberlist item 
                RememberItemUI NewRememberItem = Instantiate(RememberListMgr.Prefab); // Create the item from the prefab
                switch(RL.committee)
                {
                    case "Flagships":
                        NewRememberItem.transform.SetParent(RememberListMgr.FlagParent);
                        RememberListMgr.Flag.Add(NewRememberItem);
                        break;
                    case "Marketing":
                        NewRememberItem.transform.SetParent(RememberListMgr.MarketingParent);
                        RememberListMgr.Marketing.Add(NewRememberItem);
                        break;
                    case "Other":
                        NewRememberItem.transform.SetParent(RememberListMgr.OtherParent);
                        RememberListMgr.Other.Add(NewRememberItem);
                        break;
                    case "Sports & Recreation":
                        NewRememberItem.transform.SetParent(RememberListMgr.SportsParent);
                        RememberListMgr.Sports.Add(NewRememberItem);
                        break;
                    case "Arts & Culture":
                        NewRememberItem.transform.SetParent(RememberListMgr.ArtParent);
                        RememberListMgr.Art.Add(NewRememberItem);
                        break;
                    case "Advocacy":
                        NewRememberItem.transform.SetParent(RememberListMgr.AdvoParent);
                        RememberListMgr.Advo.Add(NewRememberItem);
                        break;
                    case "Feeding Schemes & Disabilities":
                        NewRememberItem.transform.SetParent(RememberListMgr.FeedingParent);
                        RememberListMgr.Feeding.Add(NewRememberItem);
                        break;
                    case "Environment & Infrastructure":
                        NewRememberItem.transform.SetParent(RememberListMgr.EnviroParent);
                        RememberListMgr.Enviro.Add(NewRememberItem);
                        break;
                }
                NewRememberItem.transform.localScale = Vector3.one; // Make sure the UI Element is in scale
                RememberListMgr.All.Add(NewRememberItem); // Add this item to the UI List of remember items

                //Assign the data returned to UI Elements
                NewRememberItem.CommitteeText = RL.committee;
                NewRememberItem.DescriptionText.text = RL.description;
                NewRememberItem.Key = key;

                //This disgustingly hard coded list of checks if there are items to actually show if there are none leave out the items

                if (RememberListMgr.Enviro.Count > 0)
                {
                    RememberListMgr.EnviroGO.SetActive(true);
                }
                if (RememberListMgr.Art.Count > 0)
                {
                    RememberListMgr.ArtGO.SetActive(true);
                }
                if (RememberListMgr.Sports.Count > 0)
                {
                    RememberListMgr.SportsGO.SetActive(true);
                }
                if (RememberListMgr.Flag.Count > 0)
                {
                    RememberListMgr.FlagGO.SetActive(true);
                }
                if (RememberListMgr.Feeding.Count > 0)
                {
                    RememberListMgr.FeedingGO.SetActive(true);
                }
                if (RememberListMgr.Advo.Count > 0)
                {
                    RememberListMgr.AdvoGO.SetActive(true);
                }
                if (RememberListMgr.Marketing.Count > 0)
                {
                    RememberListMgr.MarketingGO.SetActive(true);
                }
                if (RememberListMgr.Other.Count > 0)
                {
                    RememberListMgr.OtherGO.SetActive(true);
                }
            }
        });
    }

    /// <summary>
    /// Clears the UI Elements for the Rememberlist
    /// </summary>
    void ClearAllRememberLists()
    {
        RememberListMgr.OtherGO.SetActive(false);
        RememberListMgr.SportsGO.SetActive(false);
        RememberListMgr.EnviroGO.SetActive(false);
        RememberListMgr.MarketingGO.SetActive(false);
        RememberListMgr.AdvoGO.SetActive(false);
        RememberListMgr.ArtGO.SetActive(false);
        RememberListMgr.FeedingGO.SetActive(false);
        RememberListMgr.FlagGO.SetActive(false);

        RememberListMgr.Flag.Clear();
        RememberListMgr.Advo.Clear();
        RememberListMgr.Marketing.Clear();
        RememberListMgr.Sports.Clear();
        RememberListMgr.Art.Clear();
        RememberListMgr.Feeding.Clear();
        RememberListMgr.Enviro.Clear();
        RememberListMgr.Other.Clear();

        if (RememberListMgr.All.Count > 0)
        {
            for (int i = 0; i < RememberListMgr.All.Count; i++)
            {
                Destroy(RememberListMgr.All[i].gameObject);
            }
        }
        RememberListMgr.All.Clear();
    }

    /// <summary>
    /// This creates the remember list with the following parameters from the inputs
    /// </summary>
    /// <param name="desc"></param>
    /// <param name="committee"></param>
    /// <param name="key"></param>
    public void CreateRememberItem(string desc, string committee, string key)
    {
        if (key == "")
            key = reference.Child(DatabaseParents.REMEMBER_LIST).Push().Key; // Creates a generated key for the item

        RememberItem ri = new RememberItem(committee, desc, key); // Initliazes the item
        string json = JsonUtility.ToJson(ri); // Converts the item to json

        reference.Child(DatabaseParents.REMEMBER_LIST).Child(key).SetRawJsonValueAsync(json); //Add the item to the database when created under 'rememberlist/$ID/item vales'
    }
     /// <summary>
     /// Removes the item from the database 
     /// </summary>
     /// <param name="key"></param>
    public void RemoveRememberItem(string key)
    {
        reference.Child(DatabaseParents.REMEMBER_LIST).Child(key).RemoveValueAsync(); //removes a remember list item with a ID from the database
    }

    #endregion

    //Documentation done
    #region Pending PP&Bs

    public void ListenPendingPPnBList()
    {
        isLoading = true; //Load the list 

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).ValueChanged += HandlePendingpPPnBList;
    }

    public void UnSubPendingPPnBList()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).ValueChanged -= HandlePendingpPPnBList;
    }

    /// <summary>
    /// Loops through the database and returns a list of pending PP&Bs for the user to approve
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e2"></param>
    void HandlePendingpPPnBList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException()); // Database error
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllPendingPPnBListItems(); //Clears a list of UI items before loading
            
            //Loop through all the pp&b statuses
            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(ChildSnapshot.Key.ToString()).GetValueAsync().ContinueWith(task => ///
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                    }
                    else if (task.IsCompleted)
                    {
                        PPnBStatus PS = new PPnBStatus();
                        DataSnapshot snapshot = task.Result;
                        string json = snapshot.GetRawJsonValue();
                        PS = JsonUtility.FromJson<PPnBStatus>(json);

                        //Check if the pp&b's user is authenticated to the logged in user
                        if (PS.currentuser == auth.CurrentUser.UserId)
                        {
                            if(PS.status == PPnBState.PENDING) //Check if the status of the pp&b is pending
                            {
                                AddPendingPPnBItem(ChildSnapshot.Key.ToString());
                            }
                        }
                    }
                });
            }
        }

        isLoading = false;
    }

    /// <summary>
    /// Loop through the list and remove the UI items in the list 
    /// </summary>
    public void ClearAllPendingPPnBListItems()
    {
        if (UIMgr.PendingPPnBItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.PendingPPnBItems.Count; i++)
            {
                Destroy(UIMgr.PendingPPnBItems[i].gameObject);
            }
        }

        UIMgr.PendingPPnBItems.Clear();
    }

    /// <summary>
    /// Create the UI list item for the pending pp&b 
    /// </summary>
    /// <param name="key"></param>
    public void AddPendingPPnBItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception); //Database fault
            }
            else
            {
                DataSnapshot snapshot = task.Result; // Get returned information for the database 
                PPnBStatus ps = new PPnBStatus(); //Initialize the pp&b status item
                string json = "";
                json = snapshot.GetRawJsonValue(); 
                ps = JsonUtility.FromJson<PPnBStatus>(json); //Get the json value returned into the pp&b status item 
                PendingPPnBUI NewPendingItem = Instantiate(UIMgr.PendingPPnBItemPrefab); //Create the pp&b status item
                NewPendingItem.transform.SetParent(UIMgr.PendingPPnBParent); //Set the item as a child to parent to create a grid layout
                NewPendingItem.transform.localScale = Vector3.one; //Fix the scale of the item
                UIMgr.PendingPPnBItems.Add(NewPendingItem);

                //Set the UI item's info
                NewPendingItem.ProjectName.text = ps.projectname;
                NewPendingItem.Key = key;
            }
        });
    }

    #endregion

    #region PP&B Methods

    /// <summary>
    /// Creates a PP&B item in the database
    /// </summary>
    /// <param name="ppnb"></param>
    /// <param name="key"></param>
    public void CreateNewDraftPPnB(PPnB ppnb, string key)
    {
        if (key == "null")
            key = reference.Child(DatabaseParents.PPNBS).Push().Key; //Generated key assigned to the created PP&B

        ppnb.key = key; //key to the ppnb gotten from PP&B UI
        ppnb.owner = auth.CurrentUser.UserId; //Assign the owner to the current user that is logged in and created the PP&B
        ppnb.currentuser = auth.CurrentUser.UserId; //The current user also goes to the owner

        string jsonppnb = JsonUtility.ToJson(ppnb); //Convert the pp&b template to string 
        UserPPnB UP = new UserPPnB(ppnb.projectname, key); // Initialize the pp&b going under the user
        string jsonuppnb = JsonUtility.ToJson(UP); //Convert to string
        PPnBStatus PS = new PPnBStatus(key, PPnBState.DRAFT, auth.CurrentUser.UserId, auth.CurrentUser.UserId, ppnb.projectname, "", "", false); //Initialize the pp&b status class item as a draft PP&B
        string jsonppnbs = JsonUtility.ToJson(PS); //Convert to string 

        //Using the database reference to add the three classes to the database
        reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
        reference.Child(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).Child(DatabaseParents.MY_PPNBS).Child(key).SetRawJsonValueAsync(jsonuppnb);
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
    }

    /// <summary>
    /// This function submits the PP&B from the current user logged in to the next user (which is whoever is next in the hierarchy)
    /// </summary>
    /// <param name="ppnb"></param>
    /// <param name="key"></param>
    public void SubmitDraftPPnB(PPnB ppnb, string key)
    {
        string tempnextuser = "";

        if (key == "null")
            key = reference.Child(DatabaseParents.PPNBS).Push().Key; //Generated key assigned to the created PP&B

        //Get the return data of the user information from the database
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception); //error
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; // Returned data
                User user = new User(); // Initialize the user class
                string json = "";
                json = snapshot.GetRawJsonValue();
                user = JsonUtility.FromJson<User>(json); //Get the json value to the user class
                tempnextuser = user.nextuser; 

                ppnb.currentuser = tempnextuser;
                ppnb.key = key;
                ppnb.owner = auth.CurrentUser.UserId;
                //Simple switch to assign the current access user of the pp&b from the previous user to the 'nextuser'

                //Setting a stirng json value from the classes of PP&B, User PP&Bs and PP&B Statuses
                string jsonppnb = JsonUtility.ToJson(ppnb);
                UserPPnB UP = new UserPPnB(ppnb.projectname, key);
                string jsonuppnb = JsonUtility.ToJson(UP);
                PPnBStatus PS = new PPnBStatus(key, PPnBState.PENDING, auth.CurrentUser.UserId, tempnextuser, ppnb.projectname, "", "", false);
                string jsonppnbs = JsonUtility.ToJson(PS);

                //Use the database reference and update the json value
                reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
                reference.Child(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).Child(DatabaseParents.MY_PPNBS).Child(key).SetRawJsonValueAsync(jsonuppnb);
                reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
            }
        });
    }

    /// <summary>
    /// A function to remove the PP&B from the database using the key reference
    /// </summary>
    /// <param name="key"></param>
    public void RemoveDraftPPnB(string key)
    {
        reference.Child(DatabaseParents.PPNBS).Child(key).RemoveValueAsync();
        reference.Child(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).Child(DatabaseParents.MY_PPNBS).Child(key).RemoveValueAsync();
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).RemoveValueAsync();
    }

    /// <summary>
    /// Submit a PP&B that has been declined but being resubmitted
    /// </summary>
    /// <param name="ppnb"></param>
    /// <param name="key"></param>
    public void ResubmitMyDeclinedPPnB(PPnB ppnb, string key)
    {
        string tempnextuser = "";

        //Get the user information from the database
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);//Error
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result; //Get the returned data 
                User user = new User();
                string json = "";
                json = snapshot.GetRawJsonValue(); //Convert to json string
                user = JsonUtility.FromJson<User>(json); //Set the json value to the user class
                tempnextuser = ppnb.declinedrequestfixuser; //Set the next user to the user that declined the PP&B

                ppnb.currentuser = tempnextuser;

                //Simple switch also used in submitting a PP&B

                //Converting the PP&B class and PP&B Status class to json string
                string jsonppnb = JsonUtility.ToJson(ppnb);
                PPnBStatus PS = new PPnBStatus(key, PPnBState.PENDING, auth.CurrentUser.UserId, tempnextuser, ppnb.projectname, ppnb.declinedrequestfixuser, "", false);
                string jsonppnbs = JsonUtility.ToJson(PS);

                //Use the database reference to update the information in the database
                reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
                reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
            }
        });
    }

    /// <summary>
    /// Update the PP&B information to the databse
    /// </summary>
    /// <param name="ppnb"></param>
    /// <param name="key"></param>
    public void SaveMyDeclinedPPnB(PPnB ppnb, string key)
    {
        //Use the classes to convert it to a json string
        string jsonppnb = JsonUtility.ToJson(ppnb);
        PPnBStatus PS = new PPnBStatus(key, PPnBState.DECLINED, auth.CurrentUser.UserId, auth.CurrentUser.UserId, ppnb.projectname, ppnb.declinedrequestfixcomment, ppnb.declinedrequestfixuser, false);
        string jsonppnbs = JsonUtility.ToJson(PS);

        //Use the reference to update the information in the database
        reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
    }

    /// <summary>
    /// Function to convert an Approved PP&B to an active PP&B
    /// </summary>
    /// <param name="key"></param>
    public void ActivateApprovedPPnB(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                PPnB ppnb = new PPnB();
                string json = "";
                json = snapshot.GetRawJsonValue();
                ppnb = JsonUtility.FromJson<PPnB>(json);

                PPnBStatus PS = new PPnBStatus(key, PPnBState.ACTIVE, ppnb.owner, ppnb.owner, ppnb.projectname, "", "", false);
                string jsonppnbs = JsonUtility.ToJson(PS);
                ActivePPnB AP = new ActivePPnB(key, ppnb.projectstartdate, ppnb.projectenddate, ppnb.venue, ppnb.owner, ppnb.projectname, ppnb.committee);
                string jsonppnba = JsonUtility.ToJson(AP);
                ProjectRegister Projreg = new ProjectRegister(ppnb.owner, ppnb.currentuser, key, PPnBState.DRAFT);
                string jsonprojreg = JsonUtility.ToJson(Projreg);

                reference.Child(DatabaseParents.PROJECT_REGS).Child(key).SetRawJsonValueAsync(jsonprojreg);
                reference.Child(DatabaseParents.ACTIVE_PPNBS).Child(key).SetRawJsonValueAsync(jsonppnba);
                reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
            }
        });
    }

    public void CompleteMyActivePPnB(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                PPnB ppnb = new PPnB();
                string json = "";
                json = snapshot.GetRawJsonValue();
                ppnb = JsonUtility.FromJson<PPnB>(json);

                PPnBStatus PS = new PPnBStatus(key, PPnBState.COMPLETED, ppnb.owner, ppnb.owner, ppnb.projectname, "", "", false);
                string jsonppnbs = JsonUtility.ToJson(PS);
                CompletedPPnB CP = new CompletedPPnB(ppnb.projectname, key, ppnb.owner, System.DateTime.Now.ToShortDateString(), PPnBState.DRAFT);
                string jsonppnbc = JsonUtility.ToJson(CP);

                reference.Child(DatabaseParents.COMPLETED_PPNBS).Child(key).SetRawJsonValueAsync(jsonppnbc);
                reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
                reference.Child(DatabaseParents.ACTIVE_PPNBS).Child(key).RemoveValueAsync();
            }
        });
    }

    public void GetFromExistingPPnBEdit(string key)
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                PPnB returnppnb = new PPnB();
                DataSnapshot snapshot = task.Result;
                string json = "";
                json = snapshot.GetRawJsonValue();
                returnppnb = JsonUtility.FromJson<PPnB>(json);
                UIMgr.EditPPnBMgr.LoadPPnB(returnppnb);
            }
        });

        isLoading = false;
    }

    public void GetFromExistingPPnBRead(string key)
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                PPnB returnppnb = new PPnB();
                DataSnapshot snapshot = task.Result;
                string json = "";
                json = snapshot.GetRawJsonValue();
                returnppnb = JsonUtility.FromJson<PPnB>(json);
                UIMgr.ReadPPnBMgr.LoadPPnB(returnppnb);
            }
        });

        isLoading = false;
    }

    public void UpdatePPnB(PPnB ppnb, string key)
    {
        string jsonppnb = JsonUtility.ToJson(ppnb);
        PPnBStatus PS = new PPnBStatus(key, PPnBState.PENDING, ppnb.owner, auth.CurrentUser.UserId, ppnb.projectname, "", "", false);
        string jsonppnbs = JsonUtility.ToJson(PS);

        reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
    }

    public void ApprovePPnB(PPnB ppnb, string key)
    {
        string tempnextuser = "";

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                User user = new User();
                string json = "";
                json = snapshot.GetRawJsonValue();
                user = JsonUtility.FromJson<User>(json);
                tempnextuser = user.nextuser;

                if(user.position == "Mayor")
                {
                    ppnb.currentuser = ppnb.owner;

                    string jsonppnb = JsonUtility.ToJson(ppnb);
                    PPnBStatus PS = new PPnBStatus(key, PPnBState.APPROVED, ppnb.owner, ppnb.owner, ppnb.projectname,"", "", false);
                    string jsonppnbs = JsonUtility.ToJson(PS);

                    reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
                    reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
                }
                else
                {
                    ppnb.currentuser = tempnextuser;

                    string jsonppnb = JsonUtility.ToJson(ppnb);
                    PPnBStatus PS = new PPnBStatus(key, PPnBState.PENDING, ppnb.owner, tempnextuser, ppnb.projectname,"", "", false);
                    string jsonppnbs = JsonUtility.ToJson(PS);

                    reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
                    reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
                }
            }
        });
    }

    public void DeclinePPnB(PPnB ppnb, string key)
    {
        ppnb.currentuser = ppnb.owner;

        string jsonppnb = JsonUtility.ToJson(ppnb);
        PPnBStatus PS = new PPnBStatus(key, PPnBState.DECLINED, ppnb.owner, ppnb.owner, ppnb.projectname,"", "", false);
        string jsonppnbs = JsonUtility.ToJson(PS);

        reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
    }

    public void RequestFixAndDeclinePPnB(PPnB ppnb, string key,string comment)
    {
        ppnb.currentuser = ppnb.owner;
        ppnb.declinedrequestfixcomment = comment;
        ppnb.declinedrequestfixuser = auth.CurrentUser.UserId;

        string jsonppnb = JsonUtility.ToJson(ppnb);
        PPnBStatus PS = new PPnBStatus(key, PPnBState.REQUEST_FIX, ppnb.owner, ppnb.owner, ppnb.projectname,auth.CurrentUser.UserId, comment,false);
        string jsonppnbs = JsonUtility.ToJson(PS);

        reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
        reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(jsonppnbs);
    }

    #endregion

    #region MembersList

    #region Project Register Return 

    void HandleProjectRegMembersList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllProjectRegMembersListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                AddProjectRegUserButton(ChildSnapshot.Key.ToString());
            }
        }
    }

    public void AddProjectRegUserButton(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                MemberUser mu = new MemberUser();
                string json = "";
                json = snapshot.GetRawJsonValue();
                mu = JsonUtility.FromJson<MemberUser>(json);
                ProjectRegisterEditUI NewProjectRegUserItem = Instantiate(UIMgr.ProjectRegisterEditMgr.userButtonPrefab);
                NewProjectRegUserItem.transform.SetParent(UIMgr.ProjectRegisterEditMgr.userButtonsParent);
                NewProjectRegUserItem.transform.localScale = Vector3.one;
                UIMgr.ProjectRegisterEditMgr.userButtons.Add(NewProjectRegUserItem);
                NewProjectRegUserItem.Username.text = mu.username;
                NewProjectRegUserItem.uid = key;
                //UIMgr.ProjectRegisterEditMgr.LoadNewSelectedUser(key);
            }
        });
    }

    public void ClearAllProjectRegMembersListItems()
    {
        if (UIMgr.ProjectRegisterEditMgr.userButtons.Count > 0)
        {
            for (int i = 0; i < UIMgr.ProjectRegisterEditMgr.userButtons.Count; i++)
            {
                Destroy(UIMgr.ProjectRegisterEditMgr.userButtons[i].gameObject);
            }
        }

        UIMgr.ProjectRegisterEditMgr.userButtons.Clear();
    }

    #endregion

    public void ReturnMemberPosition(string userid, Text textUI)
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).Child(userid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                MemberUser mu = new MemberUser();
                string json = "";
                json = snapshot.GetRawJsonValue();
                mu = JsonUtility.FromJson<MemberUser>(json);
                textUI.text = mu.position;
            }
        });

        isLoading = false;
    }

    public void ReturnMemberName(string userid, Text textUI)
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).Child(userid).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                MemberUser mu = new MemberUser();
                string json = "";
                json = snapshot.GetRawJsonValue();
                mu = JsonUtility.FromJson<MemberUser>(json);
                textUI.text = mu.username;
            }
        });

        isLoading = false;
    }

    public void ListenMembersList()
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).ValueChanged += HandleMembersList;
    }

    public void UnSubMembersList()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).ValueChanged -= HandleMembersList;
    }

    void HandleMembersList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllMembersListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                AddMembersListItem(ChildSnapshot.Key.ToString());
            }
        }

        isLoading = false;
    }

    public void AddMembersListItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                MemberUser mu = new MemberUser();
                string json = "";
                json = snapshot.GetRawJsonValue();
                mu = JsonUtility.FromJson<MemberUser>(json);
                MemberUI NewMembersItem = Instantiate(UIMgr.MembersItemPrefab);
                NewMembersItem.transform.SetParent(UIMgr.MembersParent);
                NewMembersItem.transform.localScale = Vector3.one;
                UIMgr.MembersItems.Add(NewMembersItem);
                NewMembersItem.Name.text = mu.username;
                NewMembersItem.Position = mu.position;
                NewMembersItem.Committee = mu.committee;
                NewMembersItem.Email = mu.email;
                NewMembersItem.Key = key;
            }
        });
    }

    public void ClearAllMembersListItems()
    {
        if (UIMgr.MembersItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.MembersItems.Count; i++)
            {
                Destroy(UIMgr.MembersItems[i].gameObject);
            }
        }

        UIMgr.MembersItems.Clear();
    }

    #endregion

    #region All PP&Bs

    public void ListenAllPPnBList()
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).ValueChanged += HandleAllPPnBList;
    }

    public void UnSubAllPPnBList()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).ValueChanged -= HandleAllPPnBList;
    }

    void HandleAllPPnBList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllAllPPnBListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(ChildSnapshot.Key.ToString()).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                    }
                    else if (task.IsCompleted)
                    {
                        PPnBStatus PS = new PPnBStatus();
                        DataSnapshot snapshot = task.Result;
                        string json = snapshot.GetRawJsonValue();
                        PS = JsonUtility.FromJson<PPnBStatus>(json);

                        if (PS.status == PPnBState.PENDING)
                        {
                            AddAllPPnBItem(ChildSnapshot.Key.ToString());
                        }
                        else if(PS.status == PPnBState.APPROVED)
                        {
                            AddAllPPnBItem(ChildSnapshot.Key.ToString());
                        }
                        else if(PS.status == PPnBState.ACTIVE)
                        {
                            AddAllPPnBItem(ChildSnapshot.Key.ToString());
                        }
                        else if (PS.status == PPnBState.COMPLETED)
                        {
                            AddAllPPnBItem(ChildSnapshot.Key.ToString());
                        }
                    }
                });
            }
        }

        isLoading = false;
    }

    public void ClearAllAllPPnBListItems()
    {
        if (UIMgr.AllPPnBListItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.AllPPnBListItems.Count; i++)
            {
                Destroy(UIMgr.AllPPnBListItems[i].gameObject);
            }
        }

        UIMgr.AllPPnBListItems.Clear();
    }

    public void AddAllPPnBItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                PPnBStatus ps = new PPnBStatus();
                string json = "";
                json = snapshot.GetRawJsonValue();
                ps = JsonUtility.FromJson<PPnBStatus>(json);
                AllPPnBUI NewAllPPnBItem = Instantiate(UIMgr.AllPPnBListItemPrefab);
                NewAllPPnBItem.transform.SetParent(UIMgr.AllPPnBListParent);
                NewAllPPnBItem.transform.localScale = Vector3.one;
                UIMgr.AllPPnBListItems.Add(NewAllPPnBItem);
                NewAllPPnBItem.ProjectName.text = ps.projectname;
                ReturnMemberName(ps.currentuser, NewAllPPnBItem.CurrentUser);
                NewAllPPnBItem.ProjectStatus.text = ps.status.ToUpper();
                NewAllPPnBItem.Key = key;
            }
        });
    }

    #endregion

    #region Active PP&Bs

    public void ListenActivePPnBList()
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.ACTIVE_PPNBS).ValueChanged += HandleActivePPnBList;
    }

    public void UnSubActivePPnBList()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.ACTIVE_PPNBS).ValueChanged -= HandleActivePPnBList;
    }

    void HandleActivePPnBList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllActivePPnBListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                AddActivePPnBItem(ChildSnapshot.Key.ToString());
            }
        }

        isLoading = false;
    }

    public void ClearAllActivePPnBListItems()
    {
        if (UIMgr.ActivePPnBListItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.ActivePPnBListItems.Count; i++)
            {
                Destroy(UIMgr.ActivePPnBListItems[i].gameObject);
            }
        }

        UIMgr.ActivePPnBListItems.Clear();
    }

    public void AddActivePPnBItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.ACTIVE_PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                ActivePPnB ap = new ActivePPnB();
                string json = "";
                json = snapshot.GetRawJsonValue();
                ap = JsonUtility.FromJson<ActivePPnB>(json);
                ActivePPnBUI NewActivePPnBItem = Instantiate(UIMgr.ActivePPnBListItemPrefab);
                NewActivePPnBItem.transform.SetParent(UIMgr.ActivePPnBListParent);
                NewActivePPnBItem.transform.localScale = Vector3.one;
                UIMgr.ActivePPnBListItems.Add(NewActivePPnBItem);
                NewActivePPnBItem.ProjectName.text = ap.projectname;
                ReturnMemberName(ap.owner, NewActivePPnBItem.Owner);
                NewActivePPnBItem.StartDate.text = ap.startdate;
                NewActivePPnBItem.Committee.text = ap.committee;
                NewActivePPnBItem.EndDate = ap.enddate;
                NewActivePPnBItem.Venue = ap.venue;
                NewActivePPnBItem.Key = key;
            }
        });
    }

    #endregion

    #region My PP&Bs

    public void ListenMyPPnBs()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).Child(DatabaseParents.MY_PPNBS).ValueChanged += HandleMyPPnBListItems;
    }

    public void UnsubMyPPnBs()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).Child(DatabaseParents.MY_PPNBS).ValueChanged -= HandleMyPPnBListItems;
    }

    void HandleMyPPnBListItems(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllMyPPnBListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                AddMyPPnBItem(ChildSnapshot.Key.ToString());
            }
        }

        isLoading = false;
    }

    void ClearAllMyPPnBListItems()
    {
        if (UIMgr.MyPPnBItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.MyPPnBItems.Count; i++)
            {
                Destroy(UIMgr.MyPPnBItems[i].gameObject);
            }
        }

        UIMgr.MyPPnBItems.Clear();
    }

    void AddMyPPnBItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                PPnBStatus status = new PPnBStatus();
                string json = "";
                json = snapshot.GetRawJsonValue();
                status = JsonUtility.FromJson<PPnBStatus>(json);
                MyPPnBItem NewMyPPnBItem = Instantiate(UIMgr.MyPPnBItemPrefab);
                NewMyPPnBItem.transform.SetParent(UIMgr.MyPPnBParent);
                NewMyPPnBItem.transform.localScale = Vector3.one;
                UIMgr.MyPPnBItems.Add(NewMyPPnBItem);
                NewMyPPnBItem.ProjectNameText.text = status.projectname;
                NewMyPPnBItem.OwnerID = status.owner;
                NewMyPPnBItem.CurrentUserID = status.currentuser;
                NewMyPPnBItem.RequestFixComment = status.requestfixcomment;
                NewMyPPnBItem.RequestFixUser = status.requestfixuser;
                NewMyPPnBItem.Evaluated = status.evaluated;
                NewMyPPnBItem.Key = status.key;
                NewMyPPnBItem.RawStatus = status.status;

                if (status.owner != status.currentuser)
                {
                    NewMyPPnBItem.CurrentUserName.enabled = true;
                    ReturnMemberName(status.currentuser, NewMyPPnBItem.CurrentUserName);
                }
                else
                {
                    NewMyPPnBItem.CurrentUserName.enabled = false;
                }

                NewMyPPnBItem.ProjectStatus.text = PPnBState.ReturnApplicableStatus(status.status);
                if (status.status == PPnBState.COMPLETED)
                {
                    NewMyPPnBItem.EvaluationText.enabled = true;
                }
                else if (status.status != PPnBState.COMPLETED)
                {
                    NewMyPPnBItem.EvaluationText.enabled = false;
                }

                if (status.status == PPnBState.COMPLETED)
                {
                    FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.COMPLETED_PPNBS).Child(key).GetValueAsync().ContinueWith(taskc =>
                    {
                        if(taskc.IsFaulted)
                        {
                            Debug.LogError(taskc.Exception);
                        }
                        else if(taskc.IsCompleted)
                        {
                            DataSnapshot snapshot1 = taskc.Result;
                            CompletedPPnB completedPPnB = new CompletedPPnB();
                            string jsonc = "";
                            jsonc = snapshot1.GetRawJsonValue();
                            completedPPnB = JsonUtility.FromJson<CompletedPPnB>(jsonc);
                            Debug.Log(completedPPnB.evaluationformstatus);
                            MyCompletedPPnBItem cp = NewMyPPnBItem.gameObject.AddComponent<MyCompletedPPnBItem>();
                            cp.LoadFromCompletedPPnB(completedPPnB);
                        }
                    });
                }
            }
        });
    }

    #endregion

    #endregion

    #region Project Register (PP&B)

    public void GetFromExistingProjectReg(string key)
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PROJECT_REGS).Child(key).Child(DatabaseParents.USERS).ValueChanged += HandleProjectRegSelectedUsers;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PROJECT_REGS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = "";
                ProjectRegister projreg = new ProjectRegister();
                json = snapshot.GetRawJsonValue();
                projreg = JsonUtility.FromJson<ProjectRegister>(json);
                UIMgr.ProjectRegisterEditMgr.LoadFromProjectRegister(projreg);
                isLoading = false;
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).ValueChanged += HandleProjectRegMembersList;
    }

    void HandleProjectRegSelectedUsers(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllProjectRegUserLists();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                UIMgr.ProjectRegisterEditMgr.users.Add(ChildSnapshot.Value.ToString());

                isLoading = false;
            }
        }

    }

    void ClearAllProjectRegUserLists()
    {
        UIMgr.ProjectRegisterEditMgr.users.Clear();
    }

    public void UnsubProjectRegisterUserLists(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PROJECT_REGS).Child(key).Child(DatabaseParents.USERS).ValueChanged -= HandleProjectRegMembersList;
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.MEMBERS_LIST).ValueChanged -= HandleProjectRegMembersList;
    }

    public void SaveProjectRegSelectedUsers(ProjectRegister Projreg, List<string> users,string key)
    {
        string jsonprojreg = JsonUtility.ToJson(Projreg);
        reference.Child(DatabaseParents.PROJECT_REGS).Child(key).SetRawJsonValueAsync(jsonprojreg);

        for (int i = 0; i < users.Count; i++)
        {
            reference.Child(DatabaseParents.PROJECT_REGS).Child(key).Child(DatabaseParents.USERS).Child(i.ToString()).SetValueAsync(users[i]);
        }
    }

    #endregion

    #region List Public Methods (EvaForm)

    public void GetFromExistingEvaluationForm(string key, bool isSec)
    {
        isLoading = true;

        int opens = 0;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                PPnB returnppnb = new PPnB();
                DataSnapshot snapshot = task.Result;
                string json = "";
                json = snapshot.GetRawJsonValue();
                returnppnb = JsonUtility.FromJson<PPnB>(json);
                opens = returnppnb.numberofevaopens;

                if (opens == 0 && !isSec)
                {
                    UIMgr.EvaFormEditMgr.LoadMinerDetailsFromPPnB(returnppnb);
                    isLoading = false;
                }
                else if(opens > 0 || isSec)
                {
                    FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.EVALUATION_FORMS).Child(key).GetValueAsync().ContinueWith(taskevaform =>
                    {
                        if(taskevaform.IsFaulted)
                        {
                            Debug.LogError(taskevaform.Exception);
                        }
                        else
                        {
                            EvaluationForm evaform = new EvaluationForm();
                            DataSnapshot snapshot1 = taskevaform.Result;
                            string jsonevaform = "";
                            jsonevaform = snapshot1.GetRawJsonValue();
                            evaform = JsonUtility.FromJson<EvaluationForm>(jsonevaform);
                            UIMgr.EvaFormEditMgr.LoadEvaForm(evaform);
                            isLoading = false;
                        }
                    });
                }
            }
        });
    }

    #region Draft Evaforms

    public void SaveDraftEvaluationForm(EvaluationForm evaform, string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                PPnB returnppnb = new PPnB();
                string jsonreturnppnb = "";
                jsonreturnppnb = snapshot.GetRawJsonValue();
                returnppnb = JsonUtility.FromJson<PPnB>(jsonreturnppnb);
                returnppnb.numberofevaopens++;

                string jsonnewppnb = JsonUtility.ToJson(returnppnb);
                reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonnewppnb);
            }
        });

        string jsonevaform = JsonUtility.ToJson(evaform);
        EvaluationFormStatus evaforms = new EvaluationFormStatus(evaform.projectname, evaform.owner, key, evaform.ppnbkey, "", PPnBState.DRAFT);
        string jsonevaforms = JsonUtility.ToJson(evaforms);

        reference.Child(DatabaseParents.EVALUATION_FORM_STATUSES).Child(key).SetRawJsonValueAsync(jsonevaforms);
        reference.Child(DatabaseParents.EVALUATION_FORMS).Child(key).SetRawJsonValueAsync(jsonevaform);
    }

    public void SubmitDraftEvaluationFormToSec(EvaluationForm evaform, string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.COMPLETED_PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot completedppnbs_snapshot = task.Result;
                CompletedPPnB cppnb = new CompletedPPnB();
                string jsoncppnb = "";
                jsoncppnb = completedppnbs_snapshot.GetRawJsonValue();
                cppnb = JsonUtility.FromJson<CompletedPPnB>(jsoncppnb);

                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(tasku =>
                {
                    if (tasku.IsFaulted)
                    {
                        Debug.LogError(tasku.Exception);
                    }
                    else
                    {
                        DataSnapshot users_snapshot = tasku.Result;
                        User user = new User();
                        string jsonu = "";
                        jsonu = users_snapshot.GetRawJsonValue();
                        user = JsonUtility.FromJson<User>(jsonu);
                        evaform.currentsecuser = user.secuser;

                        CompletedPPnB CP = new CompletedPPnB(cppnb.projectname, key, cppnb.owner, cppnb.completeddate, PPnBState.PENDING);
                        string jsonppnbc = JsonUtility.ToJson(CP);
                        string jsonevaform = JsonUtility.ToJson(evaform);
                        EvaluationFormStatus evaforms = new EvaluationFormStatus(evaform.projectname, evaform.owner, key, evaform.ppnbkey, user.secuser, PPnBState.PENDING);
                        string jsonevaforms = JsonUtility.ToJson(evaforms);

                        reference.Child(DatabaseParents.EVALUATION_FORM_STATUSES).Child(key).SetRawJsonValueAsync(jsonevaforms);
                        reference.Child(DatabaseParents.EVALUATION_FORMS).Child(key).SetRawJsonValueAsync(jsonevaform);
                        reference.Child(DatabaseParents.COMPLETED_PPNBS).Child(key).SetRawJsonValueAsync(jsonppnbc);

                        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNBS).Child(key).GetValueAsync().ContinueWith(taskp =>
                        {
                            if (taskp.IsFaulted)
                            {
                                Debug.LogError(taskp.Exception);
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot ppnb_snapshot = taskp.Result;
                                PPnB ppnb = new PPnB();
                                string jsonp = "";
                                jsonp = ppnb_snapshot.GetRawJsonValue();
                                ppnb = JsonUtility.FromJson<PPnB>(jsonp);

                                ppnb.currentsecuser = user.secuser;

                                string jsonppnb = JsonUtility.ToJson(ppnb);

                                reference.Child(DatabaseParents.PPNBS).Child(key).SetRawJsonValueAsync(jsonppnb);
                            }
                        });
                    }
                });
            }
        });
    }

    #endregion

    #region Secretary Evaforms

    public void ListenSecEvaFormList()
    {
        isLoading = true;

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.EVALUATION_FORM_STATUSES).ValueChanged += HandleSecEvaFormList;
    }

    public void UnSubSecEvaFormList()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.EVALUATION_FORM_STATUSES).ValueChanged -= HandleSecEvaFormList;
    }

    void HandleSecEvaFormList(object sender, ValueChangedEventArgs e2)
    {
        if (e2.DatabaseError != null)
        {
            Debug.LogError(e2.DatabaseError.ToException());
        }

        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
        {
            ClearAllSecEvaFormListItems();

            foreach (var ChildSnapshot in e2.Snapshot.Children)
            {
                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.EVALUATION_FORM_STATUSES).Child(ChildSnapshot.Key.ToString()).GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                    }
                    else if (task.IsCompleted)
                    {
                        EvaluationFormStatus EFS = new EvaluationFormStatus();
                        DataSnapshot snapshot = task.Result;
                        string json = snapshot.GetRawJsonValue();
                        EFS = JsonUtility.FromJson<EvaluationFormStatus>(json);

                        Debug.Log("???");

                        if (EFS.currentsecuser == auth.CurrentUser.UserId)
                        {
                            if (EFS.status == PPnBState.PENDING)
                            {
                                AddSecEvaFormItem(ChildSnapshot.Key.ToString());
                            }
                            
                            if(EFS.status == PPnBState.LOGGED)
                            {
                                Debug.Log("?");
                                AddSecEvaFormItem(ChildSnapshot.Key.ToString());
                            }
                        }
                    }
                });
            }
        }

        isLoading = false;
    }

    public void ClearAllSecEvaFormListItems()
    {
        if (UIMgr.SecEvaFormListItems.Count > 0)
        {
            for (int i = 0; i < UIMgr.SecEvaFormListItems.Count; i++)
            {
                Destroy(UIMgr.SecEvaFormListItems[i].gameObject);
            }
        }

        UIMgr.SecEvaFormListItems.Clear();
    }

    public void AddSecEvaFormItem(string key)
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.EVALUATION_FORM_STATUSES).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                EvaluationFormStatus efs = new EvaluationFormStatus();
                string json = "";
                json = snapshot.GetRawJsonValue();
                efs = JsonUtility.FromJson<EvaluationFormStatus>(json);
                SecEvaFormUI NewSecEvaFormItem = Instantiate(UIMgr.SecEvaFormListItemPrefab);
                NewSecEvaFormItem.transform.SetParent(UIMgr.SecEvaFormListParent);
                NewSecEvaFormItem.transform.localScale = Vector3.one;
                UIMgr.SecEvaFormListItems.Add(NewSecEvaFormItem);
                NewSecEvaFormItem.ProjectName.text = efs.projectname;
                NewSecEvaFormItem.Status.text = efs.status.ToUpper();
                NewSecEvaFormItem.Key = key;
            }
        });
    }

    public void LogUserAnalyticsFromEvaForm(string key, List<string> regusers, EvaluationForm evaform)
    {
        for (int i = 0; i < regusers.Count; i++)
        {
            if(regusers[i] != evaform.owner)
            {
                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USER_ANALYTICS).Child(regusers[i]).GetValueAsync().ContinueWith(task =>
                {
                    if(task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                    }
                    else if(task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        string json = "";
                        json = snapshot.GetRawJsonValue();
                        UserAnalytics UA = JsonUtility.FromJson<UserAnalytics>(json);
                        UA.hours += float.Parse(evaform.projectdurationhours);
                        UA.projectsgoneto += 1;
                        string jsonua = JsonUtility.ToJson(UA);
                        reference.Child(DatabaseParents.USER_ANALYTICS).Child(regusers[i]).SetRawJsonValueAsync(jsonua);
                    }
                });

                reference.Child(DatabaseParents.USER_ANALYTICS).Child(regusers[i]).Child(DatabaseParents.PROJECT_GONETO_NAMES).Child(evaform.projectname).SetValueAsync("true");
            }
        }

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USER_ANALYTICS).Child(evaform.owner).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = "";
                UserAnalytics UA = new UserAnalytics();
                json = snapshot.GetRawJsonValue();
                UA = JsonUtility.FromJson<UserAnalytics>(json);
                UA.hours += float.Parse(evaform.projectdurationhours);
                UA.projectsdone += 1;

                string jsonoua = JsonUtility.ToJson(UA);
                reference.Child(DatabaseParents.USER_ANALYTICS).Child(evaform.owner).SetRawJsonValueAsync(jsonoua);
                reference.Child(DatabaseParents.USER_ANALYTICS).Child(evaform.owner).Child(DatabaseParents.PROJECT_DONE_NAMES).Child(evaform.projectname).SetValueAsync("true");
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.COMPLETED_PPNBS).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string jsonreturncppnb = "";
                CompletedPPnB returncppnb = new CompletedPPnB();
                jsonreturncppnb = snapshot.GetRawJsonValue();
                returncppnb = JsonUtility.FromJson<CompletedPPnB>(jsonreturncppnb);

                CompletedPPnB CP = new CompletedPPnB(returncppnb.projectname, key, evaform.owner, returncppnb.completeddate, PPnBState.LOGGED);
                string jsonppnbc = JsonUtility.ToJson(CP);
                string jsonevaform = JsonUtility.ToJson(evaform);
                EvaluationFormStatus evaforms = new EvaluationFormStatus(evaform.projectname, evaform.owner, key, evaform.ppnbkey, evaform.currentsecuser, PPnBState.LOGGED);
                string jsonevaforms = JsonUtility.ToJson(evaforms);
                ProjectRegister Projreg = new ProjectRegister(evaform.owner, evaform.owner, key, PPnBState.LOGGED);
                string jsonprojreg = JsonUtility.ToJson(Projreg);

                reference.Child(DatabaseParents.PROJECT_REGS).Child(key).SetRawJsonValueAsync(jsonprojreg);
                reference.Child(DatabaseParents.EVALUATION_FORM_STATUSES).Child(key).SetRawJsonValueAsync(jsonevaforms);
                reference.Child(DatabaseParents.EVALUATION_FORMS).Child(key).SetRawJsonValueAsync(jsonevaform);
                reference.Child(DatabaseParents.COMPLETED_PPNBS).Child(key).SetRawJsonValueAsync(jsonppnbc);
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.PPNB_STATUSES).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                PPnBStatus pPnBStatus = new PPnBStatus();
                pPnBStatus = JsonUtility.FromJson<PPnBStatus>(json);
                pPnBStatus.evaluated = true;

                string returnjson = JsonUtility.ToJson(pPnBStatus);
                reference.Child(DatabaseParents.PPNB_STATUSES).Child(key).SetRawJsonValueAsync(returnjson);
            }
        });
    }

    public void SaveSecretaryEvaluationForm(string key, EvaluationForm evaform)
    {
        string json = JsonUtility.ToJson(evaform);
        reference.Child(DatabaseParents.EVALUATION_FORMS).Child(key).SetRawJsonValueAsync(json);
    }

    #endregion

    #endregion

    #region Profile Methods

    public void LoadUserToProfile()
    {
        FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USERS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                User user = new User();
                user = JsonUtility.FromJson<User>(json);
                FirebaseDatabase.DefaultInstance.GetReference(DatabaseParents.USER_ANALYTICS).Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(taskan =>
                {
                    if(taskan.IsFaulted)
                    {
                        Debug.LogError(taskan.Exception);
                    }
                    else if(taskan.IsCompleted)
                    {
                        DataSnapshot snapshot1 = taskan.Result;
                        string jsonan = snapshot1.GetRawJsonValue();
                        UserAnalytics userAnalytics = new UserAnalytics();
                        userAnalytics = JsonUtility.FromJson<UserAnalytics>(jsonan);
                        ProfileMgr.LoadFromUser_UserA(user, userAnalytics);
                    }
                });
            }
        });
    }

    #endregion
}
