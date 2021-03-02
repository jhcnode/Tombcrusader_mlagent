using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;





namespace DeepGes
{

    public class GClassifier : MonoBehaviour
    {
        private TextAsset graphModel;
        private TFGraph graph;
        private TFSession session;
        public string graph_path= "TFModels/freeze_graph";
        public string input_name="x";
        public string output_name = "prob_y";



        int ArgMax(float[,] array, int idx)
        {
            float max = -1;
            int maxIdx = -1;
            var l = array.GetLength(1);
            for (int i = 0; i < l; i++)
                if (array[idx, i] > max)
                {
                    maxIdx = i;
                    max = array[idx, i];
                }
            return maxIdx;
        }

        public void InitClassifier()
        {
            #if UNITY_ANDROID
                        TensorFlowSharp.Android.NativeBinding.Init();
            #endif
            graphModel = Resources.Load(graph_path) as TextAsset;
            graph = new TFGraph();
            graph.Import(graphModel.bytes);
            session = new TFSession(graph);
        }
        public int RunClassifier(float[] input)
        {
            TFSession.Runner runner = session.GetRunner();
            runner.AddInput(graph[input_name][0], input);
            runner.Fetch(graph[output_name][0]);
            float[,] result = runner.Run()[0].GetValue() as float[,];
            return ArgMax(result, 0);
        }
        public void ReleaseClassifier()
        {
            session.CloseSession();
        }

    }
}