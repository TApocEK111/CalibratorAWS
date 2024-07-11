namespace Calibrator.Tests.Repository;

public class ReportRepositoryTest
{
    [Fact]
    public void CreatesDB()
    {
        var testHelper = new RepositoryTestHelper();
        var personRepository = testHelper.ReportRepository;

        Assert.Equal(1, 1);
    }

    [Fact]
    public void AddsAndGetsAllCorrectly()
    {
        var testHelper = new RepositoryTestHelper();
        var reportRepository = testHelper.ReportRepository;
        var report = testHelper.TestReport2;

        var calibrator = new Domain.Model.Calibrator.Calibrator();
        calibrator.CalculatePhysicalQuantitylValues(report);

        reportRepository.AddAsync(report).Wait();
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();

        var results = reportRepository.GetAllAsync().Result;
        Assert.True(results.Count == 2);
        Assert.Equal(results[1].Sensors[0].Channels[0].Samples.Count, 6);
        Assert.Equal(results[1].Sensors[0].Channels[0].AverageSamples.Count, 3);
        Assert.Equal(results[0].Sensors[0].Channels[0].Samples.Count, 8);
        Assert.Equal(results[0].Sensors[0].Channels[0].AverageSamples.Count, 3);

    }

    [Fact]
    public void GetsByIdCorrectly()
    {
        var testHelper = new RepositoryTestHelper();
        var reportRepository = testHelper.ReportRepository;

        var report = reportRepository.GetByIdAsync(testHelper.TestReport1.Id).Result;

        Assert.Equal(report.Sensors[0].Channels[0].Samples.Count, 8);
        Assert.Equal(report.Sensors[0].Channels[0].AverageSamples.Count, 3);


        for (int i = 0; i < testHelper.TestReport1.Sensors[0].Channels[0].Samples.Count; i++)
        {
            Assert.Equal(testHelper.TestReport1.Sensors[0].Channels[0].Samples[i].PhysicalQuantity, report.Sensors[0].Channels[0].Samples[i].PhysicalQuantity);
        }
    }

    [Fact]
    public void UpdatesDataCorrectly()
    {
        var testHelper = new RepositoryTestHelper();
        var reportRepository = testHelper.ReportRepository;
        var reports = reportRepository.GetAllAsync().Result;
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();

        Sample sample = new Sample() { ReferenceValue = -20, Parameter = -2.2 };
        reports[0].Sensors[0].Channels[0].Samples.Add(sample);

        reports[0].Sensors[0].Channels[0].Samples.RemoveAt(2);

        reports[0].Sensors[0].Channels[0].Samples[1].Parameter = -0.8;

        reports[0].Operator = "Barboskina Liza";

        reportRepository.UpdateAsync(reports[0]).Wait();

        var newReports = reportRepository.GetAllAsync().Result;

        Assert.Equal("Barboskina Liza", newReports[0].Operator);
        Assert.Equal(newReports[0].Sensors[0].Channels[0].Samples.Count, 8);
        Assert.Equal(newReports[0].Sensors[0].Channels[0].Samples[1].Parameter, -0.8);
        Assert.Equal(newReports[0].Sensors[0].Channels[0].Samples[7].ReferenceValue, -20);
    }

    [Fact]
    public void DeletesCorrectly()
    {
        var testHelper = new RepositoryTestHelper();
        var reportRepository = testHelper.ReportRepository;
        var reports = reportRepository.GetAllAsync().Result;
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();
        reportRepository.DeleteAsync(reports[0].Id).Wait();

        var context = reportRepository.UnitOfWork;
        var sensors = context.Sensors.ToList();
        var channels = context.SensorChannels.ToList();
        var samples = context.Samples.ToList();
        var avgSamples = context.AverageSamples.ToList();
        var externalImpacts = context.ExternalImpacts.ToList();
        var newReports = reportRepository.GetAllAsync().Result;

        Assert.Empty(newReports);
        Assert.Empty(sensors);
        Assert.Empty(channels);
        Assert.Empty(samples);
        Assert.Empty(avgSamples);
        Assert.Empty(externalImpacts);
    }
}
