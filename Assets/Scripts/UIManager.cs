using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// Created by James Lamusse
/// 
/// </summary>

public class UIManager : MonoBehaviour
{

    [Header("Side Menu")]
    public Animation SideMenuAnim;
    public Text SideMenuHeader;
    public Text SideMenuVersion;

    [Header("Other")]
    public GameObject ToastWidget;
    public Text ToastText;
    public Animation ToastAnim;
    public float ToastTime;
    public GameObject Loading;

    [Header("Login")]
    public GameObject LoginPage;
    public InputField InputEmail;
    public InputField InputPassword;
    public Button SignInButton;

    [Header("Bottom Menu")]
    public GameObject BottomMenu;
    public Text MenuHeader;
    public GameObject MenuPage;

    [Header("Remember")]
    public GameObject RememberPage;
    public GameObject RememberAddPage;
    public GameObject RememberEditPage;

    public InputField RememberEditDesc;
    public Text RememberEditCommittee;

    public InputField RememberListDescription;
    public Dropdown RememberListDrop;
    private RememberItemUI CurRememberItem;

    [Header("Members")]
    public GameObject MembersPage;
    public GameObject MembersPOP;

    public Text MembersNameHeader;
    public Text MembersCommitteeHeader;
    public Text MembersPositionHeader;
    public Text MembersEmailHeader;
    public List<MemberUI> MembersItems;
    public MemberUI MembersItemPrefab;
    public Transform MembersParent;

    [Header("PP&B Menu")]
    public GameObject PPnBMenuPage;
    public GameObject SecEvaFormPageButton;

    [Header("My PP&B Menu")]
    public GameObject MyPPnBMenuPage;
    public GameObject MyPPnBActionSheet;

    public MyPPnBItem MyPPnBItemPrefab;
    public Transform MyPPnBParent;
    public List<MyPPnBItem> MyPPnBItems;
    private MyPPnBItem MyCurPPnbItem;

    //public List<MyCompletedPPnBItem> CompletedIntegratedItems;

    public Text MyPPnBProjectName;
    public Text MyPPnBRequestFix;

    public GameObject EditMyPPnBButton;
    public GameObject ReadMyPPnBButton;
    public GameObject ActivateMyPPnBButton;
    public GameObject CompleteMyPPnBButton;
    public GameObject EvaluateMyPPnBButton;
    public GameObject ReadEvaluatedMyPPnBButton;
    public GameObject RegisterMyCompletedPPnBButton;
    public GameObject RegisterMyActivePPnBButton;
    public GameObject RemoveMyDraftPPnBButton;

    [Header("Pending PP&Bs")]
    public GameObject PendingPPnBPage;
    public GameObject PendingPPnBPOP;

    public Text PendingPPnBPOPHeader;
    private PendingPPnBUI CurPendingPPnBItem;
    public PendingPPnBUI PendingPPnBItemPrefab;
    public List<PendingPPnBUI> PendingPPnBItems;
    public Transform PendingPPnBParent;

    [Header("Request Fix On Decline")]
    public GameObject RequestFixPOP;
    public InputField RequestFixComment;

    [Header("Active PP&Bs")]
    public GameObject ActivePPnBPage;
    public GameObject ActivePPnBPOP;

    public Text ActivePPnBPOPHeader;
    public Text ActivePPnBPOPStartDate;
    public Text ActivePPnBPOPEndDate;
    public Text ActivePPnBPOPVenue;
    public Text ActivePPnBPOPCommittee;
    public Text ActivePPnBPOPOwner;
    private ActivePPnBUI CurActivePPnBItem;
    public ActivePPnBUI ActivePPnBListItemPrefab;
    public List<ActivePPnBUI> ActivePPnBListItems;
    public Transform ActivePPnBListParent;

    [Header("All PP&Bs")]
    public GameObject AllPPnBPage;

