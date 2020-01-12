using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using MaterialSkin;
using MaterialSkin.Controls;

namespace WindowsFormsApp2
{
    public partial class Form1 : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        private IDictionary<GeckoHtmlElement, string> elementStyles = new Dictionary<GeckoHtmlElement, string>();
        private GeckoDocument documentx;
        GeckoHtmlElement element;
        public Form1()
        {
            InitializeComponent();
            Xpcom.Initialize("Firefox64");
            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;

            // Set this to false to disable backcolor enforcing on non-materialSkin components
            // This HAS to be set before the AddFormToManage()
            materialSkinManager.EnforceBackcolorOnAllComponents = false;

            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            // MaterialSkinManager properties
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);


        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            documentx =geckoWebBrowser1.Document;
            toolStripTextBox1.Text = e.Uri.ToString();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate("www.google.com");
        }

        private void geckoWebBrowser1_DomMouseDown(object sender, DomMouseEventArgs e)
        {
            if (e.Button == GeckoMouseButton.Right)
            {
                dataGridView1.Rows.Add(richTextBox1.Text, richTextBox2.Text, richTextBox3.Text);
            }
        }

        private void geckoWebBrowser1_DomMouseOver(object sender, DomMouseEventArgs e)
        {
            try
            {
                 element = (GeckoHtmlElement)geckoWebBrowser1.Document.ElementFromPoint(e.ClientX, e.ClientY);
                if (!this.elementStyles.ContainsKey(element))
                {
                    string style = element.Style.CssText;
                    elementStyles.Add(element, style);
                    element.SetAttribute("style", style + ";border-style: solid;border-color: #FF0000;");
                    richTextBox1.Text = element.TextContent.Trim();
                    richTextBox2.Text = element.TagName.Trim();
                    richTextBox3.Text = element.OuterHtml.Trim();
                }
             
                
            }
            catch (Exception ex)
            {

            }
        }

        private void geckoWebBrowser1_DomMouseOut(object sender, DomMouseEventArgs e)
        {
            try
            {
                
                if (this.elementStyles.ContainsKey(element))
                {
                    string style = this.elementStyles[element];
                    this.elementStyles.Remove(element);
                    element.SetAttribute("style", style);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate(toolStripTextBox1.Text);
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                geckoWebBrowser1.Navigate(toolStripTextBox1.Text);
            }
        }
    }
    }