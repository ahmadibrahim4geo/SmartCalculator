# Contributing to SmartCalculator

Thank you for considering contributing.

## How to Contribute

1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/your-feature`).
3. Follow existing code style (no comments unless necessary).
4. Run all tests before committing: `dotnet run --project test-runners/regression-runner.csproj`
5. Ensure Debug and Release builds produce 0 errors, 0 warnings.
6. Commit with a clear message describing the change.
7. Open a Pull Request.

## Development Setup

- .NET 8 SDK
- Windows 10+ (WPF dependency)
- Visual Studio 2022 or JetBrains Rider (recommended)

## Test Runner

Tests are in `test-runners/regression-tests.cs`. Run with:

```powershell
dotnet run --project test-runners/regression-runner.csproj
```

## Code Style

- Follow existing patterns in the codebase.
- Do not add unnecessary comments.
- Use `var` where type is obvious.
- Use expression-bodied members where appropriate.

## Reporting Issues

Report bugs via GitHub Issues. Include:
- Steps to reproduce
- Expected behavior
- Actual behavior
- Screenshot if applicable

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