    public AllPPnBUI CurAllPPnBItem;
    public AllPPnBUI AllPPnBListItemPrefab;
    public List<AllPPnBUI> AllPPnBListItems;
    public Transform AllPPnBListParent;

    [Header("PP&B Edit/Read")]
    [HideInInspector] public PPnBEdit EditPPnBMgr;
    public GameObject PPnBEditPage;

    [HideInInspector] public PPnBRead ReadPPnBMgr;
    public GameObject PPnBReadPage;

    [Header("Evaluation Form Edit")]
    public GameObject EvaFormPage;
    [HideInInspector] public EvaluationFormEdit EvaFormEditMgr;

    [Header("Project Register")]
    public GameObject ProjectRegisterPage;
    public ProjectRegisterEdit ProjectRegisterEditMgr;

    /** MIke was here**/

    [Header("Secretary Evaforms")]
    public GameObject SecEvaFormPage;
    public GameObject SecEvaFormPOP;

    public Text SecEvaFormPOPHeader;
    public Text SecEvaFormPOPDateText;
    private SecEvaFormUI CurSecEvaForm;
    public SecEvaFormUI SecEvaFormListItemPrefab;
    public List<SecEvaFormUI> SecEvaFormListItems;
    public Transform SecEvaFormListParent;

    public GameObject SecEvaFormEvaluatePOPButton;
    public GameObject SecEvaformReadPOPButton;

    [Header("Profile Page")]
    public GameObject ProfilePage;

    [Header("Settings Page")]
    public GameObject SettingsPage;

    private FirebaseManager FirebaseMgr;

    private bool ActiveToast;
    private float tt;

    public static long openEditTime;
    public static long openReadTime;
    public bool UserIsSecretary;

    private void Awake()
    {
        ToastTime = 1.4f;
        tt = ToastTime;
    }

    private void Start()
    {
        EditPPnBMgr = FindObjectOfType(typeof(PPnBEdit)) as PPnBEdit;
        ReadPPnBMgr = FindObjectOfType(typeof(PPnBRead)) as PPnBRead;
        EvaFormEditMgr = FindObjectOfType(typeof(EvaluationFormEdit)) as EvaluationFormEdit;
        FirebaseMgr = FindObjectOfType(typeof(FirebaseManager)) as FirebaseManager;
        ProjectRegisterEditMgr = FindObjectOfType(typeof(ProjectRegisterEdit)) as ProjectRegisterEdit;
    }

    // Update is called once per frame
    void Update()
    {
        if (FirebaseManager.isLoading)
        {
            Loading.SetActive(true);
        }
        else
        {
            Loading.SetActive(false);
        }

        if (ActiveToast)
        {
            if (tt > 0)
            {
                tt -= Time.deltaTime;
                if (tt < 0.2f)
                {
                    ToastAnim.Play("ToastEnd");
                }
            }
            else
            {
                ActiveToast = false;
                ToastWidget.SetActive(false);

                tt = ToastTime;
            }
        }
    }

    public void CreateToast(string text)
    {
        ToastWidget.SetActive(true);
        ActiveToast = true;
        ToastAnim.Play("ToastStart");
        ToastText.text = text;
    }

    public void CheckForSecretary()
    {
        if (UserIsSecretary)
        {
            SecEvaFormPageButton.SetActive(true);
        }
        else
        {
            SecEvaFormPageButton.SetActive(false);
        }
    }

    #region Navigation Methods

    #region RememberList

    public void GoToRememberList()
    {
        BackFromPPnBMenu();
        BackFromMembers();
        RememberPage.SetActive(true);
        FirebaseMgr.ListenRememberList();
    }

    public void GoToRememberListAdd()
    {
        RememberPage.SetActive(false);
        RememberAddPage.SetActive(true);
    }

    public void BackFromRememberListAdd()
    {
        RememberPage.SetActive(true);
        RememberAddPage.SetActive(false);
    }

