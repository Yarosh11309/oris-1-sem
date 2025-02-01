using Moq;
using MyORMLibrary.UnitTests.Models;
using System.Data;
using System.Data.Common;

namespace MyORMLibrary.UnitTests
{
    [TestClass]
    public class TestContextTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var dbConnection = new Mock<IDbConnection>();
            var dbCommand = new Mock<IDbCommand>();
            var dbDataReader = new Mock<IDataReader>();

            var person = new Person()
            {
                Id = 1,
                Email = "test@mail.ru",
                Name = "Test",
            };

            var context = new Test1Context<Person>(dbConnection.Object);

            // ��������� ������������� IDataReader
            dbDataReader.SetupSequence(r => r.Read())
                      .Returns(true)
                      .Returns(false);

            // ����
            // TODO: �������� �� �� ��� �������� id = 1
            // TODO: �� �������� � ���� ������ ���������� ����� �� ORMContext ������ GetById, GetAll, Create, Update, Delete
            dbDataReader.Setup(c => c.GetBoolean(It.Is<int>(p => p == 1))).Returns(true);
            // ---------------------------------------------

            dbDataReader.Setup(r => r["Id"]).Returns(person.Id);
            dbDataReader.Setup(r => r["Email"]).Returns(person.Email);
            dbDataReader.Setup(r => r["Name"]).Returns(person.Name);

            // ��������� ������ ExecuteReader
            dbCommand.Setup(c => c.ExecuteReader()).Returns(dbDataReader.Object);

            // ��������� CreateCommand � Connection
            dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);

            // Act
            var result = context.GetById(person.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(person.Id, result.Id);
            Assert.AreEqual(person.Email, result.Email);
            Assert.AreEqual(person.Name, result.Name);
        }
    }
}