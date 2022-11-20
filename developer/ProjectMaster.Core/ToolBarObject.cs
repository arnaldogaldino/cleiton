using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMaster.Core
{
    public class ToolBarObject
    {
        public List<ToolBarButton> Buttons = new List<ToolBarButton>();
    }

    public class ToolBarButton
    {      
        public ToolBarButton()
        { }

        public ToolBarButton(ToolBarButtonType button, string title, string alt, string name, string onClick, string href, bool show, bool enable)
        {
            Button = button;
            Title = title;
            Alt = alt;
            Name = name;
            OnClick = onClick;
            Href = href;
            Show = show;
            Enable = enable;        
        }

        public ToolBarButtonType Button;
        public string Title;
        public string Alt;
        public string Name;
        public string OnClick;
        public string Href;
        public bool Show;
        public bool Enable;        
    }

    public enum ToolBarButtonType
    {
        CadastrarNovo=0,
        Editar=1,
        Excluir=2,
        Gravar=3,
        Cancelar=4,
        Voltar=5,
    }

}
