using System;
using System.Collections.Generic;
using System.Linq;
using Camelot.Services.Abstractions;
using Camelot.Services.Abstractions.Models;
using Camelot.ViewModels.Factories.Interfaces;
using Camelot.ViewModels.Implementations.MainWindow.Drives;
using Camelot.ViewModels.Interfaces.MainWindow.Drives;
using Moq;
using Xunit;

namespace Camelot.ViewModels.Tests
{
    public class DrivesListViewModelTests
    {
        [Fact]
        public void TestDrives()
        {
            var drives = new[]
            {
                new DriveModel
                {
                    RootDirectory = "A"
                },
                new DriveModel
                {
                    RootDirectory = "B"
                },
                new DriveModel
                {
                    RootDirectory = "C"
                }
            };
            var driveServiceMock = new Mock<IDriveService>();
            driveServiceMock
                .SetupGet(m => m.Drives)
                .Returns(drives);
            var driveViewModelFactoryMock = new Mock<IDriveViewModelFactory>();
            var driveViewModels = new List<IDriveViewModel>();
            foreach (var driveModel in drives)
            {
                var driveViewModelMock = new Mock<IDriveViewModel>();
                driveViewModelMock
                    .Setup(m => m.RootDirectory)
                    .Returns(driveModel.RootDirectory);
                driveViewModelFactoryMock
                    .Setup(m => m.Create(driveModel))
                    .Returns(driveViewModelMock.Object);

                driveViewModels.Add(driveViewModelMock.Object);
            }
            var viewModel = new DrivesListViewModel(driveServiceMock.Object,
                driveViewModelFactoryMock.Object);

            Assert.NotNull(viewModel.Drives);
            var actualDrivesViewModels = viewModel.Drives.ToArray();
            Assert.Equal(drives.Length, actualDrivesViewModels.Length);
            Assert.Equal(actualDrivesViewModels, driveViewModels);
        }

        [Fact]
        public void TestDrivesUpdate()
        {
            var driveServiceMock = new Mock<IDriveService>();
            driveServiceMock
                .SetupGet(m => m.Drives)
                .Returns(new List<DriveModel>());
            var driveViewModelFactoryMock = new Mock<IDriveViewModelFactory>();
            var viewModel = new DrivesListViewModel(driveServiceMock.Object,
                driveViewModelFactoryMock.Object);

            Assert.NotNull(viewModel.Drives);
            Assert.Empty(viewModel.Drives);

            var driveModel = new DriveModel
            {
                RootDirectory = "B"
            };
            driveServiceMock
                .SetupGet(m => m.Drives)
                .Returns(new[] {driveModel});
            var driveViewModelMock = new Mock<IDriveViewModel>();
            driveViewModelMock
                .SetupGet(m => m.RootDirectory)
                .Returns(driveModel.RootDirectory);
            driveViewModelFactoryMock
                .Setup(m => m.Create(driveModel))
                .Returns(driveViewModelMock.Object);

            driveServiceMock
                .Raise(m => m.DrivesListChanged += null, EventArgs.Empty);

            Assert.NotNull(viewModel.Drives);
            Assert.Single(viewModel.Drives);
            Assert.Equal(driveModel.RootDirectory, viewModel.Drives.Single().RootDirectory);
        }
    }
}