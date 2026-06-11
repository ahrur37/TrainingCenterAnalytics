using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace EduRequestSystemAPI.Services
{
    public class DictionariesService : IDictionariesService
    {
        private readonly ContextDb _context;

        public DictionariesService(ContextDb context)
        {
            _context = context;
        }

        public Task<IActionResult> GetDirectionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetTrainingFormatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
