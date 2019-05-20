using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] Sides;
    private SpriteRenderer rend;
    int _die1;
    int _die2;
    Random _random = new Random();
    private UIDeviceInput _input;
    // public Button _roll;

    // Start is called before the first frame update
    void Start()
    {
        // _input = GetComponent<UIDeviceInput>();
        rend = GetComponent<SpriteRenderer>();
        Sides = Resources.LoadAll<Sprite>("DiceSides/");
        Debug.Log(Sides.Length);
    }

    private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        int randomDiceSide = 0; 

        int finalSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0,5);
            rend.sprite = Sides[randomDiceSide];
            yield return new WaitForSeconds(.05f);
        }
        finalSide = randomDiceSide +1;
        Debug.Log(finalSide);

    }

    // // Update is called once per frame
    // void Update()
    // {
    //     Transform transf = _input.ComputeHitObject();

    //     bool clicked = _input.ButtonPressed();

    //     if (clicked && transf.getComponent<>());

    // }

    // Just use this as function within Unity.cs
    // Put each int as part of a textmesh for objects in scene
    // public int getRoll()
    // {
    //     int roll = 0;
    //     for (int i = 0; i < 15; i++)
    //     {
    //         int num = Random.Range(1,6);
    //         roll = num;
    //         rend.sprite = Sides[num - 1];
    //     }

    //     return roll;
    // }
}
