using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Generator : MonoBehaviour
{
    public int roomNumber = 3;
    [SerializeField] GameObject baseRoom;
    [SerializeField] GameObject endRoom;
    [SerializeField] private GameObject[] roomList;
    [SerializeField] GameObject wall;
    // [SerializeField] LayerMask roomLayer;
    //
    // [SerializeField] Vector3 roomSize;

    public List<Transform> exits;
    
    void Start()
    {
        Initialization();
        BaseRoom();
        StartCoroutine(SpawnNextRoom());
    }

    private void Initialization()
    {
        exits = new List<Transform>();
    }

    private void BaseRoom()
    {
        GameObject baseR = Instantiate(baseRoom, new Vector3(0, 0, 0), Quaternion.identity);
        //add exit
        exits.Add(baseR.transform.GetChild(2));
        roomNumber--;
    }
    
    private IEnumerator SpawnNextRoom()
    {
        while (roomNumber > 1)
        {
            Transform exit = exits[Random.Range(0, exits.Count)];
            GameObject roomPrefab = roomList[Random.Range(0, roomList.Length)];

            yield return StartCoroutine(TrySpawnNextRoom(exit, roomPrefab));
        }
        
        Transform finalExit = exits[Random.Range(0, exits.Count)];
        yield return StartCoroutine(TrySpawnNextRoom(finalExit, endRoom));
        
        FillHoles();
    }

    // private void SpawnNextRoom()
    // {
    //     while (roomNumber > 1)
    //     {
    //         Transform exit = exits[Random.Range(0, exits.Count)];
    //         GameObject roomPrefab = roomList[Random.Range(0, roomList.Length)];
    //         if (CanSpawnNextRoom(exit, roomPrefab.GetComponent<RoomData>()))
    //         {
    //             exits.Remove(exit);
    //             GameObject newRoom = Instantiate(roomPrefab, exit.position, exit.rotation);
    //             exits.Add(newRoom.transform.GetChild(2)); 
    //             roomNumber--;
    //         }
    //     }
    //
    //     if (roomNumber == 1)
    //     {
    //         Transform exit = exits[Random.Range(0, exits.Count)];
    //         if (CanSpawnNextRoom(exit, endRoom.GetComponent<RoomData>()))
    //         {
    //             exits.Remove(exit);
    //             GameObject endR = Instantiate(endRoom, exit.position, exit.rotation);
    //             roomNumber--;
    //         }
    //     }
    // }

    // private bool CanSpawnNextRoom(Transform exit, RoomData roomData)
    // {
    //     Vector3 nextRoomPosition = exit.position;
    //     Quaternion nextRoomRotation = exit.rotation;
    //
    //     Vector3 centerOffset = nextRoomRotation * roomData.center.localPosition;
    //     Vector2 boxCenter = nextRoomPosition + centerOffset;
    //
    //     Collider2D hit = Physics2D.OverlapBox(
    //         boxCenter,
    //         roomData.roomSize,
    //         nextRoomRotation.eulerAngles.z,
    //         roomLayer
    //     );
    //
    //     return hit == null;
    // }

    private IEnumerator TrySpawnNextRoom(Transform exit, GameObject roomPrefab)
    {
        GameObject newRoom = Instantiate(roomPrefab, exit.position, exit.rotation);

        // wait for physics to update
        yield return new WaitForFixedUpdate();

        RoomData data = newRoom.GetComponent<RoomData>();

        if (data.isOverlapping)
        {
            Destroy(newRoom);
        }
        else
        {
            AddAllExits(newRoom);
            exits.Remove(exit);
            roomNumber--;
        }
    }

    private void AddAllExits(GameObject newRoom)
    {
        Transform exitParent = newRoom.transform.GetChild(0);

        foreach (Transform child in exitParent)
        {
            exits.Add(child);
        }
    }

    private void FillHoles()
    {
        foreach (Transform exit in exits)
        {
            Instantiate(wall, exit.position, exit.rotation);
        }
    }




}
