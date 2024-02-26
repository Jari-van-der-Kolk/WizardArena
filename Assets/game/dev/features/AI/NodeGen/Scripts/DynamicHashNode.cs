using Saxon.NodePositioning;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Saxon.Player
{
    public class DynamicHashNode : HashNode
    {
        NodeGenerator nodeGenerator;

        private void Awake()
        {
            nodeGenerator = FindObjectOfType<NodeGenerator>();
        }

        void Update()
        {
            UpdateHashGridKey();
        }

        private bool checkHashKeyID => ID != nodeGenerator.hashGrid.GetGridKey(transform.position);
       
        public void UpdateHashGridKey()
        {
            if(checkHashKeyID)
            {
                nodeGenerator.hashGrid.Remove(this, ID);

                Vector3Int newKey = nodeGenerator.hashGrid.GetGridKey(transform.position);

                nodeGenerator.hashGrid.Add(this, transform.position);

                ID = newKey;
            }
          
        }  

    }
}