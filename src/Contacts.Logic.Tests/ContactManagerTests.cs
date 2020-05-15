using System;
using System.Linq;
using Contacts.Domain;
using Xunit;

namespace Contacts.Logic.Tests
{
    public class ContactManagerTests
    {
        [Fact]
        public void GetContact_WithAnInvalidId_ShouldReturnNull()
        {
            // Arrange 
            var contactManager = new ContactManager();
            
            // Act
            var contact = contactManager.GetContact(-1);
            
            // Assert
            Assert.Null(contact);
        }
        
        [Fact]
        public void GetContact_WithAValidId_ShouldReturnContact()
        {
            // Arrange 
            var contactManager = new ContactManager();
            
            // Act
            // Assumes that a contact record exists with the ContactId of 1
            var contact = contactManager.GetContact(1);
            
            // Assert
            Assert.NotNull(contact);
        }

        [Fact]
        public void GetContacts_ShouldReturnLists()
        {
            // Arrange
            var contactManager = new ContactManager();
            
            // Act
            var contacts = contactManager.GetContacts();
            
            // Assert
            Assert.NotNull(contacts);
        }

        [Fact]
        public void GetContacts_WithNullFirstName_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();

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
            var contactManager = new ContactManager();

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
            var contactManager = new ContactManager();

            // Act
            var contacts = contactManager.GetContacts("Joseph", "Guadagno");
            
            // Assert
            Assert.NotNull(contacts);
            Assert.True(contacts.Count > 0);
        }

        [Fact]
        public void SaveContact_WithANullContact_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();
            
            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()  => contactManager.SaveContact(null));
            
            // Assert
            Assert.Equal("contact", ex.ParamName);
            Assert.Equal("Contact is a required field (Parameter 'contact')", ex.Message);
        }

        [Fact]
        public void SaveContact_WithNullFirstName_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();

            var contact = new Contact();
            
            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("FirstName", ex.ParamName);
            Assert.Equal("FirstName is a required field (Parameter 'FirstName')", ex.Message);
            
        }
        
        [Fact]
        public void SaveContact_WithNullLastName_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();

            var contact = new Contact
            {
                FirstName = "Joseph"
            };

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("LastName", ex.ParamName);
            Assert.Equal("LastName is a required field (Parameter 'LastName')", ex.Message);
            
        }
        
        [Fact]
        public void SaveContact_WithNullEmailAddress_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno"
            };

            // Act
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("EmailAddress", ex.ParamName);
            Assert.Equal("EmailAddress is a required field (Parameter 'EmailAddress')", ex.Message);
            
        }
        
        [Fact]
        public void SaveContact_WithBirthdayInFuture_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();
            var futureDate = new DateTime(2030, 12, 31,23,59,59);

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = futureDate
            };
            
            // Act
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Birthday", ex.ParamName);
            Assert.Equal(futureDate, ex.ActualValue);
            Assert.StartsWith("The birthday can not be in the future", ex.Message);
            
        }
        
        [Fact]
        public void SaveContact_WithAnniversaryInFuture_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();
            var futureDate = new DateTime(2030, 12, 31,23,59,59);
            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now.AddDays(-1),
                Anniversary = futureDate
            };
            
            // Act
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Anniversary", ex.ParamName);
            Assert.Equal(futureDate, ex.ActualValue);
            Assert.StartsWith("The anniversary can not be in the future", ex.Message);
            
        }
        
        [Fact]
        public void SaveContact_WithAnniversaryBeforeBirthday_ShouldThrowException()
        {
            // Arrange 
            var contactManager = new ContactManager();

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now,
                Anniversary = DateTime.Now.AddDays(-1)
            };

            // Act
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() =>  contactManager.SaveContact(contact));

            // Assert
            Assert.Equal("Anniversary", ex.ParamName);
            Assert.Equal(contact.Anniversary, ex.ActualValue);
            Assert.StartsWith("The anniversary can not be earlier than the birthday.", ex.Message);
            
        }

        [Fact]
        public void SaveContact_WithValidContact_ShouldReturnTrue()
        {
            // Arrange 
            var contactManager = new ContactManager();

            var contact = new Contact
            {
                FirstName = "Joseph",
                LastName = "Guadagno",
                EmailAddress = "jguadagno@hotmail.com",
                Birthday = DateTime.Now.AddDays(-10),
                Anniversary = DateTime.Now.AddDays(-1)
            };

            // Act
            var wasSaved = contactManager.SaveContact(contact);

            // Assert
            Assert.True(wasSaved);
        }

        [Fact]
        public void DeleteContact_WithInvalidContactId_ShouldReturnFalse()
        {
            // Arrange
            var contactManager = new ContactManager();
            
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
            var contactManager = new ContactManager();
            
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
            var contactManager = new ContactManager();
            
            // Create a fake contact
            // NOTE: This is only for testing and demonstration.  Your unit tests should not
            //    be adding, removing, or changing data.  
            //    We will introduce Mocking in a later session to show how you can avoid this.
            var contact = new Contact
            {
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                EmailAddress = "TestUser@example.com",
                Birthday = DateTime.Now
            };
            var wasSaved = contactManager.SaveContact(contact);
            if (wasSaved == false)
            {
                throw new Exception("Failed to create the test record");
            }

            var contactsToDelete = contactManager.GetContacts(contact.FirstName, contact.LastName);
            if (contactsToDelete == null || contactsToDelete.Count == 0)
            {
                throw new Exception("Failed to find the test record");
            }

            var contactToDelete = contactsToDelete.First();
            
            // Act
            var wasDeleted = contactManager.DeleteContact(contactToDelete);
            
            // Assert
            Assert.True(wasDeleted);

        }
    }
}