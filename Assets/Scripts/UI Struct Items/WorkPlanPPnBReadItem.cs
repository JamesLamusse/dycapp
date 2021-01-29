using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPlanPPnBReadItem : MonoBehaviour
{
    public Text WhatInput;
    public Text WhoInput;
    public Text WhenInput;
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
