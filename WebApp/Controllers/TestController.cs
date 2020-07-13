using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaladBarWeb.DBModels;
using SaladBarWeb.Models;

namespace SaladBarWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private IdentityUser _currentUser;
        private IdentityUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = _userManager.GetUserAsync(User).Result;
                }

                return _currentUser;
            }
        }

        public TestController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var dataCollectionDates = _context.InterventionDays
                .Include(x => x.School)
                .ThenInclude(x => x.SchoolType)
                .Select(x => new TestViewModel(x))
                .ToList();

            return View(dataCollectionDates);
        }

        //list out all the data collection dates w/ school names and school type
        //pretty format
        [HttpGet]
        public IActionResult Practice1()
        {
            var dataCollectionDates = _context.InterventionDays
                .Include(x => x.School)
                .ThenInclude(x => x.SchoolType)
                .Select(x => new TestViewModel(x))
                .ToList();

            return View(dataCollectionDates);
        }

        //list out all students with student id and school name they attend, gender and grade (Students is a table in db)
        //sort by school name first, then id, then grade
        [HttpGet]
        public IActionResult Practice2()
        {
            var dataCollectionDates = _context.Students
                .Include(x => x.School)
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.StudentId)
                .Select(x => new TestViewModel2(x))
                .ToList();

            return View(dataCollectionDates);
        }

        //make a graph
        [HttpGet]
        public IActionResult Practice3()
        {
            var dataCollectionDates = _context.Students
                .Include(x => x.School)
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.Grade)
                .ThenBy(x => x.StudentId)
                .Select(x => new TestViewModel3(x))
                .ToList();

            return View(dataCollectionDates);
        }
    }
}