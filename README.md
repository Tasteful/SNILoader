# SNILoader
IIS Module to load the Microsoft.Data.SqlClient.SNI.dll from shadow copied location to avoid locked files by IIS during build and deploys.

# Usage
1. Update the [version number](https://github.com/Tasteful/SNILoader/blob/5f8cfabd94ab31c1f45ffdff2b6e6615b4b74fc9/src/SqlClient.SNILoader/SqlClient.SNILoader.csproj#L5) of the SNI package to correspond to what your dependency using.
2. Build and package the project into a nuget-package. If you publish the package, think of reading and understand the license for the different included components.
3. Add a reference to the created nuget-package and make a clean build of the solution.
4. Ensure that the SNI-dll file not exists in the bin-folder.
5. Test your solution to ensure that it is working with the shadow copying of the SNI-dll into the temporary folders for ASP.NET files.

When deploying into production for the first time the SNI-dll will still be there in the bin-folder and block the deployment and you need to do some workaround to get the files updated.
