using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCompletedPPnBItem : MonoBehaviour
{
    public string EvaluationFormStatus;
    public string RawStatus;
    public string CompletedDate;
    
    // Start is called before the first frame update
    public void LoadFromCompletedPPnB(CompletedPPnB completed)
    {
        CompletedDate = completed.completeddate;
        switch(completed.evaluationformstatus)
        {
            case ("draft"):
                EvaluationFormStatus = "Needs Evaluation";
                break;
            case ("pending"):
                EvaluationFormStatus = "Pending Evaluation";
                break;
            case ("logged"):
                EvaluationFormStatus = "Evaluated";
                break;
        }
        RawStatus = completed.evaluationformstatus;
        GetComponent<MyPPnBItem>().EvaluationText.text = EvaluationFormStatus;
    }
}
