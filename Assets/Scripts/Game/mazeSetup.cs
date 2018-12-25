using UnityEngine;
using System.Collections;

public class mazeSetup : MonoBehaviour {
	//public GameObject wall;
	public GameObject[] player = new GameObject[2];
	public GameObject[] location = new GameObject[2];
	public GameObject showWall;
	public GameObject ground;
	public GameObject coin;

	public Transform end;

	public Camera[] playerCamera = new Camera[2];
	public Camera mapCamera;
	
	private float wallHeight = 5;

	void Start(){
		RenderSettings.skybox = controller.get.skyboxes[settings.current.skybox];

		//if (settings.tutorial) {
		//	//player [0].AddComponent<tutorial> ();
		//	return;
		//}

		if (settings.current.mode == "multi" || settings.current.mode == "computer") location[1] = Instantiate(location[0]) as GameObject;
		if (settings.current.mode == "multi") playerCamera[1] = Instantiate (playerCamera[0]) as Camera;

		ground.transform.localScale = new Vector3 (allMazes.current.use.GetUpperBound (1) * settings.current.wallSpacing/10, 1.0f, allMazes.current.use.GetUpperBound (0) * settings.current.wallSpacing/10);
		controller.setAndScale (ground, controller.get.materials[settings.current.ground], new Vector2 (ground.transform.localScale.x, ground.transform.localScale.z));
		/*ground.renderer.material = settings.materials[settings.groundMaterial];
		ground.transform.localScale = new Vector3 (allMazes.current.use.GetUpperBound (1) * settings.current.wallSpacing/10, 1.0f, allMazes.current.use.GetUpperBound (0) * settings.current.wallSpacing/10);
		ground.renderer.material.SetTextureScale("_MainTex", new Vector2(ground.transform.localScale.x,ground.transform.localScale.z));
		ground.renderer.material.SetTextureScale("_BumpMap", new Vector2(ground.transform.localScale.x,ground.transform.localScale.z));
		*/

		for (int i = 0;i<allMazes.current.use.GetUpperBound(0);i++){
			for (int j = 0;j<allMazes.current.use.GetUpperBound(1);j++){
				if (allMazes.current.use[i,j] == 0 && Random.Range (0, 2) == 0 && settings.current.mode != "multi"){
					//new Vector3 ((float)j*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, 1.5f, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-(float)i*settings.current.wallSpacing);}

					GameObject tempCoin = Instantiate(coin, new Vector3 ((float)j*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, 0.63f, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-(float)i*settings.current.wallSpacing), Quaternion.Euler (0f, 180f, 90f)) as GameObject;
					tempCoin.transform.parent = GameObject.Find ("Coins").transform;
					//tempCoin.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				}
				if (allMazes.current.use[i,j]==2){
					player[0] = Instantiate(controller.get.players[settings.current.player[0]]) as GameObject;

					playerCamera[0].transform.parent = player[0].transform;
					location[0].transform.parent = player[0].transform;
					player[0].GetComponentInChildren<pulse>().color = settings.color[0];

					player[0].transform.position = new Vector3 ((float)j*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, 0f, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-(float)i*settings.current.wallSpacing);
					player[0].transform.rotation = Quaternion.Euler (0f,180f,0f);

					player[0].AddComponent<playerMovement>();
				}
				if (allMazes.current.use[i,j]==3&&(settings.current.mode=="multi"||settings.current.mode=="computer")){
					//GameObject temp = Resources.LoadAssetAtPath ("Assets/Prefabs/" + settings.player1Name + ".prefab", typeof(GameObject)) as GameObject;
					//player2 = Instantiate (temp) as GameObject;
					
					//Resources.GameObjectUtility.SetParentAndAlign(mainCamera,player1);
					//UnityEditor.GameObjectUtility.SetParentAndAlign(location1,player1);
					player[1] = Instantiate(controller.get.players[settings.current.player[1]]) as GameObject;

					//location2.SetActive(true);
					location[1].transform.parent = player[1].transform;
					player[1].GetComponentInChildren<pulse>().color = settings.color[1];

					if (settings.current.mode == "computer"){
						player[1].AddComponent<computerMovement>();
					}else if (settings.current.mode == "multi"){
						player[1].AddComponent<playerMovement>();

						playerCamera[1].transform.parent = player[1].transform;
						playerCamera[0].rect = new Rect(0f,0f,0.5f,1f);
						playerCamera[1].rect = new Rect(0.5f,0f,1f,1f);
					}
					
					player[1].transform.position = new Vector3 ((float)j*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, 0f, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-(float)i*settings.current.wallSpacing);
				}
				if (allMazes.current.use[i,j]==4){end.position = new Vector3 ((float)j*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, 1.5f, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-(float)i*settings.current.wallSpacing);}
			}
		}

		mapCamera.orthographicSize = allMazes.current.use.GetUpperBound (0)*settings.current.wallSpacing/2f;
		float size = allMazes.current.use.GetUpperBound (0);//Mathf.Max (allMazes.current.use.GetUpperBound (0), allMazes.current.use.GetUpperBound (1)) * settings.current.wallSpacing / 2f;
		float s1 = allMazes.current.use.GetUpperBound (1);// * settings.current.wallSpacing / 2f;

		//print (allMazes.current.use.GetUpperBound (0) +", "+ allMazes.current.use.GetUpperBound (1));
		//print (s1 +", " + size);
		//print (s1 / size * 0.4f * (5f / 8f));

		mapCamera.rect = new Rect (1f - s1 / size * 0.25f, 0.6f, 1f, 1f);

		//mapCamera.rect = new Rect (0.7f + 0.01f / (s1 / size), 0.6f, 1f, 1f);
		//print ((0.7f + 1f - s1/size * 0.3f) +", "+ (0.6f + 1f - s2/size));
		/*if (allMazes.current.use.GetUpperBound(0)>allMazes.current.use.GetUpperBound(1)){
			mapCamera.rect = new Rect (0.7f+(allMazes.current.use.GetUpperBound (0)-allMazes.current.use.GetUpperBound(1))*.02f, 0.6f, 1f, 1f);
		}else{
			mapCamera.rect = new Rect (0.7f, 0.6f+(allMazes.current.use.GetUpperBound (1)-allMazes.current.use.GetUpperBound(0))*(0.1f/8f), 1f, 1f);
		}*/

		int c = 0, dim1 = 0, dim2 = 0;
		for (int j = 0;j<2;j++){
			for (int i = 0;i<allMazes.current.use.Length;i++){
				if (j==0){//set up horizontal walls
					dim1 = Mathf.RoundToInt(i/(allMazes.current.use.GetUpperBound(1)+1));
					dim2 = i%(allMazes.current.use.GetUpperBound(1)+1);
				}else if (j==1){//set up vertical walls
					dim1 = i%(allMazes.current.use.GetUpperBound(0)+1);
					dim2 = Mathf.RoundToInt(i/(allMazes.current.use.GetUpperBound(0)+1));
				}

				if (allMazes.current.use[dim1,dim2]==1){c++;}

				if (allMazes.current.use[dim1,dim2]!=1||i%(allMazes.current.use.GetUpperBound(1-j)+1)==allMazes.current.use.GetUpperBound(1-j)){
					if (allMazes.current.use[dim1,dim2]!=1&&allMazes.current.use[dim1-(1-j),dim2-j*1]==0&&allMazes.current.use[dim1+(1-j),dim2+j*1]==0){
						//Instantiate (wall, new Vector3();
					}

					if (c>1){
						GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
						GameObject temp1 = GameObject.CreatePrimitive (PrimitiveType.Cube);
						temp1.AddComponent<showWall>();

						temp1.transform.localScale = new Vector3 ((c-1)*settings.current.wallSpacing*(1-j)+0.499f*(1-j)+0.5f*j, 0.1f, (c-1)*settings.current.wallSpacing*j+0.499f*(j)+0.5f*(1-j));
						temp.transform.localScale = new Vector3 ((c-1)*settings.current.wallSpacing*(1-j)+0.499f*(1-j)+0.5f*j, wallHeight, (c-1)*settings.current.wallSpacing*j+0.499f*(j)+0.5f*(1-j));
						if (i%(allMazes.current.use.GetUpperBound(1-j)+1)==allMazes.current.use.GetUpperBound(1-j)){c-=2;}

						//if (j==0){wall.transform.position = new Vector3 (dim2*settings.current.wallSpacing-(c+1+allMazes.current.use.GetUpperBound(1))*settings.current.wallSpacing/2, wallHeight/2, settings.current.wallSpacing*(allMazes.current.use.GetUpperBound(0)/2-dim1));}
						//if (j==0){wall.transform.position = new Vector3 (dim2*settings.current.wallSpacing-(c+1+allMazes.current.use.GetUpperBound(1))*settings.current.wallSpacing/2, wallHeight/2, allMazes.current.use.GetUpperBound(0)*settings.current.wallSpacing/2-dim1*settings.current.wallSpacing);}
						//if (j==1){wall.transform.position = new Vector3 (dim2*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, wallHeight/2, (allMazes.current.use.GetUpperBound(0)+1-c)*settings.current.wallSpacing/2-(dim1-c)*settings.current.wallSpacing);}
						//if (j==1){wall.transform.position = new Vector3 (dim2*settings.current.wallSpacing-allMazes.current.use.GetUpperBound(1)*settings.current.wallSpacing/2, wallHeight/2, settings.current.wallSpacing*((allMazes.current.use.GetUpperBound(0)+1+c)/2-dim1));}
						temp.transform.position = new Vector3(settings.current.wallSpacing*(dim2-(allMazes.current.use.GetUpperBound(1)+(c+1)*(1-j))/2), wallHeight/2, settings.current.wallSpacing*((allMazes.current.use.GetUpperBound(0)+(1+c)*j)/2-dim1));
						temp1.transform.position = new Vector3(settings.current.wallSpacing*(dim2-(allMazes.current.use.GetUpperBound(1)+(c+1)*(1-j))/2), -1f, settings.current.wallSpacing*((allMazes.current.use.GetUpperBound(0)+(1+c)*j)/2-dim1));
						controller.setAndScale(temp, controller.get.materials[settings.current.wall], new Vector2(Mathf.Max(temp.transform.localScale.x,temp.transform.localScale.z)/settings.current.wallSpacing,1));

						//better -> create plane x = 2, y = .5/3
						if (j==0){
							for (int a = -1;a<1;a++){
								//print (c+", "+allMazes.current.use[dim1,dim2]+", "+dim1+", "+dim2+", "+(dim1-1)+", "+(dim1+1)+", "+(dim2+c*a));
								if (allMazes.current.use[dim1,dim2]!=1&&allMazes.current.use[dim1-1,dim2-1+((c-1)*a)]==0&&allMazes.current.use[dim1+1,dim2-1+((c-1)*a)]==0){
									//GameObject temp1 = Instantiate(wall, wall.transform.position = new Vector3(settings.current.wallSpacing*(dim2-(allMazes.current.use.GetUpperBound(1)+(c+1)*(1-j))/2), wallHeight/2, settings.current.wallSpacing*((allMazes.current.use.GetUpperBound(0)+(1+c)*j)/2-dim1)), Quaternion.identity) as GameObject;
									//GameObject wallCap = Instantiate (wall, new Vector3(temp.transform.position.x-(c-13f/15f)*(a*-2-1)/2*settings.current.wallSpacing, temp.transform.position.y, temp.transform.position.z), Quaternion.identity) as GameObject;
									GameObject wallCap = GameObject.CreatePrimitive(PrimitiveType.Cube);
									wallCap.transform.position = new Vector3(temp.transform.position.x-(c-13f/15f)*(a*-2-1)/2*settings.current.wallSpacing, temp.transform.position.y, temp.transform.position.z);

									wallCap.transform.localScale = new Vector3(0.1f,wallHeight,0.499f);
									Destroy (wallCap.GetComponent ("BoxCollider"));

									controller.setAndScale(wallCap, controller.get.materials[settings.current.wall], new Vector2(0.2f,1));
								}
							}
						}else{
							for (int a = -1;a<1;a++){
								//print (c+", "+allMazes.current.use[dim1,dim2]+", "+dim1+", "+dim2+", "+(dim1-1)+", "+(dim1+1)+", "+(dim2+c*a));
								if (allMazes.current.use[dim1,dim2]!=1&&allMazes.current.use[dim1-1+((c-1)*a),dim2-1]==0&&allMazes.current.use[dim1-1+((c-1)*a),dim2+1]==0){
									//GameObject temp1 = Instantiate(wall, wall.transform.position = new Vector3(settings.current.wallSpacing*(dim2-(allMazes.current.use.GetUpperBound(1)+(c+1)*(1-j))/2), wallHeight/2, settings.current.wallSpacing*((allMazes.current.use.GetUpperBound(0)+(1+c)*j)/2-dim1)), Quaternion.identity) as GameObject;
									//GameObject wallCap = Instantiate (wall, new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z-(c-13f/15f)*(a*2+1)/2*settings.current.wallSpacing), Quaternion.identity) as GameObject;
									GameObject wallCap = GameObject.CreatePrimitive(PrimitiveType.Cube);
									wallCap.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, temp.transform.position.z-(c-13f/15f)*(a*2+1)/2*settings.current.wallSpacing);

									wallCap.transform.localScale = new Vector3(0.499f,wallHeight,0.1f);
									Destroy(wallCap.GetComponent ("BoxCollider"));

									controller.setAndScale(wallCap, controller.get.materials[settings.current.wall], new Vector2(0.2f,1));
								}
							}
						}
					}
					
					c = 0;
				}
			}
		}	
	}
}