    public void BackFromRememberList()
    {
        RememberPage.SetActive(false);
        FirebaseMgr.UnsubRememberList();
    }

    public void OpenEditRememberItem(RememberItemUI item)
    {
        CurRememberItem = item;
        RememberEditPage.GetComponent<Animation>().Play("Open");
        RememberEditDesc.text = CurRememberItem.DescriptionText.text;
        RememberEditCommittee.text = CurRememberItem.CommitteeText;
    }

    public void CloseRemoveRememberItem()
    {
        CurRememberItem = null;
        RememberEditPage.GetComponent<Animation>().Play("Close");
    }

    #endregion

    #region Side Menu

    public void OpenSideMenu()
    {
        SideMenuAnim.Play("OpenSideMenu");
    }

    public void CloseSideMenu()
    {
        SideMenuAnim.Play("CloseSideMenu");
    }

    #endregion

    #region Profile Page

    public void GoToProfile()
    {
        MenuPage.SetActive(false);
        ProfilePage.SetActive(true);
        FirebaseMgr.LoadUserToProfile();
        CloseSideMenu();
    }

    public void BackFromProfilePage()
    {
        MenuPage.SetActive(true);
        ProfilePage.SetActive(false);
    }

    #endregion

    public void GoToMenu()
    {
        BottomMenu.SetActive(true);
        MenuPage.SetActive(true);
        LoginPage.SetActive(false);
        GoToPPnBMenu();
    }

    public void SignOut()
    {
        LoginPage.SetActive(true);
        MenuPage.SetActive(false);
        FirebaseMgr.FirebaseSignOut();
        CloseSideMenu();
    }

    public void GoToMembers()
    {
        BackFromPPnBMenu();
        BackFromRememberList();
        MembersPage.SetActive(true);
        FirebaseMgr.ListenMembersList();
    }

    public void BackFromMembers()
    {
        MembersPage.SetActive(false);
        FirebaseMgr.UnSubMembersList();
    }

    public void GoToPPnBMenu()
    {
        PPnBMenuPage.SetActive(true);
        BackFromMembers();
        BackFromRememberList();
    }

    public void BackFromPPnBMenu()
    {
        PPnBMenuPage.SetActive(false);
    }

    #region Project Register

    public void GoToProjectRegisterMyActive()
    {
        ProjectRegisterPage.SetActive(true);
        MyPPnBMenuPage.SetActive(false);
        ProjectRegisterEditMgr.MyActiveTopMenu.SetActive(true);
        ProjectRegisterEditMgr.MyCompletedTopMenu.SetActive(false);
    }

    public void BackFromProjectRegisterMyActive()
    {
        ProjectRegisterPage.SetActive(false);
        MyPPnBMenuPage.SetActive(true);
        FirebaseMgr.UnsubProjectRegisterUserLists(ProjectRegisterEditMgr.ReturnProjectRegister().key);
    }

    public void GoToProjectRegisterMyCompleted()
    {
        ProjectRegisterPage.SetActive(true);
        MyPPnBMenuPage.SetActive(false);
        ProjectRegisterEditMgr.MyActiveTopMenu.SetActive(false);
        ProjectRegisterEditMgr.MyCompletedTopMenu.SetActive(true);
    }

    public void BackFromProjectRegisterMyCompleted()
    {
        ProjectRegisterPage.SetActive(false);
        MyPPnBMenuPage.SetActive(true);
        FirebaseMgr.UnsubProjectRegisterUserLists(ProjectRegisterEditMgr.ReturnProjectRegister().key);
    }

    #endregion

    #region Secretary Eva forms

    public void GoToSecEvaForms()
    {
        SecEvaFormPage.SetActive(true);
        MenuPage.SetActive(false);
        FirebaseMgr.ListenSecEvaFormList();
        CloseSideMenu();
    }

    public void BackFromSecEvaForms()
    {
        SecEvaFormPage.SetActive(false);
        MenuPage.SetActive(true);
        FirebaseMgr.UnSubSecEvaFormList();
    }

