using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskUIFix : MonoBehaviour
{
    UnityEngine.UI.Mask mask;

    // Start is called before the first frame update
    void Start()
    {
        _AssignMask();
        EnableMask();
    }

    protected void _AssignMask()
    {
        if (!mask)
            mask = GetComponent<UnityEngine.UI.Mask>();
    }
    public void EnableMask()
    {
        if (mask && !mask.IsActive())
            mask.enabled = true;
    }
}
