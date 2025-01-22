/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace MyBag
{
    public partial class UI_Component1 : GComponent
    {
        public GImage m_img_1;
        public GImage m_img_2;
        public GImage m_gchild_1;
        public GImage m_gchild_2;
        public GImage m_gchild_3;
        public GGroup m_g_1;
        public GButton m_btn_1;
        public const string URL = "ui://f5h3if95ljez0";

        public static UI_Component1 CreateInstance()
        {
            return (UI_Component1)UIPackage.CreateObject("MyBag", "Component1");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_img_1 = (GImage)GetChildAt(0);
            m_img_2 = (GImage)GetChildAt(2);
            m_gchild_1 = (GImage)GetChildAt(3);
            m_gchild_2 = (GImage)GetChildAt(4);
            m_gchild_3 = (GImage)GetChildAt(5);
            m_g_1 = (GGroup)GetChildAt(6);
            m_btn_1 = (GButton)GetChildAt(7);
        }
    }
}