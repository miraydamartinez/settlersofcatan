using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements the mouse and keyboard device
public class UIMouseandKeyboard : UIDeviceInput 
{
    public override Transform ComputeHitObject()
	{
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform;
        }
        return null;
	}

    public override bool ButtonPressed()
	{
		if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
	}
}
