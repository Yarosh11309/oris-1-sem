using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyORMLibrary.UnitTests.Models;

namespace MyORMLibrary.UnitTests;

[TestClass]
public class Test1ContextTest1S
{
    public void TestMethod1()
    {
        // Arrange
        var dbConnection = new Mock<IDbConnection>();
        var dbCommand = new Mock<IDbCommand>();
            
            
        var person = new Person()
        { 
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
        };
            
        var context = new Test1Context<Person>(dbConnection.Object);
            
        // Настройка CreateCommand в Connection
        dbConnection.Setup(c => c.CreateCommand()).Returns(dbCommand.Object);
            
        // Act
        var result = context.GetById(person.Id);
            
        // Assert 
        Assert.IsNotNull(result);
        Assert.AreEqual(person.Name, result.Name);
        Assert.AreEqual(person.Email, result.Email);
        Assert.AreEqual(person.Id, result.Id);
        }
        
}