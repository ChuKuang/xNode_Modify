using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode.SubGraph;

namespace XNodeEditor.SubGraph
{
    [CustomNodeEditor(typeof(SubGraphNode))]
    public class SubGraphNodeEditor : NodeEditor
    {
        private bool isDoubleClick = false;

        public override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            DoEvent();
        }


        public override void OnBodyGUI()
        {
            serializedObject.Update();
            string[] excludes = { "m_Script", "graph", "position", "ports", "targetGraph" };

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);
            }

            serializedObject.ApplyModifiedProperties();
            DoEvent();
        }


        public override void OnCreate()
        {
            base.OnCreate();
        }


        public void DoEvent()
        {
			Event e = Event.current;
			switch (e.type)
			{
				case EventType.MouseDown:
                    isDoubleClick = (e.clickCount == 2);
                    break;
				case EventType.MouseUp:
                    if (isDoubleClick)
                    {
                        NodeEditorWindow.Open((target as SubGraphNode).targetGraph);
                        isDoubleClick = false;
                        e.Use();
                    }
                    break;
			}

		}


        public override void AddContextMenuItems(GenericMenu menu)
        {
            bool canRemove = true;
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node)
            {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddItem(new GUIContent("Rename"), false, NodeEditorWindow.current.RenameSelectedNode);
                canRemove = NodeGraphEditor.GetEditor(node.graph, NodeEditorWindow.current).CanRemove(node);

                menu.AddItem(new GUIContent("Enter"), false, () => { NodeEditorWindow.Open((node as SubGraphNode).targetGraph); });
            }

            menu.AddSeparator("");

            if (canRemove) menu.AddItem(new GUIContent("Remove"), false, NodeEditorWindow.current.RemoveSelectedNodes);
            else menu.AddItem(new GUIContent("Remove"), false, null);
        }


        public override Color GetTint()
        {
            return new Color(0, 0.63f, 1f, 0.63f);
        }



        //修改节点名时对应修改graph的名字
        public override void OnRename()
        {
            SubGraphNode subNode = target as SubGraphNode;
            subNode.targetGraph.name = subNode.name;
        }
    }


}
