[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$stylesDirectory = Join-Path $projectRoot 'Styles'
$templatePath = Join-Path $stylesDirectory 'Theme99-VintageIvory.xaml'
$template = Get-Content -LiteralPath $templatePath -Raw

function ConvertFrom-HexColor([string]$Color) {
    $value = $Color.TrimStart('#')
    return @(
        [Convert]::ToInt32($value.Substring(0, 2), 16),
        [Convert]::ToInt32($value.Substring(2, 2), 16),
        [Convert]::ToInt32($value.Substring(4, 2), 16)
    )
}

function Mix-HexColor([string]$First, [string]$Second, [double]$SecondWeight) {
    $a = ConvertFrom-HexColor $First
    $b = ConvertFrom-HexColor $Second
    $weight = [Math]::Clamp($SecondWeight, [double]0, [double]1)
    $r = [Math]::Round($a[0] * (1 - $weight) + $b[0] * $weight)
    $g = [Math]::Round($a[1] * (1 - $weight) + $b[1] * $weight)
    $blue = [Math]::Round($a[2] * (1 - $weight) + $b[2] * $weight)
    return '#{0:X2}{1:X2}{2:X2}' -f [int]$r, [int]$g, [int]$blue
}

function Get-ContrastText([string]$Background) {
    $rgb = ConvertFrom-HexColor $Background
    $yiq = (($rgb[0] * 299) + ($rgb[1] * 587) + ($rgb[2] * 114)) / 1000
    if ($yiq -ge 150) { return '#111418' }
    return '#FFFFFF'
}

function Set-ColorResource([string]$Text, [string]$Key, [string]$Color) {
    $escapedKey = [Regex]::Escape($Key)
    $pattern = '(<Color x:Key="' + $escapedKey + '">)#[0-9A-Fa-f]{6,8}(</Color>)'
    if (-not [Regex]::IsMatch($Text, $pattern)) { throw "Color resource '$Key' was not found in the template." }
    return [Regex]::Replace($Text, $pattern, ('$1' + $Color + '$2'))
}

function Set-BrushResource([string]$Text, [string]$Key, [string]$Color) {
    $escapedKey = [Regex]::Escape($Key)
    $pattern = '(<SolidColorBrush x:Key="' + $escapedKey + '" Color=")#[0-9A-Fa-f]{6,8}("/>)'
    if (-not [Regex]::IsMatch($Text, $pattern)) { throw "Brush resource '$Key' was not found in the template." }
    return [Regex]::Replace($Text, $pattern, ('$1' + $Color + '$2'))
}

function Set-GradientStart([string]$Text, [string]$Key, [string]$Color) {
    $escapedKey = [Regex]::Escape($Key)
    $pattern = '(<LinearGradientBrush x:Key="' + $escapedKey + '"[\s\S]*?<GradientStop Color=")#[0-9A-Fa-f]{6,8}(" Offset="0"/>)'
    if (-not [Regex]::IsMatch($Text, $pattern)) { throw "Gradient '$Key' was not found in the template." }
    return [Regex]::Replace($Text, $pattern, ('$1' + $Color + '$2'), 1)
}

$themes = @(
    [pscustomobject]@{ Index=100; File='AuroraBorealis'; Name='Aurora Borealis'; Body='#10292B'; Key='#173A3D'; Accent='#31D6A5'; Lcd='#B9E8D4'; Shift='#6EF2C2'; Alpha='#FF6FAE' },
    [pscustomobject]@{ Index=101; File='ObsidianGold'; Name='Obsidian Gold'; Body='#171613'; Key='#28241B'; Accent='#D6A83E'; Lcd='#E6D5A6'; Shift='#FFD56A'; Alpha='#E46A63' },
    [pscustomobject]@{ Index=102; File='Mediterranean'; Name='Mediterranean'; Body='#0E2631'; Key='#153B48'; Accent='#25A7C8'; Lcd='#B8DFDF'; Shift='#53D6E8'; Alpha='#FF8A65' },
    [pscustomobject]@{ Index=103; File='RoseQuartz'; Name='Rose Quartz'; Body='#2C1C25'; Key='#452A37'; Accent='#D8799D'; Lcd='#EBD0D9'; Shift='#F3A8C1'; Alpha='#7DE1D1' },
    [pscustomobject]@{ Index=104; File='StormCloud'; Name='Storm Cloud'; Body='#1B2028'; Key='#2A323E'; Accent='#7698BD'; Lcd='#CAD6DF'; Shift='#A8C8E8'; Alpha='#FF7E79' },
    [pscustomobject]@{ Index=105; File='JadeDynasty'; Name='Jade Dynasty'; Body='#10261D'; Key='#193B2C'; Accent='#2FB178'; Lcd='#BBDCC7'; Shift='#62DB9E'; Alpha='#F2B84B' },
    [pscustomobject]@{ Index=106; File='BlueHour'; Name='Blue Hour'; Body='#121D35'; Key='#1A2D50'; Accent='#4F7FD7'; Lcd='#C4D1E8'; Shift='#7FA6F2'; Alpha='#FF748A' },
    [pscustomobject]@{ Index=107; File='EmberNight'; Name='Ember Night'; Body='#271712'; Key='#3E241A'; Accent='#E05B2D'; Lcd='#E4C7AF'; Shift='#FF8A4E'; Alpha='#FFD15C' },
    [pscustomobject]@{ Index=108; File='NordicFjord'; Name='Nordic Fjord'; Body='#13252B'; Key='#203942'; Accent='#4F9DA6'; Lcd='#C0D9D8'; Shift='#72C5CC'; Alpha='#E985A3' },
    [pscustomobject]@{ Index=109; File='CoffeeHouse'; Name='Coffee House'; Body='#271C17'; Key='#3B2A21'; Accent='#A66D42'; Lcd='#D9C4A9'; Shift='#D99A64'; Alpha='#7FC6AE' },
    [pscustomobject]@{ Index=110; File='MoonlitIndigo'; Name='Moonlit Indigo'; Body='#17172D'; Key='#252445'; Accent='#7067C8'; Lcd='#CAC8E4'; Shift='#9A91EE'; Alpha='#F079A9' },
    [pscustomobject]@{ Index=111; File='CopperPatina'; Name='Copper Patina'; Body='#172723'; Key='#253B34'; Accent='#B66D45'; Lcd='#C7DACB'; Shift='#E19968'; Alpha='#55C7B2' },
    [pscustomobject]@{ Index=112; File='CherryCola'; Name='Cherry Cola'; Body='#291419'; Key='#421E27'; Accent='#C33E5C'; Lcd='#E2C2C8'; Shift='#F06783'; Alpha='#FFC45B' },
    [pscustomobject]@{ Index=113; File='CitrusSlate'; Name='Citrus Slate'; Body='#20262B'; Key='#323A40'; Accent='#C6C83E'; Lcd='#D9DEB5'; Shift='#EEF06A'; Alpha='#55C8D0' },
    [pscustomobject]@{ Index=114; File='LavenderSmoke'; Name='Lavender Smoke'; Body='#25202C'; Key='#393143'; Accent='#9A78BD'; Lcd='#D6CCE0'; Shift='#C3A0E2'; Alpha='#F0838A' },
    [pscustomobject]@{ Index=115; File='OceanDepths'; Name='Ocean Depths'; Body='#071F2D'; Key='#0E3344'; Accent='#1686A8'; Lcd='#B5D8DF'; Shift='#43B5D1'; Alpha='#FF8E68' },
    [pscustomobject]@{ Index=116; File='SunsetDunes'; Name='Sunset Dunes'; Body='#302018'; Key='#493027'; Accent='#D77B48'; Lcd='#E3CBB2'; Shift='#F5A46B'; Alpha='#BF74C9' },
    [pscustomobject]@{ Index=117; File='MintChocolate'; Name='Mint Chocolate'; Body='#211C18'; Key='#332921'; Accent='#58B98D'; Lcd='#C5DDC8'; Shift='#79DBAA'; Alpha='#E78B70' },
    [pscustomobject]@{ Index=118; File='RoyalNavy'; Name='Royal Navy'; Body='#0E1930'; Key='#172745'; Accent='#315DA8'; Lcd='#C0CCDF'; Shift='#5F8ED8'; Alpha='#E2AF4A' },
    [pscustomobject]@{ Index=119; File='CrimsonNight'; Name='Crimson Night'; Body='#241117'; Key='#391A23'; Accent='#B52747'; Lcd='#DFC0C8'; Shift='#E1516C'; Alpha='#F1B74E' },
    [pscustomobject]@{ Index=120; File='ArcticViolet'; Name='Arctic Violet'; Body='#1B1F32'; Key='#2A3049'; Accent='#8B71D4'; Lcd='#CBD5E3'; Shift='#B49AF4'; Alpha='#62D6C6' },
    [pscustomobject]@{ Index=121; File='ForestNight'; Name='Forest Night'; Body='#101F18'; Key='#1C3025'; Accent='#477E55'; Lcd='#C0D2C1'; Shift='#74A982'; Alpha='#DA8B5D' },
    [pscustomobject]@{ Index=122; File='NeonCobalt'; Name='Neon Cobalt'; Body='#0C1730'; Key='#14254A'; Accent='#246BFF'; Lcd='#B9C9E8'; Shift='#4F91FF'; Alpha='#FF4FA0' },
    [pscustomobject]@{ Index=123; File='DesertNight'; Name='Desert Night'; Body='#272119'; Key='#3A3125'; Accent='#B58A48'; Lcd='#DED2B5'; Shift='#DDB66A'; Alpha='#D46F68' },
    [pscustomobject]@{ Index=124; File='SilverGraphite'; Name='Silver Graphite'; Body='#202225'; Key='#30343A'; Accent='#8D99A8'; Lcd='#D1D6D8'; Shift='#BCC5CF'; Alpha='#E16D76' },
    [pscustomobject]@{ Index=125; File='CoralLagoon'; Name='Coral Lagoon'; Body='#10272A'; Key='#193A3E'; Accent='#EE6F61'; Lcd='#BDE1D8'; Shift='#FF9788'; Alpha='#56D1C1' },
    [pscustomobject]@{ Index=126; File='AmethystGold'; Name='Amethyst Gold'; Body='#24192D'; Key='#382541'; Accent='#C59A3D'; Lcd='#DDCFB2'; Shift='#EDC467'; Alpha='#B67ADD' },
    [pscustomobject]@{ Index=127; File='CyberLime'; Name='Cyber Lime'; Body='#111C17'; Key='#1B2D22'; Accent='#73D13D'; Lcd='#C8DFB9'; Shift='#9AF263'; Alpha='#EF4D8E' },
    [pscustomobject]@{ Index=128; File='TerracottaSky'; Name='Terracotta Sky'; Body='#2A1D1B'; Key='#402B27'; Accent='#C86F57'; Lcd='#DFCCBE'; Shift='#EE9478'; Alpha='#6EB7D8' },
    [pscustomobject]@{ Index=129; File='MonochromePro'; Name='Monochrome Pro'; Body='#17191B'; Key='#282B2F'; Accent='#69717B'; Lcd='#D0D4D2'; Shift='#D9DEE2'; Alpha='#FF6868' }
)

foreach ($theme in $themes) {
    $body = $theme.Body
    $key = $theme.Key
    $accent = $theme.Accent
    $lcd = $theme.Lcd
    $operator = Mix-HexColor $body $accent 0.36
    $navy = Mix-HexColor $body $key 0.60
    $text = '#F6F8FA'
    $subText = Mix-HexColor $text $body 0.43

    $colorResources = [ordered]@{
        BodyOuterColor = Mix-HexColor $body '#000000' 0.45
        BodyMainColor = $body
        BodyMain2Color = Mix-HexColor $body '#000000' 0.20
        BodyPanelColor = Mix-HexColor $body '#FFFFFF' 0.12
        BodyBorderColor = Mix-HexColor $body '#FFFFFF' 0.30
        ScienceTopColor = Mix-HexColor $key '#FFFFFF' 0.14
        ScienceMidColor = $key
        ScienceBottomColor = Mix-HexColor $key '#000000' 0.20
        NumberTopColor = Mix-HexColor $key '#FFFFFF' 0.08
        NumberMidColor = Mix-HexColor $key '#000000' 0.08
        NumberBottomColor = Mix-HexColor $key '#000000' 0.27
        OperatorTopColor = Mix-HexColor $operator '#FFFFFF' 0.13
        OperatorMidColor = $operator
        OperatorBottomColor = Mix-HexColor $operator '#000000' 0.20
        OrangeTopColor = Mix-HexColor $accent '#FFFFFF' 0.06
        OrangeMidColor = Mix-HexColor $accent '#000000' 0.11
        OrangeBottomColor = Mix-HexColor $accent '#000000' 0.28
        NavyTopColor = Mix-HexColor $navy '#FFFFFF' 0.15
        NavyMidColor = $navy
        NavyBottomColor = Mix-HexColor $navy '#000000' 0.22
        LCDShellColor = Mix-HexColor $body '#000000' 0.55
        LCDRingColor = Mix-HexColor $lcd '#000000' 0.48
        LCDBgColor = $lcd
        LCDBg2Color = Mix-HexColor $lcd '#000000' 0.08
        LCDMutedColor = Mix-HexColor $lcd '#000000' 0.52
    }

    $brushResources = [ordered]@{
        ScienceBorderBrush = Mix-HexColor $key '#FFFFFF' 0.32
        NumberBorderBrush = Mix-HexColor $key '#FFFFFF' 0.25
        NumberTextBrush = $text
        OperatorBorderBrush = Mix-HexColor $accent '#FFFFFF' 0.16
        OperatorTextBrush = Get-ContrastText $operator
        OrangeBorderBrush = Mix-HexColor $accent '#FFFFFF' 0.22
        OrangeTextBrush = Get-ContrastText $accent
        NavyBorderBrush = Mix-HexColor $navy '#FFFFFF' 0.30
        ShiftLabelBrush = $theme.Shift
        AlphaLabelBrush = $theme.Alpha
        KeyTextBrush = $text
        SubTextBrush = $subText
        LCDTextBrush = '#14201B'
    }

    $content = $template
    $themeHeader = '<!-- ============== THEME — THEME{0} {1} ============== -->' -f $theme.Index, $theme.File.ToUpperInvariant()
    $content = [Regex]::Replace($content, '<!-- ============== THEME — .*? ============== -->', $themeHeader, 1)

    foreach ($entry in $colorResources.GetEnumerator()) {
        $content = Set-ColorResource $content $entry.Key $entry.Value
    }
    foreach ($entry in $brushResources.GetEnumerator()) {
        $content = Set-BrushResource $content $entry.Key $entry.Value
    }

    $content = Set-GradientStart $content 'BodyGradient' (Mix-HexColor $body '#000000' 0.34)
    $content = Set-GradientStart $content 'LCDBackground' (Mix-HexColor $lcd '#FFFFFF' 0.12)

    $outputPath = Join-Path $stylesDirectory ("Theme{0}-{1}.xaml" -f $theme.Index, $theme.File)
    Set-Content -LiteralPath $outputPath -Value $content -Encoding utf8NoBOM
    Write-Host "Generated $([IO.Path]::GetFileName($outputPath))"
}

Write-Host "Generated $($themes.Count) SmartCalculator v3 themes."
