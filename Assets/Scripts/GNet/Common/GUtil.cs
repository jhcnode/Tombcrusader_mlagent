using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

namespace DeepGes
{
    public class GUtil
    {
       
        public static byte[] ConvertToBytes(double[] values)
        {
            return values.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
        }

        public static double[] ConvertToDoubles(byte[] bytes)
        {
            return Enumerable.Range(0, bytes.Length / sizeof(double))
                .Select(offset => BitConverter.ToDouble(bytes, offset * sizeof(double)))
                .ToArray();
        }
      
        public static void getInterpolationBetweenLastVector(ref List<Vector3> v,float pixel_Interpolation)
        {

            List<Vector3> point_list = new List<Vector3>();
            //Interpolation Threshold에 따라 처리
            for (float t = pixel_Interpolation; t < 1; t += pixel_Interpolation)
            {
                Vector3 from = new Vector3(v[v.Count - 2].x, v[v.Count - 2].y, v[v.Count - 2].z);
                Vector3 to = new Vector3(v[v.Count - 1].x, v[v.Count - 1].y, v[v.Count - 1].z);
                Vector3 point = Vector3.Lerp(from, to, t);
                point_list.Add(point);
            }

            //Point List를 points에 Insert(Interpolation)
            for (int i = 0; i < point_list.Count; i++) v.Insert(v.Count - 1, new Vector3(point_list[i].x, point_list[i].y, point_list[i].z));


        }

        public static List<Vector3> getSupplementVector(List<Vector3>  v)
        {

            float outputLength =(float) GConfig.MaxPoint;


            float pixel_interpolation = Mathf.Ceil((outputLength/(float)v.Count));

            List<Vector3> new_v = new List<Vector3>();

            for (int i=0; i<v.Count-1; ++i)
            {
                new_v.Add(v[i]);
                for (float t = pixel_interpolation; t < 1; t += pixel_interpolation)
                {
                    Vector3 from = new Vector3(v[i].x, v[i].y, v[i].z);
                    Vector3 to = new Vector3(v[i+1].x, v[i+1].y, v[i+1].z);
                    Vector3 point = Vector3.Lerp(from, to, t);
                    new_v.Add(point);
                }
                new_v.Add(v[i+1]);
            }


            return new_v;


        }

        public static float getMin(float min, float nMin)
        {
            if (nMin < min) { min = nMin; }
            return min;
        }

        public static float getMax(float max, float nMax)
        {
            if (nMax > max) { max = nMax; }
            return max;
        }
        //Preprocess
        public static double[] Preprocess(List<Vector3> input, GConfig.Hand hand)
        {
            int count = input.Count;
            if (input.Count < GConfig.MaxPoint)
                input = getSupplementVector(input);


            input = EliminateVector(input);
            input = NormalizeVector(input);

            List<double> tmpList = new List<double>();
            tmpList.Add((int)hand);
            for (int i = 0; i < input.Count; ++i)
            {
                Vector3 p = input[i];
                tmpList.Add(p.x);
                tmpList.Add(p.y);
                tmpList.Add(p.z);
            }
            return tmpList.ToArray();
        }
        //Preprocess
        public static float[] Preprocess1(List<Vector3> input, GConfig.Hand hand)
        {
            int count = input.Count;
            if (input.Count < GConfig.MaxPoint)
                input = getSupplementVector(input);


            input = EliminateVector(input);
            input = NormalizeVector(input);

            List<float> tmpList = new List<float>();
            tmpList.Add((int)hand);
            for (int i = 0; i < input.Count; ++i)
            {
                Vector3 p = input[i];
                tmpList.Add(p.x);
                tmpList.Add(p.y);
                tmpList.Add(p.z);
            }
            return tmpList.ToArray();
        }
        //SubDivideVector
        public static List<Vector3> EliminateVector(List<Vector3> input)
        {
            int outputLength = GConfig.MaxPoint;

            float f_interval = Mathf.Round((input.Count * 1f) / (outputLength * 1f));
            int interval = (int)f_interval;
            List<Vector3> output = new List<Vector3>();

            for (int i = input.Count - 1; output.Count < outputLength; i -= interval)
            {
                if (i > 0) { output.Add(input[i]); }
                else { output.Add(input[0]); }
            }
            return output;
        }
        //NormalizeVector
        public static List<Vector3> NormalizeVector(List<Vector3> input)
        {
            float minX, maxX;
            float minY, maxY;
            float minZ, maxZ;


            Vector3 first = input[0];
            minX = maxX = first.x;
            minY = maxY = first.y;
            minZ = maxZ = first.z;

            for (int i = 0; i < input.Count; ++i)
            {
                Vector3 p = input[i];

                minX = getMin(minX, p.x);
                maxX = getMax(maxX, p.x);

                minY = getMin(minY, p.y);
                maxY = getMax(maxY, p.y);

                minZ = getMin(minZ, p.z);
                maxZ = getMax(maxZ, p.z);
            }

            float dist_x = Mathf.Abs(maxX - minX);
            float dist_y = Mathf.Abs(maxY - minY);
            float dist_Z = Mathf.Abs(maxZ - minZ);

            float axis_Max = dist_x;
            axis_Max = getMax(axis_Max, dist_y);
            axis_Max = getMax(axis_Max, dist_Z);

            Matrix4x4 translation = Matrix4x4.identity;
            translation[0, 3] = -minX;
            translation[1, 3] = -minY;
            translation[2, 3] = -minZ;

            Matrix4x4 scale = Matrix4x4.identity;
            scale[0, 0] = 1 / axis_Max;
            scale[1, 1] = 1 / axis_Max;
            scale[2, 2] = 1 / axis_Max;


            List<Vector3> normalizePoint = new List<Vector3>();
            for (int i = 0; i < input.Count; ++i)
            {
                Vector3 p = input[i];
                Vector3 newPoint = translation.MultiplyPoint3x4(p);
                newPoint = scale.MultiplyPoint3x4(newPoint);
                normalizePoint.Add(newPoint);
            }
            return normalizePoint;
        }
        //Gesture Data - Export
        public static byte[] ExportGesture(ref List<Vector3> point, GConfig.Hand hand)
        {
            double[] v = Preprocess(point, hand);
            byte[] data=GUtil.ConvertToBytes(v);
            return data;
        }

