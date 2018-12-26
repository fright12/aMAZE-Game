using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class dataFile {
	public object instance;

	public static string directory;
	private string pathName;

	public dataFile (object inst, string fileName){
		instance = inst;
		pathName = directory + fileName;

        set(inst, load());
	}

	public Hashtable load (){
		if (File.Exists (pathName)) {
			try {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (pathName, FileMode.Open); //Open the file with the data
				
				Hashtable data = (Hashtable)bf.Deserialize (file); //Change the data from binary to something that can be read by Unity
				file.Close ();
				
				return data;
			} catch {
				Debug.Log ("Error reading from file " + pathName);
			}
		}
		
		return new Hashtable ();
	}
	
	public void save (){
		BinaryFormatter bf = new BinaryFormatter(); //Create a binary formatter to change data to binary
		FileStream file = File.Create (pathName); //Open a file to the path where data will be saved
		//Debug.Log (get);
		bf.Serialize(file, get(instance)); //Convert the data to binary and save
		
		file.Close(); //Close the file
	}

	public static void set(object instance, Hashtable data){
		foreach (FieldInfo f in instance.GetType ().GetFields ()) {
            //if (f.Name == "wallString" || f.Name == "groundString" || f.Name == "skyboxString")
            //  continue;

            if (f.IsStatic && f.IsPublic && data.ContainsKey(f.Name)){
				f.SetValue (instance, data[f.Name]);
			}
		}
	}

	public static Hashtable get(object instance) { //Save data of type 'type' with file extension 'extension'
		Hashtable data = new Hashtable();
		
		foreach (FieldInfo f in instance.GetType().GetFields ()) {
            //if (f.Name == "p" || f.Name == "s" || f.Name == "current" || f.Name == "color" || f.Name == "mazeToUse")continue;
            //if (f.Name == "wallString" || f.Name == "groundString" || f.Name == "skyboxString")
              //  continue;

			if (f.IsStatic && f.IsPublic && f.FieldType != instance.GetType ()) {
				data.Add (f.Name, f.GetValue (instance));

				/*if (typesUnityCantSerialize.Contains (value.GetType ().Name)){
					MethodInfo m = current.GetType().GetMethod ("myColor");
					m.Invoke (m, new object[]{value});*/
			}
		}
		
		return data;
	}
}