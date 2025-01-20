/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MyBag
{
    public partial class UI_Component2 : GComponent
    {
        public GImage m_img_1;
        public const string URL = "ui://f5h3if95tq4m4";

        public static UI_Component2 CreateInstance()
        {
            return (UI_Component2)UIPackage.CreateObject("MyBag", "Component2");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_img_1 = (GImage)GetChildAt(0);
        }
    }
}