using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class cameraControl : MonoBehaviour {
	public Camera permanentCamera;

	new private Camera camera;

	private dragButton button;
	private Vector2 distance;
	private Vector2 size;

	public void Awake(){
        //if (GetComponentInChildren<cycle>() != null)
          //  GetComponentInChildren<cycle>().a();

		button = GetComponentInParent<dragButton> ();

		distance = permanentCamera.rect.position - toCoordinate (transform.position);
		size = permanentCamera.rect.size;
	}

	void Update(){
		if (toCoordinate (transform.position).x + button.current (distance * (button.origin.scaleRatio), distance).x > 1f){
			camera.gameObject.SetActive (false);
			return;
		} else if (!camera.gameObject.activeSelf){
			camera.gameObject.SetActive (true);
		}

		Rect position = new Rect(
			toCoordinate (transform.position) + button.current (distance * (button.origin.scaleRatio), distance),
			button.current (size * button.origin.scaleRatio, size)
		);

		if (GetComponentInParent<ScrollRect> () != null) {
            float p = 0;
            if (button.origin.position.x < -100)
                p = button.percent;
            else
                p = 1f;

            Vector2 rect = new Vector2(GetComponentInParent<ScrollRect>().transform.position.y / Screen.height - GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().rect.height * p / 2f / controller.canvas.rect.height, GetComponentInParent<ScrollRect>().transform.position.y / Screen.height + GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().rect.height * p / 2f / controller.canvas.rect.height);

            if (position.y < rect.x)
            {
                position.y = rect.x;
                position.height = Mathf.Max(0f, size.y * p - rect.x + toCoordinate(transform.position).y + distance.y * p);
            }
            else if (position.y + size.y * p > rect.y)
            {
                position.height = Mathf.Max(0f, rect.y - (toCoordinate(transform.position).y + distance.y * p));
            }
            else if (position.y > rect.y)
            {
                position.y = rect.y;
                position.height = 0f;
            }
		}

		camera.rect = position;
	}

	public void OnEnable(){
		camera = ((GameObject)GameObject.Instantiate (permanentCamera.gameObject, permanentCamera.gameObject.transform.position, permanentCamera.gameObject.transform.rotation)).GetComponent<Camera> ();
		camera.gameObject.SetActive (true);
	}

	void OnDisable(){
		if (camera != null)
			GameObject.Destroy(camera.gameObject);
	}

	public Vector2 toCoordinate(Vector2 point){
		return new Vector2 (point.x / Screen.width, point.y / Screen.height);
	}
}