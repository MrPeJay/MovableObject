using System;
using MovableObject;
using UnityEditor;

namespace Settings
{
    [Serializable]
    public class GameSettings : Settings<GameSettings>
    {
        public const string GameSettingsFileName = "GameSettings";

        public MovableObjectBase.Settings MovableObjectSettings;

#if UNITY_EDITOR

        [MenuItem("Settings/Game Settings")]
        public static void ShowItem() => ShowItem(GameSettingsFileName);

#endif
    }
}