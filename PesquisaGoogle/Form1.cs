using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace PesquisaGoogle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<Item> GetPesquisa(string query, int count)
        {
            var start = 0;

            var retorno = new List<Item>();

            var client = new HttpClient();

            while (retorno.Count < count)
            {
                var body = client.GetStringAsync("https://www.google.com.br/search?q=" + query + (start > 0 ? $"&start={start}" : "")).Result;

                var doc = new HtmlDocument();
                doc.LoadHtml(body);

                var resultList = doc.DocumentNode.SelectNodes("//div[@id='search']//div[@class='g']/h3/a");

                foreach (var node in resultList)
                {
                    if (retorno.Count == count) break;
                    var url = Regex.Replace(node.GetAttributeValue("href", "error").Replace("/url?q=", ""), "&amp.+", "");
                    var titulo = Regex.Replace(node.InnerHtml, @"<\/?b>", "");
                    retorno.Add(new Item
                    {
                        Titulo = titulo,
                        Link = url
                    });
                }

                start += 10;
            }

            return retorno;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var q = txtQuery.Text;

            if (string.IsNullOrWhiteSpace(q))
            {
                MessageBox.Show("Por favor, insira algo para fazer a busca.");
                return;
            }

            lvResultados.Items.Clear();

            var itens = GetPesquisa(q, 15);

            foreach (var item in itens)
            {
                var lvItem = new ListViewItem(item.Titulo);
                lvItem.SubItems.Add(item.Link);
                lvResultados.Items.Add(lvItem);
            }
        }
    }
}
