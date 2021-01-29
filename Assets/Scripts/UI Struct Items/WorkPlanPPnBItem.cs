using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPlanPPnBItem : MonoBehaviour
{
    public InputField WhatInput;
    public InputField WhoInput;
    public InputField WhenInput;
    public Text WhatTitle;
    public Text WhoTitle;
    public Text WhenTitle;

    private void Awake()
    {
        WhatTitle.text = "What";
        WhoTitle.text = "Who";
        WhenTitle.text = "When";
    }
}
