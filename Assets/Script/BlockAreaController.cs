using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAreaController : MonoBehaviour
{
    public List<GameObject> BlockList = new List<GameObject>();

    public void ReloadBlock()
    {
        for ( int i = 0; i < BlockList.Count; i++)
        {    
            if (!BlockList[i].activeInHierarchy && BlockList[i] != null )
            {
                BlockList[i].gameObject.SetActive(true);
            }
        }
    }
}
