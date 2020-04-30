using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RVTR.Account.DataContext;
using RVTR.Account.DataContext.Repositories;
using RVTR.Account.ObjectModel.Models;
using RVTR.Account.WebApi.Controllers;
using Xunit;

namespace RVTR.Account.UnitTesting.Tests
{
  public class ProfileControllerTest
  {
    private static readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");
    private static readonly DbContextOptions<AccountContext> _options = new DbContextOptionsBuilder<AccountContext>().UseSqlite(_connection).Options;
    private readonly ProfileController _controller;
    private readonly ILogger<ProfileController> _logger;
    private readonly UnitOfWork _unitOfWork;

    public ProfileControllerTest()
    {
      var contextMock = new Mock<AccountContext>(_options);
      var loggerMock = new Mock<ILogger<ProfileController>>();
      var repositoryMock = new Mock<Repository<ProfileModel>>(new AccountContext(_options));
      var unitOfWorkMock = new Mock<UnitOfWork>(contextMock.Object);

      repositoryMock.Setup(m => m.DeleteAsync(0)).Throws(new Exception());
      repositoryMock.Setup(m => m.DeleteAsync(1)).Returns(Task.FromResult(1));
      repositoryMock.Setup(m => m.InsertAsync(null)).Throws(new Exception());
      repositoryMock.Setup(m => m.InsertAsync(new ProfileModel() { Id = 1, Email = "email" })).Returns(Task.FromResult<ProfileModel>(null));
      repositoryMock.Setup(m => m.SelectAsync()).Returns(Task.FromResult<IEnumerable<ProfileModel>>(null));
      repositoryMock.Setup(m => m.SelectAsync(0)).Throws(new Exception());
      repositoryMock.Setup(m => m.SelectAsync(1)).Returns(Task.FromResult<ProfileModel>(null));
      repositoryMock.Setup(m => m.Update(null)).Throws(new Exception());
      repositoryMock.Setup(m => m.Update(new ProfileModel() { Id = 1, Email = "email" }));
      unitOfWorkMock.Setup(m => m.Profile).Returns(repositoryMock.Object);

      _logger = loggerMock.Object;
      _unitOfWork = unitOfWorkMock.Object;
      _controller = new ProfileController(_logger, _unitOfWork);
    }

    [Fact]
    public async void Test_Controller_Delete()
    {
      var resultFail = await _controller.Delete(0);
      var resultPass = await _controller.Delete(1);

      Assert.NotNull(resultFail);
      Assert.NotNull(resultPass);
    }

    [Fact]
    public async void Test_Controller_Get()
    {
      var resultMany = await _controller.Get();
      var resultFail = await _controller.Get(0);
      var resultOne = await _controller.Get(1);

      Assert.NotNull(resultMany);
      Assert.NotNull(resultFail);
      Assert.NotNull(resultOne);
    }

    [Fact]
    public async void Test_Controller_Post()
    {
      var resultFail = await _controller.Post(null);
      var resultPass = await _controller.Post(new ProfileModel() { Id = 1, Email = "email" });

      Assert.NotNull(resultFail);
      Assert.NotNull(resultPass);
    }

    [Fact]
    public async void Test_Controller_Put()
    {
      var resultFail = await _controller.Put(null);
      var resultPass = await _controller.Put(new ProfileModel() { Id = 1, Email = "email" });

      Assert.NotNull(resultFail);
      Assert.NotNull(resultPass);
    }
  }
}
