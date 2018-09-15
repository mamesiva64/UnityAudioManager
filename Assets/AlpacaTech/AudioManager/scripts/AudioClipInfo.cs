using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AlpacaTech
{
    [System.Serializable]
    public class AudioClipInfo
    {
        public AudioClip clip;
        public string id;
        public float volume = 1.0f;
    };

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AudioClipInfo))]
    public class AudioClipInfoDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUIUtility.labelWidth = 80 + EditorGUI.indentLevel * 20;
            label = EditorGUI.BeginProperty(position, label, property);
            this.Init(position, property, label);

            var last_indent_level = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            //--------------------------------------------
            DividedField(0.25f, "id", "");
            DividedField(0.5f, "clip", "clip");
            DividedField(0.25f, "volume", "vol");
            //--------------------------------------------

            EditorGUI.indentLevel = last_indent_level;
            EditorGUI.EndProperty();
        }

        private Rect wholeRect;
        private float partialSum;
        private float rateSum;
        private SerializedProperty prop;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        void Init(Rect position, SerializedProperty property, GUIContent label)
        {
            this.partialSum = 0;
            this.rateSum = 0;
            this.prop = property;
            this.wholeRect = EditorGUI.PrefixLabel(position, label);
        }

        /// <summary>
        /// 割合を指定して1行のなかにプロパティを表示
        /// </summary>
        /// <param name="widthRate">1行の中でとる幅(0.0～1.0)</param>
        /// <param name="propertyName">表示するプロパティの名前</param>
        /// <param name="label">ラベル名</param>
        void DividedField(float widthRate, string propertyName, string label = "", int labelWidth = 0)
        {
            Debug.Assert(widthRate <= 1);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(label != null);

            if (widthRate == 0)
            {
                widthRate = 1.0f - rateSum;
            }
            var width = this.wholeRect.width * widthRate;
            var rect = new Rect(this.wholeRect.x + this.partialSum, this.wholeRect.y, width, this.wholeRect.height);

            this.partialSum += width;
            this.rateSum += widthRate;

            //  全角2 半角1として幅を自動計算
            if (labelWidth == 0)
            {
                var labelByte = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(label);
                labelWidth = labelByte * 9;
            }

            //
            EditorGUIUtility.labelWidth = Mathf.Clamp(labelWidth, 0, rect.width - 20);
            var item = this.prop.FindPropertyRelative(propertyName);
            if (item != null)
            {
                EditorGUI.PropertyField(rect, item, new GUIContent(label));
            }
            else
            {
                Debug.LogWarningFormat("Failed to find property: '{0}' in '{1}'", propertyName, this.GetType());
            }
        }
    }
#endif
}