using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pad : MonoBehaviour
{
    public int padId;
    int objsOnPad = 0;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<asdf>())
        {
            objsOnPad++;
            plates.instance.PlacementIncrease();
            if (other.gameObject.GetComponent<asdf>().boxID == padId)
            {
                plates.instance.IncreaseCorrectPlacement();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<asdf>())
        {
            objsOnPad--;
            plates.instance.PlacementDecrease();
            if(other.gameObject.GetComponent<asdf>().boxID == padId)
            {
                plates.instance.DecreaseCorrectPlacement();
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