    public void OpenInstanceSecEvaForm(SecEvaFormUI item)
    {
        SecEvaFormPOP.GetComponent<Animation>().Play("Open");
        CurSecEvaForm = item;
        SecEvaFormPOPHeader.text = item.ProjectName.text;
        SecEvaFormPOPDateText.text = item.Status.text.ToUpper();

        if (item.Status.text == PPnBState.PENDING.ToUpper())
        {
            SecEvaFormEvaluatePOPButton.SetActive(true);
            SecEvaformReadPOPButton.SetActive(false);
        }
        else if (item.Status.text == PPnBState.LOGGED.ToUpper())
        {
            SecEvaFormEvaluatePOPButton.SetActive(false);
            SecEvaformReadPOPButton.SetActive(true);
        }
    }

    public void CloseInstanceSecEvaForm()
    {
        CurSecEvaForm = null;
        SecEvaFormPOP.GetComponent<Animation>().Play("Close");
    }

    public void GoToSecEvaFormRead()
    {
        EvaFormPage.SetActive(true);
        SecEvaFormPage.SetActive(false);
        EvaFormEditMgr.DraftEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.SecEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadEvaformTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadSecEvaformTopMenu.SetActive(true);
    }

    public void BackFromSecEvaFormRead()
    {
        EvaFormPage.SetActive(false);
        SecEvaFormPage.SetActive(true);
        FirebaseMgr.ListenSecEvaFormList();
        CloseInstanceSecEvaForm();
    }

    public void GoToSecEvaFormEdit()
    {
        EvaFormPage.SetActive(true);
        SecEvaFormPage.SetActive(false);
        FirebaseMgr.UnSubSecEvaFormList();
        EvaFormEditMgr.DraftEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.SecEvaFormTopMenu.SetActive(true);
        EvaFormEditMgr.CompletedReadEvaformTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadSecEvaformTopMenu.SetActive(false);
    }

    public void BackFromSecEvaFormEdit()
    {
        EvaFormPage.SetActive(false);
        SecEvaFormPage.SetActive(true);
        FirebaseMgr.ListenSecEvaFormList();
        CloseInstanceSecEvaForm();
    }

    public void GoToMyCompletedEvaFormEdit()
    {
        MyPPnBMenuPage.SetActive(false);
        EvaFormPage.SetActive(true);
        EvaFormEditMgr.DraftEvaFormTopMenu.SetActive(true);
        EvaFormEditMgr.SecEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadEvaformTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadSecEvaformTopMenu.SetActive(false);
    }

    public void BackFromMyCompletedEvaFormEdit()
    {
        MyPPnBMenuPage.SetActive(true);
        EvaFormPage.SetActive(false);
    }

    public void GoToMyCompletedPPnBEvaformRead()
    {
        MyPPnBMenuPage.SetActive(false);
        EvaFormPage.SetActive(true);
        EvaFormEditMgr.DraftEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.SecEvaFormTopMenu.SetActive(false);
        EvaFormEditMgr.CompletedReadEvaformTopMenu.SetActive(true);
        EvaFormEditMgr.CompletedReadSecEvaformTopMenu.SetActive(false);
    }

    public void BackFromMyCompletedEvaformRead()
    {
        MyPPnBMenuPage.SetActive(true);
        EvaFormPage.SetActive(false);
    }

    #endregion

    #region Active PP&Bs

    public void GoToActivePPnB()
    {
        ActivePPnBPage.SetActive(true);
        MenuPage.SetActive(false);
        FirebaseMgr.ListenActivePPnBList();
    }

    public void BackFromActivePPnB()
    {
        ActivePPnBPage.SetActive(false);
        MenuPage.SetActive(true);
        FirebaseMgr.UnSubActivePPnBList();
    }

