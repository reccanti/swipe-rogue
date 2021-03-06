﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * An enum of the different directions that the
 * user can access
 */
// public enum Direction {
// 	North,
// 	South,
// 	East,
// 	West
// };

// /**
//  * This class holds data related to Rooms in the floor graph.
//  * This may later be replaced with a MonoBehavior game object
//  */
// public class RoomClass {
// 	public Dictionary<Direction, RoomClass> neighbors;
// 	public int number;
// 	private RoomClass parent;
//     public int x;
//     public int y;

//     /**
// 	 * Instantiate a room with a number and it's parent
// 	 */
//     public RoomClass(int number, RoomClass parent) {
// 		this.number = number;
// 		this.parent = parent;
//         x = parent.x;
//         y = parent.y;
// 		this.neighbors = new Dictionary<Direction, RoomClass>();
// 		foreach (Direction direction in Direction.GetValues(typeof(Direction))) {
// 			this.neighbors.Add(direction, null);
// 		}
// 	}
//     public RoomClass(int number, RoomClass parent,int x,int y)
//     {
//         this.number = number;
//         this.parent = parent;
//         this.x = x;
//         this.y = y;
//         this.neighbors = new Dictionary<Direction, RoomClass>();
//         foreach (Direction direction in Direction.GetValues(typeof(Direction)))
//         {
//             this.neighbors.Add(direction, null);
//         }
//     }
// 	/**
// 	 * Check to see if the room has a free edge
// 	 */
// 	public bool HasFreeEdges() {
// 		foreach (Direction direction in Direction.GetValues(typeof(Direction))) {
// 			if (this.neighbors [direction] == null) {
// 				return true;
// 			}
// 		}
// 		return false;
// 	}

// 	/**
// 	 * Get a random direction that doesn't have a room next to it
// 	 */
// 	public Direction GetRandomFreeDirection() {

// 		// create a list of directions, and randomly remove values from it
// 		// until we find a neighbor in the direction that is unoccupied
// 		Direction[] directions = (Direction[])Direction.GetValues(typeof(Direction));
// 		List<Direction> directionList = new List<Direction> (directions);
// 		while (directionList.Count > 1) {
// 			int randomIndex = Random.Range (0, directionList.Count);
// 			if (this.neighbors [directionList[randomIndex]] == null) {
// 				return directionList [randomIndex];
// 			} else {
// 				directionList.RemoveAt(randomIndex);
// 			}
// 		}
// 		return directionList[0];
// 	}

// 	/**
// 	 * Print all of the properties of the Room object
// 	 */
// 	public override string ToString ()
// 	{
// 		string roomString = string.Format ("ID: {0}\n", this.number);
// 		roomString += (this.parent == null) ? string.Format ("Parent: null\n") : string.Format ("Parent: {0}\n", this.parent.number);
// 		roomString += string.Format("({0}, {1})", this.x, this.y);
// 		roomString += "Neighbors:\n"; 
// 		foreach (Direction direction in Direction.GetValues(typeof(Direction))) {
// 			RoomClass directionRoom = this.neighbors [direction];
// 			roomString += (directionRoom == null) ? string.Format ("\t{0}: null\n", direction.ToString()) : string.Format ("\t{0}: {1}\n", direction.ToString(), directionRoom.number);
// 		}
// 		return roomString;
// 	}


// }

public class CreateFloor : MonoBehaviour {

    //    public Transform[] spawnLocations;
    //    public GameObject RoomPrefab;
    //    public GameObject RoomClone;
    //
    //    private List<Room> roomList;
    //    public int numOfRooms = 10;
    //    private int roomsMade;
    //
    //
    //    public string[] roomTypes;
    //
    //    
    //
	private List<RoomClass> rooms;

