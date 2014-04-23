using System.Linq.Expressions;
using System.Threading.Tasks;
using Coinage.Domain.Data;
using Coinage.Domain.Entites;
using Coinage.Domain.Models;
using Coinage.Domain.Security;
using Coinage.Domain.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coinage.Domain.Tests.Services
{
    [TestFixture]
    public class CustomerServiceTest
    {
        public class GetCustomerByIdAsync
        {
            [Test]
            public async Task GetCustomerByIdAsync_NonExistentCustomerId_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                int customerId = 1;

                // Act
                var result = await service.GetCustomerByIdAsync(customerId);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task GetCustomerByIdAsync_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerId = 1;
                service.CustomerRepository.Setup(c => c.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(async () => await service.GetCustomerByIdAsync(customerId));
            }

            [Test]
            public async Task GetCustomerByIdAsync_ExistingCustomerId_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer { Id = 1 };
                service.SetupRepoFindAsync(customer);

                // Act
                var result = await service.GetCustomerByIdAsync(customer.Id);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, customer.Id);
            }
        }

        public class GetCustomerByGuidAsync
        {
            [Test]
            public async Task GetCustomerByGuidAsync_EmptyGuid_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();

                // Act
                Customer result = await service.GetCustomerByGuidAsync(customerGuid);

                // Assert
                Assert.IsNull(result);
            }

            //[Test]
            //public async Task GetCustomerByGuid_NonExistentCustomerGuid_ReturnsNull()
            //{
            //    // Arrange
            //    var service = TestableCustomerService.Create();
            //    var customerGuid = Guid.NewGuid();

            //    // Act
            //    Customer result = await service.GetCustomerByGuidAsync(customerGuid);

            //    // Assert
            //    Assert.IsNull(result);
            //}

            [Test]
            public async Task GetCustomerByGuidAsync_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();
                service.CustomerRepository.Setup(c => c.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(async () => await service.GetCustomerByGuidAsync(customerGuid));
            }

            [Test]
            public async Task GetCustomerByGuidAsync_ExistingCustomerGuid_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerGuid = Guid.NewGuid();
                var customer = new Customer { CustomerGuid = customerGuid };
                service.SetupRepoFindAsync(customer);

                // Act
                Customer result = await service.GetCustomerByGuidAsync(customerGuid);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(customerGuid, result.CustomerGuid);
            }
        }

        public class GetCustomerByEmailAsync
        {
            [Test]
            public async Task GetCustomerByEmailAsync_NullString_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                string customerEmail = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(async () => await service.GetCustomerByEmailAsync(customerEmail));
            }

            [Test]
            public async Task GetCustomerByEmailAsync_NonExistentCustomerEmail_ReturnsNull()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";

                // Act
                Customer result = await service.GetCustomerByEmailAsync(customerEmail);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task GetCustomerByEmailAsync_RepoThrowsError_RepoErrorIsThrown()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";
                service.CustomerRepository.Setup(c => c.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception("Error message"));

                // Act & Assert
                Assert.Throws<Exception>(async () => await service.GetCustomerByEmailAsync(customerEmail));
            }

            [Test]
            public async Task GetCustomerByEmailAsync_ExistingCustomerGuid_ReturnsCustomer()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customerEmail = "test@test.com";
                var customer = new Customer { Email = customerEmail };
                service.SetupRepoFindAsync(customer);

                // Act
                Customer result = await service.GetCustomerByEmailAsync(customerEmail);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(customerEmail, result.Email);
            }
        }

        public class InsertGuestCustomer
        {

        }

        public class UpdateAsync
        {
            [Test]
            public async Task UpdateAsync_NullCustomer_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                Customer customer = null;

                // Act
                EntityActionResponse result = await service.UpdateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Success);
                service.CustomerRepository.Verify(b => b.UpdateAsync(It.IsAny<Customer>()), Times.Never);
            }

            [Test]
            public async Task UpdateAsync_CustomerRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var exception = new Exception("Error");
                service.CustomerRepository.Setup(r => r.UpdateAsync(It.IsAny<Customer>())).Throws(exception);
                var customer = new Customer();

                // Act
                EntityActionResponse result = await service.UpdateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Success);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public async Task UpdateAsync_CustomerRepoUpdatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer();

                // Act
                EntityActionResponse result = await service.UpdateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success);
                Assert.IsNull(result.Exception);
                service.CustomerRepository.Verify(b => b.UpdateAsync(It.IsAny<Customer>()), Times.Once);
            }
        }

        public class CreateAsync
        {
            [Test]
            public async Task CreateAsync_NullCustomer_ReturnsUnsuccessfulWithArgumentNullException()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                Customer customer = null;

                // Act
                EntityActionResponse result = await service.CreateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<ArgumentNullException>(result.Exception);
                Assert.IsFalse(result.Success);
                service.CustomerRepository.Verify(b => b.InsertAsync(It.IsAny<Customer>()), Times.Never);
            }

            [Test]
            public async Task CreateAsync_CustomerRepoThrowsError_ReturnsUnsuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var exception = new Exception("Error");
                service.CustomerRepository.Setup(r => r.InsertAsync(It.IsAny<Customer>())).Throws(exception);
                var customer = new Customer();

                // Act
                EntityActionResponse result = await service.CreateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Exception);
                Assert.IsInstanceOf<Exception>(result.Exception);
                Assert.IsFalse(result.Success);
                Assert.AreEqual(exception.Message, result.Exception.Message);
            }

            [Test]
            public async Task CreateAsync_CustomerRepoCreatesSuccessfully_ReturnsSuccessful()
            {
                // Arrange
                var service = TestableCustomerService.Create();
                var customer = new Customer();

                // Act
                EntityActionResponse result = await service.CreateAsync(customer);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result.Success);
                Assert.IsNull(result.Exception);
                service.CustomerRepository.Verify(b => b.InsertAsync(It.IsAny<Customer>()), Times.Once);
            }
        }

        private class TestableCustomerService : CustomerService
        {
            public readonly Mock<IRepositoryAsync<Customer>> CustomerRepository;
            public readonly Mock<IRepositoryAsync<CustomerRole>> CustomerRoleRepository;
            private Mock<IEncryptionService> EncryptionService;

            private TestableCustomerService(Mock<IRepositoryAsync<Customer>> customerRepository, Mock<IRepositoryAsync<CustomerRole>> customerRoleRepository, Mock<IEncryptionService> encryptionService)
                : base(customerRepository.Object, customerRoleRepository.Object, encryptionService.Object)
            {
                CustomerRepository = customerRepository;
                CustomerRoleRepository = customerRoleRepository;
                EncryptionService = encryptionService;
            }

            public static TestableCustomerService Create()
            {
                return new TestableCustomerService(new Mock<IRepositoryAsync<Customer>>(), new Mock<IRepositoryAsync<CustomerRole>>(), new Mock<IEncryptionService>());
            }

            public void SetupRepoFindAsync(Customer customer)
            {
                CustomerRepository
                    .Setup(s => s.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
                    .Returns(Task.FromResult(customer));
            }
        }
    }
}