    public void OpenInstanceActivePPnB(ActivePPnBUI item)
    {
        ActivePPnBPOP.GetComponent<Animation>().Play("Open");
        CurActivePPnBItem = item;
        ActivePPnBPOPHeader.text = item.ProjectName.text;
        ActivePPnBPOPOwner.text = item.Owner.text;
        ActivePPnBPOPStartDate.text = item.StartDate.text;
        ActivePPnBPOPVenue.text = item.Venue;
        ActivePPnBPOPEndDate.text = item.EndDate;
        ActivePPnBPOPCommittee.text = item.Committee.text;
    }

    public void CloseInstanceActivePPnB()
    {
        ActivePPnBPOP.GetComponent<Animation>().Play("Close");
        CurActivePPnBItem = null;
    }

    #endregion

    #region MembersList

    public void OpenInstanceMembersPOP(MemberUI item)
    {
        MembersPOP.GetComponent<Animation>().Play("Open");

        MembersNameHeader.text = item.Name.text;
        MembersEmailHeader.text = item.Email;
        MembersPositionHeader.text = item.Position;
        MembersCommitteeHeader.text = item.Committee;
    }

    public void CloseInstanceMembersPOP()
    {
        MembersPOP.GetComponent<Animation>().Play("Close");
    }

    #endregion

    #region My PP&Bs

    public void GoToMyPPnBsMenu()
    {
        MenuPage.SetActive(false);
        MyPPnBMenuPage.SetActive(true);
        FirebaseMgr.ListenMyPPnBs();
    }

    public void BackFromMyPPnBMenu()
    {
        MyPPnBMenuPage.SetActive(false);
        MenuPage.SetActive(true);
        FirebaseMgr.UnsubMyPPnBs();
    }

    #endregion

    #region Pending PP&Bs

    public void GoToPendingPPnB()
    {
        PendingPPnBPage.SetActive(true);
        MenuPage.SetActive(false);
        FirebaseMgr.ListenPendingPPnBList();
    }

    public void BackFromPendingPPnB()
    {
        PendingPPnBPage.SetActive(false);
        MenuPage.SetActive(true);
        FirebaseMgr.UnSubPendingPPnBList();
    }

    public void OpenInstancePendingPPnB(PendingPPnBUI item)
    {
        PendingPPnBPOP.GetComponent<Animation>().Play("Open");
        CurPendingPPnBItem = item;
        PendingPPnBPOPHeader.text = item.ProjectName.text;
    }

    public void CloseInstancePendingPPnB()
    {
        PendingPPnBPOP.GetComponent<Animation>().Play("Close");
        CurPendingPPnBItem = null;
    }

    public void GoToPendingPPnBEdit()
    {
        PPnBEditPage.SetActive(true);
        PendingPPnBPage.SetActive(false);
        EditPPnBMgr.OpenPPnBEdit("pending", false);
    }

    public void BackFromPendingPPnBEdit()
    {
        PPnBEditPage.SetActive(false);
        PendingPPnBPage.SetActive(true);
        CurPendingPPnBItem = null;
    }

    public void GoToPendingPPnBRead()
    {
        PPnBReadPage.SetActive(true);
        PendingPPnBPage.SetActive(false);
        ReadPPnBMgr.OpenPPnBRead("pending", false);
    }

    public void BackFromPendingPPnBRead()
    {
        PPnBReadPage.SetActive(false);
        PendingPPnBPage.SetActive(true);
        CurPendingPPnBItem = null;
    }

    #endregion

    #region All PP&Bs

    public void GoToAllPPnB()
    {
        AllPPnBPage.SetActive(true);
        MenuPage.SetActive(false);
        FirebaseMgr.ListenAllPPnBList();
    }

    public void BackFromAllPPnB()
    {
        AllPPnBPage.SetActive(false);
        MenuPage.SetActive(true);
        FirebaseMgr.UnSubAllPPnBList();
    }

    #endregion

    #endregion

    #region RememberList Methods

