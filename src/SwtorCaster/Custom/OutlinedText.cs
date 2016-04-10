namespace SwtorCaster.Custom
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    [ContentProperty("Text")]
    public class OutlinedText : FrameworkElement
    {
        private Geometry _textGeometry;

        private static void OnOutlineTextInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OutlinedText)d).CreateText();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            CreateText();
            drawingContext.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), _textGeometry);
        }

        public void CreateText()
        {
            FontStyle fontStyle = FontStyles.Normal;
            FontWeight fontWeight = FontWeights.Medium;

            if (Bold) fontWeight = FontWeights.Bold;
            if (Italic) fontStyle = FontStyles.Italic;

            FormattedText formattedText = new FormattedText(
                Text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, fontStyle, fontWeight, FontStretches.Normal),
                FontSize,
                Brushes.Black // This brush does not matter since we use the geometry of the text. 
                );

            _textGeometry = formattedText.BuildGeometry(new Point(0, 0));

            MinWidth = formattedText.Width;
            MinHeight = formattedText.Height;
        }

        public bool Bold
        {
            get { return (bool)GetValue(BoldProperty); }
            set { SetValue(BoldProperty, value); }
        }

        public static readonly DependencyProperty BoldProperty = DependencyProperty.Register(
            "Bold",
            typeof(bool),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnOutlineTextInvalidated,
                null
                )
            );

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Colors.Yellow),
                flags: FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: OnOutlineTextInvalidated,
                coerceValueCallback: null
                )
            );

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public static readonly DependencyProperty FontProperty = DependencyProperty.Register(
            "FontFamily",
            propertyType: typeof(FontFamily),
            ownerType: typeof(OutlinedText),
            typeMetadata: new FrameworkPropertyMetadata(
                defaultValue:  new FontFamily(new Uri("pack://application:,,,/"), "./resources/#SF Distant Galaxy"),
                flags: FrameworkPropertyMetadataOptions.AffectsRender,
                propertyChangedCallback: OnOutlineTextInvalidated,
                coerceValueCallback: null
                )
            );

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            "FontSize",
            typeof(double),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                 36.0,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 OnOutlineTextInvalidated,
                 null
                 )
            );

        public bool Italic
        {
            get { return (bool)GetValue(ItalicProperty); }
            set { SetValue(ItalicProperty, value); }
        }

        public static readonly DependencyProperty ItalicProperty = DependencyProperty.Register(
            "Italic",
            typeof(bool),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                 false,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 OnOutlineTextInvalidated,
                 null
                 )
            );

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Brush),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                 new SolidColorBrush(Colors.Black),
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 OnOutlineTextInvalidated,
                 null
                 )
            );

        public ushort StrokeThickness
        {
            get { return (ushort)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(ushort),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                 (ushort)2,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 OnOutlineTextInvalidated,
                 null
                 )
            );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(OutlinedText),
            new FrameworkPropertyMetadata(
                 string.Empty,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 OnOutlineTextInvalidated,
                 null
                 )
            );
    }
}