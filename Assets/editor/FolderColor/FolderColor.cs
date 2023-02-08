﻿#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AltoLib
{
    public static class FolderColor
    {
        const string MenuPath = "FolderColor/On";
        static readonly string[] Keywords = {
            "scene", "material", "editor", "resource", "prefab", "shader", "script"
        };

        [MenuItem(MenuPath)]
        static void ToggleEnabled()
        {
            Menu.SetChecked(MenuPath, !Menu.GetChecked(MenuPath));
        }

        [InitializeOnLoadMethod]
        static void SetEvent()
        {
            EditorApplication.projectWindowItemOnGUI += OnGUI;
        }

        static void OnGUI(string guid, Rect selectionRect)
        {
            if (!Menu.GetChecked(MenuPath))
            {
                return;
            }

            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var pathLevel = CountWord(assetPath, "/");

            var originalColor = GUI.color;
            GUI.color = GetColor(pathLevel, assetPath);
            GUI.Box(selectionRect, string.Empty);
            GUI.color = originalColor;
        }

        static int CountWord(string source, string word)
        {
            return source.Length - source.Replace(word, "").Length;
        }

        static Color GetColor(int pathLevel, string assetPath)
        {
            var fileName = Path.GetFileName(assetPath);
            string[] folderNames = assetPath.Split('/');

            int id, depth;
            (id, depth) = GetColorIdAndDepth(pathLevel, assetPath);

            Color color = (EditorGUIUtility.isProSkin)
                ? GetColorForDarkSkin(id)
                : GetColorForLightSkin(id);

            float alphaFactor = 1.0f - (depth * 0.25f);
            alphaFactor = Mathf.Clamp(alphaFactor, 0, 1f);
            color.a *= alphaFactor;
            return color;
        }

        static (int id, int depth) GetColorIdAndDepth(int pathLevel, string assetPath)
        {
            if (pathLevel == 1) { return (0, 0); }

            int depthBase = 0;
            string[] folderNames = assetPath.Split('/');
            foreach (string folderName in folderNames)
            {
                var lowerName = folderName.ToLower();
                for (int i = 0; i < Keywords.Length; ++i)
                {
                    if (lowerName.StartsWith(Keywords[i]))
                    {
                        return ((i % 7) + 1, pathLevel - depthBase);
                    }
                }
                ++depthBase;
            }
            return (-1, 0);
        }

        static Color GetColorForDarkSkin(int id)
        {
            switch (id % 8)
            {
                case 0: return new Color(8.4f, 8.4f, 0.0f, 0.45f);
                case 1: return new Color(9.6f, 0.5f, 0.5f, 0.50f);
                case 2: return new Color(0.0f, 9.6f, 0.0f, 0.40f);
                case 3: return new Color(8.4f, 0.0f, 8.4f, 0.50f);
                case 4: return new Color(9.6f, 3.5f, 0.0f, 0.50f);
                case 5: return new Color(0.0f, 4.8f, 9.6f, 0.40f);
                case 6: return new Color(9.6f, 3.0f, 3.0f, 0.50f);
                case 7: return new Color(2.0f, 2.0f, 9.6f, 0.65f);
            }
            return new Color(0, 0, 0, 0);
        }

        static Color GetColorForLightSkin(int id)
        {
            switch (id % 8)
            {
                case 0: return new Color(1.4f, 1.4f, 0.0f, 0.15f);
                case 1: return new Color(1.6f, 0.0f, 0.0f, 0.15f);
                case 2: return new Color(0.0f, 1.6f, 0.0f, 0.15f);
                case 3: return new Color(0.8f, 0.0f, 1.4f, 0.15f);
                case 4: return new Color(1.6f, 0.5f, 0.0f, 0.15f);
                case 5: return new Color(0.0f, 0.8f, 1.6f, 0.15f);
                case 6: return new Color(1.6f, 0.4f, 0.4f, 0.15f);
                case 7: return new Color(0.2f, 0.2f, 1.6f, 0.15f);
            }
            return new Color(0, 0, 0, 0);
        }
    }
}
#endif