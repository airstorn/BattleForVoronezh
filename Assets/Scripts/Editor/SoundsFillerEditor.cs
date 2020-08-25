using GameStates;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoundsFillerEditor
{
    [MenuItem("Tools/Add sounds to Buttons on scene")]
    private static void FillButtonsSounds()
    {
        var rootObjects = Resources.FindObjectsOfTypeAll(typeof(Button));
        foreach (var vacantButton in rootObjects)
        {
            var button = vacantButton as Button;
            if (button != null)
            {
                ClickSoundInvoker invoker = button.GetComponent<ClickSoundInvoker>();

                if (invoker == null)
                {
                    button.gameObject.AddComponent<ClickSoundInvoker>();
                }
            }
        }
        
        AssetDatabase.SaveAssets();
    }
}
