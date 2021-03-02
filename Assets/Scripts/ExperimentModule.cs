using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadWriteCsv;
using System.IO;


namespace Experiment
{
    [System.Serializable]
    public class ExperimentItem
    {
        //experiment condition
        public string signName;
        public int sign_total;
        public int sign_correct_total;
        public int sign_missing_total;

        public int key;
        public int value;

        //experiment result
        public int correct_count;
        public int missing_count;

    }


    public class ExperimentModule : MonoBehaviour
    {
        public List<ExperimentItem> ExperimentList;
        public void Add_CorrectCount(string signName)
        {

            for(int i=0; i< ExperimentList.Count; ++i)
            {
                if(ExperimentList[i].signName == signName)
                {
                    ExperimentList[i].correct_count += 1;
                    break;
                }
            }
        }
        public void Add_MissingCount(string signName)
        {

            for (int i = 0; i < ExperimentList.Count; ++i)
            {
                if (ExperimentList[i].signName == signName)
                {
                    ExperimentList[i].missing_count += 1;
                    break;
                }
            }
        }
        public void Update_Data()
        {
            for (int i = 0; i < ExperimentList.Count; ++i)
            {
                ExperimentList[i].sign_total += ExperimentList[i].key;
                ExperimentList[i].sign_correct_total += ExperimentList[i].correct_count;
                ExperimentList[i].sign_missing_total += ExperimentList[i].missing_count;
                ExperimentList[i].value = 0;
                ExperimentList[i].correct_count = 0;
                ExperimentList[i].missing_count = 0;
            }
           

        }
        public void Add_all_key(int All_addVal)
        {
            for (int i = 0; i < ExperimentList.Count; ++i)
            {
                ExperimentList[i].key += All_addVal;
            }
        }

        public void SaveTotalData(string filename)
        {
            if (gameObject.GetComponent<GameUpdater>().StageCount != 0)
            {
                // Write sample data to CSV file
                CsvFileWriter writer = new CsvFileWriter(getPath(filename));

                CsvRow row = new CsvRow();
                row.Add(string.Format("Stage"));
                row.Add(string.Format("SignName"));
                row.Add(string.Format("Stage Total"));
                row.Add(string.Format("Correct Total"));
                row.Add(string.Format("Missing Total"));
                writer.WriteRow(row);

                for (int i = 0; i < ExperimentList.Count; ++i)
                {
                    CsvRow nRow = new CsvRow();
                    nRow.Add(string.Format(gameObject.GetComponent<GameUpdater>().StageCount.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].signName));
                    nRow.Add(string.Format(ExperimentList[i].sign_total.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].sign_correct_total.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].sign_missing_total.ToString()));
                    writer.WriteRow(nRow);
                }
                writer.Close();

            }

        }
        public void SaveStageData(string read_file,string new_fileName)
        {
            if (gameObject.GetComponent<GameUpdater>().StageCount!=0)
            {
                //Write sample data to CSV file
                CsvFileWriter writer = new CsvFileWriter(getPath(new_fileName));
                if (File.Exists(getPath(read_file)) == true)
                {
                    ReadData(ref writer, getPath(read_file));
                    File.Delete(getPath(read_file));
                }
                else
                {

                    CsvRow row = new CsvRow();

                    row.Add(string.Format("Stage"));
                    row.Add(string.Format("SignName"));
                    row.Add(string.Format("Stage Total"));
                    row.Add(string.Format("Stage Correct Total"));
                    row.Add(string.Format("Stage Missing Total"));
                    writer.WriteRow(row);
                }


                for (int i = 0; i < ExperimentList.Count; ++i)
                {
                    CsvRow nRow = new CsvRow();
                    nRow.Add(string.Format(gameObject.GetComponent<GameUpdater>().StageCount.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].signName));
                    nRow.Add(string.Format(ExperimentList[i].key.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].correct_count.ToString()));
                    nRow.Add(string.Format(ExperimentList[i].missing_count.ToString()));
                    writer.WriteRow(nRow);
                }
                writer.Close();

            }


        }
        public void ReadData(ref CsvFileWriter writer,string filename)
        {
            // Read sample data from CSV file
            CsvFileReader reader = new CsvFileReader(filename);
            
            CsvRow row = new CsvRow();
            while (reader.ReadRow(row))
            {
                writer.WriteRow(row);
            }
            reader.Close();
   
        }
        // Following method is used to retrive the relative path as device platform
        private string getPath(string filename)
        {
            
#if UNITY_EDITOR
            return Application.dataPath + "/" + filename;
#elif UNITY_ANDROID
        return Application.persistentDataPath+"/"+filename;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+filename;
#else
        return Application.dataPath +"/"+filename;
#endif
        }
    }


}
