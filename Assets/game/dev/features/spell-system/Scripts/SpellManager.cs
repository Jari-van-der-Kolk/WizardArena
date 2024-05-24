using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public enum ElementType
{
    Fire,
    Ice,
    Lightning
}

public class SpellManager : MonoBehaviour, IDependencyProvider
{

    [Provide]
    public SpellManager ProvideSpellManager()
    {
        return this;
    }

    public List<SpellStratagy> spellDataList;

    private Dictionary<SpellStratagy, KeyCombination> _keyCombinations;

    private void Start()
    {
        _keyCombinations = new Dictionary<SpellStratagy, KeyCombination>();
        foreach (var spellData in spellDataList)
        {
            _keyCombinations[spellData] = new KeyCombination(spellData.keyCombination);
        }
    }

   
    private void CreateSpell()
    {
        foreach (var spellData in _keyCombinations.Keys)
        {
            if (CheckCombination(_keyCombinations[spellData]))
            {
                
            }
        }
    }

    private bool CheckCombination(KeyCombination keyCombination)
    {
        foreach (KeyCode key in keyCombination.KeySequence)
        {
            if (Input.GetKeyDown(key))
            {
                if (keyCombination.CheckKey(key))
                {
                    return true;
                }
            }
        }
        return false;
    }
}


// Key Combination Class
public class KeyCombination
{
    public List<KeyCode> KeySequence { get; private set; }
    private int currentIndex;

    public KeyCombination(List<KeyCode> keySequence)
    {
        KeySequence = keySequence;
        currentIndex = 0;
    }

    public bool CheckKey(KeyCode key)
    {
        if (key == KeySequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= KeySequence.Count)
            {
                currentIndex = 0;
                return true;
            }
        }
        else
        {
            currentIndex = 0;
        }
        return false;
    }
}



