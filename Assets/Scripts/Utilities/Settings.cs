using System.IO;
using UnityEditor;
using UnityEngine;

namespace Settings
{
    public abstract class Settings<T> : SingletonScriptableObject<T> where T : ScriptableObject
    {
        public const string ScriptableObjectPath = "ScriptableObjects/Settings/";

#if UNITY_EDITOR

        public static void ShowItem(string settingsFileName)
        {
            var path = Path.Combine(Application.dataPath, "Resources", ScriptableObjectPath);
            var assetPath = Path.Combine("Assets", "Resources", ScriptableObjectPath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (File.Exists(assetPath + settingsFileName + ".asset"))
            {
                Selection.activeObject =
                    Resources.Load<T>(ScriptableObjectPath + settingsFileName);

                return;
            }

            var asset = CreateInstance<T>();

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPath + settingsFileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

#endif
    }
}