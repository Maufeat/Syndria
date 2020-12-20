using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FloatText : MonoBehaviour
    {
        public float lifespan = 5.0f;
        public string text;
        public TextMeshProUGUI textMesh;

        public Material positiveMaterial;
        public Material negativeMaterial;

        private float _timer;

        // Use this for initialization
        void Start()
        {
            Debug.Log(Convert.ToInt32(text));
            if(Convert.ToInt32(text) >= 0)
            {
                Debug.Log(true);
                textMesh.fontMaterial = positiveMaterial;
            } else
            {
                Debug.Log(false);
                textMesh.fontMaterial = negativeMaterial;
            }
            textMesh.text = text;
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer > lifespan)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}