    public void CreateRememberItem()
    {
        FirebaseMgr.CreateRememberItem(RememberListDescription.text, RememberListDrop.options[RememberListDrop.value].text, "");
        BackFromRememberListAdd();
    }

    public void RemoveRememberListItem()
    {
        FirebaseMgr.RemoveRememberItem(CurRememberItem.Key);
        CloseRemoveRememberItem();
    }

    public void EditRememberItem()
    {
        //Use same method as create as it will do the same effect if it were an edit
        FirebaseMgr.CreateRememberItem(RememberEditDesc.text, CurRememberItem.CommitteeText, CurRememberItem.Key);
        CloseRemoveRememberItem();
    }

    #endregion

    #region Draft My PP&B Methods

    public void CreateNewDraftPPnB()
    {
        PPnBEditPage.SetActive(true);
        EditPPnBMgr.OpenPPnBEdit("draft", true);
        EditPPnBMgr.Key = "null";
        EditPPnBMgr.EmptyPPnB();
        openEditTime = System.DateTime.Now.Ticks;
    }

    public void SaveMyDraftPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromMyPPnBEdit();
            FirebaseMgr.CreateNewDraftPPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromMyPPnBRead();
            FirebaseMgr.CreateNewDraftPPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    public void SubmitMyDraftPPnB()
    {
        if (openEditTime > openReadTime)
        {
            FirebaseMgr.SubmitDraftPPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
            BackFromMyPPnBEdit();
        }
        else
        {
            FirebaseMgr.SubmitDraftPPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
            BackFromMyPPnBRead();
        }
    }

    public void RemoveMyDraftPPnB()
    {
        FirebaseMgr.RemoveDraftPPnB(MyCurPPnbItem.Key);
        CloseMyPPnBItemInstance();
    }

    #endregion

    #region Pending PP&B Methods

    public void EditPendingPPnB()
    {
        GoToPendingPPnBEdit();
        FirebaseMgr.GetFromExistingPPnBEdit(CurPendingPPnBItem.Key);
    }

    public void ReadPendingPPnB()
    {
        GoToPendingPPnBRead();
        FirebaseMgr.GetFromExistingPPnBRead(CurPendingPPnBItem.Key);
    }

    public void ApprovePendingPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromPendingPPnBEdit();
            FirebaseMgr.ApprovePPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromPendingPPnBRead();
            FirebaseMgr.ApprovePPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    public void SavePendingPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromPendingPPnBEdit();
            FirebaseMgr.UpdatePPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromPendingPPnBRead();
            FirebaseMgr.UpdatePPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    public void OpenRequestFixOnPPnB()
    {
        RequestFixPOP.SetActive(true);
    }

    public void CloseRequestFixOnPPnB()
    {
        RequestFixPOP.SetActive(false);
    }

