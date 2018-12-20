using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PesquisaGoogle
{
    public class Item
    {
        public string Titulo { get; set; }
        public string Link { get; set; }

        public Item()
        {

        }
        public Item(string titulo, string link)
        {
            Titulo = titulo;
            Link = link;
        }
    }
}
