using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {


	public int height = 0 , width = 0; 
	public Material snow ;
	public GameObject Wall ; 
	public Cell[] Cells;
	public GameObject[] allWalls;
	public List<Cell> Neighbors; 
	//variables 
	System.Random r = new System.Random();
	private GameObject wallHolder; 

	void GenerateMaze(){
		List<Cell> lastCell = new List<Cell> ();
		int totalCells = width * height , visitedCells = 0 , currentIndex = 0;
		currentIndex = r.Next (totalCells ); 
		currentIndex = Cells [currentIndex].index;

		while(visitedCells  < totalCells){
		   Debug.Log ("Celda Actual:" + currentIndex.ToString());
		   Neighbors = checkNeighbors (currentIndex);
			if (Neighbors.Count > 0) {
				int randomNeighbor = r.Next (Neighbors.Count );
				DeleteCell ( currentIndex , randomNeighbor);
				Cells [currentIndex].visited = true; 
				lastCell.Add (Cells [currentIndex]);
				currentIndex = Neighbors[ randomNeighbor].index;
				visitedCells++;
				
			} 
			else
			{
				currentIndex = lastCell [lastCell.Count - 1].index;
				Neighbors = checkNeighbors (currentIndex);
				if (Neighbors.Count == 0) {
					lastCell.Remove (lastCell [lastCell.Count - 1]);
				}
			}
			
		}

	}

	void DeleteCell(int currentIndex , int randomNeighbor){
		//quitamos la pared entre el current y el neighbor
		//es el vecino de abajo
		if (Cells [currentIndex].index - Neighbors [randomNeighbor].index == 1) {
			Debug.Log ("Se borra Abajo");
			Destroy (Cells [currentIndex].south);
			Destroy (Neighbors [randomNeighbor].north);
			Cells [currentIndex].south = null; 
			Neighbors [randomNeighbor].north = null;
		}
		//es el de arriba
		else if (Cells [currentIndex].index - Neighbors [randomNeighbor].index == -1) {
			Debug.Log ("Se borra Arriba");
			Destroy (Cells [currentIndex].north);
			Destroy (Neighbors [randomNeighbor].south);
			Cells [currentIndex].north = null; 
			Neighbors [randomNeighbor].south = null;
		}
		//es el de la izquierda
		else if (Cells [currentIndex].index - Neighbors [randomNeighbor].index == 5) {
			Debug.Log ("Se borra Izquierda");
			Destroy (Cells [currentIndex].west);
			Destroy (Neighbors [randomNeighbor].east);
			Cells [currentIndex].west = null; 
			Neighbors [randomNeighbor].east = null;
		} 
		//es el de la derecha
		else
		{
			Debug.Log ("Se borra Derecha");
			Destroy (Cells [currentIndex].east);
			Destroy (Neighbors [randomNeighbor].west);
			Cells [currentIndex].east = null; 
			Neighbors [randomNeighbor].west = null;

		}

	}

	//tested 80%
	List<Cell> checkNeighbors(int index){
		List<Cell> Vecinos = new List<Cell> ();
		//checking west first
		if (index - height >= 0) {
			if (Cells [index - height].visited == false && Cells[index].west != null) {
				Vecinos.Add (Cells [index - height]);
			//	Debug.Log("Izquierda:"+ Cells[index -height].index.ToString());
			}
		}
		//checking east
		if (index + height <= (Cells.Length - 1)) {
			if (Cells [index + height].visited == false && Cells[index].east != null) {
				Vecinos.Add (Cells [index + height]);
		//	Debug.Log("Derecha:"+ Cells[index  + height].index.ToString());
			}
		}
		//checking south
		if (index < (Cells.Length - 1)  && index % width != 0 && Cells[index].south != null) {
			if (Cells [index - 1].visited == false) {
				Vecinos.Add (Cells[index - 1]);
			//	Debug.Log("Abajo:"+ Cells[index - 1].index.ToString());

			}	
		}
		//Checking north 
		if(index + 1 >= 0 && index % width != 4 && Cells[index].north != null ){
			if (Cells[index + 1 ].visited == false) {
				Vecinos.Add (Cells [index + 1]);
			//	Debug.Log("Arriba:"+ Cells[index + 1].index.ToString());

			}
		}


		return Vecinos;
	}


	// Use this for initialization
	void Start () {
		InitializeMaze (height, width);	
		GenerateMaze ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	#region Metodos Probados
	void CreateCells(){
		int childrenCount = wallHolder.transform.childCount; 
		allWalls = new GameObject[childrenCount];
		Cells = new Cell[(width * height)];
		for (int i = 0; i < childrenCount; i++) {
			allWalls [i] = wallHolder.transform.GetChild (i).gameObject;
		}
		for (int i = 0; i < Cells.Length ; i++) {
			Cell cell = new Cell ();
			cell.west = allWalls [i]; // 0
			cell.east  = allWalls [i + height]; // 6
			cell.south = allWalls [i + ((allWalls.Length) / 2) + (i / width)]; // 30
			cell.north = allWalls[(i +  ((allWalls.Length) / 2)) + (i / width) +1]; //31
			cell.index = i ; 
			Cells [i] = cell;
		}
	}
	//tested and probed 
	void CreateCollectibles(int cellPosition){
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.localScale = new Vector3 ( 0.5f  , 0.5f , 0.5f);
		sphere.GetComponent<MeshRenderer> ().material.color = Color.red;
		sphere.transform.position = new Vector3  (((float)(cellPosition / height )+ 0.5f), 1.0f, ((float)(cellPosition % width  ) + 0.5f ));
	}

	void InitializeMaze(int width , int height){
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";
		//first we generate the floor
		GameObject tempWall; 
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = new Vector3  (((float)width / 2), 0.0f, ((float)height / 2));
		cube.GetComponent<MeshRenderer> ().material = snow;
		cube.transform.localScale = new Vector3 (    (float)width , 0.15f , (float)height);
		//then we create all the walls 
		for (int i = 0; i <= width; i++) {
			for (int j = 0; j < height; j++) {
				//we create  a wall 
				tempWall = Instantiate (Wall ,  new Vector3 ((float)i, 0.5f, (float)j + 0.5f) , Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform; 
				//allWalls.Add(wall);
			}
		}
		for (int i = 0; i < width; i++) {
			for (int j = 0; j <= height; j++) {
				//we create  a wall 
				tempWall = Instantiate (Wall ,  new Vector3 ((float)i + 0.5f, 0.5f, (float)j) ,  Quaternion.Euler (0,90,0)) as GameObject;
				tempWall.transform.parent = wallHolder.transform; 

				//	allWalls.Add(wall);
			}
		}
		CreateCells ();
	}

	#endregion

}

[System.Serializable]
public class Cell{

	public bool visited ;
	public GameObject north, east , west , south ;
	public int index;
	public Cell ()
	{
		visited = false; 
		north = null;
		south = null;
		east = null;
		west = null;
	}

}