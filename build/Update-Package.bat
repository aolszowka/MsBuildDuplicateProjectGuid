@ECHO OFF

pushd ..

REM Clear any packages out of the local nuget package cache
SET PACKAGE_CACHE_FOLDER=%userprofile%\.nuget\packages\msbuildduplicateprojectguid
ECHO Attempting to Clean Existing Package Cache From
IF EXIST "%PACKAGE_CACHE_FOLDER%" (
RMDIR /Q /S "%PACKAGE_CACHE_FOLDER%"
)

dotnet tool uninstall msbuildduplicateprojectguid
dotnet tool install --add-source=nupkg --no-cache msbuildduplicateprojectguid

popd