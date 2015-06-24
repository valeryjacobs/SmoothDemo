using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WindowsInput;

namespace SmoothDemo.Agent
{
    public class OSHelper
    {
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public List<int> WindowHandles { get; set; }
        public List<string> Windows { get; set; }
        public List<int> AddedWindows { get; set; }
        public List<string> Tokens { get; set; }
        public List<string> Encodings { get; set; }

        //OS command codes
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        public OSHelper()
        {
            Windows = new List<string>();
            WindowHandles = new List<int>();
            AddedWindows = new List<int>();
        }

        private List<int> GetTaskWindows()
        {
            List<int> windowHandles = new List<int>();
            Windows.Clear();

            // Get the desktopwindow handle
            int nDeshWndHandle = NativeWin32.GetDesktopWindow();
            // Get the first child window
            int nChildHandle = NativeWin32.GetWindow(nDeshWndHandle, NativeWin32.GW_CHILD);

            while (nChildHandle != 0)
            {
                //If the child window is this (SendKeys) application then ignore it.
                IntPtr windowHandle = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;
                if (nChildHandle == windowHandle.ToInt32())
                {
                    nChildHandle = NativeWin32.GetWindow(nChildHandle, NativeWin32.GW_HWNDNEXT);
                }

                // Get only visible windows
                if (NativeWin32.IsWindowVisible(nChildHandle) != 0)
                {
                    StringBuilder sbTitle = new StringBuilder(1024);
                    // Read the Title bar text on the windows to put in combobox
                    NativeWin32.GetWindowText(nChildHandle, sbTitle, sbTitle.Capacity);
                    String sWinTitle = sbTitle.ToString();
                    {
                        if (sWinTitle.Length > 0)
                        {
                            Windows.Add(sWinTitle);
                            WindowHandles.Add(nChildHandle);
                            windowHandles.Add(nChildHandle);
                        }
                    }
                }
                // Look for the next child.
                nChildHandle = NativeWin32.GetWindow(nChildHandle, NativeWin32.GW_HWNDNEXT);
            }

            return windowHandles;
        }

        public void ExecuteAction(SmoothDemo.Agent.Models.Action action)
        {
            try
            {
                switch (action.Type)
                {
                    case "Run":
                        Newtonsoft.Json.Linq.JArray actionParms = (Newtonsoft.Json.Linq.JArray)action.Content;
                        Run(actionParms[0].ToString(), actionParms.Count > 1 ? actionParms[1].ToString() : null);
                        break;
                    case "Input":
                        Input((string)action.Content);
                        break;
                    case "ReadInput":
                        ReadInput((string)action.Content);
                        break;
                    case "Show":
                        Show((string)action.Content);
                        break;
                    case "Close":
                        CloseOpenedWindows();
                        break;
                    case "CloseAll":
                        while (AddedWindows.Count > 0)
                        {
                            CloseOpenedWindows();
                        }
                        break;
                }
            }
            catch
            {
                if (AddedWindows != null)
                {
                    CloseOpenedWindows();
                }
            }
        }

        public void CloseOpenedWindows()
        {
            if (AddedWindows.Count > 0)
            {
                if (AddedWindows.Last() == int.MinValue)
                {
                    try
                    {
                        System.Windows.Forms.SendKeys.SendWait("^(w)");
                    }
                    catch { }
                }

                try
                {
                    NativeWin32.SendMessage(AddedWindows.Last(), WM_SYSCOMMAND, SC_CLOSE, 0);
                }
                catch { }

                AddedWindows.RemoveAt(AddedWindows.Count - 1);


                if (AddedWindows.Count > 0)
                    NativeWin32.SetForegroundWindow(AddedWindows.Last());
            }
        }

        private string Parse(string content)
        {
            if (content == null) return null;

            return content.Replace("[[V1]]", V1).Replace("[[V2]]", V2).Replace("[[V3]]", V3).Replace("[[CLIPBOARD]]", Clipboard.GetText());
        }

        public void Run(string fileName, string argument = "")
        {
            List<int> windowHandles = GetTaskWindows();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Parse(fileName);
            psi.Arguments = Parse(argument);

            Process proc = new Process();
            proc.StartInfo = psi;
            proc.Start();
            try
            {
                proc.WaitForInputIdle();
            }
            catch
            { }

            List<int> newWindowsHandles = GetTaskWindows();

            foreach (int newwindow in newWindowsHandles)
            {
                if (!windowHandles.Contains(newwindow))
                {
                    AddedWindows.Add(newwindow);
                }
            }

            if (windowHandles.Count == newWindowsHandles.Count)
            {
                //TODO:Browser hack
                AddedWindows.Add(int.MinValue);
            }
        }

        public void Show(string content)
        {
            //prevent bad paste in consoles!!
            Clipboard.Clear();
            App.MainViewModel.ContentToShow = content;
        }

        public void ReadInput(string file)
        {
            string content = File.ReadAllText(file);

            Input(content);
        }

        private string Encode(string input)
        {
            if (Encodings.Contains(input))
            {
                return "{" + input + "}";
            }

            return input;
        }

        public void Input(string content)
        {
            content = Parse(content);

            int offset = 1;
            int snapOffset;

            System.Random RandNum = new System.Random();


            for (int i = 0; i < content.Length; i += offset)
            {
                if (AddedWindows.Count > 0)
                {
                    NativeWin32.SetForegroundWindow(AddedWindows.Last());
                }

                System.Threading.Thread.Sleep(RandNum.Next(8, 60));

                SnapToToken(content, i, out snapOffset);
                offset = snapOffset;

                string keys = content.Substring(i, offset);

                if (offset == 1)
                    keys = Encode(keys);


                if (keys.Contains("DOCK"))
                {
                    switch (keys)
                    {
                        case "{DOCKLEFT}":
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.LEFT);
                            break;
                        case "{DOCKRIGHT}":
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.RIGHT);
                            break;
                    }
                }
                else
                {
                    if (keys == "\r\n")
                    {
                        keys = "{ENTER}";
                    }
                    else if (keys == "\t")
                    {
                        keys = "{TAB}";
                    }

                    try
                    {
                        System.Windows.Forms.SendKeys.SendWait(keys);
                    }
                    catch { }
                }
            }
        }

        private void SnapToToken(string content, int index, out int offset)
        {
            foreach (string token in Tokens)
            {
                if ((content.Length >= index + token.Length) && content.Substring(index, token.Length) == token)
                {
                    offset = token.Length;
                    return;
                }
            }

            offset = 1;
        }
    }
}
