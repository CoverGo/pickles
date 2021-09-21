dotnet build -c Release
rm -rf pickles
dotnet publish -o pickles -c Release --no-build
rm -rf ~/Programming/CoverGo/OpenDomain/test/OpenDomain.Tests.Acceptance/pickles
cp -R pickles ~/Programming/CoverGo/OpenDomain/test/OpenDomain.Tests.Acceptance/pickles

