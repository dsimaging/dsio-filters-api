# Define build properties
$VersionPrefix= "0.1"
$VersionSuffix= ""
$Configuration = "Release"

# Get version from git tag
$git_description = git describe --long --tags --always --dirty
if ($null -ne $git_description) {
    # Parse for tag and commit
    if ($git_description -match '[v](.*?)-(.*)') {
        # extract version from tag and suffix from commit/extras
        $VersionPrefix = $Matches[1]
        $VersionSuffix = $Matches[2]
    } else {
        # version tag not found - just set the suffix to the git description
        $VersionSuffix = $git_description;
    }
}

Write-Host "Cleaning..."
dotnet clean --configuration $Configuration

Write-Host "Compiling SDK Library Version: $VersionPrefix-$VersionSuffix"
dotnet build --configuration $Configuration -p:VersionPrefix=$VersionPrefix -p:VersionSuffix=$VersionSuffix `
    .\lib\DSIO.Filters.Api.Sdk.Types\DSIO.Filters.Api.Sdk.Types.csproj
dotnet build --configuration $Configuration -p:VersionPrefix=$VersionPrefix -p:VersionSuffix=$VersionSuffix `
    .\lib\DSIO.Filters.Api.Sdk.Client\DSIO.Filters.Api.Sdk.Client.csproj

Write-Host "Compiling SDK Samples Version: $VersionPrefix-$VersionSuffix"
dotnet build --configuration $Configuration -p:VersionPrefix=$VersionPrefix -p:VersionSuffix=$VersionSuffix `
    .\samples\WpfSample\WpfSample.csproj
