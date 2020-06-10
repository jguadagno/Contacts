using System;
using System.Collections.Generic;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;
using Xunit;
using Moq;
using Range = Moq.Range;

namespace Contacts.Logic.Tests
{
    public class ContactManagerTests
    {
        [Fact]
        public void GetContact_WithAnInvalidId_ShouldReturnNull()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContact(It.IsInRange(int.MinValue, 0, Range.Inclusive))
            ).Returns<Contact>(null);

            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            var contact = contactManager.GetContact(-1); // Any number less than zero

            // Assert
            Assert.Null(contact);
        }

        [Fact]
        public void GetContact_WithAValidId_ShouldReturnContact()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContact(It.IsInRange(1, int.MaxValue, Range.Inclusive))
            ).Returns((int contactId) => new Contact
            {
                ContactId = contactId
            });

            var contactManager = new ContactManager(mockContactRepository.Object);
            const int requestedContactId = 1;

            // Act
            // Assumes that a contact record exists with the ContactId of 1
            var contact = contactManager.GetContact(requestedContactId);

            // Assert
            Assert.NotNull(contact);
            Assert.Equal(requestedContactId, contact.ContactId);
        }

        [Fact]
        public void GetContacts_ShouldReturnLists()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContacts()
            ).Returns(new List<Contact>
            {
                new Contact {ContactId = 1}, new Contact {ContactId = 2}
            });

            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            var contacts = contactManager.GetContacts();

            // Assert
            Assert.NotNull(contacts);
            Assert.Equal(2, contacts.Count);
        }

        [Fact]
        public void GetContacts_WithNullFirstName_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContacts(null, It.IsAny<string>()));

            var contactManager = new ContactManager(mockContactRepository.Object);
            // Act
            ArgumentNullException ex =
                Assert.Throws<ArgumentNullException>(() => contactManager.GetContacts(null, "Guadagno"));

            // Assert
            Assert.Equal("firstName", ex.ParamName);
            Assert.Equal("FirstName is a required field (Parameter 'firstName')", ex.Message);
        }

        [Fact]
        public void GetContacts_WithNullLastName_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContacts(It.IsAny<string>(), null));

            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            ArgumentNullException ex =
                Assert.Throws<ArgumentNullException>(() => contactManager.GetContacts("Joseph", null));

            // Assert
            Assert.Equal("lastName", ex.ParamName);
            Assert.Equal("LastName is a required field (Parameter 'lastName')", ex.Message);
        }

        [Fact]
        public void GetContacts_WithValidParameters_ShouldReturnLists()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContacts(It.IsAny<string>(), It.IsAny<string>())).Returns(
                (string firstName, string lastName) => new List<Contact>
                {
                    new Contact {ContactId = 1, FirstName = firstName, LastName = lastName}
                });

            var contactManager = new ContactManager(mockContactRepository.Object);
            const string requestedFirstName = "Joseph";
            const string requestedLastname = "Guadagno";

            // Act
            var contacts = contactManager.GetContacts(requestedFirstName, requestedLastname);

            // Assert
            Assert.NotNull(contacts);
            Assert.True(contacts.Count > 0);
            Assert.Equal(requestedFirstName, contacts[0].FirstName);
            Assert.Equal(requestedLastname, contacts[0].LastName);
        }

        [Fact]
        public void SaveContact_WithANullContact_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(null));
            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => contactManager.SaveContact(null));

            // Assert
            Assert.Equal("contact", ex.ParamName);
            Assert.Equal("Contact is a required field (Parameter 'contact')", ex.Message);
        }

        [Fact]
        public void SaveContact_WithNullFirstName_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var contact = new Contact();

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("FirstName", ex.ParamName);
            Assert.Equal("FirstName is a required field (Parameter 'FirstName')", ex.Message);
        }

        [Fact]
        public void SaveContact_WithNullLastName_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var contact = new Contact
            {
                FirstName = "Joseph"
            };

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("LastName", ex.ParamName);
            Assert.Equal("LastName is a required field (Parameter 'LastName')", ex.Message);
        }

        [Fact]
        public void SaveContact_WithNullEmailAddress_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno"
            };

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("EmailAddress", ex.ParamName);
            Assert.Equal("EmailAddress is a required field (Parameter 'EmailAddress')", ex.Message);
        }

        [Fact]
        public void SaveContact_WithBirthdayInFuture_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var futureDate = new DateTime(2030, 12, 31, 23, 59, 59);

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = futureDate
            };

            // Act
            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Birthday", ex.ParamName);
            Assert.Equal(futureDate, ex.ActualValue);
            Assert.StartsWith("The birthday can not be in the future", ex.Message);
        }

        [Fact]
        public void SaveContact_WithAnniversaryInFuture_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var futureDate = new DateTime(2030, 12, 31, 23, 59, 59);
            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now.AddDays(-1),
                Anniversary = futureDate
            };

            // Act
            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Anniversary", ex.ParamName);
            Assert.Equal(futureDate, ex.ActualValue);
            Assert.StartsWith("The anniversary can not be in the future", ex.Message);
        }

        [Fact]
        public void SaveContact_WithAnniversaryBeforeBirthday_ShouldThrowException()
        {
            // Arrange 
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.SaveContact(It.IsAny<Contact>()));
            var contactManager = new ContactManager(mockContactRepository.Object);

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now,
                Anniversary = DateTime.Now.AddDays(-1)
            };

            // Act
            ArgumentOutOfRangeException ex =
                Assert.Throws<ArgumentOutOfRangeException>(() => contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Anniversary", ex.ParamName);
            Assert.Equal(contact.Anniversary, ex.ActualValue);
            Assert.StartsWith("The anniversary can not be earlier than the birthday.", ex.Message);
        }

        [Fact]
        public void SaveContact_WithValidContact_ShouldReturnTrue()
        {
            // Arrange 
            var contact = new Contact
            {
                ContactId = 1,
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now.AddDays(-10),
                Anniversary = DateTime.Now.AddDays(-1)
            };
            
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository => contactRepository.SaveContact(It.IsAny<Contact>()))
                .Returns(() => contact);

            var contactManager = new ContactManager(mockContactRepository.Object);
            
            // Act
            var savedContact = contactManager.SaveContact(contact);

            // Assert
            Assert.NotNull(savedContact);
            Assert.Equal(contact.ContactId, savedContact.ContactId);
        }

        [Fact]
        public void DeleteContact_WithInvalidContactId_ShouldReturnFalse()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.DeleteContact(It.IsInRange(int.MinValue, 0, Range.Inclusive))).Returns(false);
            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            // This assumes that there is no record with the id of -1
            var wasDeleted = contactManager.DeleteContact(-1);

            // Assert
            Assert.False(wasDeleted);
        }

        [Fact]
        public void DeleteContact_WithNullContact_ShouldReturnFalse()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.DeleteContact(It.IsAny<Contact>())).Returns(false);
            var contactManager = new ContactManager(mockContactRepository.Object);

            // Act
            // This assumes that there is no record with the id of -1
            var wasDeleted = contactManager.DeleteContact(null);

            // Assert
            Assert.False(wasDeleted);
        }

        [Fact]
        public void DeleteContact_WithExistingContact_ShouldReturnTrue()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.DeleteContact(It.IsAny<Contact>())).Returns(true);
            var contactManager = new ContactManager(mockContactRepository.Object);

            // Create a fake contact
            var contact = new Contact
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                EmailAddress = "TestUser@example.com",
                Birthday = DateTime.Now
            };

            // Act
            var wasDeleted = contactManager.DeleteContact(contact);

            // Assert
            Assert.True(wasDeleted);
        }

        [Fact]
        public void GetContactFullName_WithAllNamesNull_ShouldReturnErrorString()
        {
            // Arrange
            string firstName = null;
            string lastName = null;
            string middleName = null;
            string expectedFullName = "Could not determine the contact name";
            
            var contact = new Contact
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName
            };

            // Act
            var fullName = contact.FullName;

            // Assert
            Assert.Equal(expectedFullName, fullName);
        }
        
        [Fact]
        public void GetContactFullName_WithNullMiddleName_ShouldReturnStringWithFirstNameAndLastName()
        {
            // Arrange
            string firstName = "Joseph";
            string lastName = "Guadagno";
            string middleName = null;
            string expectedFullName = $"{firstName} {lastName}";
            
            var contact = new Contact
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName
            };

            // Act
            var fullName = contact.FullName;

            // Assert
            Assert.Equal(expectedFullName, fullName);
        }
        
        [Fact]
        public void GetContactFullName_WithAllNamesNotNumber_ShouldReturnStringWithFirstNameAndMiddleNameAndLastName()
        {
            // Arrange
            string firstName = "Joseph";
            string lastName = "Guadagno";
            string middleName = "James";
            string expectedFullName = $"{firstName} {middleName} {lastName}";
            
            var contact = new Contact
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName
            };

            // Act
            var fullName = contact.FullName;

            // Assert
            Assert.Equal(expectedFullName, fullName);
        }
        
        // GetContactPhones

        [Fact]
        public void GetContactPhones_WithValidContactId_ShouldReturnAListOfPhones()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContactPhones(It.IsAny<int>())).Returns(new List<Phone>());
            var contactManager = new ContactManager(mockContactRepository.Object);
            
            // Act
            var phones = contactManager.GetContactPhones(1);

            // Assert
            Assert.NotNull(phones);
            
        }
        
        // GetContactPhone
        [Fact]
        public void GetContactPhone_WithValidContactIdAndPhoneId_ShouldReturnPhone()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            var setup = mockContactRepository.Setup(contactRepository =>
                    contactRepository.GetContactPhone(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new Phone());
            var contactManager = new ContactManager(mockContactRepository.Object);
            
            // Act
            var phone = contactManager.GetContactPhone(1, 1);

            // Assert
            Assert.NotNull(phone);
            
        }
        
        // GetContactAddress
        [Fact]
        public void GetContactAddress_WithValidContactIdAndAddressId_ShouldReturnAddress()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            var setup = mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContactAddress(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(new Address());
            var contactManager = new ContactManager(mockContactRepository.Object);
            
            // Act
            var address = contactManager.GetContactAddress(1, 1);

            // Assert
            Assert.NotNull(address);
            
        }
        
        // GetContactAddresses
        [Fact]
        public void GetContactAddresses_WithValidContactId_ShouldReturnAListOfAddresses()
        {
            // Arrange
            var mockContactRepository = new Mock<IContactRepository>();
            mockContactRepository.Setup(contactRepository =>
                contactRepository.GetContactAddresses(It.IsAny<int>())).Returns(new List<Address>());
            var contactManager = new ContactManager(mockContactRepository.Object);
            
            // Act
            var addresses = contactManager.GetContactAddresses(1);

            // Assert
            Assert.NotNull(addresses);
            
        }
    }
}