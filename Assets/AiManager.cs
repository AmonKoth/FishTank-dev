using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField] GameObject[] fishPrefab = new GameObject[1];
    public int fishCount = 20;
    [HideInInspector]
    public GameObject[] fishies;
    //confines of the flock 
    public Vector3 swimLimit = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos;

    [Header("Fish Settings")]
    [Range(0.3f, 2.0f)]
    public float minSpeed = 1.0f;
    [Range(2.0f, 5.0f)]
    public float maxSpeed = 1.0f;
    //The range which is consideret to be the neighbour
    [Range(0.1f, 10.0f)]
    public float neighbourDistance = 1.0f;
    [Range(0.1f, 5.0f)]
    public float rotationSpeed = 1.0f;
    //How close a fish can be before considering avoiding others
    [Range(0.1f, 5.0f)]
    public float avoidDistance = 1.0f;
    [Range(10.0f, 100.0f)]
    public float hitRangeCheck = 50.0f;

    //no code in this class used just for creating a parent game object
    private FishParent parent;

    private Vector3 PositionGenarator()
    {
        return this.transform.position + new Vector3(Random.Range(-swimLimit.x, swimLimit.x),
                                                     Random.Range(-swimLimit.y, swimLimit.y),
                                                     Random.Range(-swimLimit.z, swimLimit.z));
    }


    // Start is called before the first frame update
    void Start()
    {
        parent = FindObjectOfType<FishParent>();
        fishies = new GameObject[fishCount];
        for (int i = 0; i< fishCount; i++)
        {
            //Creates a random starting position for each fish and places it to this position
            Vector3 pos = PositionGenarator();
            fishies[i] = Instantiate(fishPrefab[Random.Range(0,fishPrefab.Length)], pos, 
                                        Quaternion.identity, parent.transform);
            //to link the fish ai and the manager so ai script can get the values from the manager
            fishies[i].GetComponent<FishAI>().manager = this;
        }

        goalPos = this.transform.position;

    }



    private void Update()
    {

       
        //will update the goal position within the swimm limits at %0.1 chance
        if (Random.Range(0, 100) < 0.1)
        {
            goalPos = PositionGenarator();
        }
    }

}
