using UnityEngine;
using System.Collections;

[System.Serializable]
public class asd {
    public item[] choices;

	public E find<E> (string name) where E : Object {
		foreach (item itm in choices)
			if (itm.obj.name.ToLower () == name.ToLower ())
				return itm.obj as E;

		return (E) new Object ();
	}
}