using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Job.SpellSystem.Data;

namespace Job.SpellSystem
{
    [CustomEditor(typeof(SpellSO))]
    [CanEditMultipleObjects]
    public class SpellEditor : Editor
    {
        private bool _showAddComponent;
        private static List<Type> _spellDataTypes = new();
        private SpellSO spellSO;

        private void OnEnable() => spellSO = target as SpellSO;

        public override void OnInspectorGUI()
        {
            if (spellSO == null)
                return;
            base.OnInspectorGUI();

            //creates the foldout for the addComponent buttons
            EditorGUILayout.Space(10);
            _showAddComponent = EditorGUILayout.Foldout(_showAddComponent, "Add Components");

            if (!_showAddComponent)
                return;
            //generates the buttons
            foreach (var type in _spellDataTypes)
            {
                if (!GUILayout.Button(type.Name))
                    continue;
                
                //adds the data to the list in the SO
                ComponentData spellComponent = Activator.CreateInstance(type) as ComponentData;

                if (spellComponent == null)
                    return;

                spellSO.AddSpellComponent(spellComponent);

                EditorUtility.SetDirty(spellSO);
            }

        }



        /// <summary>
        /// Gets all SpellData subclasses and puts them in the spellDataTypes list.
        /// </summary>
        [DidReloadScripts]
        private static void OnRecompile()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes());
            var filteredTypes = types.Where(
                type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass
            );
            _spellDataTypes = filteredTypes.ToList();
        }
    }
}