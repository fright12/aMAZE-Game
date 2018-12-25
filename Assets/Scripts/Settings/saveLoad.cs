using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class saveLoad {
	public static string directory; //The path name the files are saved at

	public static void Save(object type, string extension) { //Save data of type 'type' with file extension 'extension'
		BinaryFormatter bf = new BinaryFormatter(); //Create a binary formatter to change data to binary
		FileStream file = File.Create (directory+extension); //Open a file to the path where data will be saved
		bf.Serialize(file, type); //Convert the data to binary and save
		file.Close(); //Close the file
	}	
	
	public static object Load(object blank, string extension) { //Load data
		if (Directory.Exists ("D:/")) directory = "D:/"; //If the game is being run from a disc, save the data to the disc
		else directory = Application.persistentDataPath; //Otherwise, save the data to the path name provided by Unity

		object data = blank; //Set a new instance of the data to an empty instance of whatever type it will be; type object so it can be cast later to whatever data type is needed

		if(File.Exists(directory+extension)) { //If there is already a file created with data
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(directory+extension, FileMode.Open); //Open the file with the data
			data = bf.Deserialize(file); //Change the data from binary to something that can be read by Unity
			file.Close(); //Close the file
		}

		return data; //Return whatever was loaded from the file; if there was no file, the empty version of the data type will be returned
	}
}
