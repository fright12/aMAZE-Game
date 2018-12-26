using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Field)]
public class save : Attribute {
	public save(){
		Debug.Log ("test");
	}

    /*void OnApplicationFocus(bool status)
    {
        // Retreive the fields from the mono instance
        //foreach (UnityEngine.Object mb in UnityEngine.Object.FindObjectsOfType<UnityEngine.Object>()) {

        foreach (MonoBehaviour mb in Object.FindObjectsOfType<MonoBehaviour>())
        {
            FieldInfo[] fields = mb.GetType().GetFields();

            // search all fields and find the attribute [Position]
            for (int i = 0; i < fields.Length; i++)
            {
                save attribute = System.Attribute.GetCustomAttribute(fields[i], typeof(save)) as save;

                if (attribute != null)
                {
                    //print ("-----------" + fields[i].GetValue (mb).GetType ().BaseType);
                    /*foreach (FieldInfo f in fields[i].GetValue (mb).GetType ().GetFields()){
						print (fields[i].Name +", "+ f.Name);
					}

                    //Debug.Log ("SDFSD SDFD " + fields [i].Name); // The name of the marked variable - "node"
                    /*if (status && userPrefs.values.ContainsKey(fields[i].Name))
						fields[i].SetValue (userPrefs.get, userPrefs.values[fields[i].Name]);
					else
						userPrefs.values[fields[i].Name] = fields[i].GetValue(userPrefs.get);
                }

                // if we detect any attribute print out the data.
                /*if (attribute != null) {
					Debug.Log (attribute.position.x); // The attribute position Y for that instance variable "1"
					Debug.Log (attribute.position.y); // The attribute position X for that instance variable "0"
					//Debug.Log (mono.GetType ()); // The type of the mono script "Node"
					Debug.Log (objectFields [i].Name); // The name of the marked variable - "node"
				}
            }
        }
        Resources.LoadAll("Scripts");
    }*/
}