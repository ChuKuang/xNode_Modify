using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.SubGraph
{
    [RequireNode(typeof(SubEntryNode), typeof(SubExitNode))]
    public class NodeSubGraph : NodeGraph
    {
        /// <summary>
        /// 多层嵌套 记录上层graph
        /// </summary>
        public NodeGraph parentGraph;

    }
}

