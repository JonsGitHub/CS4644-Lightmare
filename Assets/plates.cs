using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plates : MonoBehaviour
{
    public static plates instance;
    public GameObject door;

    public List<GameObject> pads;
    public List<GameObject> obxes;
    public List<Color> colors;
    private int temp;

    public int totalCorrect;
    public int curCorrect;
    public int placements;


    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }else if (instance != this)
        {
            Destroy(gameObject);
        }
        totalCorrect = pads.Count;
    }

    void Start()
    {
        AssignColors(obxes);
        AssignColors(pads);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseCorrectPlacement()
    {
        curCorrect++;
        if (curCorrect == totalCorrect)
        {
            door.transform.position = new Vector3(12.318f, 2.77f + 6, 19.189f);
        }
        temp = curCorrect;


    }

    public void DecreaseCorrectPlacement()
    {
        curCorrect--;
        if (temp == totalCorrect && curCorrect != totalCorrect)
        {
            door.transform.position = new Vector3(12.318f, 2.77f, 19.189f);
        }
        temp = curCorrect;

    }

    public void PlacementIncrease()
    {
        placements++;

        //if()
    }
    public void PlacementDecrease()
    {
        placements--;
        
    }

    void AssignColors(List<GameObject> objs)
    {
        for(int i = 0; i < objs.Count; i++)
        {
            objs[i].GetComponent<Renderer>().material.color = colors[i];
            
            if(objs[i].GetComponent<asdf>())
            {
                objs[i].GetComponent<asdf>().boxID = i;

            }
            else
            {
                objs[i].GetComponent<pad>().padId = i;
            }
        }

    }
}
