using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusControllerTitle : MonoBehaviour
{
    public Transform virusPrefab;
    public Transform virusTemp;
    private int virusCount;
    public int virusCountMin;
    public int virusCountMax;

    // Start is called before the first frame update
    void Start()
    {
        virusCount = Random.Range(virusCountMin, virusCountMax);

        for (int i = 0; i < virusCount; i++)
        {
            virusTemp = Instantiate(virusPrefab);
            virusTemp.position = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
