namespace SmartCalculator.Models;

public sealed record SkinDefinition(
    string Id,
    string DisplayName,
    double KeyRadius,
    double InnerRadius,
    double OvalRadius,
    double NavigatorRadius,
    double NavigatorButtonRadius,
    double BorderWidth,
    double InnerBorderWidth,
    double MarginX,
    double MarginY,
    double ShadowBlur,
    double ShadowDepth,
    double ShadowOpacity,
    byte HighlightAlpha,
    byte BevelAlpha,
    byte NavigatorOverlayAlpha,
    double HighlightHeight,
    string KeyGeometry,
    string NavigatorGeometry);
