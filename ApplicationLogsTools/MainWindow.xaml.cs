using Microsoft.Win32;
using Neo;
using Neo.Wallets;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationLogsTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string FileName;

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var input = new StringBuilder(TextInput.Text);
            var json = JObject.Parse(TextInput.Text);
            var stateValue = json["notifications"][0]["state"]["value"];
            if(stateValue[0]["type"].ToString() == "ByteArray")
                stateValue[0]["value"] = (stateValue[0]["value"].ToString()).HexToString();
            if(stateValue[1]["type"].ToString() == "ByteArray" && !string.IsNullOrEmpty(stateValue[1]["value"].ToString()))
                stateValue[1]["value"] = new UInt160(stateValue[1]["value"].ToString().HexToBytes()).ToAddress();
            if (stateValue[2]["type"].ToString() == "ByteArray" && !string.IsNullOrEmpty(stateValue[2]["value"].ToString()))
                stateValue[2]["value"] = new UInt160(stateValue[2]["value"].ToString().HexToBytes()).ToAddress();
            if (stateValue[3]["type"].ToString() == "ByteArray")
                stateValue[3]["value"] = new BigInteger(stateValue[3]["value"].ToString().HexToBytes()).ToString();
            TextOutput.Text = json.ToString();
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link; 
            else e.Effects = DragDropEffects.None;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            FileName = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            var text = File.ReadAllText(FileName);
            TextInput.Text = text;
            SaveMenu.IsEnabled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
                var text = File.ReadAllText(FileName);
                TextInput.Text = text;
                SaveMenu.IsEnabled = true;
            }
        }

        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(FileName, TextOutput.Text);
            StatusText.Text = "Save Successed";
        }
    }
}
