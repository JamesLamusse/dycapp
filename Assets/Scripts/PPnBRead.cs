using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPnBRead : MonoBehaviour
{
    public Text HeaderText;

    public BasicPPnBReadItem ProjectNameField;
    public BasicPPnBReadItem CommitteeField;
    public BasicPPnBReadItem SubmissionDateField;
    public BasicPPnBReadItem ProjectStartDateField;
    public BasicPPnBReadItem ProjectEndDateField;
    public BasicPPnBReadItem VenueField;
    public BasicPPnBReadItem BudgetRequiredField;
    public BasicPPnBReadItem ProjectManagerField;
    public BasicPPnBReadItem ContactNumberPMField;
    public BasicPPnBReadItem EmailAddressPMField;
    public BasicPPnBReadItem CommitteeMemberField;
    public BasicPPnBReadItem ContactNumberCMField;

    public SignaturePPnBItem DirectorSignatureField;
    public SignaturePPnBItem SecretarySignatureField;
    public SignaturePPnBItem TreasurerSignatureField;
    public SignaturePPnBItem DeputyMayorSignatureField;
    public SignaturePPnBItem MayorSignatureField;

    public BasicPPnBReadItem ProjectDescriptionField;
    public BasicPPnBReadItem BackgroundSituationAnalysisField;
    public BasicPPnBReadItem ProjectObjectivesField;
    public BasicPPnBReadItem RiskManagementField;

    public WorkPlanPPnBReadItem WorkPlan1;
    public WorkPlanPPnBReadItem WorkPlan2;
    public WorkPlanPPnBReadItem WorkPlan3;
    public WorkPlanPPnBReadItem WorkPlan4;
    public WorkPlanPPnBReadItem WorkPlan5;

    public BasicPPnBReadItem MarketingPhotographerField;
    public BasicPPnBReadItem MarketingProjectNameField;
    public BasicPPnBReadItem MarketingCommitteeField;
    public BasicPPnBReadItem MarketingProjectManagerField;

    public BasicPPnBReadItem ProjectProgramField;

    public string Owner;
    public string CurrentUser;
    public string Key;
    public string DeclinedRequestFixComment;
    public string DeclinedRequestFixUser;
    public int NumberOfEvaOpens;
    public string CurrentSecUser;

    public GameObject DraftPPnBTopMenu;
    public GameObject PendingPPnBTopMenu;
    public GameObject MyDeclinedPPnBTopMenu;
    public GameObject MyPPnBHeader;

    public void OpenPPnBRead(string status, bool isOwner)
    {
        DraftPPnBTopMenu.SetActive(false);
        PendingPPnBTopMenu.SetActive(false);
        MyDeclinedPPnBTopMenu.SetActive(false);
        MyPPnBHeader.SetActive(true);

        switch (status)
        {
            case (PPnBState.DRAFT):
                DraftPPnBTopMenu.SetActive(true);
                MyPPnBHeader.SetActive(true);
                break;
            case (PPnBState.PENDING):
                if (!isOwner)
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

    private void Awake()
    {
        ProjectNameField.TitleString.text = "Project Name:";
        CommitteeField.TitleString.text = "Committee:";
        SubmissionDateField.TitleString.text = "SubmissionDate:";
        ProjectStartDateField.TitleString.text = "Start Date:";
        ProjectEndDateField.TitleString.text = "End Date:";
        VenueField.TitleString.text = "Venue:";
        BudgetRequiredField.TitleString.text = "Budget Required:";
        ProjectManagerField.TitleString.text = "Project Manager:";
        ContactNumberPMField.TitleString.text = "Contact Number (PM):";
        EmailAddressPMField.TitleString.text = "Email Address (PM):";
        CommitteeMemberField.TitleString.text = "Committee Member:";
        ContactNumberCMField.TitleString.text = "Contact Number (CM):";

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

        ProjectDescriptionField.TitleString.text = "Project Description:";
        BackgroundSituationAnalysisField.TitleString.text = "Background Situation and Analysis:";
        ProjectObjectivesField.TitleString.text = "Project Objectives:";
        RiskManagementField.TitleString.text = "Risk Management:";

        MarketingCommitteeField.TitleString.text = "Committee:";
        MarketingPhotographerField.TitleString.text = "Photographer:";
        MarketingProjectManagerField.TitleString.text = "Project Manager:";
        MarketingProjectNameField.TitleString.text = "Project Name:";

        ProjectProgramField.TitleString.text = "Project Program:";
    }

    public void LoadPPnB(PPnB ppnb)
    {
        ProjectNameField.FieldText.text = ppnb.projectname;
        CommitteeField.FieldText.text = ppnb.committee;
        SubmissionDateField.FieldText.text = ppnb.ppnbsubmissiondate;
        ProjectStartDateField.FieldText.text = ppnb.projectstartdate;
        ProjectEndDateField.FieldText.text = ppnb.projectenddate;
        VenueField.FieldText.text = ppnb.venue;
        BudgetRequiredField.FieldText.text = ppnb.budgetrequired;
        ProjectManagerField.FieldText.text = ppnb.projectmanager;
        ContactNumberPMField.FieldText.text = ppnb.contactnumberpm;
        EmailAddressPMField.FieldText.text = ppnb.emailaddresspm;
        CommitteeMemberField.FieldText.text = ppnb.committeemember;
        ContactNumberCMField.FieldText.text = ppnb.contactnumbercm;

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

        ProjectDescriptionField.FieldText.text = ppnb.projectdescription;
        BackgroundSituationAnalysisField.FieldText.text = ppnb.backgroundsituationanalysis;
        ProjectObjectivesField.FieldText.text = ppnb.projectobjectives;
        RiskManagementField.FieldText.text = ppnb.riskmanagement;

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

        MarketingPhotographerField.FieldText.text = ppnb.marketingphotographer;
        MarketingCommitteeField.FieldText.text = ppnb.marketingcommittee;
        MarketingProjectManagerField.FieldText.text = ppnb.marketingprojectmanager;
        MarketingProjectNameField.FieldText.text = ppnb.marketingprojectname;

        ProjectProgramField.FieldText.text = ppnb.projectprogram;

        Owner = ppnb.owner;
        CurrentUser = ppnb.currentuser;
        CurrentSecUser = ppnb.currentsecuser;
        Key = ppnb.key;
        DeclinedRequestFixComment = ppnb.declinedrequestfixcomment;
        DeclinedRequestFixUser = ppnb.declinedrequestfixuser;
        NumberOfEvaOpens = ppnb.numberofevaopens;

        UIManager.openReadTime = System.DateTime.Now.Ticks;
    }

    private void Update()
    {
        HeaderText.text = ProjectNameField.FieldText.text;
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

    public PPnB GetPPnBValue()
    {
        return new PPnB(ProjectNameField.FieldText.text, CommitteeField.FieldText.text,
            SubmissionDateField.FieldText.text, ProjectStartDateField.FieldText.text,
            ProjectEndDateField.FieldText.text, VenueField.FieldText.text,
            BudgetRequiredField.FieldText.text, ProjectManagerField.FieldText.text,
            ContactNumberPMField.FieldText.text, EmailAddressPMField.FieldText.text,
            CommitteeMemberField.FieldText.text, ContactNumberCMField.FieldText.text,
            DirectorSignatureField.SignatureInput.text, DirectorSignatureField.DateTitle.text,
            DirectorSignatureField.CommentInput.text, SecretarySignatureField.SignatureInput.text, SecretarySignatureField.DateTitle.text,
            SecretarySignatureField.CommentInput.text, TreasurerSignatureField.SignatureInput.text, TreasurerSignatureField.DateTitle.text,
            TreasurerSignatureField.CommentInput.text, DeputyMayorSignatureField.SignatureInput.text, DeputyMayorSignatureField.DateTitle.text,
            DeputyMayorSignatureField.CommentInput.text, MayorSignatureField.SignatureInput.text, MayorSignatureField.DateTitle.text,
            MayorSignatureField.CommentInput.text, ProjectDescriptionField.FieldText.text,
            BackgroundSituationAnalysisField.FieldText.text,
            ProjectObjectivesField.FieldText.text,
            RiskManagementField.FieldText.text,
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
            MarketingPhotographerField.FieldText.text,
            MarketingProjectNameField.FieldText.text,
            MarketingCommitteeField.FieldText.text,
            MarketingProjectManagerField.FieldText.text,

            ProjectProgramField.FieldText.text,

            Owner,
            CurrentUser,
            Key, DeclinedRequestFixComment, DeclinedRequestFixUser, NumberOfEvaOpens, CurrentSecUser);
    }
}
