using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements visualizing a settlement or city when
// clicked on by a player and the object is valid for purchasing or
// selecting.
// Use the property, Selection, to select whether an object is selected or not
// Use the property, location, to store the location of the object
public class SettlementVisualizerState: MonoBehaviour
{
   bool _selected = false;
   GameObject _settlement;
   
   string selectedColor;

   Dictionary<string, Color> colormap = new Dictionary<string, Color>();

  
   //-----------------------------
   //  Public interface
   //-----------------------------
   public GameObject Location
   {
       get { return _settlement; }
       set { _settlement = value; }
   }
   public string PlayerColor
   {
       get {return selectedColor; }
       set { selectedColor = value; }
   }
   public bool Selected
   {
       get { return _selected; }
       set
       {
           if (_selected != value)
           {
               _selected = value;
           }
       }
   }


   //-----------------------------
   //  Private interface
   //-----------------------------
    void Start ()
   {
       // insert all colors into dictionary
       colormap["blue"] = Color.blue;
       colormap["red"] = Color.red;
       colormap["green"] = Color.green;
       colormap["yellow"] = Color.yellow;

       _settlement = transform.gameObject;
       UpdateSelectedAppearance();
    }
    
    void Update ()
   {
       if (_selected)
       {
           Color unitycolor = colormap[selectedColor];
           SetColor(unitycolor);
       }
    }
   void UpdateSelectedAppearance()
   {
       if (_selected)
       {
           Color unitycolor = colormap[selectedColor];
           SetColor(unitycolor);
       }
   }

   public void UpdateCityColor()
   {
       if (_selected)
       {
           Color unitycolor = colormap[selectedColor] * 0.5f;
           SetColor(unitycolor);
       }
   }
   void SetColor(Color c)
   {
       if (_settlement != null)
       {
           _settlement.GetComponent<Renderer>().material.color = c;    
       }
   }
}