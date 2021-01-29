using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPlanEvaFormItem : MonoBehaviour
{
    public InputField WhatInput;
    public InputField WhoInput;
    public InputField WhenInput;
    public InputField CompleteDateInput;
    public Text WhatTitle;
    public Text WhoTitle;
    public Text WhenTitle;
    public Text CompleteDateTitle;

    private void Awake()
    {
        WhatTitle.text = "What";
        WhoTitle.text = "Who";
        WhenTitle.text = "When";
        CompleteDateTitle.text = "Complete By";
    }
}
