using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using CatanApp;

public static class diceSideValues
{
    public static List<int> sideValues = new List<int>();

    public static bool _dicerolled = false;

}

public class Dice : MonoBehaviour {

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;
    

	// Use this for initialization
	private void Start () {

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

	}
	
    // If the object is clicked then RollTheDice coroutine is started
    public void Clicked()
    {
        StartCoroutine("RollTheDice");
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            string find = $"Side{randomDiceSide + 1}";
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"DiceSides/{find}");

            // Pause before next iteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side 
        finalSide = randomDiceSide + 1;

        // Add final dice value to diceSideValues
        diceSideValues.sideValues.Add(finalSide);
        diceSideValues._dicerolled = true;
    }
}
