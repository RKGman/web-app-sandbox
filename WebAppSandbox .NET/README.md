# Web App Sandbox (.NET)

This solution is for building an example .NET Core (6) API Service.

You can pretty much ignore "EF Example" for now... It was basically a first pass at understanding the Identity library.

## Getting Started with Development ##

- Install a Microsoft SQL database of your choice... 

Developer or Exrpess should do:
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

- Restore the NuGet packages for the solution.

- Set the `Auth Example` as the startup project.

- Update the `ConnStr` in the  `appsettings.json` to the desired data source for your installed SQL database.

- Open the Package Manager Console (View > Other Windows > Package Manager Console)

- If you haven't updated your database to the latest migration, run `update-database` in the Package Manager Console.

- You should now be able to run the service in debug.

### Adding New Migrations ###

Once you've set up any new entity relationships (models, dbsets, dbcontext, etc...) you will need to run `add-migration "<DescriptionOfMigration>"` before running `update-database` in the Package Manager Console.