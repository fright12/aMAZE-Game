using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Choices")]
[System.Serializable]
public class Choices : ScriptableObject {
	public item[] array;

	public int getIndex(string name){
		for (int i = 0; i < array.Length; i++)
			if (array [i].obj.name.ToLower () == name.ToLower ())
				return i;

		return 0;
	}
}