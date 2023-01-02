using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.SubGraph
{
    [CreateNodeMenu("SubGraph")]
    public class SubGraphNode : Node
    {
        [Input(ShowBackingValue.Never)]
        public Node Input;
        [Output(ShowBackingValue.Never)]
        public Node OutPut;

        //SubNode指向的目标SubGraph
        [HideInInspector]
        public NodeSubGraph targetGraph;


        public override void OnCreate()
        {
            targetGraph =  ScriptableObject.CreateInstance<NodeSubGraph>();
            targetGraph.parentGraph = this.graph;

        }


    }
}

