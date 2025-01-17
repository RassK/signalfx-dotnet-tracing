# Dependency bumping

## Tracked by version

This section describes dependencies that require a periodical version bump.

| Dependency | Files | Bumping | Notes |
|-|-|-|-|
| NuGet | .csproj | Upstream | Test packages might need to stay on a certain version. |
| GitHub CI | ./github/workflows/*.yml | Dependabot | Bumps GitHub step templates |
| Docker | *.dockerfile | Dependabot | Bumps Docker image versions |
| Docker | docker-compose.yml | Manual | Search for `image:` |
| .NET SDK | (CI templates) | Manual | Search for `actions/setup-dotnet` or `dotnetSdkVersion:` |
| ASP.NET Runtime | *.dockerfile | Manual | Search for `./dotnet-install.sh --runtime aspnetcore` |
| GitHub CI OS | ./github/workflows/*.yml | Manual | Search for `runs-on:` |
| APT | debian.dockerfile | Manual | Search for `apt-get install` |
| Ruby gems | *.dockerfile | Manual | Search for `gem install` |

## Tracked by checksum

This section describes dependencies tracked and verified using hardcoded checksum values.

| Dependency | Files | Bumping | Checksum | Notes |
|-|-|-|-|-|
| dotnet-install.sh | *.dockerfile | Manual | SHA256 | |
