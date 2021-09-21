

MAJOR="3"
MINOR="0"
PATCH="1"
STAGE="cucumber"
APP_VERSION="$MAJOR.$MINOR.$PATCH-$STAGE"
FILE_VERSION="$MAJOR.$MINOR.$PATCH.0"
INFORMATIONAL_VERSION="$MAJOR.$MINOR $STAGE"
BUILDCONFIG="Release"
PERSONAL_ACCESS_TOKEN=${1}
#dotnet nuget add source "https://nuget.pkg.github.com/covergo/index.json" --store-password-in-clear-text --username andrey-covergo --password "$PERSONAL_ACCESS_TOKEN" --name github
#dotnet nuget update source "github" --store-password-in-clear-text --username andrey-covergo --password "$PERSONAL_ACCESS_TOKEN"

dotnet build -c $BUILDCONFIG -p:Version="$APP_VERSION" /p:FileVersion="$FILE_VERSION" /p:InformationVersion="$INFORMATIONAL_VERSION"
dotnet pack -c $BUILDCONFIG --no-build -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg /p:PackageVersion="$APP_VERSION"  /p:Version="$APP_VERSION" /p:FileVersion="$FILE_VERSION" /p:InformationVersion="$INFORMATIONAL_VERSION"

dotnet nuget push "nupkg/Pickles.CommandLine.$APP_VERSION.nupkg" --api-key "$PERSONAL_ACCESS_TOKEN" --source "covergo"