using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Interaction {
    public class SlidesGrabBall : MonoBehaviour
    {
        public GameObject attachedSlides;
        //public GameObject attachedUIPanel;
        //public Vector3 uIPanelOffset;
        public Vector3 slidesOffset;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            attachedSlides.transform.position = transform.position + slidesOffset;
            //attachedUIPanel.transform.position = transform.position + uIPanelOffset;
        }
    }
}
