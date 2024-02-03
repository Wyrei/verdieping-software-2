using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{
    [SerializeField] private float Timer;
    
    Stats stats;
    
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
