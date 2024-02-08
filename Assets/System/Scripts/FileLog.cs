using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileLog 
{
    public static void CrashErrorHandler(string errorText, bool bIsCritical)
    {
        if(bIsCritical)
            Application.Quit();
    }
}
