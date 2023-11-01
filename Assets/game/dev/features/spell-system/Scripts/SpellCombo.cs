using System;
using System.Collections.Generic;

namespace Job.SpellSystem
{
    [Serializable]
    public class SpellCombo
    {
        public EShapes shape;
        public List<EElements> elements = new();
        public List<EActivation> activations = new();

        public void ClearCombo()
        {
            shape = EShapes.None;
            elements = new();
            activations = new();
        }
    }
}

