# SQL Server Database Setup

***NOTE***: Change `<secure_password>` to a secure password

## SQL Server

```sql
USE master;
GO

-- Create the Database
CREATE DATABASE Contacts;
GO

-- Create the User for Logging in
CREATE LOGIN contact_user
    WITH Password = '***<secure_password>***';
GO

-- Change to the new database
USE Contacts
GO

-- Create the user in the database
CREATE USER  contact_user FOR LOGIN contact_user;
GO

-- Update the users permissions
ALTER ROLE db_datareader ADD MEMBER contact_user;
ALTER ROLE db_datawriter ADD MEMBER contact_user;
GO
```
