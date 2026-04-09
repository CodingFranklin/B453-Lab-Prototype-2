using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Generator : MonoBehaviour
{
    public int roomNumber = 3;
    [SerializeField] GameObject baseRoom;
    [SerializeField] GameObject endRoom;
    [SerializeField] private GameObject[] roomList;
    [SerializeField] LayerMask roomLayer;
    
    [SerializeField] Vector3 roomSize;

    private List<Transform> exits;
    
    void Start()
    {
        Initialization();
        BaseRoom();
        SpawnNextRoom();
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

    private void SpawnNextRoom()
    {
        while (roomNumber > 1)
        {
            Transform exit = exits[Random.Range(0, exits.Count)];
            exits.Remove(exit);
            GameObject newRoom = Instantiate(roomList[Random.Range(0, roomList.Length)], exit.position, exit.rotation);
            exits.Add(newRoom.transform.GetChild(2));
            roomNumber--;
        }

        if (roomNumber == 1)
        {
            Transform exit = exits[Random.Range(0, exits.Count)];
            exits.Remove(exit);
            GameObject endR = Instantiate(endRoom, exit.position, exit.rotation);
            roomNumber--;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.darkSlateGray;
        Gizmos.DrawWireCube(transform.position, roomSize);
    }

    private bool CanSpawnNextRoom(Vector3 centerPosition, Vector3 size)
    {
        Collider2D hit = Physics2D.OverlapBox(centerPosition, size, roomLayer);
        return hit == null;
    }
    
    
}
