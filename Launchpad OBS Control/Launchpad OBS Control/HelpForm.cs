using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launchpad_OBS_Control
{
    public partial class HelpForm : Form
    {

        string[] startStopString = new string[]
        {
            "Start/Stop",
            "",
            "Toggle betwen Start and Stop : Stream / Rec / V Cam",
            "",
            "Static",
            "   Color 1",
            "       Lit when the function is OFF",
            "   Color 2",
            "       Lit when the function is ON",
            "Blink",
            "   Color 1",
            "       Lit when the function is OFF",
            "   Color 2",
            "       Blinks when the function is ON",
            "Fade",
            "   Color 1",
            "       Lit when the function is OFF",
            "   Color 2",
            "       Blinks when the function is ON"
        };

        string[] sceneString = new string[]
        {
            "Scene",
            "",
            "Switch betwen Scenes",
            "",
            "Static",
            "   Color 1",
            "       Lit when the current scene is running",
            "   Color 2",
            "       Lit when the other scene is running",
            "Blink",
            "   Color 1",
            "       Blinks when the current scene is running",
            "   Color 2",
            "       Lit when the other scene is running",
            "Fade",
            "   Color 1",
            "       Blinks when the current scene is running",
            "   Color 2",
            "       Lit when the other scene is running"
        };

        string[] mediaString = new string[]
        {
            "Media",
            "",
            "Control media",
            "",
            "Visibility",
            "   Static",
            "       Color 1",
            "           Lit when media state returns true",
            "       Color 2",
            "           Lit when media state returns false",
            "       Example: For Visibility ON if media is visible Color 1 will lit",
            "           for Visibilyty OFF is the opposite,",
            "           if media is visible Color 2 will lit",
            "Play/Pause",
            "   Static",
            "       Color 1",
            "           Lit when media is running",
            "       Color 2",
            "           Lit when media isn't running",
            "   Blink",
            "       Color 1",
            "           Blinks when media is running",
            "       Color 2",
            "           Lit when media isn't running",
            "   Fade",
            "       Color 1",
            "           Blinks when media is running",
            "       Color 2",
            "           Lit when media isn't running",
            "Restart/Stop",
            "   Static",
            "       Color 1",
            "           Lit all the time"
        };

        string[] muteString = new string[]
        {
            "Mute",
            "",
            "Toggle Mute",
            "",
            "Static",
            "   Color 1",
            "       Lit when source isn't muted",
            "   Color 2",
            "       Lit when source is muted",
            "Blink",
            "   Color 1",
            "       Lit when source isn't muted",
            "   Color 2",
            "       Blinks when source is muted",
            "Fade",
            "   Color 1",
            "       Lit when source isn't muted",
            "   Color 2",
            "       Blinks when source is muted"
        };

        string[] soundString = new string[]
        {
            "Sound",
            "",
            "Plays Sound",
            "",
            "Static",
            "   Color 1",
            "       Lit when sound is playing",
            "   Color 2",
            "       Lit when sound isn't playing",
            "Blink",
            "   Color 1",
            "       Blinks when sound is playing",
            "   Color 2",
            "       Lit when sound isn't playing",
            "Fade",
            "   Color 1",
            "       Blinks when sound is playing",
            "   Color 2",
            "       Lit when sound isn't playing"
        };

        string[] clearString = new string[]
        {
            "Clear",
            "",
            "Clears settings"
        };

        /*****************************************************************************/

        CheckBox[] checkBox = new CheckBox[0];

        /*****************************************************************************/
        public HelpForm()
        {
            InitializeComponent();
            checkBox = new CheckBox[]{ StartStopCheckBox, SceneCheckBox, MediaCheckBox, MuteCheckBox, SoundCheckBox, ClearCheckBox};
            HelpTextBox.Lines = startStopString;
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if(chk.Checked)
            {
                foreach(CheckBox c in checkBox)
                {
                    if(chk != c)
                    {
                        c.Checked = false;
                    } 
                }
            }
            int checkedInt = 0;
            foreach (CheckBox c in checkBox)
            {
                if (c.Checked)
                {
                    checkedInt++;
                }
            }

            if(checkedInt == 0)
            {
                checkBox[0].Checked = true;
            }

            if (StartStopCheckBox.Checked)
            {
                HelpTextBox.Lines = startStopString;
            }
            else if (SceneCheckBox.Checked)
            {
                HelpTextBox.Lines = sceneString;
            }
            else if (MediaCheckBox.Checked)
            {
                HelpTextBox.Lines= mediaString;
            }
            else if (MuteCheckBox.Checked)
            {
                HelpTextBox.Lines = muteString;
            }
            else if (SoundCheckBox.Checked)
            {
                HelpTextBox.Lines = soundString;
            }
            else if (ClearCheckBox.Checked)
            {

            }
        }
    }
}
