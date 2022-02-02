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

        int[] obsPads = new int[99];
        int[] soundPads = new int[99];

        int[,] padColorSettingsON = new int[99, 1];
        int[,] padColorSettingsOFF = new int[99, 1];
        int[,] padColorSettingsMode = new int[99, 1]; //0-static, 1-blink, 2-fade

        /*************************************************************************************************/

        //Variables
        bool authRequired = false;

        string data = "";

        StreamReader sr;

        private IInputDevice inputDevice;
        private IOutputDevice outputDevice;

        WebSocket ws;

/*************************************************************************************************/

        //Program
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string dirjson = "./json";
            string dirset = "./json/set";

            if (!Directory.Exists(dirjson))
            {
                Directory.CreateDirectory(dirjson);
            }

            if (!Directory.Exists(dirset))
            {
                Directory.CreateDirectory (dirset);
            }

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

        private void connectButton_Click(object sender, EventArgs e)
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
                connectButton.Text = "Disconnect";

            }
            else
            {
                midiInBox.Enabled = true;
                midiOutBox.Enabled = true;
                connectButton.Text = "Connect";
                inputDevice.StopReceiving();
                inputDevice.Close();
                authRequired = false;
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
                            padColorSettingsMode[i, 0] = 0;
                        }else if(mode.Groups[0].Value == "Blink")
                        {
                            padColorSettingsMode[i, 0] = 1;
                        }
                        else if(mode.Groups[0].Value == "Fade")
                        {
                            padColorSettingsMode[i, 0] = 2;
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
                                outputDevice.Close();
                            }
                            else
                            {
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
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
                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                            outputDevice.SendSysEx(stat);
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
                                    byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                    outputDevice.SendSysEx(stat);
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
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
                                        byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                        outputDevice.SendSysEx(stat);
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
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            outputDevice.SendSysEx(stat);
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
                                    byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                    outputDevice.SendSysEx(blink);
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
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            outputDevice.SendSysEx(stat);
                                            outputDevice.Close();
                                        }
                                    }
                                    foreach (RenderOFFListClass s in padsRenderOFF)
                                    {
                                        if (s.source == itemVisible.Groups[0].Value)
                                        {
                                            outputDevice.Open();
                                            byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsON[s.id, 0], 247 };
                                            outputDevice.SendSysEx(stat);
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
                                byte[] blink = { 240, 0, 32, 41, 2, 13, 3, 1, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], (byte)padColorSettingsON[s.id, 0], 247 };
                                outputDevice.SendSysEx(blink);
                                outputDevice.Close();

                                while(wPlayer.playState != WMPPlayState.wmppsStopped)
                                {
                                    await Task.Delay(1000);
                                }
 
                                outputDevice.Open();
                                byte[] stat = { 240, 0, 32, 41, 2, 13, 3, 0, (byte)s.id, (byte)padColorSettingsOFF[s.id, 0], 247 };
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
            SetForm setform = new SetForm(buttonId, sceneList, mediaList, soundSourceList, transitionList);
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