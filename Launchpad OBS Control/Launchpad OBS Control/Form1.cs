//System
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
//Midi-dot-net2
using Midi.Devices;
using Midi.Messages;
//WebtSocketSharp
using WebSocketSharp;
//WMPLib
using WMPLib;
//Newtonsoft.Json;
using Newtonsoft.Json;
//
using System.Threading;

namespace Launchpad_OBS_Control
{
    public partial class Form1 : Form
    {
        //Dictionary from notes to padID

        Dictionary<string, int> pads = new Dictionary<string, int>()
        {
            {"BNeg1", 11 },
            {"C0", 12 },
            {"CSharp0", 13 },
            {"D0", 14 },
            {"DSharp0", 15 },
            {"E0", 16 },
            {"F0", 17 },
            {"FSharp0", 18 },
            {"19", 19 },
            {"A0", 21 },
            {"ASharp0", 22 },
            {"B0", 23 },
            {"C1", 24 },
            {"CSharp1", 25 },
            {"D1", 26 },
            {"DSharp1", 27 },
            {"E1", 28 },
            {"29", 29 },
            {"G1", 31 },
            {"GSharp1", 32 },
            {"A1", 33 },
            {"ASharp1", 34 },
            {"B1", 35 },
            {"C2", 36 },
            {"CSharp2", 37 },
            {"D2", 38 },
            {"39", 39 },
            {"F2", 41 },
            {"FSharp2", 42 },
            {"G2", 43 },
            {"GSharp2", 44 },
            {"A2", 45 },
            {"ASharp2", 46 },
            {"B2", 47 },
            {"C3", 48 },
            {"49", 49 },
            {"DSharp3", 51 },
            {"E3", 52 },
            {"F3", 53 },
            {"FSharp3", 54 },
            {"G3", 55 },
            {"GSharp3", 56 },
            {"A3", 57 },
            {"ASharp3", 58 },
            {"59", 59 },
            {"CSharp4", 61 },
            {"D4", 62 },
            {"DSharp4", 63 },
            {"E4", 64 },
            {"F4", 65 },
            {"FSharp4", 66 },
            {"G4", 67 },
            {"GSharp4", 68 },
            {"69", 69 },
            {"B4", 71 },
            {"C5", 72 },
            {"CSharp5", 73 },
            {"D5", 74 },
            {"DSharp5", 75 },
            {"E5", 76 },
            {"F5", 77 },
            {"FSharp5", 78 },
            {"79", 79 },
            {"A5", 81 },
            {"ASharp5", 82 },
            {"B5", 83 },
            {"C6", 84 },
            {"CSharp6", 85 },
            {"D6", 86 },
            {"DSharp6", 87 },
            {"E6", 88 },
            {"89", 89 },
            {"ReverbLevel", 91 },
            {"TremoloLevel", 92 },
            {"ChorusLevel", 93 },
            {"CelesteLevel", 94 },
            {"PhaserLevel", 95 },
            {"96", 96 },
            {"97", 97 },
            {"NonRegisteredParameterLSB", 98 },
            {"Logo", 99 }
        };

        List<CollorClass> collorList = new List<CollorClass>();

        List<StartStopListClass> padsRequests = new List<StartStopListClass>(); //Start Stop Rec/Stream
        List<SceneListClass> padsScenes = new List<SceneListClass>(); //Set Scene
        List<MuteListClass> padsMute = new List<MuteListClass>(); //Togle Mute
        List<PlayPauseMediaListClass> padsPlayPauseMedia = new List<PlayPauseMediaListClass>(); //Play Pause Media
        List<RestartMediaListClass> padsRestartMedia = new List<RestartMediaListClass>(); //Restart Media
        List<StopMediaListClass> padsStopMedia = new List<StopMediaListClass>(); //Stop Media

        List<SoundListClass> padsSounds = new List<SoundListClass>(); //Sound Board
        List<SoundVolumeListClass> padsSoundsVolume = new List<SoundVolumeListClass>();

        List<RenderONListClass> padsRenderON = new List<RenderONListClass>();
        List<RenderOFFListClass> padsRenderOFF = new List<RenderOFFListClass>();

        List<string> sceneList = new List<string>();
        List<MediaClass> mediaClassList = new List<MediaClass>();
        List<MediaListClass> mediaList = new List<MediaListClass>();
        List<SourceClass> soundSourceList = new List<SourceClass>();
        List<TransitionClass> transitionList = new List<TransitionClass>();

        int[] obsPads = new int[100];
        int[] soundPads = new int[100];

        int[,] padColorSettingsON = new int[100, 1];
        int[,] padColorSettingsOFF = new int[100, 1];
        int[,] padColorSettingsMode = new int[100, 1]; //0-off, 1-static, 2-blink, 3-fade

        LaunchpadButton[] buttonList = new LaunchpadButton[100];
        int[] onOff = new int[100];

        /*************************************************************************************************/

        //Variables
        bool authRequired = false;

        string data = "";

        StreamReader sr;

        private IInputDevice inputDevice;
        private IOutputDevice outputDevice;

        WebSocket ws;

        Thread t;

        /*************************************************************************************************/

        //Program
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(LaunchpadButton b in GetAll(this, typeof(LaunchpadButton)))
            {
                buttonList[b.ID] = b;
            }

            foreach(LaunchpadButton b in buttonList)
            {
                if(b!=null) b.Enabled = false;
            }
            reloadButton.Enabled = false;

            loadCollorList();

            t = new Thread(colors);
            t.Start();

            LoadSettings();

            //Searching for MIDI devices
            if (DeviceManager.InputDevices.Count == 0)
            {
                Console.WriteLine("No input devices.");
            }
            else
            {
                Console.WriteLine("Input Devices:");
                foreach (var device in DeviceManager.InputDevices)
                {
                    Console.WriteLine("  {0}", device.Name);
                    midiInBox.Items.Add(device.Name);
                }

            }

            if (DeviceManager.OutputDevices.Count == 0)
            {
                Console.WriteLine("No output devices.");
            }
            else
            {
                Console.WriteLine("Output Devices:");
                foreach (var device in DeviceManager.OutputDevices)
                {
                    Console.WriteLine("  {0}", device.Name);
                    midiOutBox.Items.Add(device.Name);
                }
            }

