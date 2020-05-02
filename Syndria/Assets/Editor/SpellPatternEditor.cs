using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class SpellPatternEditor : EditorWindow
    {
        [SerializeField]
        SpellPattern pattern;

        [MenuItem("Pocket Ninja/Create Spell Pattern")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SpellPatternEditor));
        }

        public void OnEnable()
        {
            pattern = CreateInstance<SpellPattern>();
            pattern.Resize(1, 1);
        }

        public void OnGUI()
        {
            int newWidth = EditorGUILayout.IntField("Width", pattern._width);
            int newHeight = EditorGUILayout.IntField("Height", pattern._height);

            Rect position = new Rect(0, 65, Screen.width, Screen.height - 50);


            if (newWidth != pattern._width || newHeight != pattern._height)
            {
                GUI.changed = true;
                pattern.Resize(newWidth, newHeight);
            }
            else
            {
                GUI.changed = false;
            }

            float xWidth = Mathf.Min(
                position.width / Mathf.Max(1, pattern._width),
                position.width / Mathf.Max(1, pattern._height)
                );

            GUIStyle fontStyle = new GUIStyle(EditorStyles.textField);
            fontStyle.normal.background = MakeTex(50, 50, new Color(0f, 0f, 0f, 0.8f));
            fontStyle.alignment = TextAnchor.MiddleCenter;
            fontStyle.fontSize = Mathf.FloorToInt(xWidth * 0.7f);

            for (int x = 0; x < pattern._width; x++)
            {
                for (int y = 0; y < pattern._height; y++)
                {
                    if (pattern.GetData(x, y) == 1)
                        fontStyle.normal.background = MakeTex(50, 50, new Color(1.0f, 0f, 0f, 0.2f));
                    int res = EditorGUI.IntField(new Rect(position.x + xWidth * x, position.y + xWidth * y, xWidth, xWidth), pattern.GetData(x,y), fontStyle);
                    pattern.SetData(x, y, res);
                    fontStyle.normal.background = MakeTex(50, 50, new Color(0f, 0f, 0f, 0.8f));
                }
            }

            if (GUILayout.Button("Save Pattern"))
            {
                AssetDatabase.CreateAsset(pattern, "Assets/Resources/Spells/newpattern.asset");
                EditorUtility.SetDirty(pattern);
                AssetDatabase.SaveAssets();
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }


        public static float InspectSpellPattern(Rect position, SpellPattern pattern)
        {
            float usedHeight = 0.0f;

            int newWidth = EditorGUI.IntField(new Rect(position.x, position.y, position.width * 0.5f, EditorGUIUtility.singleLineHeight), "Width", pattern._width);
            int newHeight = EditorGUI.IntField(new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, EditorGUIUtility.singleLineHeight), "Height", pattern._height);

            position.y += EditorGUIUtility.singleLineHeight;
            usedHeight += EditorGUIUtility.singleLineHeight;

            if ((newWidth != pattern._width) || (newHeight != pattern._height))
            {
                GUI.changed = true;
                pattern.Resize(newWidth, newHeight);
            }
            else
                GUI.changed = false;

            float xWidth = Mathf.Min(position.width / Mathf.Max(1, pattern._width),
                position.width / Mathf.Max(1, pattern._height));

            GUIStyle fontStyle = new GUIStyle(EditorStyles.textField);
            fontStyle.fontSize = Mathf.FloorToInt(xWidth * 0.7f);

            for (int x = 0; x < pattern._width; x++)
            {
                for (int y = 0; y < pattern._height; y++)
                {
                    int res = EditorGUI.IntField(new Rect(position.x + xWidth * x, position.y + xWidth * y, xWidth, xWidth), pattern.GetData(x, y), fontStyle);
                    pattern.SetData(x, y, res);
                }
            }

            if (GUI.changed)
                EditorUtility.SetDirty(pattern);

            GUILayout.Button($"{usedHeight}");
            return usedHeight;
        }
        
    }
}
