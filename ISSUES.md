Using a file for issues since the fork doesn't support GitHub Issues.

- SecretSanta.Data.Assignment and SecretSanta.Data.Gift have default constructors that should be removed if possible. This modification was to fix Entity Framework not knowing how to map the constructor paramters given.
- Most E2E Tests are currently "ignored" due to 30s timeouts.
- There is a bug in the SecretSantaContext 'DbSet's where if you List all users then try to create a user with the same name as an existing user it throws an exception saying you can't track multiple instances of the entity with the same primary key.
- "EnableSensitiveDataLogging" is enabled in SecretSantaContext for debugging purposes
- DBContext has a bug in the DLL as explained by https://github.com/dotnet/efcore/issues/9835 Which causes: "An exception of type 'System.InvalidOperationException' occurred in Microsoft.EntityFrameworkCore.dll but was not handled in user code: 'The instance of entity type 'User' cannot be tracked because another instance with the key value '{FirstName: string, LastName: string}' is already being tracked."