            string folderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol");
            if (!Directory.Exists(folderPath1))
            {
                try
                {
                    Directory.CreateDirectory(folderPath1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            try
            {
                try
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\def.json");
                    sr = new StreamReader(path);
                }
                catch
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\def.json");

                    using (FileStream fs = File.Create(path))
                        fs.Close();

                    StreamWriter sw = new StreamWriter(path);

                    DefClass defClass = new DefClass() { indexIN = 0, indexOUT = 0 };

                    string j = JsonConvert.SerializeObject(defClass);

                    sw.Write(j);
                    sw.Close();

                    sr = new StreamReader(path);
                }
                string settings = sr.ReadToEnd();
                sr.Close();

                try
                {
                    DefClass d = JsonConvert.DeserializeObject<DefClass>(settings);
                    midiInBox.SelectedIndex = d.indexIN;
                    midiOutBox.SelectedIndex = d.indexOUT;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Connecting to WebSocket on localhost port 4444 (OBS)
            ws = new WebSocket("ws://127.0.0.1:4444");
            //Listening for messages from ws
            ws.OnMessage += OnMessage;
            //Connecting to ws
            ws.Connect();
            //Checking if auth is required
            try
            {
                //sr = new StreamReader("./json/auth.json");
                //string auth = sr.ReadToEnd();
                //ws.Send(auth);
                ws.Send("{\"request-type\": \"GetAuthRequired\",\"message-id\": \"1\"}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            if (midiInBox.Enabled)
            {
                //Setting in adn out for MIDI
                try
                {
                    inputDevice = DeviceManager.InputDevices[midiInBox.SelectedIndex];
                }
                catch
                {
                    MessageBox.Show("Please select the correct MIDI input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    outputDevice = DeviceManager.OutputDevices[midiOutBox.SelectedIndex];
                }
                catch
                {
                    MessageBox.Show("Please select the correct MIDI output", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (authRequired)
                {

                    /***************************/
                    /*For now auth is not working please use non authRequire settings*/
                    /***************************/

                    MessageBox.Show("For now auth is not working please use non authRequire settings in OBS (You might need to reset Launchpad OBS Controll app)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                    /*
                    if (passwordBox.Text == "")
                    {
                        MessageBox.Show("Auth is enabled in OBS. Pleasei insert password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!Auth())
                    {
                        MessageBox.Show("Wrong password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    */
                }

                //Opening MIDI
                inputDevice.Open();
                inputDevice.StartReceiving(null);
                //Listening for messages from MIDI
                inputDevice.NoteOn += InputDevice_NoteOn;
                inputDevice.ControlChange += InputDevice_ControlChange;
                //Setting programer mode in Launchpad
                outputDevice.Open();
                byte[] programmer = { 240, 0, 32, 41, 2, 13, 14, 1, 247 };
                outputDevice.SendSysEx(programmer);
                outputDevice.Close();
                //Loading lights
                LoadLightSettings();
                //Getting data from OBS
                GetDataFromOBS();

                midiInBox.Enabled = false;
                midiOutBox.Enabled = false;

                foreach (LaunchpadButton b in buttonList)
                {
                    if (b != null) b.Enabled = true;
                }

                reloadButton.Enabled = true;

                connectButton.Text = "Disconnect";

                try
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\def.json");
                    StreamWriter sw = new StreamWriter(path);

                    DefClass defClass = new DefClass() { indexIN = midiInBox.SelectedIndex, indexOUT = midiOutBox.SelectedIndex };

                    string j = JsonConvert.SerializeObject(defClass);

                    sw.Write(j);
                    sw.Close();
                }
                catch(Exception ex) 
                { 
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
                midiInBox.Enabled = true;
                midiOutBox.Enabled = true;
                connectButton.Text = "Connect";
                inputDevice.StopReceiving();
                inputDevice.Close();
                authRequired = false;

                foreach (LaunchpadButton b in buttonList)
                {
                    if (b != null) b.Enabled = false;
                }
                reloadButton.Enabled = false;
            }
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            ReLoadSettings();
        }

        /*************************************************************************************************/

        //Load pad settings

        void LoadSettings()
        {
            string folderPath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\set");
            if (!Directory.Exists(folderPath1))
            {
                try
                {
                    Directory.CreateDirectory(folderPath1);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //First load settings on startup
            for (int i = 11; i <= 99; i++)
            {
                //Loading pads functions to memory 
                try
                {
                    try
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\pad" + i + ".json");
                        sr = new StreamReader(path);
                    }
                    catch
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\pad" + i + ".json");

                        using (FileStream fs = File.Create(path))

                        sr = new StreamReader(path);
                    }
                    string settings = sr.ReadToEnd();
                    sr.Close();
                    Match match = Regex.Match(settings, "(?<=\"request-type\":\").*?(?=\")");
                    if (match.Success)
                    {
                        obsPads[i] = i;
                        if (match.Groups[0].Value == "StartStopRecording")
                        {
                            padsRequests.Add(new StartStopListClass { request = "StartStopRecording", id = i });
                        }
                        else if (match.Groups[0].Value == "StartStopStreaming")
                        {
                            padsRequests.Add(new StartStopListClass { request = "StartStopStreaming", id = i });
                        }
                        else if (match.Groups[0].Value == "StartStopVirtualCam")
                        {
                            padsRequests.Add(new StartStopListClass { request = "StartStopVirtualCam", id = i });
                        }
                        else if (match.Groups[0].Value == "SetCurrentScene")
                        {
                            Match sceneName = Regex.Match(settings, "(?<=\"scene-name\":\").*?(?=\")");
                            if (sceneName.Success)
                            {
                                padsScenes.Add(new SceneListClass { scene = sceneName.Groups[0].Value, id = i });
                            }
                        }
                        else if (match.Groups[0].Value == "SetSceneItemRender")
                        {
                            Match item = Regex.Match(settings, "(?<=\"source\":\").*?(?=\")");
                            if (item.Success)
                            {
                                Match match1 = Regex.Match(settings, "(?<=\"render\":).*?(?=})");
                                if (match1.Success)
                                {
                                    if(match1.Groups[0].Value == "true")
                                    {
                                        padsRenderON.Add(new RenderONListClass { source = item.Groups[0].Value, id = i });
                                    }else
                                    {
                                        padsRenderOFF.Add(new RenderOFFListClass { source = item.Groups[0].Value, id = i });
                                    }
                                }
                            }
                        }
                        else if (match.Groups[0].Value == "ToggleMute")
                        {
                            Match source = Regex.Match(settings, "(?<=\"source\":\").*?(?=\")");
                            if (source.Success)
                            {
                                padsMute.Add(new MuteListClass { mute = source.Groups[0].Value, id = i });
                            }
                        }
                        else if (match.Groups[0].Value == "PlayPauseMedia")
                        {
                            Match source = Regex.Match(settings, "(?<=\"sourceName\":\").*?(?=\")");
                            if (source.Success)
                            {
                                padsPlayPauseMedia.Add(new PlayPauseMediaListClass { media = source.Groups[0].Value, id = i});
                            }
                        }
                        else if (match.Groups[0].Value == "RestartMedia")
                        {
                            Match source = Regex.Match(settings, "(?<=\"sourceName\":\").*?(?=\")");
                            if (source.Success)
                            {
                                padsRestartMedia.Add(new RestartMediaListClass { media = source.Groups[0].Value, id = i});
                            }
                        }
                        else if (match.Groups[0].Value == "StopMedia")
                        {
                            Match source = Regex.Match(settings, "(?<=\"sourceName\":\").*?(?=\")");
                            if (source.Success)
                            {
                                padsStopMedia.Add(new StopMediaListClass { media = source.Groups[0].Value, id = i });
                            }
                        }
                    }

                    Match match2 = Regex.Match(settings, "(?<=\"sound\":\").*?(?=\")");
                    if (match2.Success)
                    {
                        soundPads[i] = i;

                        padsSounds.Add(new SoundListClass { sound = match2.Groups[0].Value, id = i });

                        Match match3 = Regex.Match(settings, "(?<=\"volume\":\").*?(?=\")");
                        if (match3.Success)
                        {
                            padsSoundsVolume.Add(new SoundVolumeListClass { volume = Int32.Parse(match3.Groups[0].Value), id = i });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //Reading rgb value from settings and mode
                try
                {
                    try
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\set\pad" + i + "set.json");
                        sr = new StreamReader(path);
                    }
                    catch
                    {
                        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\set\pad" + i + "set.json");

                        using (FileStream fs = File.Create(path))

                        sr = new StreamReader(path);
                    }
                    
                    string settings = sr.ReadToEnd();
                    sr.Close();
                    Match mode = Regex.Match(settings, "(?<=\"Mode\":\").*?(?=\")");
                    if (mode.Success)
                    {
                        if(mode.Groups[0].Value == "Static")
                        {
                            padColorSettingsMode[i, 0] = 1;
                        }else if(mode.Groups[0].Value == "Blink")
                        {
                            padColorSettingsMode[i, 0] = 2;
                        }
                        else if(mode.Groups[0].Value == "Fade")
                        {
                            padColorSettingsMode[i, 0] = 3;
                        }
                    }
                    Match match = Regex.Match(settings, "(?<=\"ColorON\":\").*?(?=\")", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        padColorSettingsON[i, 0] = Int32.Parse(match.Groups[0].Value);
                    }
                    Match match2 = Regex.Match(settings, "(?<=\"ColorOFF\":\").*?(?=\")", RegexOptions.Singleline);
                    if (match2.Success)
                    {
                        padColorSettingsOFF[i, 0] = Int32.Parse(match2.Groups[0].Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        void LoadLightSettings() //TO DO!!!! Change this to for loop wherw each volor will be determin by settings.json
        {
            outputDevice.Open();

            foreach(StartStopListClass s in padsRequests) 
            {
                if(s.request == "StartStopStreaming")
                {
                    try
                    {
                        byte[] stream = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                        outputDevice.SendSysEx(stream);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine (ex.Message);
                    }
                }
                if(s.request == "StartStopRecording")
                {
                    try
                    {
                        byte[] recording = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                        outputDevice.SendSysEx(recording);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if(s.request == "StartStopVirtualCam")
                {
                    try
                    {
                        byte[] virtualCam = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                        outputDevice.SendSysEx(virtualCam);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            try
            {
                foreach (SceneListClass s in padsScenes)
                {
                    byte[] scene = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                    outputDevice.SendSysEx(scene);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (MuteListClass s in padsMute)
                {
                    byte[] mute = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                    outputDevice.SendSysEx(mute);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (PlayPauseMediaListClass s in padsPlayPauseMedia)
                {
                    byte[] play = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                    outputDevice.SendSysEx(play);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (RestartMediaListClass s in padsRestartMedia)
                {
                    byte[] restart = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                    outputDevice.SendSysEx(restart);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (StopMediaListClass s in padsStopMedia)
                {
                    byte[] stop = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                    outputDevice.SendSysEx(stop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (SoundListClass s in padsSounds)
                {
                    byte[] sound = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                    outputDevice.SendSysEx(sound);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach(RenderONListClass s in padsRenderON)
                {
                    byte[] render = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                    outputDevice.SendSysEx(render);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (RenderOFFListClass s in padsRenderOFF)
                {
                    byte[] render = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                    outputDevice.SendSysEx(render);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            outputDevice.Close();
        }

        void GetDataFromOBS()
        {
            try
            {
                ws.Send("{\"request-type\": \"GetRecordingStatus\",\"message-id\": \"2\"}");
                ws.Send("{\"request-type\": \"GetStreamingStatus\",\"message-id\": \"3\"}");
                ws.Send("{\"request-type\": \"GetCurrentScene\",\"message-id\": \"4\"}");

                foreach (MuteListClass s in padsMute)
                {
                    ws.Send("{\"request-type\": \"GetMute\",\"message-id\": \"5\",\"source\": \"" + s.mute + "\"}");
                }

                ws.Send("{\"request-type\": \"GetVirtualCamStatus\",\"message-id\": \"6\"}");

                foreach (PlayPauseMediaListClass s in padsPlayPauseMedia)
                {
                    ws.Send("{\"request-type\": \"GetMediaState\",\"message-id\": \"7\",\"sourceName\": \"" + s.media + "\"}");
                }

                ws.Send("{\"request-type\": \"GetMediaSourcesList\",\"message-id\": \"8\"}"); //TO DO!!!!!!! Implement this to read media state on app start (Lights)

                ws.Send("{\"request-type\": \"GetSceneList\",\"message-id\": \"9\"}");
                ws.Send("{\"request-type\": \"GetSourcesList\",\"message-id\": \"10\"}");
                ws.Send("{\"request-type\": \"GetTransitionList\",\"message-id\": \"11\"}");
            }
            catch
            {
                MessageBox.Show("An error has occurred during the connection. Make sure OBS is turned on and you have obs-websocket 4.9.1 installed ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        void ReLoadSettings()
        {

            padsRequests.Clear();
            padsScenes.Clear();
            padsMute.Clear();
            padsPlayPauseMedia.Clear();
            padsRestartMedia.Clear();
            padsStopMedia.Clear();

            padsSounds.Clear();
            padsSoundsVolume.Clear();

            padsRenderON.Clear();
            padsRenderOFF.Clear();

            sceneList.Clear();
            mediaClassList.Clear();
            mediaList.Clear();
            soundSourceList.Clear();
            transitionList.Clear();

            outputDevice.Open();

            for(int i = 11; i <= 99; i++)
            {
                byte[] reload = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)i, 0, 247 };
                outputDevice.SendSysEx(reload);
            }

            for(int i = 0; i < 100; i++)
            {
                padColorSettingsMode[i, 0] = 0;
            }

            outputDevice.Close();

            LoadSettings();

            LoadLightSettings();

            GetDataFromOBS();
        }

        /*void ReLoadSettings(int a)
        {
            LoadSettings();

            LoadLightSettings();          // <== TO DO? (load settings for one specific button)

            GetDataFromOBS();
        }*/

        /*************************************************************************************************/

        //Listening for messages from MIDI
        private void InputDevice_ControlChange(ControlChangeMessage msg)
        {
            PadPressed(pads[msg.Control.ToString()], msg.Value);
        }

        private void InputDevice_NoteOn(NoteOnMessage msg)
        {
            PadPressed(pads[msg.Pitch.ToString()], msg.Velocity);
        }
        //Listening for messages from ws
        private void OnMessage(object sender, MessageEventArgs e) // TO DO !!!!!!!!!!! Change it to be determined by settings.json if it should blink or not and to dererminate color by settings too
        {
            data = e.Data;
            Console.WriteLine(e.Data);
            //Checking if auth is required
            Match authMatch = Regex.Match(e.Data, "(?<=\"authRequired\":).*?(?=,)");
            if (authMatch.Success)
            {
                if(authMatch.Groups[0].Value == "true")
                {
                    authRequired = true;
                }
                else
                {
                    authRequired = false;
                }
            }
            //Getting recording status
            Match isRecMatch = Regex.Match(e.Data, "(?<=\"isRecording\":).*?(?=,)");
            if (isRecMatch.Success)
            {
                foreach(StartStopListClass s in padsRequests)
                {
                    if(s.request == "StartStopRecording")
                    {
                        try
                        {
                            if (isRecMatch.Groups[0].Value == "true")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }catch (Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            //Getting virtual cam status
            Match isVirtualCamMatch = Regex.Match(e.Data, "(?<=\"isVirtualCam\":).*?(?=,)");
            if (isVirtualCamMatch.Success)
            {
                foreach (StartStopListClass s in padsRequests)
                {
                    if(s.request == "StartStopVirtualCam")
                    {
                        try
                        {
                            if (isVirtualCamMatch.Groups[0].Value == "true")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if(padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            //Getting Stream Status
            Match isStreamMatch = Regex.Match(e.Data, "(?<=\"streaming\":).*?(?=,)");
            if (isStreamMatch.Success)
            {
                foreach (StartStopListClass s in padsRequests)
                {
                    if(s.request == "StartStopStreaming")
                    {
                        try
                        {
                            if (isStreamMatch.Groups[0].Value == "true")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            //Getting active scene
            Match activeScene = Regex.Match(e.Data, "(?<=\"message-id\":\"4\",\"name\":\").*?(?=\")");
            if (activeScene.Success)
            {
                foreach(SceneListClass s in padsScenes)
                {
                    if(s.scene == activeScene.Groups[0].Value)
                    {
                        try
                        {
                            outputDevice.Open();
                            byte[] dataToSend = { 0 };
                            if (padColorSettingsMode[s.id, 0] == 1)
                            {
                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                            }
                            else if (padColorSettingsMode[s.id, 0] == 2)
                            {
                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                            }
                            else if (padColorSettingsMode[s.id, 0] == 3)
                            {
                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                            }
                            onOff[s.id] = 1;
                            outputDevice.SendSysEx(dataToSend);
                            outputDevice.Close();
                        }catch (Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            //Getting sources status
            Match muted = Regex.Match(e.Data, "(?<=\"message-id\":\"5\",\"muted\":).*?(?=,)");
            if (muted.Success)
            {
                try
                {
                    Match mutedName = Regex.Match(e.Data, "(?<=\"name\":\").*?(?=\")");
                    if (mutedName.Success)
                    {
                        if (muted.Groups[0].Value == "true")
                        {
                            foreach(MuteListClass s in padsMute)
                            {
                                if(s.mute == mutedName.Groups[0].Value)
                                {
                                    outputDevice.Open();
                                    byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                    onOff[s.id] = 0;
                                    outputDevice.SendSysEx(stat);
                                    outputDevice.Close();
                                }
                            }
                        }
                        else
                        {
                            foreach(MuteListClass s in padsMute)
                            {
                                if(s.mute == mutedName.Groups[0].Value)
                                {
                                    outputDevice.Open();
                                    byte[] dataToSend = { 0 };
                                    if (padColorSettingsMode[s.id, 0] == 1)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    else if (padColorSettingsMode[s.id, 0] == 2)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    else if (padColorSettingsMode[s.id, 0] == 3)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    onOff[s.id] = 1;
                                    outputDevice.SendSysEx(dataToSend);
                                    outputDevice.Close();
                                }
                            }
                        }
                    }
                }catch (Exception ex)
                {
                    outputDevice.Close();
                    Console.WriteLine(ex.Message);
                }
            }

            Match scenes = Regex.Match(e.Data, "(?<=\"message-id\":\"9\",\"scenes\":).*?(?=,\"status\")");
            if (scenes.Success)
            {
                try
                {

                    MatchCollection sceneListMatch = Regex.Matches(scenes.Groups[0].Value, "(?<={\"name\":\").*?(?=\",\"sources\")");

                    foreach (Match scene in sceneListMatch)
                    {
                        sceneList.Add(scene.Groups[0].Value);
                        Match mediaMatch = Regex.Match(scenes.Groups[0].Value, "(?<={\"name\":\"" + scene.Value +"\",\"sources\":).*?(?<=])");
                        if (mediaMatch.Success)
                        {
                            List<MediaClass> l = JsonConvert.DeserializeObject<List<MediaClass>>(mediaMatch.Value);

                            mediaList.Add(new MediaListClass() { sceneName = scene.Value, mediaClasses = l });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Match soundSource = Regex.Match(e.Data, "(?<=\"message-id\":\"10\",\"sources\":\\[).*?(?=],\"status\")");
            if (soundSource.Success)
            {
                try
                {
                    MatchCollection soundSourceCollection = Regex.Matches(soundSource.Groups[0].Value, "(?={).*?(?<=})");
                    foreach (Match match in soundSourceCollection)
                    {
                        SourceClass l = JsonConvert.DeserializeObject<SourceClass>(match.Value);

                        if(l.typeId == "wasapi_input_capture" || l.typeId == "wasapi_output_capture")
                        {
                            soundSourceList.Add(l);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Match transition = Regex.Match(e.Data, "(?<=\"message-id\":\"11\",\"status\":\"ok\",\"transitions\":).*?(?<=])");
            if (transition.Success)
            {
                transitionList = JsonConvert.DeserializeObject<List<TransitionClass>>(transition.Groups[0].Value);
            }

            //Match update-type
            Match upadte_type = Regex.Match(e.Data, "(?<=\"update-type\":\").*?(?=\")");
            if (upadte_type.Success)
            {
                try
                {
                    if (upadte_type.Groups[0].Value == "RecordingStarted")
                    {
                        foreach(StartStopListClass s in padsRequests)
                        {
                            if(s.request == "StartStopRecording")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "RecordingStopped")
                    {
                        foreach (StartStopListClass s in padsRequests)
                        {
                            if (s.request == "StartStopRecording")
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "StreamStarted")
                    {
                        foreach(StartStopListClass s in padsRequests)
                        {
                            if(s.request == "StartStopStreaming")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "StreamStopped")
                    {
                        foreach (StartStopListClass s in padsRequests)
                        {
                            if (s.request == "StartStopStreaming")
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "VirtualCamStarted")
                    {
                        foreach (StartStopListClass s in padsRequests)
                        {
                            if (s.request == "StartStopVirtualCam")
                            {
                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "VirtualCamStopped")
                    {
                        foreach (StartStopListClass s in padsRequests)
                        {
                            if (s.request == "StartStopVirtualCam")
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "TransitionVideoEnd")
                    {
                        try
                        {
                            Match fromScene = Regex.Match(e.Data, "(?<=\"from-scene\":\").*?(?=\")");
                            if (fromScene.Success)
                            {
                                foreach(SceneListClass s in padsScenes)
                                {
                                    if(s.scene == fromScene.Groups[0].Value)
                                    {
                                        outputDevice.Open();
                                        byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                        onOff[s.id] = 0;
                                        outputDevice.SendSysEx(stat);
                                        outputDevice.Close();
                                    }
                                }
                            }
                        }catch (Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            Match toScene = Regex.Match(e.Data, "(?<=\"to-scene\":\").*?(?=\")");
                            if (toScene.Success)
                            {
                                foreach(SceneListClass s in padsScenes)
                                {
                                    if(s.scene == toScene.Groups[0].Value)
                                    {
                                        outputDevice.Open();
                                        byte[] dataToSend = { 0 };
                                        if (padColorSettingsMode[s.id, 0] == 1)
                                        {
                                            dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                        }
                                        else if (padColorSettingsMode[s.id, 0] == 2)
                                        {
                                            dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                        }
                                        else if (padColorSettingsMode[s.id, 0] == 3)
                                        {
                                            dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                        }
                                        onOff[s.id] = 1;
                                        outputDevice.SendSysEx(dataToSend);
                                        outputDevice.Close();
                                    }
                                }
                            }
                        }catch(Exception ex)
                        {
                            outputDevice.Close();
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "SourceMuteStateChanged")
                    {
                        Match sourceName = Regex.Match(e.Data, "(?<=\"sourceName\":\").*?(?=\")");
                        if (sourceName.Success)
                        {
                            Match isMuted = Regex.Match(e.Data, "(?<=\"muted\":).*?(?=,)");
                            if (isMuted.Success)
                            {
                                if(isMuted.Groups[0].Value == "false")
                                {
                                    foreach(MuteListClass s in padsMute)
                                    {
                                        if(s.mute == sourceName.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] dataToSend = { 0 };
                                            if (padColorSettingsMode[s.id, 0] == 1)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 2)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 3)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            onOff[s.id] = 1;
                                            outputDevice.SendSysEx(dataToSend);
                                            outputDevice.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (MuteListClass s in padsMute)
                                    {
                                        if (s.mute == sourceName.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                            onOff[s.id] = 0;
                                            outputDevice.SendSysEx(stat);
                                            outputDevice.Close();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "MediaStarted")
                    {
                        Match sourceName = Regex.Match(e.Data, "(?<=\"sourceName\":\").*?(?=\")");
                        if (sourceName.Success)
                        {
                            foreach(PlayPauseMediaListClass s in padsPlayPauseMedia)
                            {
                                if(s.media == sourceName.Groups[0].Value)
                                {
                                    outputDevice.Open();
                                    byte[] dataToSend = { 0 };
                                    if (padColorSettingsMode[s.id, 0] == 1)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    else if (padColorSettingsMode[s.id, 0] == 2)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    else if (padColorSettingsMode[s.id, 0] == 3)
                                    {
                                        dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                    }
                                    onOff[s.id] = 1;
                                    outputDevice.SendSysEx(dataToSend);
                                    outputDevice.Close();
                                }
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "MediaPaused")
                    {
                        Match sourceName = Regex.Match(e.Data, "(?<=\"sourceName\":\").*?(?=\")");
                        if (sourceName.Success)
                        {
                            foreach (PlayPauseMediaListClass s in padsPlayPauseMedia)
                            {
                                if (s.media == sourceName.Groups[0].Value)
                                {
                                    outputDevice.Open();
                                    byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                    onOff[s.id] = 0;
                                    outputDevice.SendSysEx(stat);
                                    outputDevice.Close();
                                }
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "MediaEnded")
                    {
                        Match sourceName = Regex.Match(e.Data, "(?<=\"sourceName\":\").*?(?=\")");
                        if (sourceName.Success)
                        {
                            foreach(PlayPauseMediaListClass s in padsPlayPauseMedia)
                            {
                                if(s.media == sourceName.Groups[0].Value)
                                {
                                    outputDevice.Open();
                                    byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                    onOff[s.id] = 0;
                                    outputDevice.SendSysEx(stat);
                                    outputDevice.Close();
                                }
                            }
                        }
                    }
                    else if (upadte_type.Groups[0].Value == "SceneItemVisibilityChanged")
                    {
                        Match itemVisible = Regex.Match(e.Data, "(?<=\"item-name\":\").*?(?=\")");
                        if (itemVisible.Success)
                        {
                            Match match = Regex.Match(e.Data, "(?<=\"item-visible\":).*?(?=,)");
                            if (match.Success)
                            {
                                if(match.Groups[0].Value == "true")
                                {
                                    foreach(RenderONListClass s in padsRenderON)
                                    {
                                        if (s.source == itemVisible.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] dataToSend = { 0 };
                                            if (padColorSettingsMode[s.id, 0] == 1)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 2)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 3)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            onOff[s.id] = 1;
                                            outputDevice.SendSysEx(dataToSend);
                                            outputDevice.Close();
                                        }
                                    }
                                    foreach (RenderOFFListClass s in padsRenderOFF)
                                    {
                                        if (s.source == itemVisible.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] dataToSend = { 0 };
                                            if (padColorSettingsMode[s.id, 0] == 1)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 2)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            else if (padColorSettingsMode[s.id, 0] == 3)
                                            {
                                                dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            }
                                            onOff[s.id] = 1;
                                            outputDevice.SendSysEx(dataToSend);
                                            outputDevice.Close();
                                        }
                                    }
                                }
                                else if(match.Groups[0].Value == "false")
                                {
                                    foreach (RenderONListClass s in padsRenderON)
                                    {
                                        if (s.source == itemVisible.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                            onOff[s.id] = 0;
                                            outputDevice.SendSysEx(stat);
                                            outputDevice.Close();
                                        }
                                    }
                                    foreach (RenderOFFListClass s in padsRenderOFF)
                                    {
                                        if (s.source == itemVisible.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                            onOff[s.id] = 0;
                                            outputDevice.SendSysEx(stat);
                                            outputDevice.Close();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputDevice.Close();
                    Console.WriteLine(ex.Message);
                }
            }

        }

        bool Auth() //Will not work becouse obs-socket have some isue with it
        {
            string password = passwordBox.Text;
            string salt = Regex.Match(data, "(?<=\"salt\":\").*?(?=\")").Groups[0].Value;
            string challenge = Regex.Match(data, "(?<=\"challenge\":\").*?(?=\")").Groups[0].Value;

            string secret_string = password + salt;
            string secret_hash = ComputeSha256Hash(secret_string);
            string secret = EncodeTo64(secret_hash);

            string auth_response_string = secret + challenge;
            string auth_response_hash = ComputeSha256Hash(auth_response_string);
            string auth_response = EncodeTo64(auth_response_hash);

            Console.WriteLine(password);
            Console.WriteLine(salt);
            Console.WriteLine(challenge);
            Console.WriteLine(secret_hash);
            Console.WriteLine(secret);
            Console.WriteLine(auth_response_hash);
            Console.WriteLine(auth_response);
            Console.WriteLine("{\"request-type\":\"Authenticate\",\"message-id\":\"1\",\"auth\":\"" + auth_response + "\"}");

            ws.Send("{\"request-type\":\"Authenticate\",\"message-id\":\"1\",\"auth\":\"" + auth_response +"\"}");

            //{"error":"Authentication Failed.","message-id":"1","status":"error"}


            return false;
        }

/*************************************************************************************************/

        async void PadPressed(int padID, int velocity) //TO DO Make color feedback
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\pad" + padID + ".json");
            if (velocity > 0)
            {
                int index = Array.IndexOf(obsPads, padID);

                if (index > -1)
                {
                    try
                    {
                        sr = new StreamReader(path);
                        string message = sr.ReadToEnd();
                        sr.Close();
                        ws.Send(message);

                        foreach (var launchpadButton in this.Controls.OfType<LaunchpadButton>())
                        {
                            if (launchpadButton.ID == padID)
                            {
                                //launchpadButton.Invoke(new MethodInvoker(delegate { launchpadButton.Enabled = false; }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        if(sr != null)
                        {
                            sr.Close();
                        }
                    }
                }

                int index2 = Array.IndexOf(soundPads, padID);

                if(index2 > -1)
                {
                    try
                    {
                        WMPLib.WindowsMediaPlayer wPlayer = new WMPLib.WindowsMediaPlayer();

                        foreach(SoundListClass s in padsSounds)
                        {
                            if(s.id == padID)
                            {
                                wPlayer.URL = s.sound;

                                foreach(SoundVolumeListClass v in padsSoundsVolume)
                                {
                                    if(v.id == s.id)
                                    {
                                        wPlayer.settings.volume = v.volume;
                                    }
                                }


                                wPlayer.controls.play();

                                outputDevice.Open();
                                byte[] dataToSend = { 0 };
                                if (padColorSettingsMode[s.id, 0] == 1)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 2)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                else if (padColorSettingsMode[s.id, 0] == 3)
                                {
                                    dataToSend = new byte[] { 240, 0, 32, 41, 2, 13, 3, 2, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                }
                                onOff[s.id] = 1;
                                outputDevice.SendSysEx(dataToSend);
                                outputDevice.Close();

                                while(wPlayer.playState != WMPPlayState.wmppsStopped)
                                {
                                    await Task.Delay(1000);
                                }
 
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
                                onOff[s.id] = 0;
                                outputDevice.SendSysEx(stat);
                                outputDevice.Close();
                                
                            }
                        }

                        foreach (var launchpadButton in this.Controls.OfType<LaunchpadButton>())
                        {
                            if (launchpadButton.ID == padID)
                            {
                                //launchpadButton.Invoke(new MethodInvoker(delegate { launchpadButton.Enabled = false; }));
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }


        /*************************************************************************************************/

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        static public string EncodeTo64(string toEncode){

            byte[] toEncodeAsBytes= System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue= System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
/*************************************************************************************************/
        //Open settings
        private void launchpadButton_Click(object sender, EventArgs e)
        {
            LaunchpadButton button = (LaunchpadButton)sender;
            int buttonId = button.ID;
            SetForm setform = new SetForm(buttonId, sceneList, mediaList, soundSourceList, transitionList, sender);
            DialogResult dialogresult = setform.ShowDialog();
            if (dialogresult == DialogResult.OK)
            {
                ReLoadSettings();
            }
            else if (dialogresult == DialogResult.Cancel)
            {
            }
            setform.Dispose();
        }
        /*************************************************************************************************/
        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        public void colors()
        {
            while (true)
            {
                foreach(LaunchpadButton button in buttonList)
                {
                    if (button != null)
                    {
                        if (padColorSettingsMode[button.ID, 0] == 1)
                        {
                            if(onOff[button.ID] == 1)
                            {
                                button.BackColor = collorList.Find(x => x.color == padColorSettingsON[button.ID, 0]).rgb;
                            }
                            else if (onOff[button.ID] == 0)
                            {
                                button.BackColor = collorList.Find(x => x.color == padColorSettingsOFF[button.ID, 0]).rgb;
                            }
                        }
                        else if(padColorSettingsMode[button.ID, 0] == 2 || padColorSettingsMode[button.ID, 0] == 3)
                        {
                            if (onOff[button.ID] == 1)
                            {
                                button.BackColor = button.BackColor == collorList.Find(x => x.color == padColorSettingsON[button.ID, 0]).rgb ? collorList.Find(x => x.color == padColorSettingsOFF[button.ID, 0]).rgb : collorList.Find(x => x.color == padColorSettingsON[button.ID, 0]).rgb;
                            }
                            else if (onOff[button.ID] == 0)
                            {
                                button.BackColor = collorList.Find(x => x.color == padColorSettingsOFF[button.ID, 0]).rgb;
                            }
                        }
                        else
                        {
                            button.BackColor = Color.LightGray;
                        }

                        if(padsRenderON.Exists(x => x.id == button.ID) || padsRenderOFF.Exists(x => x.id == button.ID) || padsStopMedia.Exists(x => x.id == button.ID) || padsRestartMedia.Exists(x => x.id == button.ID))
                        {
                            button.BackColor = collorList.Find(x => x.color == padColorSettingsON[button.ID, 0]).rgb;
                        }
                    }
                }
                Thread.Sleep(500);
            }
        }

        /*************************************************************************************************/
        //collor translate

        void loadCollorList()
        {
            collorList.Add(new CollorClass() { color = 0, rgb = Color.FromArgb(97,97,97) });
            collorList.Add(new CollorClass() { color = 1, rgb = Color.FromArgb(179, 179, 179) });
            collorList.Add(new CollorClass() { color = 2, rgb = Color.FromArgb(211, 211, 211) });
            collorList.Add(new CollorClass() { color = 3, rgb = Color.FromArgb(255, 255, 255) });
            collorList.Add(new CollorClass() { color = 4, rgb = Color.FromArgb(255, 179, 179) });
            collorList.Add(new CollorClass() { color = 5, rgb = Color.FromArgb(255, 97, 97) });
            collorList.Add(new CollorClass() { color = 6, rgb = Color.FromArgb(221, 97, 97) });
            collorList.Add(new CollorClass() { color = 7, rgb = Color.FromArgb(179, 97, 97) });
            collorList.Add(new CollorClass() { color = 8, rgb = Color.FromArgb(255, 243, 213) });
            collorList.Add(new CollorClass() { color = 9, rgb = Color.FromArgb(255, 179, 97) });
            collorList.Add(new CollorClass() { color = 10, rgb = Color.FromArgb(221, 140, 97) });
            collorList.Add(new CollorClass() { color = 11, rgb = Color.FromArgb(179, 118, 97) });
            collorList.Add(new CollorClass() { color = 12, rgb = Color.FromArgb(255, 238, 161) });
            collorList.Add(new CollorClass() { color = 13, rgb = Color.FromArgb(255, 255, 97) });
            collorList.Add(new CollorClass() { color = 14, rgb = Color.FromArgb(221, 221, 97) });
            collorList.Add(new CollorClass() { color = 15, rgb = Color.FromArgb(179, 179, 97) });
            collorList.Add(new CollorClass() { color = 16, rgb = Color.FromArgb(221, 255, 161) });
            collorList.Add(new CollorClass() { color = 17, rgb = Color.FromArgb(194, 255, 97) });
            collorList.Add(new CollorClass() { color = 18, rgb = Color.FromArgb(161, 221, 97) });
            collorList.Add(new CollorClass() { color = 19, rgb = Color.FromArgb(129, 179, 97) });
            collorList.Add(new CollorClass() { color = 20, rgb = Color.FromArgb(194, 255, 179) });
            collorList.Add(new CollorClass() { color = 21, rgb = Color.FromArgb(97, 255, 97) });
            collorList.Add(new CollorClass() { color = 22, rgb = Color.FromArgb(97, 221, 97) });
            collorList.Add(new CollorClass() { color = 23, rgb = Color.FromArgb(97, 179, 97) });
            collorList.Add(new CollorClass() { color = 24, rgb = Color.FromArgb(194, 255, 194) });
            collorList.Add(new CollorClass() { color = 25, rgb = Color.FromArgb(97, 255, 140) });
            collorList.Add(new CollorClass() { color = 26, rgb = Color.FromArgb(97, 221, 118) });
            collorList.Add(new CollorClass() { color = 27, rgb = Color.FromArgb(97, 179, 107) });
            collorList.Add(new CollorClass() { color = 28, rgb = Color.FromArgb(194, 255, 204) });
            collorList.Add(new CollorClass() { color = 29, rgb = Color.FromArgb(97, 255, 204) });
            collorList.Add(new CollorClass() { color = 30, rgb = Color.FromArgb(97, 221, 161) });
            collorList.Add(new CollorClass() { color = 31, rgb = Color.FromArgb(97, 179, 129) });
            collorList.Add(new CollorClass() { color = 32, rgb = Color.FromArgb(194, 255, 243) });
            collorList.Add(new CollorClass() { color = 33, rgb = Color.FromArgb(97, 255, 233) });
            collorList.Add(new CollorClass() { color = 34, rgb = Color.FromArgb(97, 221, 194) });
            collorList.Add(new CollorClass() { color = 35, rgb = Color.FromArgb(97, 179, 150) });
            collorList.Add(new CollorClass() { color = 36, rgb = Color.FromArgb(194, 243, 255) });
            collorList.Add(new CollorClass() { color = 37, rgb = Color.FromArgb(97, 238, 255) });
            collorList.Add(new CollorClass() { color = 38, rgb = Color.FromArgb(97, 199, 221) });
            collorList.Add(new CollorClass() { color = 39, rgb = Color.FromArgb(97, 161, 179) });
            collorList.Add(new CollorClass() { color = 40, rgb = Color.FromArgb(194, 221, 255) });
            collorList.Add(new CollorClass() { color = 41, rgb = Color.FromArgb(97, 199, 255) });
            collorList.Add(new CollorClass() { color = 42, rgb = Color.FromArgb(97, 161, 221) });
            collorList.Add(new CollorClass() { color = 43, rgb = Color.FromArgb(97, 129, 179) });
            collorList.Add(new CollorClass() { color = 44, rgb = Color.FromArgb(161, 140, 255) });
            collorList.Add(new CollorClass() { color = 45, rgb = Color.FromArgb(97, 97, 255) });
            collorList.Add(new CollorClass() { color = 46, rgb = Color.FromArgb(97, 97, 221) });
            collorList.Add(new CollorClass() { color = 47, rgb = Color.FromArgb(97, 97, 179) });
            collorList.Add(new CollorClass() { color = 48, rgb = Color.FromArgb(204, 179, 255) });
            collorList.Add(new CollorClass() { color = 49, rgb = Color.FromArgb(161, 97, 255) });
            collorList.Add(new CollorClass() { color = 50, rgb = Color.FromArgb(129, 97, 221) });
            collorList.Add(new CollorClass() { color = 51, rgb = Color.FromArgb(118, 97, 179) });
            collorList.Add(new CollorClass() { color = 52, rgb = Color.FromArgb(255, 179, 255) });
            collorList.Add(new CollorClass() { color = 53, rgb = Color.FromArgb(255, 97, 255) });
            collorList.Add(new CollorClass() { color = 54, rgb = Color.FromArgb(221, 97, 221) });
            collorList.Add(new CollorClass() { color = 55, rgb = Color.FromArgb(179, 97, 179) });
            collorList.Add(new CollorClass() { color = 56, rgb = Color.FromArgb(255, 179, 213) });
            collorList.Add(new CollorClass() { color = 57, rgb = Color.FromArgb(255, 97, 194) });
            collorList.Add(new CollorClass() { color = 58, rgb = Color.FromArgb(221, 97, 161) });
            collorList.Add(new CollorClass() { color = 59, rgb = Color.FromArgb(179, 97, 140) });
            collorList.Add(new CollorClass() { color = 60, rgb = Color.FromArgb(255, 118, 97) });
            collorList.Add(new CollorClass() { color = 61, rgb = Color.FromArgb(233, 179, 97) });
            collorList.Add(new CollorClass() { color = 62, rgb = Color.FromArgb(221, 194, 97) });
            collorList.Add(new CollorClass() { color = 63, rgb = Color.FromArgb(161, 161, 97) });
            collorList.Add(new CollorClass() { color = 64, rgb = Color.FromArgb(97, 179, 97) });
            collorList.Add(new CollorClass() { color = 65, rgb = Color.FromArgb(97, 179, 140) });
            collorList.Add(new CollorClass() { color = 66, rgb = Color.FromArgb(97, 140, 213) });
            collorList.Add(new CollorClass() { color = 67, rgb = Color.FromArgb(97, 97, 255) });
            collorList.Add(new CollorClass() { color = 68, rgb = Color.FromArgb(97, 179, 179) });
            collorList.Add(new CollorClass() { color = 69, rgb = Color.FromArgb(140, 97, 243) });
            collorList.Add(new CollorClass() { color = 70, rgb = Color.FromArgb(204, 179, 194) });
            collorList.Add(new CollorClass() { color = 71, rgb = Color.FromArgb(140, 118, 129) });
            collorList.Add(new CollorClass() { color = 72, rgb = Color.FromArgb(255, 97, 97) });
            collorList.Add(new CollorClass() { color = 73, rgb = Color.FromArgb(243, 255, 161) });
            collorList.Add(new CollorClass() { color = 74, rgb = Color.FromArgb(238, 252, 97) });
            collorList.Add(new CollorClass() { color = 75, rgb = Color.FromArgb(204, 255, 97) });
            collorList.Add(new CollorClass() { color = 76, rgb = Color.FromArgb(118, 221, 97) });
            collorList.Add(new CollorClass() { color = 77, rgb = Color.FromArgb(97, 255, 204) });
            collorList.Add(new CollorClass() { color = 78, rgb = Color.FromArgb(97, 233, 255) });
            collorList.Add(new CollorClass() { color = 79, rgb = Color.FromArgb(97, 161, 255) });
            collorList.Add(new CollorClass() { color = 80, rgb = Color.FromArgb(140, 97, 255) });
            collorList.Add(new CollorClass() { color = 81, rgb = Color.FromArgb(204, 97, 252) });
            collorList.Add(new CollorClass() { color = 82, rgb = Color.FromArgb(238, 140, 221) });
            collorList.Add(new CollorClass() { color = 83, rgb = Color.FromArgb(161, 118, 97) });
            collorList.Add(new CollorClass() { color = 84, rgb = Color.FromArgb(255, 161, 97) });
            collorList.Add(new CollorClass() { color = 85, rgb = Color.FromArgb(221, 249, 97) });
            collorList.Add(new CollorClass() { color = 86, rgb = Color.FromArgb(213, 255, 140) });
            collorList.Add(new CollorClass() { color = 87, rgb = Color.FromArgb(97, 255, 97) });
            collorList.Add(new CollorClass() { color = 88, rgb = Color.FromArgb(179, 255, 161) });
            collorList.Add(new CollorClass() { color = 89, rgb = Color.FromArgb(204, 252, 213) });
            collorList.Add(new CollorClass() { color = 90, rgb = Color.FromArgb(179, 255, 246) });
            collorList.Add(new CollorClass() { color = 91, rgb = Color.FromArgb(204, 228, 255) });
            collorList.Add(new CollorClass() { color = 92, rgb = Color.FromArgb(161, 194, 246) });
            collorList.Add(new CollorClass() { color = 93, rgb = Color.FromArgb(213, 194, 249) });
            collorList.Add(new CollorClass() { color = 94, rgb = Color.FromArgb(249, 140, 255) });
            collorList.Add(new CollorClass() { color = 95, rgb = Color.FromArgb(255, 97, 204) });
            collorList.Add(new CollorClass() { color = 96, rgb = Color.FromArgb(255, 194, 97) });
            collorList.Add(new CollorClass() { color = 97, rgb = Color.FromArgb(243, 238, 97) });
            collorList.Add(new CollorClass() { color = 98, rgb = Color.FromArgb(228, 255, 97) });
            collorList.Add(new CollorClass() { color = 99, rgb = Color.FromArgb(221, 204, 97) });
            collorList.Add(new CollorClass() { color = 100, rgb = Color.FromArgb(179, 161, 97) });
            collorList.Add(new CollorClass() { color = 101, rgb = Color.FromArgb(97, 186, 118) });
            collorList.Add(new CollorClass() { color = 102, rgb = Color.FromArgb(118, 194, 140) });
            collorList.Add(new CollorClass() { color = 103, rgb = Color.FromArgb(129, 129, 161) });
            collorList.Add(new CollorClass() { color = 104, rgb = Color.FromArgb(129, 140, 204) });
            collorList.Add(new CollorClass() { color = 105, rgb = Color.FromArgb(204, 170, 129) });
            collorList.Add(new CollorClass() { color = 106, rgb = Color.FromArgb(221, 97, 97) });
            collorList.Add(new CollorClass() { color = 107, rgb = Color.FromArgb(249, 179, 161) });
            collorList.Add(new CollorClass() { color = 108, rgb = Color.FromArgb(249, 186, 118) });
            collorList.Add(new CollorClass() { color = 109, rgb = Color.FromArgb(255, 243, 140) });
            collorList.Add(new CollorClass() { color = 110, rgb = Color.FromArgb(233, 249, 161) });
            collorList.Add(new CollorClass() { color = 111, rgb = Color.FromArgb(213, 238, 118) });
            collorList.Add(new CollorClass() { color = 112, rgb = Color.FromArgb(129, 129, 161) });
            collorList.Add(new CollorClass() { color = 113, rgb = Color.FromArgb(249, 249, 213) });
            collorList.Add(new CollorClass() { color = 114, rgb = Color.FromArgb(221, 252, 228) });
            collorList.Add(new CollorClass() { color = 115, rgb = Color.FromArgb(233, 233, 255) });
            collorList.Add(new CollorClass() { color = 116, rgb = Color.FromArgb(228, 213, 255) });
            collorList.Add(new CollorClass() { color = 117, rgb = Color.FromArgb(179, 179, 179) });
            collorList.Add(new CollorClass() { color = 118, rgb = Color.FromArgb(213, 213, 213) });
            collorList.Add(new CollorClass() { color = 119, rgb = Color.FromArgb(249, 255, 255) });
            collorList.Add(new CollorClass() { color = 120, rgb = Color.FromArgb(233, 97, 97) });
            collorList.Add(new CollorClass() { color = 121, rgb = Color.FromArgb(170, 97, 97) });
            collorList.Add(new CollorClass() { color = 122, rgb = Color.FromArgb(129, 246, 97) });
            collorList.Add(new CollorClass() { color = 123, rgb = Color.FromArgb(97, 179, 97) });
            collorList.Add(new CollorClass() { color = 124, rgb = Color.FromArgb(243, 238, 97) });
            collorList.Add(new CollorClass() { color = 125, rgb = Color.FromArgb(179, 161, 97) });
            collorList.Add(new CollorClass() { color = 126, rgb = Color.FromArgb(238, 194, 97) });
            collorList.Add(new CollorClass() { color = 127, rgb = Color.FromArgb(194, 118, 97) });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t.IsAlive)
            {
                t.Abort();
            }
        }
    }
}

/*************************************************************************************************/
/*************************************************************************************************/
/*************************************************************************************************/

/*public class MediaClass
{
    public string SceneName { get; set; }
    public string MediaName { get; set; }
    public string MediaRender { get; set; }
    public string MediaMuted { get; set; }
}*/

public class DefClass
{
    public int indexIN { get; set; }
    public int indexOUT { get; set; }
}

public class MediaListClass
{
    public string sceneName { get; set; }
    public List<MediaClass> mediaClasses { get; set; }
}

public class MediaClass
{
    public int alignment { get; set; }
    public double cx { get; set; }
    public double cy { get; set; }
    public int id { get; set; }
    public bool locked { get; set; }
    public bool muted { get; set; }
    public string name { get; set; }
    public bool render { get; set; }
    public int source_cx { get; set; }
    public int source_cy { get; set; }
    public string type { get; set; }
    public double volume { get; set; }
    public double x { get; set; }
    public double y { get; set; }
}

public class SourceClass
{
    public string name { get; set; }
    public string type { get; set; }
    public string typeId { get; set; }
}

public class TransitionClass
{
    public string name { get; set; }
}

public class StartStopListClass
{
    public string request { get; set; }
    public int id { set; get; }
}

public class SceneListClass
{
    public string scene { get; set; }
    public int id { set; get; }
}

public class MuteListClass
{
    public string mute { get; set; }
    public int id { set; get; }
}

public class PlayPauseMediaListClass
{
    public string media { get; set; }
    public int id { set; get; }
}

public class RestartMediaListClass
{
    public string media { get; set; }
    public int id { set; get; }
}

public class StopMediaListClass
{
    public string media { get; set; }
    public int id { set; get; }
}

public class SoundListClass
{
    public string sound { get; set; }
    public int id { set; get; }
}

public class SoundVolumeListClass
{
    public int volume { get; set; }
    public int id { set; get; }
}

public class RenderONListClass
{
    public string source { get; set; }
    public int id { set; get; }
}

public class RenderOFFListClass
{
    public string source { get; set; }
    public int id { set; get; }
}

public class CollorClass
{
    public int color { set; get; }
    public Color rgb { set; get; }
}