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

using Newtonsoft.Json;

namespace Launchpad_OBS_Control
{
    public partial class SetForm : Form
    {

        int padID;
        List<string> sceneList;
        List<MediaListClass> mediaList = new List<MediaListClass>();
        List<SourceClass> soundSourceList = new List<SourceClass>();
        List<TransitionClass> transitionList = new List<TransitionClass>();

        /******************************************************************************/

        Color color1 = Color.Black;
        Color color2 = Color.Black;

        /******************************************************************************/

        CheckBox[] boxList = new CheckBox[0];
        CheckBox[] LightBoxList = new CheckBox[0];
        CheckBox[] MediaBoxList = new CheckBox[0];

        /******************************************************************************/

        StreamWriter sw;

        

        public SetForm(int ID, List<string> scenes, List<MediaListClass> media, List<SourceClass> sound, List<TransitionClass> transition)
        {
            InitializeComponent();
            padID = ID;
            sceneList = scenes;
            mediaList = media;
            soundSourceList = sound;
            transitionList = transition;
        }

        private void SetForm_Load(object sender, EventArgs e)
        {
            idLabel.Text = "Pad ID: " + padID.ToString();
            boxList = new CheckBox[] { StreamCheckBox, RecCheckBox, VirtualCamCheckBox, SceneCheckBox, MediaCheckBox, MuteCheckBox, SoundCheckBox };
            LightBoxList = new CheckBox[] { StaticLightCheckBox, BlinkLightCheckBox, FadeLightCheckBox };
            MediaBoxList = new CheckBox[] { VisibilityONCheckBox, VisibilityOFFCheckBox, PlayPauseCheckBox, RestartMediaCheckBox, StopMediaCheckBox };

            LoadSceneList();
            LoadMediaList();
            LoadMuteList();
        }

        /******************************************************************************/
        //List load section

        void LoadSceneList()
        {
            foreach(string scene in sceneList)
            {
                SceneListBox.Items.Add(scene);
            }
        }

        void LoadMediaList()
        {
            foreach(MediaListClass media in mediaList)
            {
                TabPage newPage = new TabPage();
                newPage.Text = media.sceneName;
                MediaTabControl.Controls.Add(newPage);
                ListBox listBox = new ListBox();
                listBox.SelectedIndexChanged += selectedIndex_Listbox; 
                listBox.Size = new Size(192, 92);
                newPage.Controls.Add(listBox);
                foreach (MediaClass mediaClass in media.mediaClasses)
                {
                    listBox.Items.Add(mediaClass.name);
                }
            }
        }

        void LoadMuteList()
        {
            foreach(SourceClass soure in soundSourceList)
            {
                MuteListBox.Items.Add(soure.name);
            }
        }

