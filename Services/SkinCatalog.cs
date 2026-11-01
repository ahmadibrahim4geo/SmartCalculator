using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using SmartCalculator.Models;

namespace SmartCalculator.Services;

public static class SkinCatalog
{
    // Clean, ergonomic geometries engineered for scientific calculators:
    // Every key maintains at least 84px of flat width at top/bottom, guaranteeing zero label clipping.
    private const string RoundedKey = "M8,0 H92 Q100,0 100,8 V52 Q100,60 92,60 H8 Q0,60 0,52 V8 Q0,0 8,0 Z";
    private const string SoftKey = "M12,0 H88 Q100,0 100,12 V48 Q100,60 88,60 H12 Q0,60 0,48 V12 Q0,0 12,0 Z";
    private const string RaisedKey = "M6,0 H94 Q100,0 100,6 V54 Q100,60 94,60 H6 Q0,60 0,54 V6 Q0,0 6,0 Z";
    private const string SquareKey = "M3,0 H97 Q100,0 100,3 V57 Q100,60 97,60 H3 Q0,60 0,57 V3 Q0,0 3,0 Z";
    private const string ChamferKey = "M5,0 H95 L100,5 V55 L95,60 H5 L0,55 V5 Z";
    private const string ConcaveKey = "M7,0 H93 Q100,0 100,7 V53 Q100,60 93,60 H7 Q0,60 0,53 V7 Q0,0 7,0 Z";
    private const string FlatKey = "M2,0 H98 Q100,0 100,2 V58 Q100,60 98,60 H2 Q0,60 0,58 V2 Q0,0 2,0 Z";
    private const string PebbleKey = "M16,0 H84 Q100,0 100,16 V44 Q100,60 84,60 H16 Q0,60 0,44 V16 Q0,0 16,0 Z";
    private const string MechanicalKey = "M5,0 H95 L98,6 V54 L95,60 H5 L2,54 V6 Z";
    private const string GlassKey = "M10,0 H90 Q100,0 100,10 V50 Q100,60 90,60 H10 Q0,60 0,50 V10 Q0,0 10,0 Z";
    private const string OctagonKey = "M7,0 H93 L100,7 V53 L93,60 H7 L0,53 V7 Z";
    private const string RetroKey = "M2,0 H98 V58 H2 Z";
    private const string LineKey = "M1,0 H99 V59 H1 Z";
    private const string CapsuleKey = "M18,0 H82 Q100,0 100,18 V42 Q100,60 82,60 H18 Q0,60 0,42 V18 Q0,0 18,0 Z";
    private const string ArcadeKey = "M14,0 H86 Q100,0 100,14 V46 Q100,60 86,60 H14 Q0,60 0,46 V14 Q0,0 14,0 Z";
    private const string ExecutiveKey = "M6,0 H94 L97,4 V56 L94,60 H6 L3,56 V4 Z";
    private const string TouchKey = "M4,0 H96 Q100,0 100,4 V56 Q100,60 96,60 H4 Q0,60 0,56 V4 Q0,0 4,0 Z";
    private const string CyberKey = "M4,0 H94 L100,6 V56 Q100,60 96,60 H4 Q0,60 0,56 V4 Q0,0 4,0 Z";
    private const string CompactKey = "M4,0 H96 Q100,0 100,4 V56 Q100,60 96,60 H4 Q0,60 0,56 V4 Q0,0 4,0 Z";
    private const string HighContrastKey = "M6,0 H94 Q100,0 100,6 V54 Q100,60 94,60 H6 Q0,60 0,54 V6 Q0,0 6,0 Z";

    // Mathematically proportioned navigation rockers that fully enclose the 60x42 center NAV button
    private const string PillNav = "M36,0 H86 C106,0 122,16 122,36 C122,56 106,72 86,72 H36 C16,72 0,56 0,36 C0,16 16,0 36,0 Z";
    private const string RoundNav = "M14,0 H108 Q122,0 122,14 V58 Q122,72 108,72 H14 Q0,72 0,58 V14 Q0,0 14,0 Z";
    private const string EllipseNav = "M61,0 A61,36 0 1 1 60.9,0 Z";
    private const string HexNav = "M18,0 H104 L122,36 L104,72 H18 L0,36 Z";
    private const string CrossNav = "M28,0 H94 V12 H122 V60 H94 V72 H28 V60 H0 V12 H28 Z";
    private const string OctagonNav = "M18,0 H104 L122,18 V54 L104,72 H18 L0,54 V18 Z";
    private const string DiamondNav = "M61,0 Q100,0 122,36 Q100,72 61,72 Q22,72 0,36 Q22,0 61,0 Z";
    private const string BowNav = "M20,0 H102 Q122,36 102,72 H20 Q0,36 20,0 Z";
    private const string SquircleNav = "M24,0 H98 C116,0 122,6 122,24 V48 C122,66 116,72 98,72 H24 C6,72 0,66 0,48 V24 C0,6 6,0 24,0 Z";
    private const string ChicletNav = "M30,0 H92 Q122,0 122,30 V42 Q122,72 92,72 H30 Q0,72 0,42 V30 Q0,0 30,0 Z";

