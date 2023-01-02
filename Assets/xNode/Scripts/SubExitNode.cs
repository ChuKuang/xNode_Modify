using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.SubGraph
{
    [HideNodeMenu]
    public class SubExitNode :Node
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)]
        public Node Input;
    }

}
