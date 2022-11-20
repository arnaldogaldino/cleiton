using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProjectMaster.Core.Extensions
{
    public static class ExtensionSelectList
    {
        public static SelectList ToSelectList<T>(this IEnumerable<T> list, Func<T, string> text, Func<T, object> value, string opcaoSelecionada = "")
        {
            var itens = list.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f).ToString()
            }).ToList();

            return string.IsNullOrEmpty(opcaoSelecionada) ? new SelectList(itens, "Value", "Text") : new SelectList(itens, "Value", "Text", opcaoSelecionada);
        }

        public static void AdicionarOpcaoPadrao(List<SelectListItem> itens, string textoPadrao, string valorPadrao)
        {
            if (!string.IsNullOrEmpty(textoPadrao))
            {
                itens.Insert(0, new SelectListItem()
                {
                    Text = textoPadrao,
                    Value = valorPadrao
                });
            }
        }
    }
}