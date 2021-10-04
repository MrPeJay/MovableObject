using Settings;
using Sirenix.OdinInspector;
using UnityEngine;

public class SettingsReferences : MonoBehaviour
{
    [InfoBox(
        "Assign every settings scriptable object asset that is used in scripts. " +
        "Unity will not import the settings assets which are not referenced anywhere in" +
        " the editor which will result in null reference errors.",
        InfoMessageType.Warning)]

    [SerializeField]
    private GameSettings gameSettingsReference;
}