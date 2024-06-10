using Codice.Client.BaseCommands.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayerSpellCaster : MonoBehaviour
{
    //config
    [SerializeField] private KeyCode _useSpellKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _cancelSpellKey = KeyCode.None;
    [SerializeField] private TargetLayerData _castableTargetLayerData;

    private KeyCode[] ignoreKeys = 
    {
        KeyCode.W, KeyCode.D, KeyCode.A, KeyCode.S , KeyCode.Mouse0, KeyCode.Mouse1,
        KeyCode.Escape, KeyCode.Space        
    };
    

    //mutable
    [SerializeField]private List<KeyCode> _pressedKeys = new List<KeyCode>();


    private void Update()
    {
        KeyCode[] keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

        for (int i = 0; i < keyCodes.Length; i++)
        {
            KeyCode keyCode = keyCodes[i];

            if (Input.GetKeyDown(keyCode) && !IsKeyIgnored(keyCode))
            {
                _pressedKeys.Add(keyCode);

                Debug.Log("Key Pressed: " + keyCode);
            }
        }

        if (Input.GetKeyDown(_useSpellKey) && _pressedKeys.Count > 0)
        {
            SpellManager.CastSpellByKeyID(_pressedKeys, transform, _castableTargetLayerData);  
            _pressedKeys.Clear();

        }

        if (Input.GetKeyDown(_cancelSpellKey))
        {
            _pressedKeys.Clear();
        }
    }
    private bool IsKeyIgnored(KeyCode keyCode)
    {
        for (int i = 0; i < ignoreKeys.Length; i++)
        {
            if (keyCode == ignoreKeys[i])
            {
                return true;
            }
        }
        return false;
    }
}
