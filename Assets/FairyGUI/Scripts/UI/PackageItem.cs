using System.Collections.Generic;
using UnityEngine;
using FairyGUI.Utils;
using static FairyGUI.UIObjectFactory;

namespace FairyGUI
{
    /// <summary>
    /// 
    /// </summary>
    public class PackageItem
    {
        public UIPackage owner;

        public PackageItemType type;    //Component             //Atlas

        public ObjectType objectType;   //ObjectType.Component

        public string id;               //iz4i0                 //atlas0

        public string name;             //BagComponent          //null

        public int width;               //500                   //512

        public int height;              //500                   //256

        public string file;             //null                  //MyBag_atlas0.png

        public bool exported;           //true                  //false

        public NTexture texture;

        public ByteBuffer rawData;

        public string[] branches;

        public string[] highResolution;

        //image
        public Rect? scale9Grid;

        public bool scaleByTile;

        public int tileGridIndice;

        public PixelHitTestData pixelHitTestData;

        //movieclip
        public float interval;

        public float repeatDelay;

        public bool swing;

        public MovieClip.Frame[] frames;

        //component
        public bool translated;

        public GComponentCreator extensionCreator;

        //font
        public BitmapFont bitmapFont;

        //sound
        public NAudioClip audioClip;

        //spine/dragonbones
        public Vector2 skeletonAnchor;

        public object skeletonAsset;

        public HashSet<GLoader3D> skeletonLoaders;

        public object Load()
        {
            return owner.GetItemAsset(this);
        }

        public PackageItem getBranch()
        {
            if (branches != null && owner._branchIndex != -1)
            {
                string itemId = branches[owner._branchIndex];

                if (itemId != null)

                    return owner.GetItem(itemId);
            }

            return this;
        }

        public PackageItem getHighResolution()
        {
            if (highResolution != null && GRoot.contentScaleLevel > 0)
            {
                int i = GRoot.contentScaleLevel - 1;

                if (i >= highResolution.Length)

                    i = highResolution.Length - 1;

                string itemId = highResolution[i];

                if (itemId != null)

                    return owner.GetItem(itemId);
            }

            return this;
        }
    }
}
