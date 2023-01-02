using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.SubGraph
{
    [HideNodeMenu]
    public class SubEntryNode : Node
    {
        [Output(ShowBackingValue.Never,ConnectionType.Override,TypeConstraint.None)]
        public Node Output;
    }

}
