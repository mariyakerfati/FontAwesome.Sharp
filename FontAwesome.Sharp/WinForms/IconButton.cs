using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FontAwesome.Sharp
{
    public class IconButton : IconButton<IconChar>, IHaveIconFont
    {
        public IconButton() : base(IconChar.Star.FontFamilyFor())
        {
        }

        protected override FontFamily FontFor(IconChar icon)
        {
            return icon.FontFamilyFor(_iconFont);
        }

        private IconFont _iconFont = Sharp.IconFont.Auto;

        [Category("FontAwesome")]
        public IconFont IconFont
        {
            get => _iconFont;
            set
            {
                if (_iconFont.CompareTo(value) == 0) return;
                _iconFont = value;
                UpdateImage();

            }
        }
    }

    public abstract class IconButton<TEnum> : Button, IFormsIcon<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        private readonly FontFamily _fontFamily;
        private Color _color = Color.Black;
        private TEnum _icon;
        private int _size = IconHelper.DefaultSize;
        private FlipOrientation _flip = FlipOrientation.Normal;
        private double _rotation;

        protected IconButton(FontFamily fontFamily = null)
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum.");
            _fontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
            UpdateImage();
        }

        protected virtual FontFamily FontFor(TEnum icon)
        {
            return _fontFamily;
        }

        [Category("FontAwesome")]
        [TypeConverter(typeof(IconConverter))]
        public TEnum IconChar
        {
            get => _icon;
            set
            {
                if (_icon.CompareTo(value) == 0) return;
                _icon = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public int IconSize
        {
            get => _size;
            set
            {
                if (_size == value) return;
                _size = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public Color IconColor
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public FlipOrientation Flip
        {
            get => _flip;
            set
            {
                if (_flip == value) return;
                _flip = value;
                UpdateImage();
            }
        }

        [Category("FontAwesome")]
        public double Rotation
        {
            get => _rotation;
            set
            {
                var v = value % 360.0;
                if (Math.Abs(_rotation - v) <= 0.5) return;
                _rotation = v;
                UpdateImage();
            }
        }

        [ReadOnly(true)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        protected void UpdateImage()
        {
            Image = FontFor(_icon).ToBitmap(_icon, _size, _color, _rotation, _flip);
        }

        public bool ShouldSerializeImage() { return false; }
    }
}
