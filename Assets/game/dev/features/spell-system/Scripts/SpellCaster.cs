using System.Collections.Generic;
using UnityEngine;

namespace Job.SpellSystem
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private KeyCode[] _comboKeys;
        [SerializeField] private KeyCode _resetComboKey;

        [SerializeField] private EShapes[] _selectedShapes = new EShapes[2];
        [SerializeField] private EElements[] _selectedElements = new EElements[2];
        [SerializeField] private EActivation[] _selectedActivation = new EActivation[2];

        //put back to private when done debugging
        public SpellCombo _currentCombo;


        private void Update()
        {
            if (Input.GetKeyDown(_resetComboKey))
            {
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

        private void CheckInput<T>(T[] selectedItems, List<T> currentItems, KeyCode[] inputKeys, int startIndex, int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (Input.GetKeyDown(inputKeys[i]))
                {
                    if (currentItems.Count == 0)
                    {
                        currentItems.Add(selectedItems[i - startIndex]);
                    }
                    if (currentItems[0].Equals(selectedItems[i - startIndex]) || currentItems.Count >= 2)
                        return;
                    currentItems.Add(selectedItems[i - startIndex]);
                }
            }
        }
    }
}
