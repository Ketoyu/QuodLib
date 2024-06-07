using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Forms;
using GFont = QuodLib.WinForms.Drawing.Objects.classFonts.GFont;
using CString = QuodLib.WinForms.Drawing.Objects.classFonts.CString;

using Debug = System.Diagnostics.Debug;

namespace QuodLib.WinForms.Objects
{
    class CRadioButton : CHoverable, ICObject//, ICHoverable
    {
		#region Fields
        public string Group = "Default";
        public static List<CRadioButton> Companions {get; private set;}
        public bool Checked;
        public bool HasPicture {
            get {
                if (b_text != "" && b_picture != null) throw new Exception("Error: Text is not empty while picture is not null.");
                return (Picture != null);
            }
        }
        public Image b_picture;
        private string b_text;
		#endregion //Fields
        #region Properties
            #region ReadOnly
        private List<CRadioButton> MyCompanions {
            get {
                return (from CRadioButton rad in Companions where (rad != this && rad.Group == this.Group) select rad).ToList<CRadioButton>();
            }
        }
        public static Type Type {
            get {
                return (new CRadioButton(false)).GetType();
            }
        }
            #endregion //ReadOnly
        public Image Picture {
            get {
                return b_picture;
            }
            set {
                b_text = "";
                b_picture = value;
            }
        }
        public string Text {
            get {
                return b_text;
            }
            set {
                b_text = value;
                Picture = null;
            }
        }
        #endregion //Properties

        #region Constructors
        public CRadioButton()
        {
            if (Companions == null) Companions = new List<CRadioButton>();
            Companions.Add(this);
        }
        private CRadioButton(bool AddCompanion)
        {
            if (AddCompanion)
            {
                if (Companions == null) Companions = new List<CRadioButton>();
                Companions.Add(this);
            }
        }
        public CRadioButton(string group) : this()
        {
            Group = group;
        }
		#endregion //Constructos

        #region Subroutines
        public override void Redraw()
        {

        }
            #region EventHelpers
        public override void OnMouseUp()
        {
            if (Enabled && IsHovered)
            {
                foreach (CRadioButton rad in MyCompanions) rad.Checked = false;
                Checked = true;
                State = 0;
                Redraw();
            }
        }
            #endregion //EventHelpers
        #endregion //Subroutines

        #region Functions
        public static List<CRadioButton> GetGroup(string groupName)
        {
            return (from rad in Companions where rad.Group == groupName select rad).ToList<CRadioButton>();
        }
        #endregion //Functions
    }
}
