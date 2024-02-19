using System;
using System.Collections.Generic;
using UnityEngine;
using Job.SpellSystem.Spells;

namespace Job.SpellSystem
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private KeyCode[] _comboKeys;
        [SerializeField] private KeyCode _resetComboKey;

        private EShapes[] _selectedShapes = new EShapes[2];
        private EElements[] _selectedElements = new EElements[2];
        private EActivation[] _selectedActivation = new EActivation[2];

        //put back to private when done debugging
        public SpellCombo _currentCombo;

        private void Start()
        {
            GetSelectedCastOptions();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_resetComboKey))
            {
                _currentCombo.ClearCombo();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!CheckValidCombo())
                    return;

                SpellSO spell = SpellManager.Instance.GetSpellFromCombo(_currentCombo);
                if (spell == null)
                {
                    Debug.Log("No spell with this combo exist");
                }
                else
                {
                    CastSpell(spell);
                }

                _currentCombo.ClearCombo();
            }

            if (_currentCombo.shape == EShapes.None)
            {
                CheckShapeInput();
                return;
            }

            if (_currentCombo.elements.Count > 0)
            {
                CheckInput(_selectedActivation, _currentCombo.activations, _comboKeys, 4, 5);
            }

            if (_currentCombo.activations.Count <= 0)
            {
                CheckInput(_selectedElements, _currentCombo.elements, _comboKeys, 2, 3);
                return;
            }
        }

        private void CheckShapeInput()
        {
            for (int i = 0; i < 2; i++)
            {
                if (Input.GetKeyDown(_comboKeys[i]))
                {
                    if (_currentCombo.shape == EShapes.None)
                    {
                        _currentCombo.shape = _selectedShapes[i];
                    }

                    return;
                }
            }
        }

        private void CheckInput<T>(T[] selectedItems, List<T> currentItems, KeyCode[] inputKeys, int startIndex,
            int endIndex)
        {
            // Iterate through the specified range of input keys
            for (int i = startIndex; i <= endIndex; i++)
            {
                // Check if the current input key is pressed, if not, exit the method
                if (!Input.GetKeyDown(inputKeys[i])) continue;
                // If no items are selected, add the item corresponding to the pressed key
                if (currentItems.Count == 0)
                    currentItems.Add(selectedItems[i - startIndex]);

                // If the newly selected item is the same as the already selected item or two items are already selected, exit the method
                if (currentItems[0].Equals(selectedItems[i - startIndex]) || currentItems.Count >= 2)
                    return;

                // Add the newly selected item to the list of current items
                currentItems.Add(selectedItems[i - startIndex]);
            }
        }

        private void GetSelectedCastOptions()
        {
            _selectedShapes = SpellManager.Instance.GetSelectedShapes();
            _selectedElements = SpellManager.Instance.GetsSelectedElements();
            _selectedActivation = SpellManager.Instance.GetSelectedActivations();
        }

        private bool CheckValidCombo()
        {
            bool shapeCorrect = _currentCombo.shape != EShapes.None;
            bool elementCorrect = _currentCombo.elements.Count > 0;
            bool activationCorrect = _currentCombo.activations.Count > 0;

            return shapeCorrect && elementCorrect && activationCorrect;
        }

        private void CastSpell(SpellSO spell)
        {
            GameObject spellObject = new GameObject(spell.name);
            SpellHolder holder = spellObject.AddComponent<SpellHolder>();
            holder.SetSpellSO(spell);
            
            List<Spell> components = new List<Spell>();
            foreach (var component in spell.GetAllDependencies())
            {
                Spell spellComponent = (Spell)spellObject.AddComponent(component);
                
                components.Add(spellComponent);
            }
            holder.SetSpellComponents(components);
            holder.LaunchSpell();
        }
    }
}