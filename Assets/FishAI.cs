using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    public AiManager manager;
    float speed = 0.0f;
    private Animator animator;
    bool turn = false;
    private float hitCheck = 0.0f;




    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(manager.minSpeed, manager.maxSpeed);
        animator = this.GetComponent<Animator>();
        animator.speed = speed;
        hitCheck = manager.hitRangeCheck;
    }
    void ApplyRules()
    {
        GameObject[] allFish;
        allFish = manager.fishies;
        //avg Center of the group
        Vector3 vCentre = Vector3.zero;
        //avg Avoidence vector
        Vector3 vAvoid = Vector3.zero;
        //avg global Speed of the group
        float globalSpeed = 0.01f;
        //Distance of the neigbours
        float nDistance = 0.0f;
        //how many of the fishes in the flock are in the group
        int groupSize = 0;

        foreach(GameObject GO in allFish)
        {
            if(GO !=this.gameObject)
            {
                nDistance = Vector3.Distance(GO.transform.position, this.transform.position);
                if(nDistance <= manager.neighbourDistance)
                {
                    vCentre += GO.transform.position;
                    groupSize++;
                    //fish needs to avoid close ones
                    if (nDistance < manager.avoidDistance)
                    {
                        vAvoid = vAvoid + (this.transform.position - GO.transform.position);
                    }

                    FishAI tempFish = GO.GetComponent<FishAI>();
                    globalSpeed += tempFish.speed;
                }
            }
        }

        if(groupSize>0)
        {
            //taking the avrage of the group center and speed
            vCentre = vCentre / groupSize + (manager.goalPos-this.transform.position);
            speed = globalSpeed / groupSize;

            //to give the flock effect
            Vector3 dir = (vCentre + vAvoid) - transform.position;
            if(dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir),
                                                      manager.rotationSpeed * Time.deltaTime);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        //Create the bounds where the fish can swim
        Bounds b = new Bounds(manager.transform.position,manager.swimLimit*2);

        //Raycasting to find if the fish is about to collide with something
        RaycastHit hit = new RaycastHit();
        Vector3 dir = Vector3.zero ;
        if (!b.Contains(this.transform.position))
        {
            turn = true;
            dir = manager.transform.position - this.transform.position;
        }
        else if(Physics.Raycast(this.transform.position, this.transform.forward * hitCheck, out hit))
        {
            turn = true;
            dir = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turn = false;
        }
        if (turn)
        {
            transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir),
                                                  manager.rotationSpeed * Time.deltaTime);
        }
        //Onyl works if fish is in the defined bounds
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                ApplyRules();
            }
        }
        transform.Translate(0,0,speed * Time.deltaTime);
    }
}
