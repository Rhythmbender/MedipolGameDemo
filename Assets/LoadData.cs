using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // en g�ncel bilgiler otomatikman panel'e �ekmi� oldum.
        DataCtrl.instance.LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
