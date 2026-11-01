# Security Policy

## Supported Versions

| Version | Supported |
|---------|-----------|
| 3.0.x   | ✅ |
| 2.0.x   | Security fixes only |
| 1.0.x   | ❌ |

## Reporting a Vulnerability

SmartCalculator is a calculator-only desktop application with no network access,
accounts, or telemetry. It stores user preferences (theme, skin, compact mode,
and always-on-top state) in the current user's local application-data folder
(`%LocalAppData%\SmartCalculator\settings.json`). Official v3 packages are self-contained.

If you discover a security issue, please open an issue on GitHub rather than
emailing privately — there are no credentials, tokens, or user data involved.

For vulnerabilities in the .NET runtime itself, please report to
Microsoft via https://msrc.microsoft.com.
