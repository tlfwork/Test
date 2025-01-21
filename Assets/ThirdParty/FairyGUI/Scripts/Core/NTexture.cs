﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FairyGUI
{
    public enum DestroyMethod
    {
        Destroy,

        Unload,

        None,

        ReleaseTemp,

        Custom
    }

    public class NTexture
    {
        /// <summary>
        /// This event will trigger when a texture is destroying if its destroyMethod is Custom
        /// </summary>
        public static event Action<Texture> CustomDestroyMethod;

        public Rect uvRect;

        public bool rotated;

        public int refCount;

        public float lastActive;

        public DestroyMethod destroyMethod;

        /// <summary>
        /// This event will trigger when texture reloaded and size changed.
        /// </summary>
        public event Action<NTexture> onSizeChanged;

        /// <summary>
        /// This event will trigger when ref count is zero.
        /// </summary>
        public event Action<NTexture> onRelease;

        Texture _nativeTexture;

        Texture _alphaTexture;

        Rect _region;

        Vector2 _offset;

        Vector2 _originalSize;

        NTexture _root; 

        Dictionary<string, MaterialManager> _materialManagers;

        internal static Texture2D CreateEmptyTexture()
        {
            Texture2D emptyTexture = new Texture2D(1, 1, TextureFormat.RGB24, false);

            emptyTexture.name = "White Texture";

            emptyTexture.hideFlags = DisplayObject.hideFlags;

            emptyTexture.SetPixel(0, 0, Color.white);

            emptyTexture.Apply();

            return emptyTexture;
        }

        static NTexture _empty;

        public static NTexture Empty
        {
            get
            {
                if (_empty == null)

                    _empty = new NTexture(CreateEmptyTexture());

                return _empty;
            }
        }

        public static void DisposeEmpty()
        {
            if (_empty != null)
            {
                NTexture tmp = _empty;

                _empty = null;

                tmp.Dispose();
            }
        }

        #region

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitializeOnLoad()
        {
            DisposeEmpty();

            CustomDestroyMethod = null;
        }
#endif

        #endregion

        public NTexture(Texture texture) : this(texture, null, 1, 1)
        {
        }

        //Atals走这里  tex null 1 1
        public NTexture(Texture texture, Texture alphaTexture, float xScale, float yScale)
        {
            _root = this;

            _nativeTexture = texture;

            _alphaTexture = alphaTexture;

            uvRect = new Rect(0, 0, xScale, yScale);

            if (yScale < 0)
            {
                uvRect.y = -yScale;

                uvRect.yMax = 0;
            }
            if (xScale < 0)
            {
                uvRect.x = -xScale;

                uvRect.xMax = 0;
            }
            if (_nativeTexture != null)

                _originalSize = new Vector2(_nativeTexture.width, _nativeTexture.height);

            _region = new Rect(0, 0, _originalSize.x, _originalSize.y);
        }

        public NTexture(Texture texture, Rect region)
        {
            _root = this;

            _nativeTexture = texture;

            _region = region;

            _originalSize = new Vector2(_region.width, _region.height);

            if (_nativeTexture != null)

                uvRect = new Rect(region.x / _nativeTexture.width, 1 - region.yMax / _nativeTexture.height,

                    region.width / _nativeTexture.width, region.height / _nativeTexture.height);
            else
                uvRect.Set(0, 0, 1, 1);
        }

        public NTexture(NTexture root, Rect region, bool rotated)
        {
            _root = root;

            this.rotated = rotated;

            region.x += root._region.x;

            region.y += root._region.y;

            uvRect = new Rect(region.x * root.uvRect.width / root.width, 1 - region.yMax * root.uvRect.height / root.height,

                region.width * root.uvRect.width / root.width, region.height * root.uvRect.height / root.height);

            if (rotated)
            {
                float tmp = region.width;

                region.width = region.height;

                region.height = tmp;

                tmp = uvRect.width;

                uvRect.width = uvRect.height;

                uvRect.height = tmp;
            }

            _region = region;

            _originalSize = _region.size;
        }

        //img 走这里 x:302.00, y:189.00, width:100.00, height:100.00   false       100.00, 100.00      0, 0
        public NTexture(NTexture root, Rect region, bool rotated, Vector2 originalSize, Vector2 offset)

            : this(root, region, rotated)
        {
            _originalSize = originalSize;

            _offset = offset;
        }

        public NTexture(Sprite sprite)
        {
            Rect rect = sprite.textureRect;

            rect.y = sprite.texture.height - rect.yMax;

            _root = this;

            _nativeTexture = sprite.texture;

            _region = rect;

            _originalSize = new Vector2(_region.width, _region.height);

            uvRect = new Rect(_region.x / _nativeTexture.width, 1 - _region.yMax / _nativeTexture.height,

                _region.width / _nativeTexture.width, _region.height / _nativeTexture.height);
        }

        public int width
        {
            get { return (int)_region.width; }
        }

        public int height
        {
            get { return (int)_region.height; }
        }

        public Vector2 offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public Vector2 originalSize
        {
            get { return _originalSize; }
            set { _originalSize = value; }
        }

        public Rect GetDrawRect(Rect drawRect)
        {
            return GetDrawRect(drawRect, FlipType.None);
        }

        public Rect GetDrawRect(Rect drawRect, FlipType flip)
        {
            if (_originalSize.x == _region.width && _originalSize.y == _region.height)
                return drawRect;

            float sx = drawRect.width / _originalSize.x;
            float sy = drawRect.height / _originalSize.y;
            Rect rect = new Rect(_offset.x * sx, _offset.y * sy, _region.width * sx, _region.height * sy);

            if (flip != FlipType.None)
            {
                if (flip == FlipType.Horizontal || flip == FlipType.Both)
                {
                    rect.x = drawRect.width - rect.xMax;
                }
                if (flip == FlipType.Vertical || flip == FlipType.Both)
                {
                    rect.y = drawRect.height - rect.yMax;
                }
            }

            return rect;
        }

        public void GetUV(Vector2[] uv)
        {
            uv[0] = uvRect.position;
            uv[1] = new Vector2(uvRect.xMin, uvRect.yMax);
            uv[2] = new Vector2(uvRect.xMax, uvRect.yMax);
            uv[3] = new Vector2(uvRect.xMax, uvRect.yMin);
            if (rotated)
            {
                float xMin = uvRect.xMin;
                float yMin = uvRect.yMin;
                float yMax = uvRect.yMax;

                float tmp;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 m = uv[i];
                    tmp = m.y;
                    m.y = yMin + m.x - xMin;
                    m.x = xMin + yMax - tmp;
                    uv[i] = m;
                }
            }
        }

        public NTexture root
        {
            get { return _root; }
        }

        public bool disposed
        {
            get { return _root == null; }
        }

        public Texture nativeTexture
        {
            get { return _root != null ? _root._nativeTexture : null; }
        }

        public Texture alphaTexture
        {
            get { return _root != null ? _root._alphaTexture : null; }
        }

        public MaterialManager GetMaterialManager(string shaderName)
        {
            if (_root != this)
            {
                if (_root == null)

                    return null;
                else
                    return _root.GetMaterialManager(shaderName);
            }

            if (_materialManagers == null)

                _materialManagers = new Dictionary<string, MaterialManager>();

            MaterialManager mm;

            if (!_materialManagers.TryGetValue(shaderName, out mm))
            {
                mm = new MaterialManager(this, ShaderConfig.GetShader(shaderName));

                _materialManagers.Add(shaderName, mm);
            }

            return mm;
        }

        public void Unload()
        {
            Unload(false);
        }

        public void Unload(bool destroyMaterials)
        {
            if (this == _empty)

                return;

            if (_root != this)

                throw new Exception("Unload is not allow to call on none root NTexture.");

            if (_nativeTexture != null)
            {
                DestroyTexture();

                if (destroyMaterials)

                    DestroyMaterials();
                else
                    RefreshMaterials();
            }
        }

        void DestroyTexture()
        {
            switch (destroyMethod)
            {
                case DestroyMethod.Destroy:

                    Object.DestroyImmediate(_nativeTexture, true);

                    if (_alphaTexture != null) Object.DestroyImmediate(_alphaTexture, true);

                    break;

                case DestroyMethod.Unload:

                    Resources.UnloadAsset(_nativeTexture);

                    if (_alphaTexture != null) Resources.UnloadAsset(_alphaTexture);

                    break;

                case DestroyMethod.ReleaseTemp:

                    RenderTexture.ReleaseTemporary((RenderTexture)_nativeTexture);

                    if (_alphaTexture is RenderTexture) RenderTexture.ReleaseTemporary((RenderTexture)_alphaTexture);

                    break;

                case DestroyMethod.Custom:

                    if (CustomDestroyMethod == null) Debug.LogWarning("NTexture.CustomDestroyMethod must be set to handle DestroyMethod.Custom");
                    else
                    {
                        CustomDestroyMethod(_nativeTexture);

                        if (_alphaTexture != null)

                            CustomDestroyMethod(_alphaTexture);
                    }

                    break;
            }

            _nativeTexture = null;

            _alphaTexture = null;
        }

        public void Reload(Texture nativeTexture, Texture alphaTexture)
        {
            if (_root != this)

                throw new System.Exception("Reload is not allow to call on none root NTexture.");

            if (_nativeTexture != null && _nativeTexture != nativeTexture)

                DestroyTexture();

            _nativeTexture = nativeTexture;

            _alphaTexture = alphaTexture;

            Vector2 lastSize = _originalSize;

            if (_nativeTexture != null)

                _originalSize = new Vector2(_nativeTexture.width, _nativeTexture.height);
            else
                _originalSize = Vector2.zero;

            _region = new Rect(0, 0, _originalSize.x, _originalSize.y);

            RefreshMaterials();

            if (onSizeChanged != null && lastSize != _originalSize)

                onSizeChanged(this);
        }

        void RefreshMaterials()
        {
            if (_materialManagers != null && _materialManagers.Count > 0)
            {
                Dictionary<string, MaterialManager>.Enumerator iter = _materialManagers.GetEnumerator();
                while (iter.MoveNext())
                    iter.Current.Value.RefreshMaterials();
                iter.Dispose();
            }
        }

        void DestroyMaterials()
        {
            if (_materialManagers != null && _materialManagers.Count > 0)
            {
                Dictionary<string, MaterialManager>.Enumerator iter = _materialManagers.GetEnumerator();

                while (iter.MoveNext())

                    iter.Current.Value.DestroyMaterials();

                iter.Dispose();
            }
        }

        public void AddRef()
        {
            if (_root == null) //disposed

                return;

            if (_root != this && refCount == 0)

                _root.AddRef();

            refCount++;
        }

        public void ReleaseRef()
        {
            if (_root == null) //disposed

                return;

            refCount--;

            if (refCount == 0)
            {
                if (_root != this)

                    _root.ReleaseRef();

                if (onRelease != null)

                    onRelease(this);
            }
        }

        public void Dispose()
        {
            if (this == _empty)

                return;

            if (_root == this)

                Unload(true);

            _root = null;

            onSizeChanged = null;

            onRelease = null;
        }
    }
}