    public void RequestFixOnPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromPendingPPnBEdit();
            CloseRequestFixOnPPnB();
            FirebaseMgr.RequestFixAndDeclinePPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key, RequestFixComment.text);
        }
        else
        {
            BackFromPendingPPnBRead();
            CloseRequestFixOnPPnB();
            FirebaseMgr.RequestFixAndDeclinePPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key, RequestFixComment.text);
        }
    }

    public void DeclinePendingPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromPendingPPnBEdit();
            FirebaseMgr.DeclinePPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromPendingPPnBRead();
            FirebaseMgr.DeclinePPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    #endregion

    #region My PP&Bs

    public void GoToMyPPnBRead()
    {
        MyPPnBMenuPage.SetActive(false);
        PPnBReadPage.SetActive(true);
        ReadPPnBMgr.OpenPPnBRead(MyCurPPnbItem.RawStatus, true);
        FirebaseMgr.GetFromExistingPPnBRead(MyCurPPnbItem.Key);
    }

    public void BackFromMyPPnBRead()
    {
        MyPPnBMenuPage.SetActive(true);
        PPnBReadPage.SetActive(false);
    }

    public void GoToMyPPnBEdit()
    {
        MyPPnBMenuPage.SetActive(false);
        PPnBEditPage.SetActive(true);
        EditPPnBMgr.OpenPPnBEdit(MyCurPPnbItem.RawStatus, true);
        FirebaseMgr.GetFromExistingPPnBEdit(MyCurPPnbItem.Key);
    }

    public void BackFromMyPPnBEdit()
    {
        MyPPnBMenuPage.SetActive(true);
        PPnBEditPage.SetActive(false);
    }

    public void OpenMyPPnBItemInstance(MyPPnBItem item, string completedstatus)
    {
        MyPPnBActionSheet.GetComponent<Animation>().Play("Open");
        MyCurPPnbItem = item;

        MyPPnBProjectName.text = item.ProjectNameText.text;
        MyPPnBRequestFix.transform.parent.gameObject.SetActive(false);
        ReadMyPPnBButton.SetActive(true);
        ActivateMyPPnBButton.SetActive(false);
        CompleteMyPPnBButton.SetActive(false);
        EditMyPPnBButton.SetActive(false);
        EvaluateMyPPnBButton.SetActive(false);
        RegisterMyActivePPnBButton.SetActive(false);
        RegisterMyCompletedPPnBButton.SetActive(false);
        RemoveMyDraftPPnBButton.SetActive(false);
        ReadEvaluatedMyPPnBButton.SetActive(false);

        switch (item.RawStatus)
        {
            case (PPnBState.REQUEST_FIX):
                MyPPnBRequestFix.transform.parent.gameObject.SetActive(false);
                EditMyPPnBButton.SetActive(true);
                break;
            case (PPnBState.ACTIVE):
                CompleteMyPPnBButton.SetActive(true);
                RegisterMyActivePPnBButton.SetActive(true);
                break;
            case (PPnBState.APPROVED):
                ActivateMyPPnBButton.SetActive(true);
                break;
            case (PPnBState.COMPLETED):
                if(completedstatus != null)
                {
                    if (completedstatus == "draft")
                    {
                        EvaluateMyPPnBButton.SetActive(true);
                        RegisterMyCompletedPPnBButton.SetActive(true);
                    }
                    else if(item.Evaluated)
                    {
                        ReadEvaluatedMyPPnBButton.SetActive(true);
                    }
                }
                break;
            case (PPnBState.DRAFT):
                RemoveMyDraftPPnBButton.SetActive(true);
                EditMyPPnBButton.SetActive(true);
                break;
        }
    }

    public void CloseMyPPnBItemInstance()
    {
        MyCurPPnbItem = null;
        MyPPnBActionSheet.GetComponent<Animation>().Play("Close");
    }

    #endregion

    #region My Declined PP&Bs Methods

    public void SaveMyDeclinedPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromMyPPnBEdit();
            FirebaseMgr.SaveMyDeclinedPPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromMyPPnBRead();
            FirebaseMgr.SaveMyDeclinedPPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    public void ResubmittedMyDeclinedPPnB()
    {
        if (openEditTime > openReadTime)
        {
            BackFromMyPPnBEdit();
            FirebaseMgr.ResubmitMyDeclinedPPnB(EditPPnBMgr.GetPPnBValue(), EditPPnBMgr.GetPPnBValue().key);
        }
        else
        {
            BackFromMyPPnBRead();
            FirebaseMgr.ResubmitMyDeclinedPPnB(ReadPPnBMgr.GetPPnBValue(), ReadPPnBMgr.GetPPnBValue().key);
        }
    }

    #endregion

    #region My Approved PP&Bs Methods

    public void ActivateMyApprovedPPnB()
    {
        FirebaseMgr.ActivateApprovedPPnB(MyCurPPnbItem.Key);
        CloseMyPPnBItemInstance();
    }

    #endregion

    #region My Active PP&Bs Methods

    public void RegisterMyActivePPnB()
    {
        GoToProjectRegisterMyActive();
        FirebaseMgr.GetFromExistingProjectReg(MyCurPPnbItem.Key);
    }

    public void SaveMyActiveProjectRegister()
    {
        BackFromProjectRegisterMyActive();
        FirebaseMgr.SaveProjectRegSelectedUsers(ProjectRegisterEditMgr.ReturnProjectRegister(), ProjectRegisterEditMgr.users, ProjectRegisterEditMgr.ReturnProjectRegister().key);
    }

    public void CompleteMyActivePPnB()
    {
        FirebaseMgr.CompleteMyActivePPnB(MyCurPPnbItem.Key);
        CloseMyPPnBItemInstance();
    }

    #endregion

    #region My Completed PP&Bs Methods

    //TODO: My Completed Methods

    public void RegisterMyCompletedPPnB()
    {
        GoToProjectRegisterMyCompleted();
        FirebaseMgr.GetFromExistingProjectReg(MyCurPPnbItem.Key);
    }

    public void SaveMyCompletedProjectRegister()
    {
        BackFromProjectRegisterMyCompleted();
        FirebaseMgr.SaveProjectRegSelectedUsers(ProjectRegisterEditMgr.ReturnProjectRegister(), ProjectRegisterEditMgr.users, ProjectRegisterEditMgr.ReturnProjectRegister().key);  
    }

    public void EvaluateMyCompletedPPnB()
    {
        GoToMyCompletedEvaFormEdit();
        FirebaseMgr.GetFromExistingEvaluationForm(MyCurPPnbItem.Key, false);
    }

    public void SaveMyCompletedPPnBEvaForm()
    {
        BackFromMyCompletedEvaFormEdit();
        FirebaseMgr.SaveDraftEvaluationForm(EvaFormEditMgr.GetEvaFormValue(), EvaFormEditMgr.GetEvaFormValue().key);
    }

    public void SubmitMyCompletedPPnBEvaForm()
    {
        BackFromMyCompletedEvaFormEdit();
        FirebaseMgr.SubmitDraftEvaluationFormToSec(EvaFormEditMgr.GetEvaFormValue(), EvaFormEditMgr.GetEvaFormValue().key);
    }

    public void ReadMyEvaluatedMyCompletedPPnB()
    {
        GoToMyCompletedPPnBEvaformRead();
        FirebaseMgr.GetFromExistingEvaluationForm(MyCurPPnbItem.Key, false);
    }

    public void BackFromMyCompletedEvaluationForm()
    {
        BackFromMyCompletedEvaformRead();
    }

    #endregion

    #region Secretary Evaluation Forms Applications

    public void EvaluationSecEvaForm()
    {
        GoToSecEvaFormEdit();
        FirebaseMgr.GetFromExistingEvaluationForm(CurSecEvaForm.Key, true);
        FirebaseMgr.GetFromExistingProjectReg(CurSecEvaForm.Key);
    }

    public void ReadEvaluationSecEvaForm()
    {
        GoToSecEvaFormRead();
        FirebaseMgr.GetFromExistingEvaluationForm(CurSecEvaForm.Key, true);
        FirebaseMgr.GetFromExistingProjectReg(CurSecEvaForm.Key);
    }

    public void SaveSecEvaForm()
    {
        BackFromSecEvaFormEdit();
        FirebaseMgr.SaveSecretaryEvaluationForm(EvaFormEditMgr.GetEvaFormValue().key, EvaFormEditMgr.GetEvaFormValue());
    }
    
    public void LogSecEvaForm()
    {
        BackFromSecEvaFormEdit();
        FirebaseMgr.LogUserAnalyticsFromEvaForm(EvaFormEditMgr.GetEvaFormValue().key, ProjectRegisterEditMgr.users, EvaFormEditMgr.GetEvaFormValue());
    }

    #endregion
}
