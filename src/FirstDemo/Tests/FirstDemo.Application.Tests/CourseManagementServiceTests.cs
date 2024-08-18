using Autofac.Extras.Moq;
using FirstDemo.Application.Features.Training.Services;
using FirstDemo.Domain.Entities;
using FirstDemo.Domain.Exceptions;
using FirstDemo.Domain.Repositories;
using Moq;
using Shouldly;

namespace FirstDemo.Application.Tests
{
    public class CourseManagementServiceTests
    {
        private AutoMock _mock;
        private Mock<ICourseRepository> _courseRepositoryMock;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private CourseManagementService _courseManagementService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = _mock.Mock<ICourseRepository>();
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _courseManagementService = _mock.Create<CourseManagementService>();
        }

        [TearDown]
        public void TearDown()
        {
            _courseRepositoryMock?.Reset();
            _unitOfWorkMock?.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task CreateCourseAsync_TitleUnique_CreatesNewCourse()
        {
            // Arrange
            const string title = "C# beginner";
            const uint fees = 2000;
            const string description = "A beginner guide to C#";

            var course = new Course
            {
                Title = title,
                Fees = fees,
                Description = description
            };

            _unitOfWorkMock.SetupGet(x => x.CourseRepository).Returns(_courseRepositoryMock.Object).Verifiable();
            _courseRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(title, null)).ReturnsAsync(false).Verifiable();
            
            _courseRepositoryMock.Setup(x => x.AddAsync(It.Is<Course>(y => y.Title == title 
                && y.Fees == fees && y.Description == description))).Returns(Task.CompletedTask).Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            // Act
            await _courseManagementService.CreateCourseAsync(title, fees, description);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _unitOfWorkMock.VerifyAll(),
                () => _courseRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public async Task CreateCourseAsync_TitleDuplicate_ThrowsException()
        {
            // Arrange
            const string title = "C# beginner";
            const uint fees = 2000;
            const string description = "A beginner guide to C#";

            var course = new Course
            {
                Title = title,
                Fees = fees,
                Description = description
            };

            _unitOfWorkMock.SetupGet(x => x.CourseRepository).Returns(_courseRepositoryMock.Object).Verifiable();
            _courseRepositoryMock.Setup(x => x.IsTitleDuplicateAsync(title, null)).ReturnsAsync(true).Verifiable();

            // Act && Assert
            await Should.ThrowAsync<DuplicateTitleException>(async
                () => await _courseManagementService.CreateCourseAsync(title, fees, description)
            );
        }
    }
}