using Repository.Interface;
using Repository.Models;
using Repository.Repository;
using Services.Interface;
using System.Collections.Generic;
using System.Linq;


namespace Services.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly IGenericRepository<TestResult> _testResultRepository;
        private readonly BloodDonationDbContext _context;
        public TestResultService()
        {
            _testResultRepository = new GenericRepository<TestResult>();
            _context = new BloodDonationDbContext();
        }
        public List<TestResult> GetAllTestResults()
        {
            return _context.TestResults.ToList();
        }
    }
}
