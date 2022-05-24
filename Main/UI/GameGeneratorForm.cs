using FastColoredTextBoxNS;
using FoenixIDE.GameGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FoenixIDE.UI
{
    public enum IrqType
    {
        SOF,
        SOL,
        TIMER0,
        TIMER1,
        TIMER2,
        KEYBOARD,
        MOUSE
    }

    public partial class GameGeneratorForm : Form
    {
        Dictionary<string, List<string>> templates;

        AutocompleteMenu popupMenu;
        string[] keywords = {"ASSET", "COPY", "GOTO", "FILL", "VGM_INIT", "VGM_PLAY", "ENABLE_INTERRUPTS",
            "ENABLE_SPRITE", "DISABLE_SPRITE", "SET_SPRITE_POS",
            "ENABLE_BITMAP", "DISABLE_BITMAP", "ENABLE_TILEMAP", "DISABLE_TILEMAP", "SET_TILEMAP_POS",
            "IF", "ELSE", "VAR", "INCR", "DECR", "FOR"};
        //string[] methods = { "Equals()", "GetHashCode()", "GetType()", "ToString()" };
        string[] snippets = { "if(^)\n{\n;\n}", "if(^)\n{\n;\n}\nelse\n{\n;\n}", "for(^;;)\n{\n;\n}", "while(^)\n{\n;\n}", "do${\n^;\n}while();", "switch(^)\n{\ncase : break;\n}" };
        /*string[] declarationSnippets = {
               "public class ^\n{\n}", "private class ^\n{\n}", "internal class ^\n{\n}",
               "public struct ^\n{\n;\n}", "private struct ^\n{\n;\n}", "internal struct ^\n{\n;\n}",
               "public void ^()\n{\n;\n}", "private void ^()\n{\n;\n}", "internal void ^()\n{\n;\n}", "protected void ^()\n{\n;\n}",
               "public ^{ get; set; }", "private ^{ get; set; }", "internal ^{ get; set; }", "protected ^{ get; set; }"
               };*/

        public GameGeneratorForm()
        {
            InitializeComponent();

            //create autocomplete popup menu
            popupMenu = new AutocompleteMenu(CodeTextBox);
            //popupMenu.Items.ImageList = imageList1;
            popupMenu.SearchPattern = @"[\w\.:=!]";
            popupMenu.AllowTabKey = true;
            //
            BuildAutocompleteMenu();
        }

        private void BuildAutocompleteMenu()
        {
            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });
            //foreach (var item in declarationSnippets)
            //    items.Add(new DeclarationSnippet(item) { ImageIndex = 0 });
            //foreach (var item in methods)
            //    items.Add(new MethodAutocompleteItem(item) { ImageIndex = 2 });
            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));

            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));
            items.Add(new InsertEnterSnippet());

            //set as autocomplete source
            popupMenu.Items.SetAutocompleteItems(items);
        }

        /// <summary>
        /// This item appears when any part of snippet text is typed
        /// </summary>
        class DeclarationSnippet : SnippetAutocompleteItem
        {
            public DeclarationSnippet(string snippet)
                : base(snippet)
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var pattern = Regex.Escape(fragmentText);
                if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                    return CompareResult.Visible;
                return CompareResult.Hidden;
            }
        }

        /// <summary>
        /// Divides numbers and words: "123AND456" -> "123 AND 456"
        /// Or "i=2" -> "i = 2"
        /// </summary>
        class InsertSpaceSnippet : AutocompleteItem
        {
            string pattern;

            public InsertSpaceSnippet(string pattern) : base("")
            {
                this.pattern = pattern;
            }

            public InsertSpaceSnippet()
                : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                if (Regex.IsMatch(fragmentText, pattern))
                {
                    Text = InsertSpaces(fragmentText);
                    if (Text != fragmentText)
                        return CompareResult.Visible;
                }
                return CompareResult.Hidden;
            }

            public string InsertSpaces(string fragment)
            {
                var m = Regex.Match(fragment, pattern);
                if (m == null)
                    return fragment;
                if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                    return fragment;
                return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return Text;
                }
            }
        }

        /// <summary>
        /// Inerts line break after '}'
        /// </summary>
        class InsertEnterSnippet : AutocompleteItem
        {
            Place enterPlace = Place.Empty;

            public InsertEnterSnippet()
                : base("[Line break]")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var r = Parent.Fragment.Clone();
                while (r.Start.iChar > 0)
                {
                    if (r.CharBeforeStart == '}')
                    {
                        enterPlace = r.Start;
                        return CompareResult.Visible;
                    }

                    r.GoLeftThroughFolded();
                }

                return CompareResult.Hidden;
            }

            public override string GetTextForReplace()
            {
                //extend range
                Range r = Parent.Fragment;
                Place end = r.End;
                r.Start = enterPlace;
                r.End = r.End;
                //insert line break
                return Environment.NewLine + r.Text;
            }

            public override void OnSelected(AutocompleteMenu popupMenu, SelectedEventArgs e)
            {
                base.OnSelected(popupMenu, e);
                if (Parent.Fragment.tb.AutoIndent)
                    Parent.Fragment.tb.DoAutoIndent();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return "Insert line break after '}'";
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog()
            {
                Title = "Pick a Foenix Game File to Open",
                Filter = "FGM (*.fgm)|*.fgm"
            };
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                CodeTextBox.Text = File.ReadAllText(openDlg.FileName);

                // Check which checkboxes to check
                FoenixLexer fl = new FoenixLexer(CodeTextBox.Text);
                cbSOF.Checked = fl.GetSub("SOF_IRQ_HANDLER") != null;
                cbSOL.Checked = fl.GetSub("SOL_IRQ_HANDLER") != null;
                cbTimer0.Checked = fl.GetSub("TIMER0_IRQ_HANDLER") != null;
                cbTimer1.Checked = fl.GetSub("TIMER1_IRQ_HANDLER") != null;
                cbTimer2.Checked = fl.GetSub("TIMER2_IRQ_HANDLER") != null;
                cbMouse.Checked = fl.GetSub("MOUSE_IRQ_HANDLER") != null;

                cbKeyboard.Checked = fl.GetSub("KEYBOARD_IRQ_HANDLER") != null; ;
                cbCollision0.Checked = fl.GetSub("STS_COL_IRQ_HANDLER") != null;
                cbCollision1.Checked = fl.GetSub("STT_COL_IRQ_HANDLER") != null;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog
            {
                Title = "Pick a Foenix Game File to Save",
                Filter = "FGM (*.fgm)|*.fgm"
            };
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(saveDlg.FileName, CodeTextBox.Lines);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void GenerateASMButton_Click(object sender, EventArgs e)
        {
            // parse the code and generate .asm file(s)
            FoenixLexer fl = new FoenixLexer(CodeTextBox.Text);
            GG_Validate(fl);
            FolderBrowserDialog saveDlg = new FolderBrowserDialog()
            {
                Description = "Select Destination Folder",
                ShowNewFolderButton = true,
                SelectedPath = Application.StartupPath
            };

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                ReloadTemplates();

                // copy the standard asm files - includes and definitions
                string folder = saveDlg.SelectedPath;
                Directory.CreateDirectory(folder + Path.DirectorySeparatorChar + "includes");
                DirectoryInfo dir = new DirectoryInfo(@"Resources\\base_asm");
                foreach (FileInfo fi in dir.GetFiles("*.asm"))
                {
                    fi.CopyTo(folder + Path.DirectorySeparatorChar + "includes" + Path.DirectorySeparatorChar + fi.Name, true);

                    // Read the assignments from the file
                    if (".asm".Equals(fi.Extension))
                    {
                        string definition = File.ReadAllText(fi.FullName);
                        // parser definition into VARs/ADDRs
                    }
                }
                foreach (FileInfo fi in dir.GetFiles("*.bat"))
                {
                    fi.CopyTo(folder + Path.DirectorySeparatorChar + fi.Name, true);
                }

                // Generate the header for the main file
                List<string> lines = new List<string>()
                {
                    "; *************************************************************************",
                    "; * Assembly generated by Foenix IDE Game Generator",
                    "; *************************************************************************",
                    ".cpu \"65816\"",
                    ".include \"includes/macros_inc.asm\"",
                    ".include \"includes/bank_00_inc.asm\"",
                    ".include \"includes/timer_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_GENERAL_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_BITMAP_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_SPRITE_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_TILEMAP_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_SDMA_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_VDMA_def.asm\"",
                    ".include \"includes/VKYII_CFP9553_COLLISION_def.asm\"",
                    ".include \"includes/interrupt_def.asm\"",
                    ".include \"includes/io_def.asm\"",
                    ".include \"includes/kernel_inc.asm\"",
                    ".include \"includes/math_def.asm\"",
                    ".include \"includes/gabe_control_registers_def.asm\"",
                    ".include \"includes/base.asm\"",
                    ".include \"includes/EXP_C200_EVID_def.asm\"",
                    "",
                    "* = $000500",
                    ".include \"includes/keyboard_def.asm\"",
                    "* = $000710",
                    ".include \"vars.asm\"",
                    "* = $10000",
                    ".include \"includes/interrupt_handler.asm\"",
                    ".include \"includes/helper_functions.asm\"",
                    ".include \"includes/vgm_player.asm\"",
                    "",
                    "GAME_START",
                    "                setas",
                    "                setxl",
                    "                sei",
                    ""
                };

                List<string> variables = new List<string>();
                if (cbEVID.Checked)
                {
                    // Add code to push the address of start_debug_text to stack
                    // Add code to JSL to Display_EVID_Log
                    // Add variable to start_debug_text
                }


                List<TokenMatch> codeMatches = fl.GetCode();
                foreach (TokenMatch tm in codeMatches)
                {
                    switch (tm.TokenType)
                    {
                        case TokenType.LABEL:
                            lines.Add(tm.Value);
                            break;
                        case TokenType.ENABLE_INTERRUPTS:
                            tm.groups.Clear();
                            tm.groups.Add(BuildIrqReg0String(cbSOF.Checked, cbSOL.Checked, cbTimer0.Checked, cbTimer1.Checked, cbTimer2.Checked, false, false, cbMouse.Checked));
                            tm.groups.Add(BuildIrqReg1String(cbKeyboard.Checked, cbCollision0.Checked, cbCollision1.Checked, false, false, false, false, false));
                            tm.groups.Add("$0");
                            tm.groups.Add("$0");
                            lines.AddRange(GetTemplate(tm));
                            break;
                        case TokenType.VAR:
                            variables.AddRange(GetTemplate(tm));
                            break;
                        default:
                            lines.AddRange(GetTemplate(tm));
                            break;
                    }
                }

                foreach (Asset asset in fl.GetAssets())
                {
                    lines.AddRange(GetTemplate(asset));
                }

                // Write the code
                File.WriteAllLines(folder + Path.DirectorySeparatorChar + "game_main.asm", lines);
                File.WriteAllLines(folder + Path.DirectorySeparatorChar + "vars.asm", variables);

                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_sof_handler.asm", fl, IrqType.SOF);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_sol_handler.asm", fl, IrqType.SOL);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_timer0_handler.asm", fl, IrqType.TIMER0);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_timer1_handler.asm", fl, IrqType.TIMER1);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_timer2_handler.asm", fl, IrqType.TIMER2);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_mouse_handler.asm", fl, IrqType.MOUSE);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_keyboard_handler.asm", fl, IrqType.KEYBOARD);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_collision0_handler.asm", fl, IrqType.KEYBOARD);
                WriteInterruptHandler(folder + Path.DirectorySeparatorChar + "_collision1_handler.asm", fl, IrqType.KEYBOARD);
            }
        }

        private string BuildIrqReg0String(bool irqSOF, bool irqSOL, bool irqTMR0, bool irqTMR1, bool irqTMR2, bool irqRTC, bool irqFDC, bool irqMOUSE)
        {
            List<string> irqs = new List<string>();
            if (irqSOF)
            {
                irqs.Add("FNX0_INT00_SOF");
            }
            if (irqSOL)
            {
                irqs.Add("FNX0_INT01_SOL");
            }
            if (irqTMR0)
            {
                irqs.Add("FNX0_INT02_TMR0");
            }
            if (irqTMR1)
            {
                irqs.Add("FNX0_INT03_TMR1");
            }
            if (irqTMR2)
            {
                irqs.Add("FNX0_INT04_TMR2");
            }
            if (irqMOUSE)
            {
                irqs.Add("FNX0_INT07_MOUSE");
            }
            if (irqs.Count == 0)
            {
                return "$0";
            }
            else
            {
                return string.Join("|", irqs);
            }
        }


        private string BuildIrqReg1String(bool irqKBD, bool irqSC0, bool irqSC1, bool irqCOM2, bool irqCOM1, bool irqMPU401, bool irqLPT, bool irqSDCARD)
        {
            List<string> irqs = new List<string>();
            if (irqKBD)
            {
                irqs.Add("FNX1_INT00_KBD");
            }
            if (irqSC0)
            {
                irqs.Add("FNX1_INT01_SC0");
            }
            if (irqSC1)
            {
                irqs.Add("FNX1_INT02_SC1");
            }
            if (irqs.Count == 0)
            {
                return "$0";
            }
            else
            {
                return string.Join("|", irqs);
            }
        }

        private void WriteInterruptHandler(string filename, FoenixLexer fl, IrqType irq)
        {
            List<TokenMatch> sub = fl.GetSub(irq.ToString() + "_IRQ_HANDLER");
            List<string> lines = new List<string>();
            if (sub != null)
            {
                foreach (TokenMatch tm in sub)
                {
                    lines.AddRange(GetTemplate(tm));
                }
            }
            File.WriteAllLines(filename, lines);
        }

        private void ViewAssetsButton_Click(object sender, EventArgs e)
        {
            // show the asset window
            if (AssetWindow.Instance.Visible)
            {
                AssetWindow.Instance.BringToFront();
            }
            else
            {
                AssetWindow.Instance.Show();
            }
        }

        private void GameGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // this cancels the close event.
            this.Hide();
        }

        private List<string> GetTemplate(Asset asset)
        {
            return new List<string>()
            {
                asset.label,
                ".binary \"" + asset.filename.Replace("\"","") + "\""
            };
        }

        private List<string> GetTemplate(TokenMatch tm)
        {
            List<string> template;
            templates.TryGetValue(tm.TokenType.ToString(), out template);
            List<string> result = new List<string>();
            if (template != null)
            {
                foreach (string line in template)
                {

                    //string value = Regex.Replace(line, "(.*)\\{([0-9]*)\\}(.*)", m =>
                    string value = Regex.Replace(line, @"\{([0-9]+)\}", m =>
                    {
                        if (m.Success && m.Groups.Count > 0)
                        {
                            int index = int.Parse(m.Groups[1].Value);
                            if (tm.groups.Count > index - 1)
                            {
                                return tm.groups[index - 1];
                            }
                            else
                            {
                                return m.Value;
                            }

                        }
                        else
                        {
                            return m.Value;
                        }
                    });
                    result.Add(value);
                }
            }
            return result;
        }

        private void ReloadTemplates()
        {
            templates = new Dictionary<string, List<string>>();
            string[] AllTemplates = File.ReadAllLines(@"Resources\\GameGeneratorTemplates.txt");
            string templateName = null; ;
            List<string> lines = new List<string>();
            foreach (string line in AllTemplates)
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (templateName != null)
                    {
                        templates.Add(templateName, lines);
                        lines = new List<string>();
                    }
                    templateName = line.Substring(1, line.Length - 2);
                }
                else
                {
                    lines.Add(line);
                }
            }
            if (templateName != null)
            {
                templates.Add(templateName, lines);
            }
        }
        private void cbIRQ_CheckedChanged(object sender, EventArgs e)
        {
            // if the IRQ subroutine doesn't exist, add it.
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
            {
                string irq = cb.Text.Replace("IRQ", "").Trim().ToUpper();
                FoenixLexer fl = new FoenixLexer(CodeTextBox.Text);
                if (fl.GetSub(irq + "_IRQ_HANDLER") == null)
                {
                    CodeTextBox.Text += "\r\n" +
                        irq + "_IRQ_HANDLER\r\n" +
                        "{\r\n" +
                        "}\r\n";
                }
            }
        }

        // Call the lexer and print the errors to the user
        private void GG_Validate(FoenixLexer fl)
        {
            // Get errors
            // Display errors
        }
    }
}