    // Use this for initialization
    private void Start () {

        //initialize multidimensional array 
        
        // grid cords for starting room & new initialized rooms
        int x = 5;  
        int y = 5;

        // How many rooms there should be. 
        int totalRooms = 10;           

        // initialize the list of rooms with a single parent room
        rooms = new List<RoomClass> ();
		rooms.Add(new RoomClass(0, null,x,y));  // ###################### initializing first room with overloaded method

        // randomly add 10 connected rooms to the floor. Is increased by one when a room overlaps so there will be (10) rooms
        for (int i = 1; i < totalRooms; i++) {

            bool overlap = false;

            // get the old node and the new node we will be linking to it
            RoomClass newParent = GetRandomRoomWithFreeNeighbors ();
			RoomClass newRoom = new RoomClass (i, newParent);

			// get the directions so we can bind the rooms both ways
			Direction direction = newParent.GetRandomFreeDirection ();
			Direction oppositeDirection;
			if (direction == Direction.North) 
				oppositeDirection = Direction.South;
			else if (direction == Direction.South)
				oppositeDirection = Direction.North;
			else if (direction == Direction.West)
				oppositeDirection = Direction.East;
			else
				oppositeDirection = Direction.West;

            // Sets newRoom's x & y to parents
            newRoom.x = newParent.x;
            newRoom.y = newParent.y;
            
            // Checks which direction the newRoom is compared to its parent and changes x or y accordingly.
            if (oppositeDirection == Direction.South)
            {
                newRoom.x++;
            }
            else if (oppositeDirection == Direction.North)
            {
                newRoom.x--;
            }
            else if (oppositeDirection == Direction.East)
            {
                newRoom.y++;
            }
            else if (oppositeDirection == Direction.West)
            {
                newRoom.y--;
            }

            //print("PARENT ID: " +newParent.number + "  x: " + newParent.x + "  y: " + newParent.y);
            //print("ROOM ID: " + newRoom.number + "   x: " + newRoom.x + "  y: " + newRoom.y + "  # of Rooms: " + rooms.Count);

            // Loops through current rooms in the list and checks if newRoom x&y match any other rooms
            for (int j = 1;j < rooms.Count; j++)
            {
                // If there is a match break out of loop and set overlap to true
                if(newRoom.x == rooms[j].x && newRoom.y == rooms[j].y)
                {
                    overlap = true;
                    print("OVERLAP: ID: " + newRoom.number + " newRoom.x: " + newRoom.x + "  newRoom.y: " + newRoom.y + " rooms[" + j + "] ID: " + rooms[j].number +" room[" + j+"].x: " + rooms[j].x + "  room[" + j + "].y: " + rooms[j].y);
                    totalRooms++;
                    break;
                }
            }


            // If not true then the newRoom is not overlaping another room so its ok to use
            if (!overlap)
            {
                // link the two nodes both ways
                newParent.neighbors[direction] = newRoom;
                newRoom.neighbors[oppositeDirection] = newParent;


                // add the new room to the list so we can search again
                rooms.Add(newRoom);
            }

		}

		// test that this worked
		foreach (RoomClass r in rooms) {
			Debug.Log (r.ToString ());
		}
	}

	/**
	 * A function to randomly get a room with free edges
	 */ 
	private RoomClass GetRandomRoomWithFreeNeighbors() {

		// create a shallow copy of the list so that we don't remove actual values
		List<RoomClass> shuffleRooms = this.rooms.GetRange (0, this.rooms.Count);

		// randomly remove rooms from the list
		// until we find one with free edges
		while (shuffleRooms.Count > 0) {
			int randomIndex = Random.Range (0, shuffleRooms.Count);
			if (shuffleRooms [randomIndex].HasFreeEdges ()) {
				return shuffleRooms [randomIndex];
			} else {
				shuffleRooms.RemoveAt (randomIndex);
			}
		}
		return null;
	}
//		
//    // Each Room is created with open slots for adjacent rooms before they are added in
//
//    void CreateRooms()
//    {
//        // 1: bottom   2: Left   3: Top   4: Right
//
//
//
//        //room = new Room[roomNum];
//       // room = new List<Room>();
//       //
//       //
//       // room[0] = new Room(0, "entrance");
//       //
//       // room[0].BuildRoom(1);    //,"entrance");
//       //
//        for (int i = 0; i < numOfRooms; i++)
//        {
//
//            RoomClone = Instantiate(RoomPrefab,transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
//
//       //     // need to a way to track how many rooms are being made
//       //     room[i] = new Room(i,room[i-1], roomTypes[UnityEngine.Random.Range(1, 4)]);
//       //
//       //    // room[i].BuildRoom(UnityEngine.Random.Range(1, 3));  // ,roomTypes[UnityEngine.Random.Range(1, 4)]);
//        }
//
//
//    }

}
