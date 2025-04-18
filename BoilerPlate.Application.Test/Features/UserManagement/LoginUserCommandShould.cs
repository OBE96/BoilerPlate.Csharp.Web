//using AutoMapper;
//using BoilerPlate.Application.Features.UserManagement.Commands;
//using BoilerPlate.Application.Features.UserManagement.Dtos;
//using BoilerPlate.Application.Features.UserManagement.Handlers;
//using BoilerPlate.Domain.Entities;
//using BoilerPlate.Infrastructure.Repository.Interface;
//using BoilerPlate.Infrastructure.Services.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System.Linq.Expressions;
//using Xunit;

//namespace BoilerPlate.Application.Test.Features.UserManagement
//{
//    public class LoginUserCommandHandlerShould
//    {
//        private readonly IMapper _mapper;
//        private readonly Mock<IRepository<User>> _userRepositoryMock;
//        private readonly Mock<IRepository<LastLogin>> _lastLoginRepositoryMock;
//        private readonly Mock<IPasswordService> _passwordServiceMock;
//        private readonly Mock<ITokenService> _tokenServiceMock;
//        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

//        public LoginUserCommandHandlerShould()
//        {
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<User, UserResponseDto>();
//            });
//            _mapper = config.CreateMapper();

//            _userRepositoryMock = new Mock<IRepository<User>>();
//            _lastLoginRepositoryMock = new Mock<IRepository<LastLogin>>();
//            _passwordServiceMock = new Mock<IPasswordService>();
//            _tokenServiceMock = new Mock<ITokenService>();
//            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
//        }

//        private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> elements) where T : class
//        {
//            var queryable = elements.AsQueryable();
//            var mockDbSet = new Mock<DbSet<T>>();

//            mockDbSet.As<IAsyncEnumerable<T>>()
//                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
//                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

//            mockDbSet.As<IQueryable<T>>()
//                .Setup(m => m.Provider)
//                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

//            mockDbSet.As<IQueryable<T>>()
//                .Setup(m => m.Expression)
//                .Returns(queryable.Expression);
//            mockDbSet.As<IQueryable<T>>()
//                .Setup(m => m.ElementType)
//                .Returns(queryable.ElementType);
//            mockDbSet.As<IQueryable<T>>()
//                .Setup(m => m.GetEnumerator())
//                .Returns(queryable.GetEnumerator());

//            return mockDbSet;
//        }
//        [Fact]
//        public async Task ReturnSuccessResponseForValidCredentials()
//        {
//            // Arrange
//            var user = new User
//            {
//                Id = Guid.NewGuid(),
//                Email = "test@example.com",
//                Password = "hashedPassword",
//                PasswordSalt = "salt",
//                Status = "Active",
//                Organizations = new List<Organization>(),
//                Subscriptions = new List<Subscription>()
//            };

//            // Create a mock DbSet that properly supports async operations
//            var mockDbSet = CreateMockDbSet(new List<User> { user });

//            // Setup repository to return our mock DbSet
//            _userRepositoryMock.Setup(repo => repo.GetQueryableBySpec(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(mockDbSet.Object);

//            // Setup password service to return true for our test password
//            _passwordServiceMock.Setup(service =>
//                service.IsPasswordEqual("password", user.PasswordSalt, user.Password))
//                .Returns(true);

//            // Setup token service to return our test token for any user
//            _tokenServiceMock.Setup(service =>
//                service.GenerateJwt(It.IsAny<User>(), 5))
//                .Returns("token");

//            // Setup HttpContextAccessor to return a dummy IP
//            _httpContextAccessorMock.Setup(a => a.HttpContext.Connection.RemoteIpAddress)
//                .Returns(System.Net.IPAddress.Parse("127.0.0.1"));

//            var handler = new LoginUserCommandHandler(
//                _userRepositoryMock.Object,
//                _lastLoginRepositoryMock.Object,
//                _mapper,
//                _passwordServiceMock.Object,
//                _tokenServiceMock.Object,
//                _httpContextAccessorMock.Object);

//            var command = new CreateUserLoginCommand(new UserLoginRequestDto
//            {
//                Email = "test@example.com",
//                Password = "password"
//            });

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("login successful", result.Message);

//            // Debugging output if the test fails
//            if (result.AccessToken == null)
//            {
//                Console.WriteLine("Token generation failed. Possible reasons:");
//                Console.WriteLine($"- Was GenerateJwt called? {_tokenServiceMock.Invocations.Any(i => i.Method.Name == "GenerateJwt")}");
//                Console.WriteLine($"- Password check passed? {_passwordServiceMock.Invocations.Any(i => i.Method.Name == "IsPasswordEqual")}");
//                Console.WriteLine($"- User status: {user.Status}");
//            }

