pushd ..
dotnet tool uninstall msbuildduplicateprojectguid
dotnet tool install msbuildduplicateprojectguid --add-source=nupkg --no-cache
popd