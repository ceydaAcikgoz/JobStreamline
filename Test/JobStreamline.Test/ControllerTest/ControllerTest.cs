using JobStreamline.Service;
using JobStreamline.Shared;
using Moq;
using Xunit;
using System;
using JobStreamline.Api.Controllers;
using JobStreamline.Entity;
using Microsoft.AspNetCore.Mvc;
using JobStreamline.Entity.Enum;
using JobStreamline.Test;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Elastic.Apm.Api;

namespace JobStreamline.Tests
{
    public class ControllerTests
    {
        private readonly Mock<ICompanyService> _mockCompanyService;
        private readonly CompanyController _companyController;
        private readonly Mock<IJobService> _mockJobService;
        private readonly JobController _jobController;
        private readonly HttpClient _client;
        private readonly string _token;
        public ControllerTests()
        {
            _mockCompanyService = new Mock<ICompanyService>();
            _companyController = new CompanyController(_mockCompanyService.Object);
            _mockJobService = new Mock<IJobService>();
            _jobController = new JobController(_mockJobService.Object);

            _client = new HttpClient();
        }

        [Fact]
        public async void Create_ShouldReturnCreatedCompany()
        {
            var companyJobs = TestUtil.GenerateCompanyJobs(1);
            // Arrange
            InputCompanyDto inputCompanyDto = companyJobs.FirstOrDefault().Company;
            OutputCompanyDto outputCompanyDto = new OutputCompanyDto();

            _mockCompanyService.Setup(c => c.Create(inputCompanyDto)).Returns(outputCompanyDto);

            // Act
            var result = _companyController.Create(inputCompanyDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(outputCompanyDto, result.Value);
            _mockCompanyService.Verify(c => c.Create(inputCompanyDto), Times.Once);

            InputJobDTO inputJobDto = companyJobs.FirstOrDefault().Jobs.FirstOrDefault();
            OutputJobDto outputJobDto = new OutputJobDto();

            _mockJobService.Setup(s => s.Create(inputJobDto)).ReturnsAsync(outputJobDto);

            // Act
            var resultJob = await _jobController.Create(inputJobDto) as OkObjectResult;

            // Assert
            Assert.NotNull(resultJob);
            Assert.Equal(200, resultJob.StatusCode);
            Assert.Equal(outputJobDto, resultJob.Value);
            _mockJobService.Verify(s => s.Create(inputJobDto), Times.Once);
        }

        [Fact]
        public async Task SearchJobs_ShouldReturnJobList_WhenJobsFound()
        {
            // Arrange
            var searchText = "Software";
            var jobList = new List<JobElasticDTO>();

            _mockJobService.Setup(s => s.SearchJobs(searchText)).ReturnsAsync(jobList);

            // Act
            var result = await _jobController.SearchJobs(searchText);

            // Assert
            Assert.NotNull(result);
            _mockJobService.Verify(s => s.SearchJobs(searchText), Times.Once);
        }

    }
}