//            Assert.Equal("token", result.AccessToken);
//            Assert.NotNull(result.Data);
//            Assert.Equal(user.Email, result.Data.User.Email);

//            // Verify all expected interactions occurred
//            _tokenServiceMock.Verify(service => service.GenerateJwt(It.IsAny<User>() , 5), Times.Once);
//            _lastLoginRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<LastLogin>()), Times.Once);
//            _lastLoginRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public async Task ReturnInvalidCredentialsForWrongPassword()
//        {
//            // Arrange
//            var user = new User
//            {
//                Email = "test@example.com",
//                Password = "hashedPassword",
//                PasswordSalt = "salt",
//                Status = "Active"
//            };

//            var mockDbSet = CreateMockDbSet(new List<User> { user });

//            _userRepositoryMock.Setup(repo => repo.GetQueryableBySpec(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(mockDbSet.Object);

//            _passwordServiceMock.Setup(service => service.IsPasswordEqual("wrongpassword", user.PasswordSalt, user.Password))
//                .Returns(false);

//            var handler = new LoginUserCommandHandler(
//                _userRepositoryMock.Object,
//                _lastLoginRepositoryMock.Object,
//                _mapper,
//                _passwordServiceMock.Object,
//                _tokenServiceMock.Object,
//                _httpContextAccessorMock.Object);

//            var command = new CreateUserLoginCommand(new UserLoginRequestDto
//            {
//                Email = "test@example.com",
//                Password = "wrongpassword"
//            });

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Invalid credentials", result.Message);
//            Assert.Null(result.Data);
//            Assert.Null(result.AccessToken);
//            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
//        }

//        [Fact]
//        public async Task ReturnInvalidCredentialsForNonExistentUser()
//        {
//            // Arrange
//            var mockDbSet = CreateMockDbSet(new List<User>());

//            _userRepositoryMock.Setup(repo => repo.GetQueryableBySpec(It.IsAny<Expression<Func<User, bool>>>()))
//                .Returns(mockDbSet.Object);

//            var handler = new LoginUserCommandHandler(
//                _userRepositoryMock.Object,
//                _lastLoginRepositoryMock.Object,
//                _mapper,
//                _passwordServiceMock.Object,
//                _tokenServiceMock.Object,
//                _httpContextAccessorMock.Object);

//            var command = new CreateUserLoginCommand(new UserLoginRequestDto
//            {
//                Email = "nonexistent@example.com",
//                Password = "password"
//            });

//            // Act
//            var result = await handler.Handle(command, CancellationToken.None);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("Invalid credentials", result.Message);
//            Assert.Null(result.Data);
//            Assert.Null(result.AccessToken);
//            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
//        }

//        // Helper classes for async support
//        internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
//        {
//            private readonly IEnumerator<T> _inner;

//            public TestAsyncEnumerator(IEnumerator<T> inner)
//            {
//                _inner = inner;
//            }

//            public ValueTask DisposeAsync()
//            {
//                _inner.Dispose();
//                return ValueTask.CompletedTask;
//            }

//            public ValueTask<bool> MoveNextAsync()
//            {
//                return ValueTask.FromResult(_inner.MoveNext());
//            }

//            public T Current => _inner.Current;
//        }

//        internal class TestAsyncQueryProvider<T> : IAsyncQueryProvider
//        {
//            private readonly IQueryProvider _inner;

//            internal TestAsyncQueryProvider(IQueryProvider inner)
//            {
//                _inner = inner;
//            }

//            public IQueryable CreateQuery(Expression expression)
//            {
//                return new TestAsyncEnumerable<T>(expression);
//            }

//            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
//            {
//                return new TestAsyncEnumerable<TElement>(expression);
//            }

//            public object Execute(Expression expression)
//            {
//                return _inner.Execute(expression);
//            }

//            public TResult Execute<TResult>(Expression expression)
//            {
//                return _inner.Execute<TResult>(expression);
//            }

//            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
//            {
//                var resultType = typeof(TResult).GetGenericArguments()[0];
//                var executionResult = typeof(IQueryProvider)
//                    .GetMethod(
//                        name: nameof(IQueryProvider.Execute),
//                        genericParameterCount: 1,
//                        types: new[] { typeof(Expression) })
//                    .MakeGenericMethod(resultType)
//                    .Invoke(this, new[] { expression });

//                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
//                    .MakeGenericMethod(resultType)
//                    .Invoke(null, new[] { executionResult });
//            }
//        }

//        internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
//        {
//            public TestAsyncEnumerable(IEnumerable<T> enumerable)
//                : base(enumerable)
//            { }

//            public TestAsyncEnumerable(Expression expression)
//                : base(expression)
//            { }

//            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
//            {
//                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
//            }
//        }
//    }
//}