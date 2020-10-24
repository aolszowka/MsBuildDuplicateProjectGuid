@ECHO OFF

SET PACKAGE_NAME=msbuildduplicateprojectguid

pushd ..

REM Clear any packages out of the local nuget package cache
REM This is because `dotnet tool install --no-cache` appears to be broken?
SET PACKAGE_CACHE_FOLDER=%userprofile%\.nuget\packages\%PACKAGE_NAME%
ECHO Attempting to Clean Existing Package Cache From %PACKAGE_CACHE_FOLDER%
IF EXIST "%PACKAGE_CACHE_FOLDER%" (
RMDIR /Q /S "%PACKAGE_CACHE_FOLDER%"
)

ECHO.
ECHO Delete Existing Packs
IF EXIST nupkg (
RMDIR /q /s nupkg
)

ECHO.
ECHO Building / Packing
dotnet pack --configuration Debug --version-suffix "dev"

ECHO.
ECHO Uninstall Existing Tooling
dotnet tool uninstall %PACKAGE_NAME%

ECHO.
ECHO Install the Latest Prerelease
dotnet tool install --add-source=nupkg --no-cache %PACKAGE_NAME% --version=*-*

popd