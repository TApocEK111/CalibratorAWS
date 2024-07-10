namespace Calibrator.Tests.Repository;

public class ReportRepositoryTest
{
    [Fact]
    //Тест, проверяющий, что база данных создалась
    public void VoidTest()
    {
        var testHelper = new TestHelper();
        var personRepository = testHelper.ReportRepository;

        Assert.Equal(1, 1);
    }

    [Fact]
    public void TestAdd()
    {
        var testHelper = new TestHelper();
        var reportRepository = testHelper.ReportRepository;
        var report = testHelper.TestReport;

        var calibrator = new Domain.Model.Calibrator.Calibrator();
        calibrator.CalculatePhysicalQuantitylValues(report);

        reportRepository.AddAsync(report).Wait();
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();

        var results = reportRepository.GetAllAsync().Result;
        Assert.True(results.Count == 2);
        Assert.Equal(results[1].Sensors[0].Channels[0].Samples.Count, 6);
        Assert.Equal(results[0].Sensors[0].Channels[0].Samples.Count, 8);

    }

    [Fact]
    public void TestUpdateAdd()
    {
        var testHelper = new TestHelper();
        var reportRepository = testHelper.ReportRepository;
        var reports = reportRepository.GetAllAsync().Result;
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();
        reports[0].Operator = "Barboskina Liza";

        reportRepository.UpdateAsync(reports[0]).Wait();

        Assert.Equal("Barboskina Liza", reportRepository.GetAllAsync().Result[0].Operator);
    }

    [Fact]
    public void TestDelete()
    {
        var testHelper = new TestHelper();
        var reportRepository = testHelper.ReportRepository;
        var reports = reportRepository.GetAllAsync().Result;
        //Запрещаем отслеживание сущностей (разрываем связи с БД)
        reportRepository.ChangeTrackerClear();
        reportRepository.DeleteAsync(reports[0].Id).Wait();

        var newReports = reportRepository.GetAllAsync().Result;

        Assert.Empty(newReports);
    }
}
