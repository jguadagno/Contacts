CREATE DATABASE Contacts
    ON
    ( NAME = Contacts_Data,
        FILENAME = '/var/opt/mssql/data/contacts.mdf',
        SIZE = 10,
        MAXSIZE = 50,
        FILEGROWTH = 5 )
    LOG ON
    ( NAME = Contacts_Log,
        FILENAME = '/var/opt/mssql/data/contacts.ldf',
        SIZE = 5MB,
        MAXSIZE = 25MB,
        FILEGROWTH = 5MB ) ;
GO

USE master

-- ***NOTE***: Change `<secure_password>` to a secure password

CREATE Login contacts_user
    WITH PASSWORD='P@ssw0rd1'
GO

USE Contacts

CREATE USER contacts_user 
    FOR LOGIN contacts_user WITH DEFAULT_SCHEMA=dbo;

ALTER ROLE db_datareader ADD MEMBER contacts_user;
ALTER ROLE db_datawriter ADD MEMBER contacts_user;
GO
