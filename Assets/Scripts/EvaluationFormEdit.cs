using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationFormEdit : MonoBehaviour
{
    public Text HeaderText;
    public Text ProjectName;
    public Text ProjectManager;
    public Text Committee;
    public BasicPPnBItem ProjectDurationHours;
    public BasicPPnBItem NumberOfCouncilors;
    public BasicPPnBItem MediaCoverageReceived;
    
    public ToggleEvaFormItem CompletedByProposedDate;
    public ToggleEvaFormItem CompletedWithinBudget;
    public ToggleEvaFormItem CompletedObjectivesMet;

    public IntEvaFormItem TotalIncome;
    public IntEvaFormItem TotalExpenses;
    public IntEvaFormItem NetProfit;

    public BasicPPnBItem ProjectManagerComments;

    public WorkPlanEvaFormItem WorkPlanEva1;
    public WorkPlanEvaFormItem WorkPlanEva2;
    public WorkPlanEvaFormItem WorkPlanEva3;
    public WorkPlanEvaFormItem WorkPlanEva4;
    public WorkPlanEvaFormItem WorkPlanEva5;

    public string Key;
    public string Owner;
    public string CurrentSecUser;
    public string PPnBKey;

    public GameObject DraftEvaFormTopMenu;
    public GameObject SecEvaFormTopMenu;
    public GameObject CompletedReadEvaformTopMenu;
    public GameObject CompletedReadSecEvaformTopMenu;

    void Awake()
    {
        HeaderText.text = "Evaluation Form";

        ProjectDurationHours.TitleText.text = "Duration (hours)";
        NumberOfCouncilors.TitleText.text = "Number of councilors:";
        MediaCoverageReceived.TitleText.text = "Media coverage received:";

        CompletedByProposedDate.TitleText.text = "Completed by proposed date?";
        CompletedWithinBudget.TitleText.text = "Completed within budget?";
        CompletedObjectivesMet.TitleText.text = "Completed with objectives met?";

        TotalIncome.TitleText.text = "Total Income:";
        TotalExpenses.TitleText.text = "Total Expenses:";
        NetProfit.TitleText.text = "Net Profit:";

        TotalIncome.FieldInput.text = "0";
        TotalExpenses.FieldInput.text = "0";
        NetProfit.FieldInput.text = "0";

        ProjectManagerComments.TitleText.text = "Project Manager Comments:";
    }

    public void LoadMinerDetailsFromPPnB(PPnB ppnb)
    {
        ProjectName.text = ppnb.projectname;
        Committee.text = ppnb.committee;
        ProjectManager.text = ppnb.projectmanager;

        Owner = ppnb.owner;
        Key = ppnb.key;
        PPnBKey = ppnb.key;
    }

    public void LoadEvaForm(EvaluationForm EvaForm)
    {
        ProjectName.text = EvaForm.projectname;
        Committee.text = EvaForm.committee;
        ProjectManager.text = EvaForm.projectmanager;
        ProjectDurationHours.FieldInput.text = EvaForm.projectdurationhours;
        NumberOfCouncilors.FieldInput.text = EvaForm.numberofcouncilorsattended;
        MediaCoverageReceived.FieldInput.text = EvaForm.mediacoveragereceived;

        CompletedByProposedDate.FieldToggle.isOn = EvaForm.completedbyproposeddate;
        CompletedWithinBudget.FieldToggle.isOn = EvaForm.completedwithinbudget;
        CompletedObjectivesMet.FieldToggle.isOn = EvaForm.completedobjectivesmet;

        TotalIncome.FieldInput.text = EvaForm.totalincome.ToString();
        TotalExpenses.FieldInput.text = EvaForm.totalexpenses.ToString();
        NetProfit.FieldInput.text = EvaForm.netprofit.ToString();

        ProjectManagerComments.FieldInput.text = EvaForm.projectmanagercomments;

        WorkPlanEva1.WhatInput.text = EvaForm.workplanevawhat1;
        WorkPlanEva2.WhatInput.text = EvaForm.workplanevawhat2;
        WorkPlanEva3.WhatInput.text = EvaForm.workplanevawhat3;
        WorkPlanEva4.WhatInput.text = EvaForm.workplanevawhat4;
        WorkPlanEva5.WhatInput.text = EvaForm.workplanevawhat5;

        WorkPlanEva1.WhoInput.text = EvaForm.workplanevawho1;
        WorkPlanEva2.WhoInput.text = EvaForm.workplanevawho2;
        WorkPlanEva3.WhoInput.text = EvaForm.workplanevawho3;
        WorkPlanEva4.WhoInput.text = EvaForm.workplanevawho4;
        WorkPlanEva5.WhoInput.text = EvaForm.workplanevawho5;

        WorkPlanEva1.WhenInput.text = EvaForm.workplanevawhen1;
        WorkPlanEva2.WhenInput.text = EvaForm.workplanevawhen2;
        WorkPlanEva3.WhenInput.text = EvaForm.workplanevawhen3;
        WorkPlanEva4.WhenInput.text = EvaForm.workplanevawhen4;
        WorkPlanEva5.WhenInput.text = EvaForm.workplanevawhen5;

        WorkPlanEva1.CompleteDateInput.text = EvaForm.workplanevadate1;
        WorkPlanEva2.CompleteDateInput.text = EvaForm.workplanevadate2;
        WorkPlanEva3.CompleteDateInput.text = EvaForm.workplanevadate3;
        WorkPlanEva4.CompleteDateInput.text = EvaForm.workplanevadate4;
        WorkPlanEva5.CompleteDateInput.text = EvaForm.workplanevadate5;

        Key = EvaForm.key;
        Owner = EvaForm.owner;
        CurrentSecUser = EvaForm.currentsecuser;
        PPnBKey = EvaForm.ppnbkey;
    }

    public EvaluationForm GetEvaFormValue()
    {
        return new EvaluationForm(ProjectName.text, Committee.text, ProjectManager.text,
            ProjectDurationHours.FieldInput.text, NumberOfCouncilors.FieldInput.text,
            MediaCoverageReceived.FieldInput.text, CompletedByProposedDate.FieldToggle.isOn,
            CompletedWithinBudget.FieldToggle.isOn, CompletedObjectivesMet.FieldToggle.isOn,
            int.Parse(TotalIncome.FieldInput.text), int.Parse(TotalExpenses.FieldInput.text),
            int.Parse(NetProfit.FieldInput.text),
            ProjectManagerComments.FieldInput.text, 
            WorkPlanEva1.WhatInput.text,
            WorkPlanEva1.WhoInput.text,
            WorkPlanEva1.WhenInput.text,
            WorkPlanEva1.CompleteDateInput.text,

            WorkPlanEva2.WhatInput.text,
            WorkPlanEva2.WhoInput.text,
            WorkPlanEva2.WhenInput.text,
            WorkPlanEva2.CompleteDateInput.text,

            WorkPlanEva3.WhatInput.text,
            WorkPlanEva3.WhoInput.text,
            WorkPlanEva3.WhenInput.text,
            WorkPlanEva3.CompleteDateInput.text,

            WorkPlanEva4.WhatInput.text,
            WorkPlanEva4.WhoInput.text,
            WorkPlanEva4.WhenInput.text,
            WorkPlanEva4.CompleteDateInput.text,

            WorkPlanEva5.WhatInput.text,
            WorkPlanEva5.WhoInput.text,
            WorkPlanEva5.WhenInput.text,
            WorkPlanEva5.CompleteDateInput.text,
            Owner, CurrentSecUser, Key, PPnBKey);
    }
}