        //Gesture Data - Export data
        public static void SaveData(ref List<Vector3> data, int label)
        {
       
            string parent_path = Application.dataPath + "/CollectData/";
#if UNITY_EDITOR
            parent_path = "../CollectData/";
#endif
            if (Directory.Exists(parent_path) == false)
            {
                Directory.CreateDirectory(parent_path);
            }

            string sub_parent_path = parent_path + label;

            if (Directory.Exists(parent_path) == false)
            {
                Directory.CreateDirectory(parent_path);
            }



            string path = sub_parent_path + "/raw/";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            DateTime date = System.DateTime.Now;
            StreamWriter w = File.CreateText(path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".vec");
            for (int i = 0; i < data.Count; ++i)
            {
                w.WriteLine(data[i].x.ToString() + "," + data[i].y.ToString() + "," + data[i].z.ToString());
            }
            w.Close();



            path = sub_parent_path + "/preprocess/";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter w1 = File.CreateText(path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".d3g");
            double[] f_data = Preprocess(data, GConfig.Hand.RIGHT);
            Debug.Log(f_data.Length);
            for (int i = 0; i < f_data.Length; ++i)
            {
                if (i == f_data.Length - 1)
                    w1.Write(f_data[i]);
                else
                    w1.Write(f_data[i] + ",");
            }
            w1.Close();

        }
        //Gesture Data - Export data(include local data)
        public static List<Vector3> ReadData(string path)
        {
            List<Vector3> rVec = new List<Vector3>();
            StreamReader reader = new StreamReader(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] val = line.Split(',');
                float x = float.Parse(val[0]);
                float y = float.Parse(val[1]);
                float z = float.Parse(val[2]);
                Vector3 v = new Vector3(x, y, z);
                rVec.Add(v);
            }

            reader.Close();

            return rVec;
        }
        //Gesture Data - Export data(include local data)
        public static void SaveData(ref List<Vector3> data, ref List<Vector3> t_data, int label)
        {

            string parent_path = Application.dataPath + "/CollectData/";
#if UNITY_EDITOR
            parent_path = "../CollectData/";
#endif
            if (Directory.Exists(parent_path) == false)
            {
                Directory.CreateDirectory(parent_path);
            }

            string sub_parent_path = parent_path + label;

            if (Directory.Exists(parent_path) == false)
            {
                Directory.CreateDirectory(parent_path);
            }



            string path = sub_parent_path + "/raw/";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            DateTime date = System.DateTime.Now;
            StreamWriter w = File.CreateText(path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".vec");
            for (int i = 0; i < data.Count; ++i)
            {
                w.WriteLine(data[i].x.ToString() + "," + data[i].y.ToString() + "," + data[i].z.ToString());
            }
            w.Close();



            path = sub_parent_path + "/preprocess/";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter w1 = File.CreateText(path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".d3g");
            double[] f_data = Preprocess(data, GConfig.Hand.RIGHT);
            for (int i = 0; i < f_data.Length; ++i)
            {
                if (i == f_data.Length - 1)
                    w1.Write(f_data[i]);
                else
                    w1.Write(f_data[i] + ",");
            }
            w1.Close();


            path = sub_parent_path + "/preprocess(local)/";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter w2 = File.CreateText(path + date.ToString("yyyy_MM_dd_hh_mm_ss.fffffff_tt") + ".d3g");
            double[] tf_data = Preprocess(t_data, GConfig.Hand.RIGHT);
            for (int i = 0; i < tf_data.Length; ++i)
            {
                if (i == tf_data.Length - 1)
                    w2.Write(tf_data[i]);
                else
                    w2.Write(tf_data[i] + ",");
            }
            w2.Close();



        }


    }
}


