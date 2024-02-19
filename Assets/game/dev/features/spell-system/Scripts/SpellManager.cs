using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Job.SpellSystem
{

    public class SpellManager : MonoBehaviour
    {
        public static SpellManager Instance;
        [SerializeField] private SpellSO[] _spells;

        [SerializeField] private EShapes[] _selectedShapes = new EShapes[2];
        [SerializeField] private EElements[] _selectedElements = new EElements[2];
        [SerializeField] private EActivation[] _selectedActivation = new EActivation[2];

        private List<SpellSO> _possibleSpells = new List<SpellSO>();

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            GetPossibleSpells();
        }
        private void GetPossibleSpells()
        {
            foreach (SpellSO spell in _spells)
            {
                bool shapesMatch = _selectedShapes.Contains(spell.spellCombo.shape);
                bool elementsMatch = spell.spellCombo.elements.All(_selectedElements.Contains);
                bool activationsMatch = spell.spellCombo.activations.All(_selectedActivation.Contains);

                if (shapesMatch && elementsMatch && activationsMatch)
                {
                    _possibleSpells.Add(spell);
                }
            }
        }

        public SpellSO GetSpellFromCombo(SpellCombo combo)
        {
            foreach (SpellSO spell in _possibleSpells)
            {
                bool shapesMatch = spell.spellCombo.shape == combo.shape;
                if (!shapesMatch)
                    continue;

                bool elementsMatch = spell.spellCombo.elements.OrderBy(x => x).SequenceEqual(combo.elements.OrderBy(x => x));
                if (!elementsMatch)
                    continue;

                bool activationsMatch = spell.spellCombo.activations.OrderBy(x => x).SequenceEqual(combo.activations.OrderBy(x => x));
                if (!activationsMatch)
                    continue;

                if (shapesMatch && elementsMatch && activationsMatch)
                {
                    return spell;
                }
            }

            return null;
        }

        public EShapes[] GetSelectedShapes()
        {
            return _selectedShapes;
        }

        public EElements[] GetsSelectedElements()
        {
            return _selectedElements;
        }

        public EActivation[] GetSelectedActivations()
        {
            return _selectedActivation;
        }
    }
}


