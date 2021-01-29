using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAnalytics
{
    public float hours;
    public int projectssubmitted;
    public int projectsdone;
    public int projectsgoneto;
    public int councilsabsent;
    public int boardmeetingsabsent;

    public UserAnalytics()
    {

    }

    public UserAnalytics(float hours, int projectssubmitted, int projectsdone, int projectsgoneto, int councilsabsent, int boardmeetingsabsent)
    {
        this.hours = hours;
        this.projectssubmitted = projectssubmitted;
        this.projectsdone = projectsdone;
        this.projectsgoneto = projectsgoneto;
        this.councilsabsent = councilsabsent;
        this.boardmeetingsabsent = boardmeetingsabsent;
    }
}
