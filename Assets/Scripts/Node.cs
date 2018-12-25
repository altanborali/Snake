using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class Node    // monobehaviour kalınca GameManager Node işi patladı. Neden bilmiim. Not gece 03:20 03:53 bununla geçti. 
    {

        public int x;
        public int y;
        public Vector3 worldPosition;


        /*public Vector3 worldPosition  böyle yaparsak scale de problem olurmuş
        {
            get
            {
                Vector3 p = Vector3.zero;
                p.x = x;
                p.y = y;
                return p;
            }
        }*/
        
            
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
