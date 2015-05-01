using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelGenerator : MonoBehaviour {

    private const float LOWER_LEVEL_HEIGHT = -15.4f;

    private GameObject[] availableOverworldRooms;
    private GameObject[] availableUnderworldRooms;

    //only used for visibility purposes at the moment
    public List<GameObject> currentOverworldRooms;
    public List<GameObject> currentUnderworldRooms;

    private float screenWidthInPoints;



	// Use this for initialization
	void Start () {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        availableOverworldRooms = GameObject.FindObjectOfType<GameManager>().availableOverworldRooms;
        availableUnderworldRooms = GameObject.FindObjectOfType<GameManager>().availableUnderworldRooms;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate () 
    {
        //Debug.Log(playerX - screenWidthInPoints);
        GenerateRoomIfRequired();
    }

    void AddRoom(float farthestRoomEndX)
    {
            //Picks a random index of the room type (Prefab) to generate.
            int randomRoomIndex = Random.Range(0, availableOverworldRooms.Length);

            //Creates a room object from the array of available rooms using the random index above.
            GameObject room = (GameObject)Instantiate(availableOverworldRooms[randomRoomIndex]);

            //Gets the width of the room
            float roomWidth = room.GetComponent<Renderer>().bounds.size.x;

            //Gets the center of the room
            float roomCenter = farthestRoomEndX + roomWidth * 0.5f;

            //This sets the position of the room. You need to change only the x-coordinate since all rooms have the same y and z coordinates equal to zero.
            room.transform.position = new Vector3(roomCenter, 0, 0);

            //Finally you add the room to the list of current rooms.
            currentOverworldRooms.Add(room);

            //Picks a random index of the room type (Prefab) to generate.
            int randomUnderworldRoomIndex = Random.Range(0, availableUnderworldRooms.Length-2);

            //Creates a room object from the array of available rooms using the random index above.
            GameObject underworld_room;

            if (randomRoomIndex == 2)
                underworld_room = (GameObject)Instantiate(availableUnderworldRooms[2]);
            else if (randomRoomIndex == 4)
                underworld_room = (GameObject)Instantiate(availableUnderworldRooms[3]);
            else
                underworld_room = (GameObject)Instantiate(availableUnderworldRooms[randomUnderworldRoomIndex]);   

                    

            //This sets the position of the room. You need to change only the x-coordinate since all rooms have the same y and z coordinates equal to zero.
            underworld_room.transform.position = new Vector3(roomCenter, LOWER_LEVEL_HEIGHT, 0);

            //Finally you add the room to the list of current rooms.
            currentUnderworldRooms.Add(underworld_room);
    }

    void GenerateRoomIfRequired()
    {
        //Creates a new list to store rooms that needs to be removed.
        List<GameObject> roomsToRemove = new List<GameObject>();

        //2
        bool addRooms = true;

        //Saves player position.
        float playerX = GameManager.Player.transform.position.x;

        //This is the point after which the room should be removed. If room position is behind this point (to the left), it needs to be removed. 
        float removeRoomX = playerX - screenWidthInPoints * 3; //the * 2 is an arbitrary constant we add to adjust things

        //If there is no room after addRoomX point you need to add a room, since the end of the level is closer then the screen width.
        float addRoomX = playerX + screenWidthInPoints;

        //In farthestRoomEndX you store the point where the level currently ends. You will use this variable to add new room if required, 
        //since new room should start at that point to make the level seamless.
        float farthestRoomEndX = 0;

        foreach (var room in currentOverworldRooms)
        {
            //Getting the parameters of the current rooms
            float roomWidth = room.GetComponent<Renderer>().bounds.size.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            //Specifies if we don't need to add new rooms
            if (roomStartX > addRoomX)
                addRooms = false;

            //Removes a room that is no longer on screen
            if (roomEndX < removeRoomX)
                roomsToRemove.Add(room);

            //Tells us the room that is the farthest away
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach (var room in currentUnderworldRooms)
        {
            //Getting the parameters of the current rooms
            float roomWidth = room.GetComponent<Renderer>().bounds.size.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            //Specifies if we don't need to add new rooms
            if (roomStartX > addRoomX)
                addRooms = false;

            //Removes a room that is no longer on screen
            if (roomEndX < removeRoomX)
                roomsToRemove.Add(room);

            //Tells us the room that is the farthest away
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }


        //Removes the rooms that are off screen
        foreach (var room in roomsToRemove)
        {
            if(currentOverworldRooms.Contains(room))
                currentOverworldRooms.Remove(room);
            else
                currentUnderworldRooms.Remove(room);

            Destroy(room);
        }

        //adds rooms if we're at that point
        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
            //AddRoom(farthestRoomEndX, LOWER_LEVEL_HEIGHT);
        }
    }
}
