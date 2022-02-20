using LiveCharts;
using LiveCharts.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Wpf_Run.Models;

namespace Wpf_Run.Services
{
    class ViewsModuleData
    {
        private readonly string PATH;
        private List<BindingList<RunnersDayData>> _Files = new List<BindingList<RunnersDayData>>();
        List<string> FileNames = new List<string>();
        private BindingList<RunnersDayData> _RunnersOne;

        private void ShowDateGrid(string name)
        {
           
        }
        public ViewsModuleData(string path)
        {
            PATH = path;
        }
        public void LoadDate() 
        {
            FileNames = this.GetFilesNames(PATH);
            foreach (string file in FileNames )
            {
                using (var reader = File.OpenText(file))
                {
                    var FileTExt = reader.ReadToEnd();
                    _RunnersOne = JsonConvert.DeserializeObject<BindingList<RunnersDayData>>(FileTExt);
                    _Files.Add(_RunnersOne);
                }
            }
            
        }
        public BindingList<RunnersAllData> FillDateGrid()
        {
            List<string> Names = new List<string>();
            RunnersAllData runner = new RunnersAllData();
            BindingList<RunnersAllData> list = new BindingList<RunnersAllData>();
            double avr=0;
            int count = 0;
            foreach (var file in _Files)
            {
                foreach (var record in file)
                {
                    if (!Names.Contains(record.User))
                    {
                        Names.Add(record.User);
                    }
                }
            }
            foreach (var name in Names)
            {
                runner = new RunnersAllData();
                runner.User = name;
                foreach (var file in _Files)
                {
                    foreach (var record in file)
                    {
                        if (runner.User == record.User)
                        {
                            if (runner.minimum_Staps == 0)
                                runner.minimum_Staps = record.Steps;
                            if (runner.maximum_staps < record.Steps)
                            {
                                runner.maximum_staps = record.Steps;
                            }
                            if (runner.minimum_Staps > record.Steps)
                            {
                                runner.minimum_Staps = record.Steps;
                            }
                            avr = +record.Steps;
                            count++;
                        }
                    }
                }
                runner.average_staps = Math.Round(avr / count, 2);
                avr = 0;
                count = 0;
                if (!list.Contains(runner))
                    list.Add(runner);
            }
            return list;
        }

        public void save_file(RunnersAllData Object)
        {
            TextWriter writer = null;
            string FilePath;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = Object.User;
            dlg.DefaultExt = ".json";
            dlg.Filter = "Json-files (.json)|*.json";
            if (dlg.ShowDialog() == true)
            {
                FilePath = dlg.FileName;
                var contentsToWriteToFile = JsonConvert.SerializeObject(Object);
                writer = new StreamWriter(FilePath);
                writer.WriteLine(contentsToWriteToFile);
                foreach (var file in _Files)
                {
                    foreach (var record in file)
                    {
                        if(record.User == Object.User)
                        {
                            contentsToWriteToFile = JsonConvert.SerializeObject(record);
                            writer.WriteLine(contentsToWriteToFile);
                        }
                    }
                }
                writer.Close();
            }
        }

        public ChartValues<ObservablePoint> GetChartValues(RunnersAllData UserName, ref int max, ref int min)
        {
            int count = 0;
            ChartValues<ObservablePoint> ponints = new ChartValues<ObservablePoint>();
            foreach (var file in _Files)
            {
                foreach (var record in file)
                {
                    if (min == 0)
                        min = record.Steps;
                    if (record.User == UserName.User)
                    {
                        count++;
                        if (min > record.Steps)
                            min = record.Steps;
                        if (max < record.Steps)
                            max = record.Steps;
                        ponints.Add(new ObservablePoint(count,record.Steps));
                    }
                }
            }
            return ponints;
        }
        private List<string> GetFilesNames(String mypath)
        {

            return Directory
                .GetFiles(mypath, "*", SearchOption.AllDirectories)
                .ToList();
        }


    }
}
