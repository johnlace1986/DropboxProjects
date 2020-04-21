using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaPlayer.Presentation.UserControls.Effects
{
    /// <summary>
    /// Interaction logic for InnerGlow.xaml
    /// </summary>
    public partial class InnerGlow : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty GlowThicknessProperty = DependencyProperty.Register("GlowThickness", typeof(Thickness), typeof(InnerGlow));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InnerGlow));

        public static readonly DependencyProperty ColourProperty = DependencyProperty.Register("Colour", typeof(Color), typeof(InnerGlow));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the thickness of the glow
        /// </summary>
        public Thickness GlowThickness
        {
            get { return (Thickness)GetValue(InnerGlow.GlowThicknessProperty); }
            set { SetValue(InnerGlow.GlowThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the inner glow
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(InnerGlow.CornerRadiusProperty); }
            set { SetValue(InnerGlow.CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the colour of the glow
        /// </summary>
        public Color Colour
        {
            get { return (Color)GetValue(InnerGlow.ColourProperty); }
            set { SetValue(InnerGlow.ColourProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the InnerGlow class
        /// </summary>
        public InnerGlow()
        {
            InitializeComponent();

            GlowThickness = new Thickness(6);
            CornerRadius = new CornerRadius(0);
            Colour = Colors.White;
        }

        #endregion
    }
}