        /******************************************************************************/
        //Madia
        private void selectedIndex_Listbox(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            
            foreach(MediaListClass media in mediaList)
            {
                try
                {
                    foreach(MediaClass mediaClass in media.mediaClasses)
                    {
                        if(mediaClass.name == listBox.SelectedItem.ToString())
                        {
                            if(mediaClass.type == "ffmpeg_source")
                            {
                                PlayPauseCheckBox.Checked = true;
                                PlayPauseCheckBox.Enabled = true;
                                RestartMediaCheckBox.Enabled = true;
                                StopMediaCheckBox.Enabled = true;
                                VisibilityONCheckBox.Enabled = false;
                                VisibilityOFFCheckBox.Enabled = false;
                            }
                            else
                            {
                                VisibilityONCheckBox.Checked = true;
                                PlayPauseCheckBox.Enabled = false;
                                RestartMediaCheckBox.Enabled = false;
                                StopMediaCheckBox.Enabled = false;
                                VisibilityONCheckBox.Enabled = true;
                                VisibilityOFFCheckBox.Enabled = true;
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        /******************************************************************************/
        //Color section



        /******************************************************************************/
        //Function checkboxes

        private void ListCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            foreach(CheckBox b in boxList)
            {
                if (checkBox.Checked)
                {
                    if(checkBox != b)
                    {
                        b.Checked = false;
                    }
                }
            }
        }

        /******************************************************************************/
        //Light mode checkboxes

        private void LightCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            foreach (CheckBox b in LightBoxList)
            {
                if (checkBox.Checked)
                {
                    if (checkBox != b)
                    {
                        b.Checked = false;
                    }
                }
            }
        }

        /******************************************************************************/
        //Media checkboxes

        private void MediaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            foreach (CheckBox b in MediaBoxList)
            {
                if (checkBox.Checked)
                {
                    if (checkBox != b)
                    {
                        b.Checked = false;
                    }
                }
            }
        }

        /******************************************************************************/
        //Sound

        private void FileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio files(*.mp3;*.wav)|*.mp3;*.wav";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = openFileDialog.FileName;
            }
        }

        private void TestSoundButton_Click(object sender, EventArgs e)
        {
            try
            {
                WMPLib.WindowsMediaPlayer windowsMediaPlayer = new WMPLib.WindowsMediaPlayer();
                windowsMediaPlayer.URL = FileTextBox.Text;
                windowsMediaPlayer.settings.volume = (int)VolumeNumericUpDown.Value;
                windowsMediaPlayer.controls.play();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void VolumeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            VolumeTrackBar.Value = (int)VolumeNumericUpDown.Value;
        }

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            VolumeNumericUpDown.Value = (int)VolumeTrackBar.Value;
        }

        /******************************************************************************/
        //Clear

        private void ClearButton_Click(object sender, EventArgs e)
        {
            string pathSet = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\set\pad" + padID + "set.json");
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\pad" + padID + ".json");
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure ?", "!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(dr == DialogResult.Yes)
                {
                    sw = new StreamWriter(pathSet);
                    sw.Write("");
                    sw.Close();
                    sw = new StreamWriter(path);
                    sw.Write("");
                    sw.Close();
                    MessageBox.Show("Settings cleared", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /******************************************************************************/
        //Apply

        private void applyButton_Click(object sender, EventArgs e)
        {
            /******************************************************************************/
            //settings
            string pathSet = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\set\pad" + padID + "set.json");
            string mode = "Static";

            foreach (CheckBox c in LightBoxList)
            {
                if (c.Checked)
                {
                    mode = c.Text;
                }
            }

            SettingsClass set = new SettingsClass()
            {
                Mode = mode,

                ColorON = Color1NumericUpDown.Value.ToString(),

                ColorOFF = Color2NumericUpDown.Value.ToString()
            };

            string jsons = JsonConvert.SerializeObject(set);

            try
            {
                sw = new StreamWriter(pathSet);
                sw.Write(jsons);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            /******************************************************************************/
            //commands

            string requestName = "Stream";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OBScontrol\json\pad" + padID + ".json");

            foreach (CheckBox c in boxList)
            {
                if (c.Checked)
                {
                    requestName = c.Text;
                }
            }
            try
            {
                if (requestName == "Stream")
                {
                    StartStopClass startStopClass = new StartStopClass()
                    {
                        requesttype = "StartStopStreaming"
                    };

                    string json = JsonConvert.SerializeObject(startStopClass);

                    try
                    {
                        sw = new StreamWriter(path);
                        sw.Write(json);
                        sw.Close();
                        success(set, startStopClass);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (requestName == "Recording")
                {
                    StartStopClass startStopClass = new StartStopClass()
                    {
                        requesttype = "StartStopRecording"
                    };

                    string json = JsonConvert.SerializeObject(startStopClass);

                    try
                    {
                        sw = new StreamWriter(path);
                        sw.Write(json);
                        sw.Close();
                        success(set, startStopClass);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (requestName == "Virtual Cam")
                {
                    StartStopClass startStopClass = new StartStopClass()
                    {
                        requesttype = "StartStopVirtualCam"
                    };

                    string json = JsonConvert.SerializeObject(startStopClass);

                    try
                    {
                        sw = new StreamWriter(path);
                        sw.Write(json);
                        sw.Close();
                        success(set, startStopClass);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (requestName == "Scene")
                {
                    
                        if(SceneListBox.SelectedItem != null)
                        {
                            SceneClass sceneClass = new SceneClass()
                            {
                                scenename = SceneListBox.SelectedItem.ToString()
                            };

                            string json = JsonConvert.SerializeObject(sceneClass);

                            try
                            {
                                sw = new StreamWriter(path);
                                sw.Write(json);
                                sw.Close();
                                success(set, sceneClass);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("You have to choose a scene", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    
                }
                else if (requestName == "Media")
                {
                    TabPage tabPage = MediaTabControl.SelectedTab;
                    ListBox listBox = tabPage.Controls.OfType<ListBox>().FirstOrDefault();
                    if(listBox.SelectedItem == null)
                    {
                        MessageBox.Show("You have to pick a media", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        foreach (MediaListClass media in mediaList)
                        {
                            try
                            {
                                foreach (MediaClass mediaClass in media.mediaClasses)
                                {
                                    if (mediaClass.name == listBox.SelectedItem.ToString())
                                    {
                                        if (mediaClass.type == "ffmpeg_source")
                                        {
                                            string mediaAction = "Play/Pause Media";

                                            foreach(CheckBox c in MediaBoxList)
                                            {
                                                if (c.Checked)
                                                {
                                                    mediaAction = c.Text;
                                                }
                                            }

                                            if(mediaAction == "Play/Pause Media")
                                            {
                                                MediaFFMPEGClass mediaFFMPEGClass = new MediaFFMPEGClass()
                                                {
                                                    requesttype = "PlayPauseMedia",
                                                    sourceName = listBox.SelectedItem.ToString()
                                                };

                                                string json = JsonConvert.SerializeObject(mediaFFMPEGClass);

                                                try
                                                {
                                                    sw = new StreamWriter(path);
                                                    sw.Write(json);
                                                    sw.Close();
                                                    success(set, mediaFFMPEGClass);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }
                                            }
                                            else if(mediaAction == "Restart Media")
                                            {
                                                MediaFFMPEGClass mediaFFMPEGClass = new MediaFFMPEGClass()
                                                {
                                                    requesttype = "RestartMedia",
                                                    sourceName = listBox.SelectedItem.ToString()
                                                };

                                                string json = JsonConvert.SerializeObject(mediaFFMPEGClass);

                                                try
                                                {
                                                    sw = new StreamWriter(path);
                                                    sw.Write(json);
                                                    sw.Close();
                                                    success(set, mediaFFMPEGClass);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }
                                            }
                                            else if (mediaAction == "Stop Media")
                                            {
                                                MediaFFMPEGClass mediaFFMPEGClass = new MediaFFMPEGClass()
                                                {
                                                    requesttype = "StopMedia",
                                                    sourceName = listBox.SelectedItem.ToString()
                                                };

                                                string json = JsonConvert.SerializeObject(mediaFFMPEGClass);

                                                try
                                                {
                                                    sw = new StreamWriter(path);
                                                    sw.Write(json);
                                                    sw.Close();
                                                    success(set, mediaFFMPEGClass);
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show(ex.Message);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            bool render1 = true;
                                            if (VisibilityONCheckBox.Checked)
                                            {
                                                render1 = true;
                                            }
                                            else if(VisibilityOFFCheckBox.Checked)
                                            {
                                                render1 = false;
                                            }
                                            Media1Class media1Class = new Media1Class()
                                            {
                                                scenename = tabPage.Text,
                                                source = listBox.SelectedItem.ToString(),
                                                render = render1
                                            };

                                            string json = JsonConvert.SerializeObject(media1Class);

                                            try
                                            {
                                                sw = new StreamWriter(path);
                                                sw.Write(json);
                                                sw.Close();
                                                success(set, media1Class);
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                else if (requestName == "Mute")
                {
                    if(MuteListBox.SelectedItem != null)
                    {
                        MuteClass muteClass = new MuteClass()
                        {
                            source = MuteListBox.SelectedItem.ToString()
                        };

                        string json = JsonConvert.SerializeObject(muteClass);

                        try
                        {
                            sw = new StreamWriter(path);
                            sw.Write(json);
                            sw.Close();
                            success(set, muteClass);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have to pick a media", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (requestName == "Sound")
                {
                    if(FileTextBox.Text != "")
                    {
                        SoundClass soundClass = new SoundClass()
                        {
                            sound = FileTextBox.Text,
                            volume = VolumeNumericUpDown.Value.ToString()
                        };

                        string json = JsonConvert.SerializeObject(soundClass);

                        try
                        {
                            sw = new StreamWriter(path);
                            sw.Write(json);
                            sw.Close();
                            success(set, soundClass);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have to pick a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /******************************************************************************/

        void success(SettingsClass settingsClass, StartStopClass startStopClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + startStopClass.requesttype, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, SceneClass sceneClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + sceneClass.requesttype + "\nSwitch to: " + sceneClass.scenename, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, SceneTransitionClass sceneTransitionClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + sceneTransitionClass.requesttype + "\nSwitch to: " + sceneTransitionClass.sceneName + "\nTransition: " + sceneTransitionClass.transitionName, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, Media1Class media1Class)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + media1Class.requesttype + "\nSource: " + media1Class.source + "\nRender: " + media1Class.render, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, MediaFFMPEGClass mediaFFMPEGClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + mediaFFMPEGClass.requesttype + "\nSource: " + mediaFFMPEGClass.sourceName, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, MuteClass muteClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nRequest type: " + muteClass.requesttype + "\nSource: " + muteClass.source, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        void success(SettingsClass settingsClass, SoundClass soundClass)
        {
            MessageBox.Show("Light mode: " + settingsClass.Mode + "\nFile: " + soundClass.sound + "\nVolume: " + soundClass.volume, "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }

        /******************************************************************************/
        //Help

        private void HelpButton_Click(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            DialogResult result = helpForm.ShowDialog();
            if (result == DialogResult.OK)
            {

            }
            helpForm.Dispose();
        }
    }
}

/******************************************************************************/
/******************************************************************************/
/******************************************************************************/

public class SettingsClass
{
    public string Mode { get; set; }
    public string ColorON { get; set; }
    public string ColorOFF { get; set; }

}

public class StartStopClass
{
    [JsonProperty("request-type")]
    public string requesttype { get; set; }

    [JsonProperty("message-id")]
    public string messageid = "0";
}

public class SceneClass
{
    [JsonProperty("request-type")]
    public string requesttype = "SetCurrentScene";

    [JsonProperty("message-id")]
    public string messageid = "0";

    [JsonProperty("scene-name")]
    public string scenename { get; set; }
}

public class SceneTransitionClass
{
    [JsonProperty("request-type")]
    public string requesttype = "SetSceneTransitionOverride";

    [JsonProperty("message-id")]
    public string messageid = "0";

    public string sceneName { get; set; }

    public string transitionName { get; set; }
}

public class Media1Class
{
    [JsonProperty("request-type")]
    public string requesttype = "SetSceneItemRender";

    [JsonProperty("message-id")]
    public string messageid = "0";

    [JsonProperty("scene-name")]
    public string scenename { get; set; }

    public string source { get; set; }

    public bool render { get; set; }
}

public class MediaFFMPEGClass
{
    [JsonProperty("request-type")]
    public string requesttype { get; set; }

    [JsonProperty("message-id")]
    public string messageid = "0";

    public string sourceName { get; set; }
}

public class MuteClass
{
    [JsonProperty("request-type")]
    public string requesttype = "ToggleMute";

    [JsonProperty("message-id")]
    public string messageid = "0";

    public string source { get; set; }
}

public class SoundClass
{
    public string sound { get; set; }

    public string volume { get; set; }
}

/******************************************************************************/

/*
 * 
 * TO DO
 * 
 * FORM
 *  Start/Stop rec/stream       V
 *  Scene list                  V
 *  Mute list                   V
 *  Media list                  V
 *  Sound board list            V
 *  Color 1/2                   V
 *  Mode static/...             V
 * 
 * CODE
 *  Generate files
 */
