using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPnB
{
    public string projectname;
    public string committee;
    public string ppnbsubmissiondate;
    public string projectstartdate;
    public string projectenddate;
    public string venue;
    public string budgetrequired;
    public string projectmanager;
    public string contactnumberpm;
    public string emailaddresspm;
    public string committeemember;
    public string contactnumbercm;

    public string directorsignature;
    public string directorsignaturedate;
    public string directorsignaturecomment;

    public string secretarysignature;
    public string secretarysignaturedate;
    public string secretarysignaturecomment;

    public string treasurersignature;
    public string treasurersignaturedate;
    public string treasurersignaturecomment;

    public string deputymayorsignature;
    public string deputymayorsignaturedate;
    public string deputymayorsignaturecomment;

    public string mayorsignature;
    public string mayorsignaturedate;
    public string mayorsignaturecomment;

    public string projectdescription;
    public string backgroundsituationanalysis;
    public string projectobjectives;
    public string riskmanagement;

    public string projectworkplanwhat1;
    public string projectworkplanwho1;
    public string projectworkplanwhen1;

    public string projectworkplanwhat2;
    public string projectworkplanwho2;
    public string projectworkplanwhen2;

    public string projectworkplanwhat3;
    public string projectworkplanwho3;
    public string projectworkplanwhen3;

    public string projectworkplanwhat4;
    public string projectworkplanwho4;
    public string projectworkplanwhen4;

    public string projectworkplanwhat5;
    public string projectworkplanwho5;
    public string projectworkplanwhen5;

    public string marketingphotographer;
    public string marketingprojectname;
    public string marketingcommittee;
    public string marketingprojectmanager;

    public string projectprogram;

    public string owner;
    public string currentuser;
    public string currentsecuser;
    public string key;
    public string declinedrequestfixcomment;
    public string declinedrequestfixuser;
    public int numberofevaopens;

    public PPnB()
    {

    }

    public PPnB(string ProjectName, string Committee, string PPnBSubmissionDate, string ProjectStartDate, string ProjectEndDate, string Venue, string BudgetRequired
        , string ProjectManager, string ContactNumberPM, string EmailAddressPM, string CommitteeMember, string ContactNumberCM, string DirectorSignature
        , string DirectorSignatureDate, string DirectorSignatureComment, string SecretarySignature, string SecretarySignatureDate, string SecretarySignatureComment
        , string TreasurerSignature, string TreasurerSignatureDate, string TreasurerSignatureComment, string DeputyMayorSignature, string DeputyMayorSignatureDate
        , string DeputyMayorSignatureComment, string MayorSignature, string MayorSignatureDate, string MayorSignatureComment, string ProjectDescription
        , string BackgroundSituationAnalysis, string ProjectObjectives, string RiskManagement, string ProjectWorkPlanWhat1, string ProjectWorkPlanWho1
        , string ProjectWorkPlanWhen1, string ProjectWorkPlanWhat2, string ProjectWorkPlanWho2
        , string ProjectWorkPlanWhen2, string ProjectWorkPlanWhat3, string ProjectWorkPlanWho3
        , string ProjectWorkPlanWhen3, string ProjectWorkPlanWhat4, string ProjectWorkPlanWho4
        , string ProjectWorkPlanWhen4, string ProjectWorkPlanWhat5, string ProjectWorkPlanWho5
        , string ProjectWorkPlanWhen5, string MarketingPhotographer, string MarketingProjectName, string MarketingCommittee, string MarketingProjectManager
        , string ProjectProgram, string Owner, string CurrentUser, string Key, string DeclinedRequestFixComment, string DeclinedRequestFixUser
        , int NumberOfEvaOpens, string currentsecuser)
    {
        this.projectname = ProjectName;
        this.committee = Committee;
        this.ppnbsubmissiondate = PPnBSubmissionDate;
        this.projectstartdate = ProjectStartDate;
        this.projectenddate = ProjectEndDate;
        this.venue = Venue;
        this.budgetrequired = BudgetRequired;
        this.projectmanager = ProjectManager;
        this.contactnumberpm = ContactNumberPM;
        this.emailaddresspm = EmailAddressPM;
        this.committeemember = CommitteeMember;
        this.contactnumbercm = ContactNumberCM;
        this.directorsignature = DirectorSignature;
        this.directorsignaturedate = DirectorSignatureDate;
        this.directorsignaturecomment = DirectorSignatureComment;
        this.secretarysignature = SecretarySignature;
        this.secretarysignaturedate = SecretarySignatureDate;
        this.secretarysignaturecomment = SecretarySignatureComment;
        this.treasurersignature = TreasurerSignature;
        this.treasurersignaturedate = TreasurerSignatureDate;
        this.treasurersignaturecomment = TreasurerSignatureComment;
        this.deputymayorsignature = DeputyMayorSignature;
        this.deputymayorsignaturedate = DeputyMayorSignatureDate;
        this.deputymayorsignaturecomment = DeputyMayorSignatureComment;
        this.mayorsignature = MayorSignature;
        this.mayorsignaturedate = MayorSignatureDate;
        this.mayorsignaturecomment = MayorSignatureComment;
        this.projectdescription = ProjectDescription;
        this.backgroundsituationanalysis = BackgroundSituationAnalysis;
        this.projectobjectives = ProjectObjectives;
        this.riskmanagement = RiskManagement;

        this.projectworkplanwhat1 = ProjectWorkPlanWhat1;
        this.projectworkplanwho1 = ProjectWorkPlanWho1;
        this.projectworkplanwhen1 = ProjectWorkPlanWhen1;

        this.projectworkplanwhat2 = ProjectWorkPlanWhat2;
        this.projectworkplanwho2 = ProjectWorkPlanWho2;
        this.projectworkplanwhen2 = ProjectWorkPlanWhen2;

        this.projectworkplanwhat3 = ProjectWorkPlanWhat3;
        this.projectworkplanwho3 = ProjectWorkPlanWho3;
        this.projectworkplanwhen3 = ProjectWorkPlanWhen3;

        this.projectworkplanwhat4 = ProjectWorkPlanWhat4;
        this.projectworkplanwho4 = ProjectWorkPlanWho4;
        this.projectworkplanwhen4 = ProjectWorkPlanWhen4;

        this.projectworkplanwhat5 = ProjectWorkPlanWhat5;
        this.projectworkplanwho5 = ProjectWorkPlanWho5;
        this.projectworkplanwhen5 = ProjectWorkPlanWhen5;

        this.marketingphotographer = MarketingPhotographer;
        this.marketingprojectname = MarketingProjectName;
        this.marketingcommittee = MarketingCommittee;
        this.marketingprojectmanager = MarketingProjectManager;

        this.projectprogram = ProjectProgram;

        this.owner = Owner;
        this.currentuser = CurrentUser;
        this.key = Key;
        this.currentsecuser = currentsecuser;

        this.declinedrequestfixcomment = DeclinedRequestFixComment;
        this.declinedrequestfixuser = DeclinedRequestFixUser;
        this.numberofevaopens = NumberOfEvaOpens;
    }

}
