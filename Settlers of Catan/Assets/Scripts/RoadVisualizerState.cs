using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implements visualizing a road when clicked on by a
// player and the object is valid for purchasing or selecting.
// Use the property, Selection, to select whether an object is selected or not
// Use the property, location, to store the location of the object
public class RoadVisualizerState: MonoBehaviour
{
   bool _selected = false;
   GameObject _road;
   
   string selectedColor;

   Dictionary<string, Color> colormap = new Dictionary<string, Color>();

  
   //-----------------------------
   //  Public interface
   //-----------------------------
   public GameObject Location
   {
       get { return _road; }
       set { _road = value; }
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

       _road = transform.gameObject;
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
   void SetColor(Color c)
   {
       if (_road != null)
       {
           _road.GetComponent<Renderer>().material.color = c;    
       }
   }
}