using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coinage.Domain.Abstract.Data;
using Coinage.Domain.Concrete;
using Coinage.Domain.Concrete.Entities;
using Coinage.Domain.Concrete.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Coinage.Domain.Tests.Services
{
    [TestFixture]
    public class CustomerServiceTest
    {
        public class GetCustomerById
        {
            [Test]
            public void GetCustomerById_NonExistentCustomerId_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                int customerId = 1;

                // Act
                var result = service.GetCustomerById(customerId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerById_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerId = 1;
                service.CustomerRepository.Setup(c => c.GetById(It.IsAny<int>())).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(() => service.GetCustomerById(customerId));
            }

            [Test]
            public void GetCustomerById_ExistingCustomerId_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer { Id = 1 };
                service.SetupRepoGetById(customer);

                // Act
                var result = service.GetCustomerById(customer.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, customer.Id);
            }
        }

        public class GetCustomerByGuid
        {
            [Test]
            public void GetCustomerByGuid_EmptyGuid_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();

                // Act
                Customer result = service.GetCustomerByGuid(customerGuid);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerByGuid_NonExistentCustomerGuid_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();
                var customer = new Customer { CustomerGuid = Guid.NewGuid() };
                service.SetupRepoTable(new[] { customer });

                // Act
                Customer result = service.GetCustomerByGuid(customerGuid);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerByGuid_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();
                service.CustomerRepository.Setup(c => c.Table).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(() => service.GetCustomerByGuid(customerGuid));
            }

            [Test]
            public void GetCustomerByGuid_ExistingCustomerGuid_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();
                var customer = new Customer { CustomerGuid = customerGuid };
                service.SetupRepoTable(new[] { customer, new Customer { CustomerGuid = Guid.NewGuid()} }); // Added an extra customer to prove that we get the correct result back
                
                // Act
                Customer result = service.GetCustomerByGuid(customerGuid);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(customerGuid, result.CustomerGuid);
            }
        }

        public class GetCustomerByEmail
        {
            [Test]
            public void GetCustomerByEmail_NullString_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                string customerEmail = null;

                // Act
                Customer result = service.GetCustomerByEmail(customerEmail);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerByEmail_NonExistentCustomerEmail_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";
                var customer = new Customer { Email = "another-test@test.com"};
                service.SetupRepoTable(new[] { customer });

                // Act
                Customer result = service.GetCustomerByEmail(customerEmail);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public void GetCustomerByEmail_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";
                service.CustomerRepository.Setup(c => c.Table).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(() => service.GetCustomerByEmail(customerEmail));
            }

            [Test]
            public void GetCustomerByEmail_ExistingCustomerGuid_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";
                var customer = new Customer { Email = customerEmail };
                service.SetupRepoTable(new[] { customer, new Customer { Email = "another-test@test.com" } }); // Added an extra customer to prove that we get the correct result back

                // Act
                Customer result = service.GetCustomerByEmail(customerEmail);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(customerEmail, result.Email);
            }
        }

        public class InsertGuestCustomer
        {

        }

        public class Update
        {
            [Test]
            public void Update_NullCustomer_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                Customer customer = null;

                // Act
                EntityActionResponse result = service.Update(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Update_CustomerRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var exception = new Exception("Error");
                service.CustomerRepository.Setup(r => r.Update(It.IsAny<Customer>())).Throws(exception);
                var customer = new Customer();

                // Act
                EntityActionResponse result = service.Update(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Update_CustomerRepoUpdatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer();

                // Act
                EntityActionResponse result = service.Update(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
            }
        }

        public class Create
        {
            [Test]
            public void Create_NullCustomer_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                Customer customer = null;

                // Act
                EntityActionResponse result = service.Update(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Successful);
            }

            [Test]
            public void Create_CustomerRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var exception = new Exception("Error");
                service.CustomerRepository.Setup(r => r.Insert(It.IsAny<Customer>())).Throws(exception);
                var customer = new Customer();

                // Act
                EntityActionResponse result = service.Create(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Successful);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public void Create_CustomerRepoCreatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer();

                // Act
                EntityActionResponse result = service.Create(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Successful);
                Assert.IsNull(result.Exception);
            }
        }

        private class TestableCustomerService : CustomerService
        {
            public readonly Mock<IRepository<Customer>> CustomerRepository;

            private TestableCustomerService(Mock<IRepository<Customer>> customerRepository)
                : base(customerRepository.Object)
            {
                CustomerRepository = customerRepository;
            }

            public static TestableCustomerService Create()
            {
                return new TestableCustomerService(new Mock<IRepository<Customer>>());
            }

            public void SetupRepoTable(IEnumerable<Customer> customers)
            {
                CustomerRepository
                    .Setup(s => s.Table)
                    .Returns(customers.AsQueryable());
            }

            public void SetupRepoGetById(Customer customer)
            {
                CustomerRepository
                    .Setup(s => s.GetById(It.IsAny<int>()))
                    .Returns(customer);
            }
        }
    }
}
