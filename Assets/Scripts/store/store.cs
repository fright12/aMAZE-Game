using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class store : MonoBehaviour {
    public static ArrayList bought = new ArrayList() {"Vincent", "pavement pattern 14", "pavement pattern 16", "Sunny1 Skybox" };
    public static int money = 0;

	public Choices players;
	public Choices materials;
	public Choices skyboxes;

	private static object instance;

	void Awake() {
        instance = this;
    }

    public static bool buy(item itm) {
        if (money > itm.price) {
            money -= itm.price;
            bought.Add(itm.obj.name);
            return true;
        }

        return false;
    }

	public static item findItem (string name) {
		FieldInfo[] fields = instance.GetType().GetFields ();

		for (int i = 0; i < fields.Length; i++){
            if (fields[i].FieldType == typeof(Choices))
            {
                foreach (item itm in ((Choices)fields[i].GetValue(instance)).array)
                {
                    if (itm.obj.name.ToLower() == name.ToLower())
                        return itm;
                }
            }
		}

        return new item ();
	}

    public static object findObj(string name) {
        return findItem(name).obj;
    }
}