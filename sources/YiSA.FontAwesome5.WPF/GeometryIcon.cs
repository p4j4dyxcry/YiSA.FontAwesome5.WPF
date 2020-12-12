﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SvgResourceGenerator;

namespace YiSA.FontAwesome5
{
    /// <summary>
    /// This control draws the Geometry
    /// </summary>
    public class GeometryIcon : Control
    {
        /// <summary>
        /// Set icon with enums.
        /// </summary>
        #region FaIconType
        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            nameof(Icon), typeof(IconType), typeof(GeometryIcon), new FrameworkPropertyMetadata(default(IconType),FrameworkPropertyMetadataOptions.AffectsRender));

        public IconType Icon
        {
            get => (IconType)GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
        }
        #endregion

        /// <summary>
        /// Set icon stretch.
        /// </summary>
        #region FaIconType
        public static readonly DependencyProperty Property = DependencyProperty.Register(
            nameof(Stretch), typeof(Stretch), typeof(GeometryIcon), new FrameworkPropertyMetadata(Stretch.Uniform,FrameworkPropertyMetadataOptions.AffectsRender));

        public Stretch Stretch
        {
            get => (Stretch)GetValue(Property);
            set => SetValue(Property, value);
        }
        #endregion

        /// <summary>
        /// Set disabled Foreground Brush.
        /// </summary>
        #region DisableBrushProperty
        public static readonly DependencyProperty DisableForegroundBrushProperty = DependencyProperty.Register(
            nameof(DisableForegroundBrush), typeof(Brush), typeof(GeometryIcon), new FrameworkPropertyMetadata(Brushes.DimGray,FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush DisableForegroundBrush
        {
            get => (Brush)GetValue(DisableForegroundBrushProperty);
            set => SetValue(DisableForegroundBrushProperty, value);
        }
        #endregion

        /// <summary>
        /// Set Rotate Angle.
        /// </summary>
        #region Angle
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
            nameof(Angle), typeof(double), typeof(GeometryIcon), new FrameworkPropertyMetadata(0.0d, UpdateTransformPropertyChangedCallback));

        public double Angle
        {
            get => (double) GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
        #endregion


        private TranslateTransform Translate { get; } = new TranslateTransform();
        private RotateTransform Rotate { get; } = new RotateTransform();
        private ScaleTransform Scale { get; } = new ScaleTransform();
        
        // caches
        private static readonly Dictionary<IconType, Geometry> GeometryCache = new Dictionary<IconType, Geometry>();
        private static readonly Dictionary<int, DrawingBrush> BrushCache = new Dictionary<int, DrawingBrush>();
        
        /// <summary>
        /// static Ctor
        /// </summary>
        static GeometryIcon()
        {
            IsEnabledProperty.OverrideMetadata(typeof(GeometryIcon), new FrameworkPropertyMetadata(true,FrameworkPropertyMetadataOptions.AffectsRender));
            BackgroundProperty.OverrideMetadata(typeof(GeometryIcon), new FrameworkPropertyMetadata(Brushes.Transparent,FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public GeometryIcon()
        {
            var matrix = new TransformGroup();
            matrix.Children.Add(Scale);
            matrix.Children.Add(Rotate);
            matrix.Children.Add(Translate);
            RenderTransform = matrix;

            LayoutUpdated += (s, e) => UpdateTransformImpl();
        }

        /// <summary>
        /// Get a brush that matches the icon and color.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="foreground"></param>
        /// <returns></returns>
        public static DrawingBrush GetOrCreateBrush(IconType icon, Brush foreground)
        {
            int hash = icon.GetHashCode() ^
                       foreground.GetHashCode();

            if (BrushCache.TryGetValue(hash, out var brush) is false)
            {
                if (GeometryCache.TryGetValue(icon, out var geometry) is false)
                {
                    geometry = GeometryCache[icon] = _IconFactory.Icons[icon].Value;
                }

                var drawing = new GeometryDrawing()
                {
                    Geometry = geometry,
                    Brush = foreground,
                };

                brush = new DrawingBrush()
                {
                    Drawing = drawing,
                };

                BrushCache[hash] = brush;
            }

            return brush;
        }

        
        /// <summary>
        /// UpdateTransform for property changed call back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void UpdateTransformPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is GeometryIcon fa5) fa5.UpdateTransformImpl();
        }

        /// <summary>
        /// UpdateTransform
        /// </summary>
        private void UpdateTransformImpl()
        {
            if (!double.IsNaN(ActualWidth) && !double.IsNaN(ActualHeight))
            {
                this.Rotate.Angle = Angle;
                this.Rotate.CenterX = ActualWidth / 2d;
                this.Rotate.CenterY = ActualHeight / 2d;
            }
        }

        /// <summary>
        /// Layout
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            // Temporary size when it cannot be calculated
            var tempSize = 18d;

            var w = double.IsNaN(Width)  ? constraint.Width : Width;
            var h = double.IsNaN(Height) ? constraint.Height : Height;

            if (double.IsInfinity(w))
                w = tempSize;
            if (double.IsInfinity(h))
                h = tempSize;

            return new Size(w, h);
        }

        /// <summary>
        /// Render for Font Awesome icon.
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Background != null)
                drawingContext.DrawRectangle(Background,null,new Rect(0,0,ActualWidth,ActualHeight));

            var iconCBrush = IsEnabled ? Foreground : DisableForegroundBrush;
            iconCBrush ??= Brushes.Transparent;

            var brush = GetOrCreateBrush(
                Icon,
                iconCBrush);
            brush.Stretch = Stretch;

            drawingContext.DrawRectangle(brush,null,new Rect(0,0,ActualWidth,ActualHeight));
        }

    }
}