    private static SkinDefinition Skin(
        string id, string name,
        double keyRadius, double innerRadius, double ovalRadius,
        double navigatorRadius, double navigatorButtonRadius,
        double borderWidth, double innerBorderWidth,
        double marginX, double marginY,
        double shadowBlur, double shadowDepth, double shadowOpacity,
        byte highlightAlpha, byte bevelAlpha, byte navigatorOverlayAlpha,
        double highlightHeight, string keyGeometry, string navigatorGeometry)
        => new(id, name, keyRadius, innerRadius, ovalRadius, navigatorRadius,
            navigatorButtonRadius, borderWidth, innerBorderWidth, marginX, marginY,
            shadowBlur, shadowDepth, shadowOpacity, highlightAlpha, bevelAlpha,
            navigatorOverlayAlpha, highlightHeight, keyGeometry, navigatorGeometry);

    public static IReadOnlyList<SkinDefinition> All { get; } = Array.AsReadOnly(new[]
    {
        Skin("classic-scientific", "Classic Scientific", 8, 6, 15, 36, 18, 1, 1, 1.5, 1.5, 2, 1, .28, 18, 18, 12, 14, RoundedKey, PillNav),
        Skin("classwiz-soft", "Modern Scientific Soft", 12, 10, 15, 22, 13, 1, 1, 1.5, 1.5, 2, 1, .18, 14, 14, 10, 11, SoftKey, EllipseNav),
        Skin("es-plus-raised", "Scientific Raised", 6, 5, 14, 34, 16, 1.5, 1, 1.5, 1.5, 4, 2, .38, 24, 22, 16, 15, RaisedKey, PillNav),
        Skin("graphing-pro", "Graphing Pro", 4, 3, 9, 14, 7, 1, .5, 1, 1, 1, 0, .12, 10, 8, 8, 7, SquareKey, RoundNav),
        Skin("prime-edge", "Graph Edge", 3, 2, 8, 18, 8, 1.5, .5, 1, 1, 3, 1, .24, 18, 26, 12, 6, ChamferKey, OctagonNav),
        Skin("sharp-compact", "Compact Concave", 7, 5, 13, 28, 13, 1, .5, 1, 1, 2, 1, .22, 14, 14, 10, 9, ConcaveKey, BowNav),
        Skin("modern-flat", "Modern Flat", 2, 2, 8, 10, 5, 1, 0, 1.5, 1.5, 0, 0, 0, 0, 0, 7, 0, FlatKey, CrossNav),
        Skin("soft-rounded", "Soft Rounded", 16, 14, 15, 34, 18, 1, .5, 1.5, 1.5, 3, 1, .20, 12, 10, 10, 10, PebbleKey, SquircleNav),
        Skin("mechanical-keycap", "Mechanical Keycap", 5, 3, 8, 12, 6, 2, 1.5, 2, 2, 5, 3, .46, 28, 34, 18, 10, MechanicalKey, CrossNav),
        Skin("glass-panel", "Glass Panel", 10, 8, 15, 30, 15, 1, 1, 1.5, 1.5, 5, 1, .25, 42, 34, 26, 16, GlassKey, EllipseNav),
        Skin("metallic-bevel", "Metallic Bevel", 7, 5, 11, 18, 9, 2, 1, 1.5, 1.5, 4, 2, .34, 30, 40, 20, 12, OctagonKey, HexNav),
        Skin("retro-desktop", "Retro Desktop", 2, 1, 5, 8, 4, 2, 1, 2, 2, 4, 2, .42, 16, 28, 14, 8, RetroKey, DiamondNav),
        Skin("minimal-line", "Minimal Line", 1, 1, 4, 6, 3, 1, 0, 1, 1, 0, 0, 0, 0, 0, 5, 0, LineKey, RoundNav),
        Skin("pill-keys", "Pill Keys", 18, 16, 15, 36, 18, 1, .5, 1.5, 1.5, 2, 1, .18, 12, 12, 9, 8, CapsuleKey, PillNav),
        Skin("arcade", "Arcade", 14, 12, 15, 32, 16, 2, 1.5, 2, 2, 6, 3, .48, 32, 30, 22, 14, ArcadeKey, CrossNav),
        Skin("executive", "Executive", 6, 5, 12, 20, 10, 1, 1, 1.5, 1.5, 3, 1, .25, 12, 24, 10, 9, ExecutiveKey, BowNav),
        Skin("touch-panel", "Touch Panel", 4, 3, 10, 14, 7, .5, .5, 1, 1, 1, 0, .08, 8, 6, 10, 4, TouchKey, EllipseNav),
        Skin("angular-tech", "Angular Tech", 4, 3, 5, 18, 8, 1.5, .5, 1.5, 1.5, 3, 1, .28, 20, 18, 16, 7, CyberKey, HexNav),
        Skin("compact-pro", "Compact Pro", 4, 3, 8, 16, 8, 1, .5, .75, .75, 2, 1, .20, 10, 12, 8, 6, CompactKey, OctagonNav),
        Skin("high-contrast", "High Contrast", 6, 5, 14, 24, 12, 2, 1, 1.5, 1.5, 2, 1, .30, 32, 36, 28, 13, HighContrastKey, ChicletNav)
    });

