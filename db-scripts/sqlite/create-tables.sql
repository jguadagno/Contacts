create table main.AddressTypes
(
    AddressTypeId integer,
    Type          nvarchar,
    Description   nvarchar
);

create table main.Addresses
(
    AddressId        integer,
    StreetAddress    nvarchar,
    SecondaryAddress nvarchar,
    Unit             nvarchar,
    City             nvarchar,
    State            nvarchar,
    Country          nvarchar,
    PostalCode       nvarchar,
    AddressTypeId    integer,
    ContactId        integer
);

create table main.Contacts
(
    ContactId    integer,
    FirstName    nvarchar,
    MiddleName   nvarchar,
    LastName     nvarchar,
    EmailAddress nvarchar,
    ImageUrl     nvarchar,
    Birthday     datetime,
    Anniversary  datetime
);

create table main.PhoneTypes
(
    PhoneTypeId integer,
    Type        nvarchar,
    Description nvarchar
);

create table main.Phones
(
    PhoneId     integer,
    PhoneNumber nvarchar,
    Extension   nvarchar,
    PhoneTypeId integer,
    ContactId   integer
);

create table main.Logs (
  Id              integer,
  Message         nvarchar,
  MessageTemplate nvarchar,
  Level           nvarchar,
  TimeStamp       datetime,
  Exception       nvarchar,
  Properties      nvarchar
);
