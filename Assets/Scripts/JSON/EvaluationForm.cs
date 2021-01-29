using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationForm
{
    public string projectname;
    public string committee;
    public string projectmanager;
    public string projectdurationhours;
    public string numberofcouncilorsattended;
    public string mediacoveragereceived;

    public bool completedbyproposeddate;
    public bool completedwithinbudget;
    public bool completedobjectivesmet;

    public int totalincome;
    public int totalexpenses;
    public int netprofit;

    public string projectmanagercomments;

    public string workplanevawhat1;
    public string workplanevawho1;
    public string workplanevawhen1;
    public string workplanevadate1;

    public string workplanevawhat2;
    public string workplanevawho2;
    public string workplanevawhen2;
    public string workplanevadate2;

    public string workplanevawhat3;
    public string workplanevawho3;
    public string workplanevawhen3;
    public string workplanevadate3;

    public string workplanevawhat4;
    public string workplanevawho4;
    public string workplanevawhen4;
    public string workplanevadate4;

    public string workplanevawhat5;
    public string workplanevawho5;
    public string workplanevawhen5;
    public string workplanevadate5;
    public string owner;
    public string currentsecuser;
    public string key;
    public string ppnbkey;

    public EvaluationForm()
    {

    }

    public EvaluationForm(
        string projectname,
     string committee,
     string projectmanager,
     string projectdurationhours,
     string numberofcouncilorsattended,
     string mediacoveragereceived,
     bool completedbyproposeddate,
     bool completedwithinbudget,
     bool completedobjectivesmet,

     int totalincome,
     int totalexpenses,
     int netprofit,

     string projectmanagercomments,

     string workplanevawhat1,
     string workplanevawho1,
     string workplanevawhen1,
     string workplanevadate1,

     string workplanevawhat2,
     string workplanevawho2,
     string workplanevawhen2,
     string workplanevadate2,

     string workplanevawhat3,
     string workplanevawho3,
     string workplanevawhen3,
     string workplanevadate3,

     string workplanevawhat4,
     string workplanevawho4,
     string workplanevawhen4,
     string workplanevadate4,

     string workplanevawhat5,
     string workplanevawho5,
     string workplanevawhen5,
     string workplanevadate5, string owner,
     string currentsecuser, string key, string ppnbkey)
    {
        this.projectname = projectname;
        this.committee = committee;
        this.projectmanager = projectmanager;
        this.projectdurationhours = projectdurationhours;
        this.numberofcouncilorsattended = numberofcouncilorsattended;
        this.mediacoveragereceived = mediacoveragereceived;
        this.completedbyproposeddate = completedbyproposeddate;
        this.completedwithinbudget = completedwithinbudget;
        this.completedobjectivesmet =completedobjectivesmet;

        this.totalincome = totalincome;
        this.totalexpenses = totalexpenses;
        this.netprofit = netprofit;

        this.projectmanagercomments = projectmanagercomments;

        this.workplanevawhat1 = workplanevawhat1;  
        this.workplanevawho1 = workplanevawho1;
        this.workplanevawhen1 = workplanevawhen1;
        this.workplanevadate1 = workplanevadate1;

        this.workplanevawhat2 = workplanevawhat2;
        this.workplanevawho2 = workplanevawho2;
        this.workplanevawhen2 = workplanevawhen2;
        this.workplanevadate2 = workplanevadate2;

        this.workplanevawhat3 = workplanevawhat3;
        this.workplanevawho3 = workplanevawho3;
        this.workplanevawhen3 = workplanevawhen3;
        this.workplanevadate3 = workplanevadate3;

        this.workplanevawhat4 = workplanevawhat4;
        this.workplanevawho4 = workplanevawho4;
        this.workplanevawhen4 = workplanevawhen4;
        this.workplanevadate4 = workplanevadate4;

        this.workplanevawhat5 = workplanevawhat5;
        this.workplanevawho5 = workplanevawho5;
        this.workplanevawhen5 = workplanevawhen5;
        this.workplanevadate5 = workplanevadate5;

        this.owner = owner;
        this.key = key;
        this.currentsecuser = currentsecuser;
        this.ppnbkey = ppnbkey;
    }
}
