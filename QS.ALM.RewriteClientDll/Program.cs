using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QS.ALM.RewriteClientDll
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = "..\\..\\..\\TestType4DotNet\\bin\\Debug";
            string dest = "C:\\Users\\Ticomsoft_L1\\AppData\\Local\\HP\\ALM-Client\\localhost";

            string name = "ALM-Client".ToLower();
            string name1 = "iexplore".ToLower();
            string name2 = "dllhost".ToLower();
            System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process anti in etc)
                if (anti.ProcessName.ToLower().Contains(name) || anti.ProcessName.ToLower().Contains(name1) || anti.ProcessName.ToLower().Contains(name2))
                { 
                    anti.Kill(); 
                }
            System.Threading.Thread.Sleep(1000);
            File.Copy(Path.Combine(source, "CustomTestType.dll"), Path.Combine(dest, "CustomTestType.dll"), true);
            File.Copy(Path.Combine(source, "CustomTestType.pdb"), Path.Combine(dest, "CustomTestType.pdb"), true);
            File.Copy(Path.Combine(source, "QS.ALM.CloudShellApi.dll"), Path.Combine(dest, "Quali", "QS.ALM.CloudShellApi.dll"), true);
            File.Copy(Path.Combine(source, "QS.ALM.CloudShellApi.pdb"), Path.Combine(dest, "Quali", "QS.ALM.CloudShellApi.pdb"), true);
            File.Copy(Path.Combine("..\\..\\..\\CSRemoteAgent\\bin\\Debug", "RemoteAgent.dll"), Path.Combine(dest, "Quali", "RemoteAgent.dll"), true);
            File.Copy(Path.Combine("..\\..\\..\\CSRemoteAgent\\bin\\Debug", "RemoteAgent.pdb"), Path.Combine(dest, "Quali", "RemoteAgent.pdb"), true);
        }
    }
}
