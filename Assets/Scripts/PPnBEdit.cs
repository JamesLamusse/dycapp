using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPnBEdit : MonoBehaviour
{
    public Text HeaderText;

    public BasicPPnBItem ProjectNameField;
    public BasicPPnBItem CommitteeField;
    public BasicPPnBItem SubmissionDateField;
    public BasicPPnBItem ProjectStartDateField;
    public BasicPPnBItem ProjectEndDateField;
    public BasicPPnBItem VenueField;
    public BasicPPnBItem BudgetRequiredField;
    public BasicPPnBItem ProjectManagerField;
    public BasicPPnBItem ContactNumberPMField;
    public BasicPPnBItem EmailAddressPMField;
    public BasicPPnBItem CommitteeMemberField;
    public BasicPPnBItem ContactNumberCMField;

    public SignaturePPnBItem DirectorSignatureField;
    public SignaturePPnBItem SecretarySignatureField;
    public SignaturePPnBItem TreasurerSignatureField;
    public SignaturePPnBItem DeputyMayorSignatureField;
    public SignaturePPnBItem MayorSignatureField;

    public BasicPPnBItem ProjectDescriptionField;
    public BasicPPnBItem BackgroundSituationAnalysisField;
    public BasicPPnBItem ProjectObjectivesField;
    public BasicPPnBItem RiskManagementField;

    public WorkPlanPPnBItem WorkPlan1;
    public WorkPlanPPnBItem WorkPlan2;
    public WorkPlanPPnBItem WorkPlan3;
    public WorkPlanPPnBItem WorkPlan4;
    public WorkPlanPPnBItem WorkPlan5;

    public BasicPPnBItem MarketingPhotographerField;
    public BasicPPnBItem MarketingProjectNameField;
    public BasicPPnBItem MarketingCommitteeField;
    public BasicPPnBItem MarketingProjectManagerField;

    public BasicPPnBItem ProjectProgramField;

    public string Owner;
    public string CurrentUser;
    public string CurrentSecUser;
    public string Key;
    public string DeclinedRequestFixComment;
    public string DeclinedRequestFixUser;
    public int NumberOfEvaOpens;

    public GameObject[] Pages;
    public Text CurrentPageText;
    public GameObject NextPageButton;
    public GameObject PrevPageButton;
    public GameObject DraftPPnBTopMenu;
    public GameObject PendingPPnBTopMenu;
    public GameObject MyDeclinedPPnBTopMenu;
    public GameObject MyPPnBHeader;
    public int curPage;

    private void Awake()
    {
        ProjectNameField.TitleText.text = "Project Name:";
        CommitteeField.TitleText.text = "Committee:";
        SubmissionDateField.TitleText.text = "SubmissionDate:";
        ProjectStartDateField.TitleText.text = "Start Date:";
        ProjectEndDateField.TitleText.text = "End Date:";
        VenueField.TitleText.text = "Venue:";
        BudgetRequiredField.TitleText.text = "Budget Required:";
        ProjectManagerField.TitleText.text = "Project Manager:";
        ContactNumberPMField.TitleText.text = "Contact Number (PM):";
        EmailAddressPMField.TitleText.text = "Email Address (PM):";
        CommitteeMemberField.TitleText.text = "Committee Member:";
        ContactNumberCMField.TitleText.text = "Contact Number (CM):";

        DirectorSignatureField.SignatureTitle.text = "Deputy Director:";
        DirectorSignatureField.CommentTitle.text = "Comment:";

        SecretarySignatureField.SignatureTitle.text = "Director:";
        SecretarySignatureField.CommentTitle.text = "Comment:";

        TreasurerSignatureField.SignatureTitle.text = "Secretary:";
        TreasurerSignatureField.CommentTitle.text = "Comment:";

        DeputyMayorSignatureField.SignatureTitle.text = "Treasurer:";
        DeputyMayorSignatureField.CommentTitle.text = "Comment:";

        MayorSignatureField.SignatureTitle.text = "Deputy Mayor:";
        MayorSignatureField.CommentTitle.text = "Comment:";

        ProjectDescriptionField.TitleText.text = "Project Description:";
        BackgroundSituationAnalysisField.TitleText.text = "Background Situation and Analysis:";
        ProjectObjectivesField.TitleText.text = "Project Objectives:";
        RiskManagementField.TitleText.text = "Risk Management:";

        MarketingCommitteeField.TitleText.text = "Committee:";
        MarketingPhotographerField.TitleText.text = "Photographer:";
        MarketingProjectManagerField.TitleText.text = "Project Manager:";
        MarketingProjectNameField.TitleText.text = "Project Name:";

        ProjectProgramField.TitleText.text = "Project Program:";
    }
    
    public void OpenPPnBEdit(string status, bool isOwner)
    {
        DraftPPnBTopMenu.SetActive(false);
        PendingPPnBTopMenu.SetActive(false);
        MyDeclinedPPnBTopMenu.SetActive(false);
        MyPPnBHeader.SetActive(true);

        switch (status)
        {
            case (PPnBState.DRAFT):
                DraftPPnBTopMenu.SetActive(true);
                MyPPnBHeader.SetActive(false);
                break;
            case (PPnBState.PENDING):
                if(!isOwner)
                {
                    PendingPPnBTopMenu.SetActive(true);
                    MyPPnBHeader.SetActive(false);
                }
                break;
            case (PPnBState.REQUEST_FIX):
                MyDeclinedPPnBTopMenu.SetActive(true);
                MyPPnBHeader.SetActive(false);
                break;
            case (""):
                Debug.LogError("No status - Database Error ?");
                break;
        }
    }

    public void EmptyPPnB()
    {
        ProjectNameField.FieldInput.text = "";
        CommitteeField.FieldInput.text = "";
        SubmissionDateField.FieldInput.text = "";
        ProjectStartDateField.FieldInput.text = "";
        ProjectEndDateField.FieldInput.text = "";
        VenueField.FieldInput.text = "";
        BudgetRequiredField.FieldInput.text = "";
        ProjectManagerField.FieldInput.text = "";
        ContactNumberPMField.FieldInput.text = "";
        EmailAddressPMField.FieldInput.text = "";
        CommitteeMemberField.FieldInput.text = "";
        ContactNumberCMField.FieldInput.text = "";

        DirectorSignatureField.SignatureInput.text = "";
        DirectorSignatureField.CommentInput.text = "";
        DirectorSignatureField.DateTitle.text = "";

        SecretarySignatureField.SignatureInput.text = "";
        SecretarySignatureField.CommentInput.text = "";
        SecretarySignatureField.DateTitle.text = "";

        TreasurerSignatureField.SignatureInput.text = "";
        TreasurerSignatureField.CommentInput.text = "";
        TreasurerSignatureField.DateTitle.text = "";

        DeputyMayorSignatureField.SignatureInput.text = "";
        DeputyMayorSignatureField.CommentInput.text = "";
        DeputyMayorSignatureField.DateTitle.text = "";

        MayorSignatureField.SignatureInput.text = "";
        MayorSignatureField.CommentInput.text = "";
        MayorSignatureField.DateTitle.text = "";

        ProjectDescriptionField.FieldInput.text = "";
        BackgroundSituationAnalysisField.FieldInput.text = "";
        ProjectObjectivesField.FieldInput.text = "";
        RiskManagementField.FieldInput.text = "";

        WorkPlan1.WhatInput.text = "";
        WorkPlan2.WhatInput.text = "";
        WorkPlan3.WhatInput.text = "";
        WorkPlan4.WhatInput.text = "";
        WorkPlan5.WhatInput.text = "";

        WorkPlan1.WhoInput.text = "";
        WorkPlan2.WhoInput.text = "";
        WorkPlan3.WhoInput.text = "";
        WorkPlan4.WhoInput.text = "";
        WorkPlan5.WhoInput.text = "";

        WorkPlan1.WhenInput.text = "";
        WorkPlan2.WhenInput.text = "";
        WorkPlan3.WhenInput.text = "";
        WorkPlan4.WhenInput.text = "";
        WorkPlan5.WhenInput.text = "";

        MarketingPhotographerField.FieldInput.text = "";
        MarketingCommitteeField.FieldInput.text = "";
        MarketingProjectManagerField.FieldInput.text = "";
        MarketingProjectNameField.FieldInput.text = "";

        ProjectProgramField.FieldInput.text = "";

        Owner = "";
        CurrentUser = "";
        CurrentSecUser = "";
        DeclinedRequestFixComment = "";
        DeclinedRequestFixUser = "";
        NumberOfEvaOpens = 0;

        UIManager.openEditTime = System.DateTime.Now.Ticks;
    }

    public void LoadPPnB(PPnB ppnb)
    {
        ProjectNameField.FieldInput.text = ppnb.projectname;
        CommitteeField.FieldInput.text = ppnb.committee;
        SubmissionDateField.FieldInput.text = ppnb.ppnbsubmissiondate;
        ProjectStartDateField.FieldInput.text = ppnb.projectstartdate;
        ProjectEndDateField.FieldInput.text = ppnb.projectenddate;
        VenueField.FieldInput.text = ppnb.venue;
        BudgetRequiredField.FieldInput.text = ppnb.budgetrequired;
        ProjectManagerField.FieldInput.text = ppnb.projectmanager;
        ContactNumberPMField.FieldInput.text = ppnb.contactnumberpm;
        EmailAddressPMField.FieldInput.text = ppnb.emailaddresspm;
        CommitteeMemberField.FieldInput.text = ppnb.committeemember;
        ContactNumberCMField.FieldInput.text = ppnb.contactnumbercm;

        DirectorSignatureField.SignatureInput.text = ppnb.directorsignature;
        DirectorSignatureField.CommentInput.text = ppnb.directorsignaturecomment;
        DirectorSignatureField.DateTitle.text = ppnb.directorsignaturedate;

        SecretarySignatureField.SignatureInput.text = ppnb.secretarysignature;
        SecretarySignatureField.CommentInput.text = ppnb.secretarysignaturecomment;
        SecretarySignatureField.DateTitle.text = ppnb.secretarysignaturedate;

        TreasurerSignatureField.SignatureInput.text = ppnb.treasurersignature;
        TreasurerSignatureField.CommentInput.text = ppnb.treasurersignaturecomment;
        TreasurerSignatureField.DateTitle.text = ppnb.treasurersignaturedate;

        DeputyMayorSignatureField.SignatureInput.text = ppnb.deputymayorsignature;
        DeputyMayorSignatureField.CommentInput.text = ppnb.deputymayorsignaturecomment;
        DeputyMayorSignatureField.DateTitle.text = ppnb.deputymayorsignaturedate;

        MayorSignatureField.SignatureInput.text = ppnb.mayorsignature;
        MayorSignatureField.CommentInput.text = ppnb.mayorsignaturecomment;
        MayorSignatureField.DateTitle.text = ppnb.mayorsignaturedate;

        ProjectDescriptionField.FieldInput.text = ppnb.projectdescription;
        BackgroundSituationAnalysisField.FieldInput.text = ppnb.backgroundsituationanalysis;
        ProjectObjectivesField.FieldInput.text = ppnb.projectobjectives;
        RiskManagementField.FieldInput.text = ppnb.riskmanagement;

        WorkPlan1.WhatInput.text = ppnb.projectworkplanwhat1;
        WorkPlan2.WhatInput.text = ppnb.projectworkplanwhat2;
        WorkPlan3.WhatInput.text = ppnb.projectworkplanwhat3;
        WorkPlan4.WhatInput.text = ppnb.projectworkplanwhat4;
        WorkPlan5.WhatInput.text = ppnb.projectworkplanwhat5;

        WorkPlan1.WhoInput.text = ppnb.projectworkplanwho1;
        WorkPlan2.WhoInput.text = ppnb.projectworkplanwho2;
        WorkPlan3.WhoInput.text = ppnb.projectworkplanwho3;
        WorkPlan4.WhoInput.text = ppnb.projectworkplanwho4;
        WorkPlan5.WhoInput.text = ppnb.projectworkplanwho5;

        WorkPlan1.WhenInput.text = ppnb.projectworkplanwhen1;
        WorkPlan2.WhenInput.text = ppnb.projectworkplanwhen2;
        WorkPlan3.WhenInput.text = ppnb.projectworkplanwhen3;
        WorkPlan4.WhenInput.text = ppnb.projectworkplanwhen4;
        WorkPlan5.WhenInput.text = ppnb.projectworkplanwhen5;

        MarketingPhotographerField.FieldInput.text = ppnb.marketingphotographer;
        MarketingCommitteeField.FieldInput.text = ppnb.marketingcommittee;
        MarketingProjectManagerField.FieldInput.text = ppnb.marketingprojectmanager;
        MarketingProjectNameField.FieldInput.text = ppnb.marketingprojectname;

        ProjectProgramField.FieldInput.text = ppnb.projectprogram;

        Owner = ppnb.owner;
        CurrentUser = ppnb.currentuser;
        CurrentSecUser = ppnb.currentsecuser;
        Key = ppnb.key;
        DeclinedRequestFixComment = ppnb.declinedrequestfixcomment;
        DeclinedRequestFixUser = ppnb.declinedrequestfixuser;
        NumberOfEvaOpens = ppnb.numberofevaopens;

        UIManager.openEditTime = System.DateTime.Now.Ticks;
    }

    private void Update()
    {
        HeaderText.text = ProjectNameField.FieldInput.text;

        if(curPage == 0)
        {
            PrevPageButton.SetActive(false);
        }
        else if(curPage > 0)
        {
            PrevPageButton.SetActive(true);
        }

        if (curPage == Pages.Length-1)
        {
            NextPageButton.SetActive(false);
        }
        else
        {
            NextPageButton.SetActive(true);
        }
    }

    public void OnDirectorSignatureSign()
    {
        DirectorSignatureField.DateTitle.text = System.DateTime.Now.ToShortDateString();
    }

    public void OnSecretarySignatureSign()
    {
        SecretarySignatureField.DateTitle.text = System.DateTime.Now.ToShortDateString();
    }

    public void OnTreasurerSignatureSign()
    {
        TreasurerSignatureField.DateTitle.text = System.DateTime.Now.ToShortDateString();
    }

    public void OnDeputyMayorSignatureSign()
    {
        DeputyMayorSignatureField.DateTitle.text = System.DateTime.Now.ToShortDateString();
    }
    public void OnMayorSignatureSign()
    {
        MayorSignatureField.DateTitle.text = System.DateTime.Now.ToShortDateString();
    }

    public void NextPage()
    {
        Pages[curPage].SetActive(false);
        curPage++;
        Pages[curPage].SetActive(true);

        CurrentPageText.text = (curPage + 1).ToString();
    }

    public void PrevPage()
    {
        Pages[curPage].SetActive(false);
        curPage--;
        Pages[curPage].SetActive(true);

        CurrentPageText.text = (curPage + 1).ToString();
    }

    public PPnB GetPPnBValue()
    {
        return new PPnB(ProjectNameField.FieldInput.text, CommitteeField.FieldInput.text,
            SubmissionDateField.FieldInput.text, ProjectStartDateField.FieldInput.text,
            ProjectEndDateField.FieldInput.text, VenueField.FieldInput.text,
            BudgetRequiredField.FieldInput.text, ProjectManagerField.FieldInput.text,
            ContactNumberPMField.FieldInput.text, EmailAddressPMField.FieldInput.text,
            CommitteeMemberField.FieldInput.text, ContactNumberCMField.FieldInput.text,
            DirectorSignatureField.SignatureInput.text, DirectorSignatureField.DateTitle.text,
            DirectorSignatureField.CommentInput.text, SecretarySignatureField.SignatureInput.text, SecretarySignatureField.DateTitle.text,
            SecretarySignatureField.CommentInput.text, TreasurerSignatureField.SignatureInput.text, TreasurerSignatureField.DateTitle.text,
            TreasurerSignatureField.CommentInput.text, DeputyMayorSignatureField.SignatureInput.text, DeputyMayorSignatureField.DateTitle.text,
            DeputyMayorSignatureField.CommentInput.text, MayorSignatureField.SignatureInput.text, MayorSignatureField.DateTitle.text,
            MayorSignatureField.CommentInput.text, ProjectDescriptionField.FieldInput.text,
            BackgroundSituationAnalysisField.FieldInput.text,
            ProjectObjectivesField.FieldInput.text,
            RiskManagementField.FieldInput.text,
            WorkPlan1.WhatInput.text,
            WorkPlan1.WhoInput.text,
            WorkPlan1.WhenInput.text,

            WorkPlan2.WhatInput.text,
            WorkPlan2.WhoInput.text,
            WorkPlan2.WhenInput.text,

            WorkPlan3.WhatInput.text,
            WorkPlan3.WhoInput.text,
            WorkPlan3.WhenInput.text,

            WorkPlan4.WhatInput.text,
            WorkPlan4.WhoInput.text,
            WorkPlan4.WhenInput.text,

            WorkPlan5.WhenInput.text,
            WorkPlan5.WhoInput.text,
            WorkPlan5.WhenInput.text,
            MarketingPhotographerField.FieldInput.text,
            MarketingProjectNameField.FieldInput.text,
            MarketingCommitteeField.FieldInput.text,
            MarketingProjectManagerField.FieldInput.text,

            ProjectProgramField.FieldInput.text,

            Owner,
            CurrentUser,
            Key, DeclinedRequestFixComment, DeclinedRequestFixUser,
            NumberOfEvaOpens,CurrentSecUser);
    }
}