    public static int IndexOfId(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return -1;

        for (int i = 0; i < All.Count; i++)
        {
            if (string.Equals(All[i].Id, id, StringComparison.OrdinalIgnoreCase))
                return i;
        }

        return -1;
    }

    public static ResourceDictionary CreateResources(SkinDefinition skin)
    {
        var keyEffect = new DropShadowEffect
        {
            BlurRadius = skin.ShadowBlur,
            ShadowDepth = skin.ShadowDepth,
            Opacity = skin.ShadowOpacity,
            Color = Colors.Black,
            Direction = 270
        };

        return new ResourceDictionary
        {
            ["SkinKeyCornerRadius"] = new CornerRadius(skin.KeyRadius),
            ["SkinInnerCornerRadius"] = new CornerRadius(skin.InnerRadius),
            ["SkinOvalCornerRadius"] = new CornerRadius(skin.OvalRadius),
            ["SkinNavigatorCornerRadius"] = new CornerRadius(skin.NavigatorRadius),
            ["SkinNavigatorButtonCornerRadius"] = new CornerRadius(skin.NavigatorButtonRadius),
            ["SkinKeyBorderThickness"] = new Thickness(skin.BorderWidth),
            ["SkinInnerBorderThickness"] = new Thickness(skin.InnerBorderWidth),
            ["SkinNavigatorBorderThickness"] = new Thickness(Math.Max(1, skin.BorderWidth)),
            ["SkinKeyMargin"] = new Thickness(skin.MarginX, skin.MarginY, skin.MarginX, skin.MarginY),
            ["SkinKeyEffect"] = keyEffect,
            ["SkinHighlightBrush"] = BrushFromAlpha(skin.HighlightAlpha),
            ["SkinBevelBrush"] = BrushFromAlpha(skin.BevelAlpha),
            ["SkinNavigatorOverlayBrush"] = BrushFromAlpha(skin.NavigatorOverlayAlpha),
            ["SkinHighlightHeight"] = skin.HighlightHeight,
            ["SkinKeyStrokeWidth"] = skin.BorderWidth,
            ["SkinInnerStrokeWidth"] = skin.InnerBorderWidth,
            ["SkinKeyGeometry"] = ParseGeometry(skin.KeyGeometry),
            ["SkinNavigatorGeometry"] = ParseGeometry(skin.NavigatorGeometry)
        };
    }

    private static Geometry ParseGeometry(string data)
    {
        var geometry = Geometry.Parse(data);
        geometry.Freeze();
        return geometry;
    }

    private static SolidColorBrush BrushFromAlpha(byte alpha)
    {
        var brush = new SolidColorBrush(Color.FromArgb(alpha, 255, 255, 255));
        brush.Freeze();
        return brush;
    }
}
