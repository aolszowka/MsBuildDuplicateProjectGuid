# MsBuildDuplicateProjectGuid
Utility to find duplicated MsBuild Project GUIDs

## Background
In the MSBuild Project System, the ProjectGuid is expected to exist in each MSBuild Style Project. It is used in several places; most importantly:

* Any ProjectReference should contain the Project Guid (in the Project Tag)
    * [Microsoft Docs: Common MSBuild Project Items - ProjectReference](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2017#projectreference)
* The ProjectGuid is used in the Visual Studio Solution File as a way to Reference the Project
    * [Microsoft Docs: ProjectInSolution.ProjectGuid Property](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.workspace.extensions.msbuild.projectinsolution.projectguid?view=visualstudiosdk-2017)

As the name suggests (Project**Guid**) this is expected to be a **G**lobally **U**nique **ID**entifier.

The tooling is so dependent upon this fact that if you attempt to import two projects which have identical ProjectGuid into a Visual Studio Solution File one will be chosen as the loser and its Guid Regenerated.

Duplication most often happens because a developer was attempting to use an existing project as a "template" for a new project.

There is nothing wrong with making a copy of an existing project, but when you do so you need to take special care to generate a new ProjectGuid tag.

## When To Use This Tool
This tool will help you identify projects (within a subfolder) that are not "Globally Unique".

However, this tool does not attempt to make corrections. In order to do so look at https://github.com/aolszowka/MsBuildSetProjectGuid


## Usage
```
Usage: MsBuildDuplicateProjectGuid C:\DirectoryWithProjects

Scans given directory for MsBuild Projects; reporting any duplicate ProjectGuids
it finds.

Arguments:

              <>            The directory to scan for MSBuild Projects
  -?, -h, --help             Show this message and exit
```

## Hacking
The most likely change you will want to make is changing the supported project files. In theory this tool should support any MSBuild Project Format that utilizes a ProjectGuid.

See MsBuildDuplicateProjectGuid.GetProjectsInDirectory(string) for the place to modify this.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.

## Third Party Licenses
This project uses other open source contributions see [LICENSES.md](LICENSES.md) for a comprehensive listing.
