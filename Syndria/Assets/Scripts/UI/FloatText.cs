using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FloatText : MonoBehaviour
    {
        public float lifespan = 5.0f;
        public string text;
        public TextMeshProUGUI textMesh;

        private float _timer;

        // Use this for initialization
        void Start()
        {
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
