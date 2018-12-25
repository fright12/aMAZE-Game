using UnityEngine;
using System.Collections;

public class computerMovement : MonoBehaviour {
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	
	private Animator anim;
	private hashIDs hash;

	private float horizontal, vertical;
	private double times = 0;
	private int [,] maze;
	private bool deadEnd = false;

	ArrayList choices = new ArrayList ();

	Vector3 pTransform;

	void Start() {
		anim = GetComponent<Animator> ();
		hash = GameObject.Find ("GameController").GetComponent<hashIDs> ();

		//print (allMazes.current.use [0, 0]);
		maze = new int[allMazes.current.use.GetUpperBound(0)+1, allMazes.current.use.GetUpperBound (1)+1];
		//print (allMazes.current.use.GetUpperBound (0)+", "+ maze.GetUpperBound (0)+", "+allMazes.current.use.GetUpperBound (1)+", "+ maze.GetUpperBound (1));
		for (int i = 0; i<allMazes.current.use.GetUpperBound (0)+1; i++)
			for (int j = 0; j<allMazes.current.use.GetUpperBound (1)+1; j++)
				maze[i,j] = allMazes.current.use[i,j];
	}
	
	void FixedUpdate(){
		if (gameController.playing){
			times+=.04;

			//print (Mathf.Rad2Deg*(gameObject.transform.rotation.y));
			//transform.position = Vector3.Lerp (transform.position, new Vector3(Mathf.Round(transform.position.x/settings.current.wallSpacing)*settings.current.wallSpacing,transform.position.y,Mathf.Round(transform.position.z/settings.current.wallSpacing)*settings.current.wallSpacing), 1f);

			if (times>=1){
				times = 0;

				transform.position = new Vector3(Mathf.Round(transform.position.x/settings.current.wallSpacing)*settings.current.wallSpacing,transform.position.y,Mathf.Round(transform.position.z/settings.current.wallSpacing)*settings.current.wallSpacing);

				int x = (int)((transform.position.x + maze.GetUpperBound (1)*settings.current.wallSpacing/2) / settings.current.wallSpacing);
				int y = (int)((transform.position.z - maze.GetUpperBound (0)*settings.current.wallSpacing/2) / -settings.current.wallSpacing);
				
				if (maze[y+1,x]!=1&&vertical!=1){choices.Add (2);}//0 degrees
				if (maze[y,x-1]!=1&&horizontal!=1){choices.Add (4);}//270 degrees
				if (maze[y-1,x]!=1&&vertical!=-1){choices.Add (1);}//180 degrees
				if (maze[y,x+1]!=1&&horizontal!=-1){choices.Add (3);}//90 degrees

				//if (choices.Count>1){Debug.Log (choices.Count);}
			
				//Debug.Log (choices[0]+", "+choices[1]+", "+choices[2]);
				if (choices.Count>0){
					if (deadEnd==true&&choices.Count>1){
						//Debug.Log ((int)((pTransform.x + d1) / settings.wallSpacing)+", "+(int)((pTransform.z - d2) / -settings.wallSpacing));
						
						maze[(int)((pTransform.z - maze.GetUpperBound (0)*settings.current.wallSpacing/2) / -settings.current.wallSpacing),(int)((pTransform.x + maze.GetUpperBound (1)*settings.current.wallSpacing/2) / settings.current.wallSpacing)]=1;
						
						//wall.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
						//wall.transform.position = pTransform;
						//Instantiate(wall);
						
						deadEnd = false;
					}
					
					int direction = (int)choices[Random.Range(0,choices.Count)];
					vertical = (float)(direction*-2+3)*Mathf.Round ((direction+1)%4/2);
					horizontal = (float)(direction*-2+7)*Mathf.Round ((direction-1)/2);

					//Debug.Log (choices.Count+", "+direction+", "+horizontal+", "+vertical);
				}else{
					if (horizontal!=0){horizontal*=-1;}
					if (vertical!=0){vertical*=-1;}
					deadEnd = true;
				}
				
				choices.Clear();
				
				pTransform = transform.position;
			}
			//if (transform.position.x>Mathf.Round (transform.position.x)-0.5f&&transform.position.x<Mathf.Round(transform.position.x)+0.5f&&
			//    transform.position.z>Mathf.Round (transform.position.z)-0.5f&&transform.position.z<Mathf.Round(transform.position.z)+0.5f){
			//	Debug.Log ("hoi'");
			//}

			//float h = 0f;
			//float v = -1f;

			movementManagement (horizontal,vertical);
		}else{
			anim.SetFloat (hash.speedFloat, 0);
		}
	}
	
	void movementManagement(float horizontal, float vertical){
		if (horizontal != 0f || vertical != 0f){
			Rotating (horizontal,vertical);
			anim.SetFloat (hash.speedFloat, 3.5f, speedDampTime, Time.deltaTime);
		}else{
			anim.SetFloat (hash.speedFloat, 0);
		}
		
	}
	
	void Rotating (float horizontal, float vertical){
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}
}
