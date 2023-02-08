// ---------------------------------------------------------  
// SceneViewButton.cs  
//   
// 作成日:  
// 作成者:  sasaki rio
// ---------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[InitializeOnLoad]
[CustomEditor(typeof(TestEditorObject))]
public class SceneViewButton : Editor
{
    private const float ButtonWidth = 50.0f;
    private const float ButtonHeight = 30.0f;

    static SceneViewButton()
    {
        //SceneView.onSceneGUIDelegate += OnGUI;
        SceneView.duringSceneGui += OnGUI;
    }

    private static void OnGUI(SceneView sceneView)
    {
        //2D描画の開始
        Handles.BeginGUI();

        if (GUILayout.Button("ボタン", 
            GUILayout.Width(ButtonWidth),
            GUILayout.Height(ButtonHeight)))
        {
            
        }

        //2D描画の終了
        Handles.EndGUI();
    }



    /// <summary>
    /// DisplayDialog
    /// </summary>
    private void DisplayDialog()
    {
        EditorUtility.DisplayDialog("ボタンが押されました", "ボタンが押しましたか？", "押しました", "いいや押してない");
    }



    /// <summary>
    /// DisplayProgressBar
    /// </summary>
    private void DisplayProgressBar()
    {
        int loopCount = 1000000;
        for (int i = 0; i < loopCount; i++)
        {
            float progress = (float)i / loopCount;
            EditorUtility.DisplayProgressBar(
                "ループ中",
                i.ToString() + "回目(" + (progress * 100).ToString("F2") + "%)",
                progress);
        }
        EditorUtility.ClearProgressBar();
    }



    /// <summary>
    /// DisplayCancelableProgressBar
    /// </summary>
    private void DisplayCancelableProgressBar()
    {
        int loopCount = 1000000;
        for (int i = 0; i < loopCount; i++)
        {
            float progress = (float)i / loopCount;
            EditorUtility.DisplayCancelableProgressBar(
                "ループ中",
                i.ToString() + "回目(" + (progress * 100).ToString("F2") + "%)",
                progress);
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Tools/CollectDependencies")]
    private static void CollectDependencies()
    {
        var objects = Selection.objects;
        var dependencies = EditorUtility.CollectDependencies(objects);
        Selection.objects = dependencies;
    }



    [MenuItem("Tools/CollectDeepHierarchy")]
    private static void CollectDeepHierarchy()
    {
        var objects = Selection.objects;
        var DeepHierarchy = EditorUtility.CollectDeepHierarchy(objects);
    }




}

public sealed class SasakiWindow : EditorWindow
{
    [MenuItem("Window/SasakiEditor/Show")]
    private static void ShowWindow()
    {
        GetWindow<SasakiWindow>().Show();
        //var window = GetWindow<SasakiWindow>();
        //window.Show();
        //Debug.Log("static関数が動いたよ！");
    }


    [MenuItem("Window/SasakiEditor/ShowModal")]
    private static void ShowModalWindow()
    {
        GetWindow<SasakiWindow>().ShowModal();
        //var window = GetWindow<SasakiWindow>();
        //window.Show();
        //Debug.Log("static関数が動いたよ！");
    }


    [MenuItem("Window/SasakiEditor/ShowUtility")]
    private static void ShowUtilityWindow()
    {
        CreateInstance<SasakiWindow>().ShowUtility();
        //var window = GetWindow<SasakiWindow>();
        //window.Show();
        //Debug.Log("static関数が動いたよ！");
    }
}
