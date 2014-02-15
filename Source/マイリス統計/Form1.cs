using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml;

namespace マイリス統計
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] formats = {
                @"mm\:ss",
                @"m\:ss",
                @"hhh\:mm\:ss",
                @"hh\:mm\:ss",
                @"h\:mm\:ss",
            };
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string rowxml = wc.DownloadString("http://www.nicovideo.jp/mylist/" + int.Parse(textBox1.Text) + "?rss=2.0");
            rowxml = rowxml.Replace("<![CDATA[", "").Replace("]]>", "").Replace("&", "%26");
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(rowxml);
            var items = xd["rss"]["channel"];
            dataGridView1.Rows.Clear();
            var alltime = new TimeSpan();
            XmlNodeList xnl = items.GetElementsByTagName("item");
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            foreach (XmlNode node in xnl)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["Title"].Value = node["title"].InnerText;
                XmlNode strong = node.SelectNodes("child::description/p[last()]")[0]["small"]["strong"];
                dataGridView1.Rows[index].Cells["Time"].Value = strong.InnerText;
                var ts = TimeSpan.ParseExact(strong.InnerText, formats, culture);
                alltime = alltime.Add(ts);
                dataGridView1.Rows[index].Cells["Link"].Value = node["link"].InnerText;
                dataGridView1.Rows[index].Cells["guid"].Value = node["guid"].InnerText;
                dataGridView1.Rows[index].Cells["pubDate"].Value = node["pubDate"].InnerText;
                dataGridView1.Rows[index].Cells["description"].Value = node["description"].InnerText;
            }
            label3.Text = alltime.ToString(@"hh\:mm\:ss");
        }
    }
}
