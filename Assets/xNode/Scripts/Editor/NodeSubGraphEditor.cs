using FpNodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNode.SubGraph;

namespace XNodeEditor.SubGraph
{
    [CustomNodeGraphEditor(typeof(NodeSubGraph))]
    public class NodeSubGraphEditor : NodeGraphEditor
    {

        public override void OnOpen()
        {
            AddAllRequired();
        }

        public override void OnGUI() 
        {
            DrawHeader();
            DrawEvent();
        }

        private void DrawEvent()
        {
            if (Event.current.modifiers == EventModifiers.Control && Event.current.keyCode == KeyCode.S)
            {
                AssetDatabase.SaveAssets();
                Event.current.Use();
            }
        }
        public void DrawHeader()
        {
            NodeSubGraph graph = target as NodeSubGraph;
            GUILayout.BeginArea(new Rect(0f, 0.0f, window.position.width, 18f), "", "HeaderButton");
            GUILayout.BeginHorizontal() ;
            {
                nodeGraphs.Clear();
                nodeGraphs.Push(graph);
                DrawGraphSequence(graph);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }


        Stack<NodeGraph> nodeGraphs = new Stack<NodeGraph>();
        private void DrawGraphSequence(NodeGraph graph)
        {
            if (graph is NodeSubGraph)
            {
                NodeGraph parent = (graph as NodeSubGraph).parentGraph;
                nodeGraphs.Push(parent);
                DrawGraphSequence(parent);
            }

            if(nodeGraphs.Count > 0)
            {
                NodeGraph targetGraph = nodeGraphs.Pop();
                if (targetGraph is NodeSubGraph)
                {
                    NodeSubGraph sunGraph = targetGraph as NodeSubGraph;
                    DrawGraphButton(sunGraph, sunGraph.name, false);
                }
                else
                    DrawGraphButton(targetGraph, targetGraph.name, true);

            }
        }


        private void DrawGraphButton(NodeGraph graph, string name, bool isOriginGraph)
        {
            GUIStyle style = isOriginGraph ? "GUIEditor.BreadcrumbLeftBackground" : "GUIEditor.BreadcrumbMidBackground" ;
            name = "  " + name;
            float width = CaculateWidth(name);
            if(GUILayout.Button(name, style, GUILayout.Width(width), GUILayout.ExpandHeight(true)))
            {
                NodeEditorWindow.Open(graph);
            }


        }


        private float CaculateWidth(string str)
        {
            return str.Length * 7f;
        }


        private void AddAllRequired()
        {
            NodeSubGraph graph = target as NodeSubGraph;
            Type graphType = graph.GetType();
            NodeGraph.RequireNodeAttribute[] attribs = Array.ConvertAll(
                graphType.GetCustomAttributes(typeof(NodeGraph.RequireNodeAttribute), true), x => x as NodeGraph.RequireNodeAttribute);
            Name = ParseGraphConfig.GetGraphName();

            Vector2 position = Vector2.zero;
            foreach (NodeGraph.RequireNodeAttribute attrib in attribs)
            {
                if (attrib.type0 != null) AddRequired(graph, attrib.type0, ref position);
                if (attrib.type1 != null) AddRequired(graph, attrib.type1, ref position);
                if (attrib.type2 != null) AddRequired(graph, attrib.type2, ref position);
            }
        }

        private void AddRequired(NodeGraph graph, Type type, ref Vector2 position)
        {
            if (!graph.nodes.Any(x => x.GetType() == type))
            {
                XNode.Node node = graph.AddNode(type);
                node.position = position;
                position.x += 300;
                if (node.name == null || node.name.Trim() == "") node.name = NodeEditorUtilities.NodeDefaultName(type);
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(graph)))
                {
                    node.hideFlags = HideFlags.HideInHierarchy;
                    AssetDatabase.AddObjectToAsset(node, graph);
                }
                   
            }
        }
    }

}
