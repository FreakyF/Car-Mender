# Car-Mender
 A robust Angular and .NET application for mechanics to manage vehicle repairs, record issues, costs, and part replacements.


## Creating migrations :)

```
dotnet ef migrations add "Title" --startup-project .\Car-Mender.API\Car-Mender.API.csproj --project .\Car-Mender.Infrastructure\Car-Mender.Infrastructure.csproj --context AppDbContext
```

```
dotnet ef database update --startup-project .\Car-Mender.API\Car-Mender.API.csproj --project .\Car-Mender.Infrastructure\Car-Mender.Infrastructure.csproj
```