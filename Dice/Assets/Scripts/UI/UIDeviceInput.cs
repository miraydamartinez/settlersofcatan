using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all point and click interfaces
// This class implements a simple mouse and keyboard UI
// Devices supporting user input override this class
public abstract class UIDeviceInput : MonoBehaviour
{

    // Based on the current device state, compute whether the user 
    // is pointing towards a Transform in the scene
    // returns: the transform pointed to; or null if none is found
    public abstract Transform ComputeHitObject();

    // Based on the current device state, compute whether the user 
    // is pressing a button. 
    // returns: true if a button is pressed; false otherwise
    public abstract bool ButtonPressed();
}