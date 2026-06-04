using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReorderUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Recreate Users table with correct column order
            // 1. Create temp table with desired column order
            migrationBuilder.Sql(@"
                CREATE TABLE [Users_New] (
                    [Id]           INT            NOT NULL IDENTITY(1,1),
                    [Email]        NVARCHAR(256)  NOT NULL DEFAULT '',
                    [PasswordHash] NVARCHAR(MAX)  NOT NULL DEFAULT '',
                    [Role]         NVARCHAR(MAX)  NOT NULL DEFAULT '',
                    [Name]         NVARCHAR(100)  NOT NULL DEFAULT '',
                    [LastName]     NVARCHAR(MAX)  NULL,
                    [Phone]        NVARCHAR(MAX)  NULL,
                    [City]         NVARCHAR(MAX)  NULL,
                    [Street]       NVARCHAR(MAX)  NULL,
                    CONSTRAINT [PK_Users_New] PRIMARY KEY ([Id])
                );
            ");

            // 2. Copy all existing data
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT [Users_New] ON;
                INSERT INTO [Users_New] ([Id], [Email], [PasswordHash], [Role], [Name], [LastName], [Phone], [City], [Street])
                SELECT [Id], [Email], [PasswordHash], [Role], [Name], [LastName], [Phone], [City], [Street]
                FROM [Users];
                SET IDENTITY_INSERT [Users_New] OFF;
            ");

            // 3. Drop foreign key constraints referencing Users
            migrationBuilder.Sql(@"
                DECLARE @sql NVARCHAR(MAX) = '';
                SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
                    + '.' + QUOTENAME(OBJECT_NAME(parent_object_id))
                    + ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
                FROM sys.foreign_keys
                WHERE referenced_object_id = OBJECT_ID('Users');
                EXEC sp_executesql @sql;
            ");

            // 4. Drop old table and old index
            migrationBuilder.Sql("DROP TABLE [Users];");

            // 5. Rename new table
            migrationBuilder.Sql("EXEC sp_rename 'Users_New', 'Users';");
            migrationBuilder.Sql("EXEC sp_rename 'PK_Users_New', 'PK_Users', 'OBJECT';");

            // 6. Re-add unique index on Name
            migrationBuilder.Sql("CREATE UNIQUE INDEX [IX_Users_Name] ON [Users] ([Name]);");

            // 7. Re-add foreign keys that referenced Users
            migrationBuilder.Sql(@"
                ALTER TABLE [Carts] ADD CONSTRAINT [FK_Carts_Users_UserId]
                    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE [Favorites] ADD CONSTRAINT [FK_Favorites_Users_UserId]
                    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Users_UserId]
                    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE [TradeInRequests] ADD CONSTRAINT [FK_TradeInRequests_Users_UserId]
                    FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No rollback needed for column reordering
        }
    }
}
