using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaladBarWeb.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaladBarWeb.Models.DataEntryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SaladBarWeb.Models;

namespace SaladBarWeb.Controllers
{
    [Authorize]
    public class DataEntryController : Controller
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

        public DataEntryController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        ////////
        [HttpGet]
        public IActionResult AllDataCollection()
        {
            var dataCollectionDates = _context.InterventionDays
                .Include(x => x.School)
                .Select(x => new AllDataCollectionViewModel(x))
                .ToList();

            return View(dataCollectionDates);
        }



        [HttpGet]
        public IActionResult Index()
        {
            var schools = _context.Schools
                .Include(x => x.SchoolType);

            var now = DateTime.Now;
            var nowMinus30Min = now.AddMinutes(-30);
            var dataEntryLocks = _context.DataEntryLocks
                .Where(x => x.Locked == "Y" && (x.DtModified < nowMinus30Min || (x.DtModified == null && x.DtCreated < nowMinus30Min)))
                .ToList();

            if (dataEntryLocks.Count() > 0)
            {
                _context.DataEntryLocks.AttachRange(dataEntryLocks);

                dataEntryLocks.ForEach(x => {
                    x.Locked = "N";
                    x.DtModified = now;
                    x.ModifiedBy = "system";
                });

                _context.SaveChanges();
            }
            
            return View(schools);
        }

        [HttpGet]
        public IActionResult DataEntryMeasurementDays(int schoolId)
        {
            var school = _context.Schools
                .Where(x => x.Id == schoolId)
                .FirstOrDefault();
            ViewBag.SchoolName = school.Name;

            var dataEntryMeasurementDays = _context.InterventionDays
                .Where(x => x.SchoolId == schoolId)
                .GroupJoin(_context.DataEntryLocks, ids => ids.Id, del => del.InterventionDayId, (ids, del) => new { ids, del })
                .SelectMany(x => x.del
                        .Where(del => del.Locked == "Y")
                        .DefaultIfEmpty(),
                    (ids, del) => new { ids.ids, del })
                .GroupJoin(_context.AspNetUsers, result1 => result1.del.AspNetUserId, anu => anu.Id, (result1, anu) => new { result1, anu })
                .SelectMany(x => x.anu
                        .DefaultIfEmpty(),
                   (result1, anu) => new { result1.result1, anu.Email })
                .GroupJoin(_context.ResearchTeamMembers, result2 => result2.Email, rtm => rtm.Email, (result2, rtm) => new { result2, rtm })
                .SelectMany(x => x.rtm
                        .DefaultIfEmpty(),
                   (result2, rtm) => new { result2.result2.result1.ids, result2.result2.result1.del, rtm })
                .Where(x => x.ids.Id > 0)
                .OrderBy(x => x.ids.DtIntervention)
                .ToList()
                .Select(x => new DataEntryMeasurementDayViewModel
                {
                    InterventionDay = x.ids,
                    DataEntryLock = x.del,
                    LockedUser = x.rtm,
                    TotalWeighings = NumberOfWeighingsForInterventionDay((int)x.ids.Id),
                    TotalCompletedFirstDataEntry = NumberOfCompletedNthDataEntriesForInterventionDay((int)x.ids.Id, 1),
                    TotalCompletedSecondDataEntry = NumberOfCompletedNthDataEntriesForInterventionDay((int)x.ids.Id, 2),
                    TotalCompletedThirdDataEntry = NumberOfCompletedNthDataEntriesForInterventionDay((int)x.ids.Id, 3)
                });

            return View(dataEntryMeasurementDays);
        }

        private int NumberOfWeighingsForInterventionDay(int interventionDayId)
        {
            // LINQ
            //var numberOfWeighings = _context.Weighings
            //    .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
            //    .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
            //    .Where(result2 => result2.result1.w.InterventionDayId == interventionDayId)
            //    .GroupBy(x => x.rst.RandomizedStudentId)
            //    .Count();

            var numberOfWeighings = _context.Weighings
                .FromSql($@"select rst.randomized_student_id from Weighings w
                            inner join WeighingTrays wt on w.id = wt.weighing_id
                            inner join RandomizedStudentTrays rst on wt.tray_id = rst.tray_id
                            where w.intervention_day_id = {interventionDayId}
                            group by rst.randomized_student_id")
                 .Count();

            return numberOfWeighings;
        }

        // n is the nth completed data entry(i.e. n = 2, return data entries completed twice) 
        private int NumberOfCompletedNthDataEntriesForInterventionDay(int interventionDayId, int n)
        {
            /********* Original Query  *********
            select A.randomized_student_id from
	            (select weighing_id, count(*) cnt from WeighingMeasurements
                where active = 'Y'
	            group by weighing_id) WM
            join
	            (select WT.weighing_id, WT.tray_id, RST.randomized_student_id from RandomizedStudents RS
	            join RandomizedStudentTrays RST on RS.id = RST.randomized_student_id
	            join WeighingTrays WT on RST.tray_id = WT.tray_id
	            where RS.intervention_day_id = @intervention_day_id) A
            on WM.weighing_id = A.weighing_id
            where WM.cnt >= @n
            group by A.randomized_student_id*/
            
            var numberOfCompletedNthDataEntries = _context.WeighingMeasurements
                .Where(x => x.Active == "Y")
                .GroupBy(x => new { x.WeighingId })
                .Select(x => new { x.Key.WeighingId, Count = x.Count() })
                .Where(x => x.Count >= n)
                .Join(_context.WeighingTrays, wm => wm.WeighingId, wt => wt.WeighingId, (wm, wt) => new { wt })
                .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1.wt, rst })
                .Join(_context.RandomizedStudents, result2 => result2.rst.RandomizedStudentId, rs => rs.Id, (result2, rs) => new { result2.rst, rs })
                .Where(x => x.rs.InterventionDayId == interventionDayId)
                .GroupBy(x => new { x.rst.RandomizedStudentId })
                .Count();

            return numberOfCompletedNthDataEntries;
        }

        private bool IsDataEntryForInterventionDayLocked(int interventionDayId)
        {
            var dataEntryLock = _context.DataEntryLocks
                .Where(x => x.InterventionDayId == interventionDayId 
                    && x.Locked == "Y"
                    && x.AspNetUserId != CurrentUser.Id)
                .FirstOrDefault();

            if (dataEntryLock == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<bool> LockDataEntryForInterventionDay(int interventionDayId)
        {
            var dataEntryLock = _context.DataEntryLocks
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.AspNetUserId == CurrentUser.Id)
                .FirstOrDefault();

            if (dataEntryLock == null)
            {
                try
                {
                    dataEntryLock = new DataEntryLocks
                    {
                        AspNetUserId = CurrentUser.Id,
                        InterventionDayId = interventionDayId,
                        Locked = "Y",
                        CreatedBy = CurrentUser.Email,
                        DtCreated = DateTime.Now
                    };

                    _context.DataEntryLocks.Add(dataEntryLock);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: Log error.
                    return false;
                }
            }
            else
            {
                return await RelockDataEntryForInterventionDay(interventionDayId);
            }
        }

        private async Task<bool> RelockDataEntryForInterventionDay(int interventionDayId)
        {
            var dataEntryLock = _context.DataEntryLocks
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.AspNetUserId == CurrentUser.Id)
                .FirstOrDefault();

            if (dataEntryLock == null)
            {
                return false;
            }
            else
            {
                try
                {
                    dataEntryLock.Locked = "Y";
                    dataEntryLock.ModifiedBy = CurrentUser.Email;
                    dataEntryLock.DtModified = DateTime.Now;

                    _context.DataEntryLocks.Update(dataEntryLock);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: Log error.
                    return false;
                }
            }
        }

        private async Task<bool> UnlockDataEntryForInterventionDay(int interventionDayId)
        {
            var dataEntryLocks = _context.DataEntryLocks
                .Where(x => x.InterventionDayId == interventionDayId)
                .ToList();

            if (dataEntryLocks == null)
            {
                return false;
            }
            else
            {
                foreach (var dataEntryLock in dataEntryLocks)
                {
                    dataEntryLock.Locked = "N";
                    dataEntryLock.ModifiedBy = CurrentUser.Email;
                    dataEntryLock.DtModified = DateTime.Now;
                }

                try
                {
                    _context.DataEntryLocks.UpdateRange(dataEntryLocks);
                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // TODO: Log error.
                    return false;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Weighings(int schoolId, int interventionDayId, string message = null)
        {
            // Check to see the intervention day is already lock
            if (IsDataEntryForInterventionDayLocked(interventionDayId))
            {
                return RedirectToAction("DataEntryMeasurementDays", new { schoolId });
            }

            // Lock the intervention day, so no one else can data enter for the same day
            var isLockSuccessful = await LockDataEntryForInterventionDay(interventionDayId);
            if (!isLockSuccessful)
            {
                // TODO: Log error, the interventionday was not able to be locked.
                // Return message to DataEntryMeasurementDays so the user is aware of the issue.
                return RedirectToAction("DataEntryMeasurementDays", new { schoolId });
            }

            var model = new DataEntryViewModel();

            //select * from
            //    (select A.weighing_id, A.tray_id, B.created_by, B.Count from
            //        (select WT.weighing_id, WT.tray_id from RandomizedStudents RS
            //        join RandomizedStudentTrays RST on RS.id = RST.randomized_student_id
            //        join WeighingTrays WT on RST.tray_id = WT.tray_id
            //        where RS.intervention_day_id = 0) A
            //    join
            //        (select WT.weighing_id, WT.tray_id, WM.created_by, WM.Count from WeighingTrays WT
            //        left join
            //            (select weighing_id, created_by, count(*) Count from WeighingMeasurements
            //            group by weighing_id, created_by) WM
            //        on WT.weighing_id = WM.weighing_id
            //        where(WM.Count < 3 or WM.weighing_id is NULL)) B
            //    on A.weighing_id = B.weighing_id
            //    where created_by <> 'tsungyen@asu.edu' or created_by is NULL) result
            //join 
            //    Weighings 
            //on result.weighing_id = Weighings.id;

            /************ Original SQL query ************
            select WT.weighing_id, WT.tray_id from RandomizedStudents RS
            join RandomizedStudentTrays RST on RS.id = RST.randomized_student_id
            join WeighingTrays WT on RST.tray_id = WT.tray_id
            where RS.intervention_day_id = 0)*/
            var setA = _context.RandomizedStudents
                .Join(_context.RandomizedStudentTrays, rs => rs.Id, rst => rst.RandomizedStudentId, (rs, rst) => new { rs, rst })
                .Join(_context.WeighingTrays, result1 => result1.rst.TrayId, wt => wt.TrayId, (result1, wt) => new { result1, wt })
                .Where(result2 => result2.result1.rs.InterventionDayId == interventionDayId)
                .Select(finalResult => new { finalResult.wt.WeighingId, finalResult.result1.rs });

            /************ Original SQL query ************
            select WT.weighing_id, WT.tray_id, WM.created_by, WM.Count from WeighingTrays WT
            left join
                (select weighing_id, created_by, count(*) Count from WeighingMeasurements
                    group by weighing_id, created_by) WM
            on WT.weighing_id = WM.weighing_id
            where(WM.Count < 3 or WM.weighing_id is NULL)*/
            var subSetB = _context.WeighingMeasurements
                .Where(x => x.Active == "Y")
                .GroupBy(x => new { x.WeighingId })
                .Select(wm => new { wm.Key.WeighingId, Count = wm.Count() });

            // Seperating the "CreatedBy" column so the correct group by is done
            var subSubSetB = _context.WeighingMeasurements
                .Join(subSetB, wm => wm.WeighingId, wmc => wmc.WeighingId, (wm, wmc) => new { wm.WeighingId, wm.CreatedBy, wmc.Count });

            var setB = _context.WeighingTrays
                .GroupJoin(subSubSetB, wt => wt.WeighingId, wm => wm.WeighingId, (wt, wm) => new { wt, wm })
                .SelectMany(x => x.wm
                        .DefaultIfEmpty()
                        //.Where(wm => wm.Count < 3 || wm == null),
                        .Where(wm => wm.Count < 2 || wm == null),
                    (wt, wm) => new { wt, wm });

            /************ Abbreviated Original SQL query ************
            select top 1 A.randomized_student_id from
                A
            join
                B
            on A.weighing_id = B.weighing_id
            where created_by <> 'tsungyen@asu.edu' or created_by is NULL
            group by A.randomized_student_id;*/
            var availableRandomizedStudents = setA
                .Join(setB, a => a.WeighingId, b => b.wt.wt.WeighingId, (a, b) => new { a, b })
                .Where(x => x.b.wm == null || x.b.wm.CreatedBy != User.Identity.Name)
                .GroupBy(x => new { x.a.rs.Id, x.a.rs.StudentId })
                .Select(x => new { x.Key.Id, x.Key.StudentId })
                .ToList();
            
            // This means there are no trays/students avaialble to be entered.
            if (availableRandomizedStudents.Count() < 1)
            {
                ViewBag.Message = "All Done! #398";
                return View("WeighingsError");
            }
            var randomizedStudentIndex = 0;
            var firstAvailableRandomizedStudent = availableRandomizedStudents[randomizedStudentIndex];
            IOrderedQueryable<Weighings> weighings;
            // Using this loop to skip students that does not have valid tray
            do
            {
                try
                {
                    firstAvailableRandomizedStudent = availableRandomizedStudents[randomizedStudentIndex];
                }
                catch
                {
                    ViewBag.Message = "All Done!";
                    return View("WeighingsError");
                }
                
                /************ Original SQL query ************
                select* from RandomizedStudentTrays rst
                    inner join WeighingTrays wt on rst.tray_id = wt.tray_id
                    inner join Weighings w on wt.weighing_id = w.id
                where rst.randomized_student_id = 1
                and w.intervention_day_id = 0;*/
                weighings = _context.Weighings
                    .Include(x => x.WeighingTrays)
                    .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
                    .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
                    .Where(result2 => result2.rst.RandomizedStudentId == firstAvailableRandomizedStudent.Id
                        && result2.result1.w.InterventionDayId == interventionDayId)
                    .Select(x => x.result1.w)
                    .OrderBy(x => x.DtCreated);

                randomizedStudentIndex++;
            } while (weighings.Count() < 1);
            
            ViewBag.FirstAvailableRandomizedStudentStudentId = firstAvailableRandomizedStudent.StudentId;
            ViewBag.FirstAvailableRandomizedStudentRowId = firstAvailableRandomizedStudent.Id;

            var randomizedStudents = setA
                .Join(setB, a => a.WeighingId, b => b.wt.wt.WeighingId, (a, b) => new { a, b })
                .Where(x => x.b.wm == null || x.b.wm.CreatedBy != User.Identity.Name)
                .GroupBy(x => new { x.a.rs.Id, x.a.rs.StudentId })
                .Select(x => new { x.Key.Id, x.Key.StudentId });

            var randomizedStudentsCount = _context.Weighings
                .Include(x => x.WeighingTrays)
                .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
                .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
                .GroupJoin(randomizedStudents, w => w.rst.RandomizedStudentId, rs => rs.Id, (w, rs) => new { w.result1, rs })
                .SelectMany(x => x.rs.DefaultIfEmpty(),
                    (w, rs) => new { w.result1, rs })
                .Where(x => x.result1.w.InterventionDayId == interventionDayId)
                .Select(x => x.rs)
                // TODO: Use LinqPad to figure out why some interventionDayId will generate error
                //.ToList()
                //.GroupBy(x => new { x.Id, x.StudentId })
                //.Select(x => new { x.Key.Id, x.Key.StudentId })
                .Count();
            ViewBag.RandomizedStudentsCount = randomizedStudentsCount;

            ViewBag.Weighings = weighings;
            model.Weighings = weighings.ToList();

            var school = _context.Schools
                .Where(x => x.Id == schoolId)
                .FirstOrDefault();
            ViewBag.SchoolName = school.Name;
            ViewBag.SchoolId = school.Id;
            ViewBag.InterventionDayId = interventionDayId;

            var interventionDay = _context.InterventionDays
                .Where(x => x.Id == interventionDayId)
                .FirstOrDefault();
            ViewBag.MeasurementDate = interventionDay.DtIntervention;

            // Dropdown menu options
            var weighStationTypes = _context.WeighStationTypes
                .Select(x => x)
                .ToList();

            var imageTypes = _context.ImageTypes
                .Where(x => x.Active == "Y")
                .Select(x => x)
                .ToList();

            var missingTrayTypes = weighStationTypes
                .SelectMany(wt => imageTypes, (wt, it) => new { Type = $"{wt.Type} - {it.Type}" })
                .Select(x => x.Type)
                .ToList();

            var interventionDayTrayTypes = _context.InterventionDayTrayTypes
                .Include(x => x.TrayType)
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.Active == "Y")
                .ToList();

            ViewBag.WeighStationTypes = weighStationTypes;
            ViewBag.ImageTypes = imageTypes;
            ViewBag.MissingTrayTypes = missingTrayTypes;
            ViewBag.InterventionDayTrayTypes = interventionDayTrayTypes;

            var menuId = _context.Menus
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.Active == "Y")
                .Select(x => x.Id)
                .FirstOrDefault();

            var menuItems = _context.MenuItems
                .Include(x => x.MenuItemType)
                .Where(x => x.MenuId == menuId
                    && x.Active == "Y"
                    && x.Name != "None")
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Quantifiable)
                .Select(x => new MenuItemViewModel(x))
                .ToList();

            var noneMenuItems = _context.MenuItems
                .Include(x => x.MenuItemType)
                .Where(x => x.MenuId == menuId
                    && x.Active == "Y"
                    && x.Name == "None")
                .OrderBy(x => x.Name)
                .Select(x => new MenuItemViewModel(x))
                .ToList();

            menuItems.AddRange(noneMenuItems);

            ViewBag.MenuItems = menuItems;
            model.MenuItems = menuItems;

            var menuItemTypes = _context.MenuItemTypes
                .Where(x => x.Active == "Y")
                .Select(x => new MenuItemTypeViewModel(x));

            ViewBag.MenuItemTypes = menuItemTypes;
            model.MenuItemTypes = menuItemTypes.ToList();

            var imageMetadata = _context.ImageMetadata
                .Where(x => x.Active == "Y")
                .Select(x => new ImageMetadataViewModel(x));

            var duplicateImageOptions = _context.DuplicateImageOptions
                .Where(x => x.Active == "Y")
                .ToList();

            ViewBag.ImageMetadata = imageMetadata;
            ViewBag.DuplicateImageOptions = duplicateImageOptions;
            ViewBag.UserName = User.Identity.Name;
            model.ImageMetadata = imageMetadata.ToList();

            // Global Info
            var globalInfoItems = _context.GlobalInfoItems
                .Where(x => x.Active == "Y")
                .Select(x => new GlobalInfoItemViewModel(x));

            ViewBag.GlobalInfoItems = globalInfoItems;

            if (message != null)
            {
                ViewBag.Message = message;
            }

            return View(model);
        }
        
        [HttpPost]
        public IActionResult Weighings(int schoolId, int interventionDayId, DataEntryViewModel model)
        {
            //return Json(new { schoolId, interventionDayId, model });

            var atLeastOneOptionSelected = true;
            var menuItemType = _context.MenuItemTypes
                .Where(x => x.Active == "Y")
                .ToList();

            // Remove not selected items and check if at least one option is
            // selected in all categories
            foreach (var wm in model.WeighingMeasurements)
            {
                if (wm.WeighingMeasurementMenuItems != null)
                {
                    wm.WeighingMeasurementMenuItems
                    .RemoveAll(x => x.Selected == false);

                    foreach (var type in menuItemType)
                    {
                        var menuTypeSelected = wm.WeighingMeasurementMenuItems
                            .Any(x => x.MenuItem.MenuItemTypeId == type.Id);

                        if (!menuTypeSelected)
                        {
                            atLeastOneOptionSelected = false;
                            ModelState.AddModelError($"{type.Type}[{wm.WeighingId}]", $"Select at least one option for {type.Type}.");
                        }
                    }

                    wm.WeighingMeasurementMenuItems.ForEach(x => {
                        x.MenuItem = null;
                    });
                }

                if (wm.WeighingMeasurementImageMetadata != null)
                {
                    wm.WeighingMeasurementImageMetadata
                    .RemoveAll(x => x.Selected == false);
                }
            }

            if (ModelState.IsValid && atLeastOneOptionSelected)
            {
                var weighingMeasurements = model
                    .WeighingMeasurements
                    //.Where(x => x.WeighingMeasurementMenuItems
                    //    .Any(y => y.Selected)
                    //)
                    .Select(x => x.ConvertToWeighingMeasurements());

                var weighingMeasurementGlobalInfoItems = model
                    .WeighingMeasurementGlobalInfoItems
                    .Where(x => x.Value != null)
                    .Select(x => x.ConvertToWeighingMeasurmentGlobalInfoItems());

                _context.WeighingMeasurements.AddRange(weighingMeasurements);
                _context.WeighingMeasurmentGlobalInfoItems.AddRange(weighingMeasurementGlobalInfoItems);
                _context.SaveChanges();

                var message = "Save successfully.";

                return RedirectToAction("Weighings", new { schoolId, interventionDayId, message });
            }

            var errors = ModelState.Select(x => x)
                .Where(x => x.Value.Errors.Count() > 0)
                .Select(x => x.Key)
                .ToList();

            ViewBag.Errors = String.Join(",", errors);

            /************ Original SQL query ************
            select WT.weighing_id, WT.tray_id from RandomizedStudents RS
            join RandomizedStudentTrays RST on RS.id = RST.randomized_student_id
            join WeighingTrays WT on RST.tray_id = WT.tray_id
            where RS.intervention_day_id = 0)*/
            var setA = _context.RandomizedStudents
                .Join(_context.RandomizedStudentTrays, rs => rs.Id, rst => rst.RandomizedStudentId, (rs, rst) => new { rs, rst })
                .Join(_context.WeighingTrays, result1 => result1.rst.TrayId, wt => wt.TrayId, (result1, wt) => new { result1, wt })
                .Where(result2 => result2.result1.rs.InterventionDayId == interventionDayId)
                .Select(finalResult => new { finalResult.wt.WeighingId, finalResult.result1.rs });

            /************ Original SQL query ************
            select WT.weighing_id, WT.tray_id, WM.created_by, WM.Count from WeighingTrays WT
            left join
                (select weighing_id, created_by, count(*) Count from WeighingMeasurements
                    group by weighing_id, created_by) WM
            on WT.weighing_id = WM.weighing_id
            where(WM.Count < 3 or WM.weighing_id is NULL)*/
            var subSetB = _context.WeighingMeasurements
                .Where(x => x.Active == "Y")
                .GroupBy(x => new { x.WeighingId })
                .Select(wm => new { wm.Key.WeighingId, Count = wm.Count() });

            // Seperating the "CreatedBy" column so the correct group by is done
            var subSubSetB = _context.WeighingMeasurements
                .Join(subSetB, wm => wm.WeighingId, wmc => wmc.WeighingId, (wm, wmc) => new { wm.WeighingId, wm.CreatedBy, wmc.Count });

            var setB = _context.WeighingTrays
                .GroupJoin(subSubSetB, wt => wt.WeighingId, wm => wm.WeighingId, (wt, wm) => new { wt, wm })
                .SelectMany(x => x.wm
                        .DefaultIfEmpty()
                        //.Where(wm => wm.Count < 3 || wm == null),
                        .Where(wm => wm.Count < 2 || wm == null),
                    (wt, wm) => new { wt, wm });

            /************ Abbreviated Original SQL query ************
            select top 1 A.randomized_student_id from
                A
            join
                B
            on A.weighing_id = B.weighing_id
            where created_by <> 'tsungyen@asu.edu' or created_by is NULL
            group by A.randomized_student_id;*/
            var availableRandomizedStudents = setA
                .Join(setB, a => a.WeighingId, b => b.wt.wt.WeighingId, (a, b) => new { a, b })
                .Where(x => x.b.wm == null || x.b.wm.CreatedBy != User.Identity.Name)
                .GroupBy(x => new { x.a.rs.Id, x.a.rs.StudentId })
                .Select(x => new { x.Key.Id, x.Key.StudentId })
                .ToList();
            
            // This means there are no trays/students avaialble to be entered.
            if (availableRandomizedStudents.Count() < 1)
            {
                ViewBag.Message = "All Done! #725";
                return View("WeighingsError");
            }
            var randomizedStudentIndex = 0;
            var firstAvailableRandomizedStudent = availableRandomizedStudents[randomizedStudentIndex];
            IOrderedQueryable<Weighings> weighings;
            // Using this loop to skip students that does not have valid tray
            do
            {
                try
                {
                    firstAvailableRandomizedStudent = availableRandomizedStudents[randomizedStudentIndex];
                }
                catch
                {
                    ViewBag.Message = "All Done!";
                    return View("WeighingsError");
                }

                /************ Original SQL query ************
                select* from RandomizedStudentTrays rst
                    inner join WeighingTrays wt on rst.tray_id = wt.tray_id
                    inner join Weighings w on wt.weighing_id = w.id
                where rst.randomized_student_id = 1
                and w.intervention_day_id = 0;*/
                weighings = _context.Weighings
                    .Include(x => x.WeighingTrays)
                    .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
                    .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
                    .Where(result2 => result2.rst.RandomizedStudentId == firstAvailableRandomizedStudent.Id
                        && result2.result1.w.InterventionDayId == interventionDayId)
                    .Select(x => x.result1.w)
                    .OrderBy(x => x.DtCreated);

                randomizedStudentIndex++;
            } while (weighings.Count() < 1);

            ViewBag.FirstAvailableRandomizedStudentStudentId = firstAvailableRandomizedStudent.StudentId;
            ViewBag.FirstAvailableRandomizedStudentRowId = firstAvailableRandomizedStudent.Id;
            
            var randomizedStudents = setA
                .Join(setB, a => a.WeighingId, b => b.wt.wt.WeighingId, (a, b) => new { a, b })
                .Where(x => x.b.wm == null || x.b.wm.CreatedBy != User.Identity.Name)
                .GroupBy(x => new { x.a.rs.Id, x.a.rs.StudentId })
                .Select(x => new { x.Key.Id, x.Key.StudentId });

            var randomizedStudentsCount = _context.Weighings
                .Include(x => x.WeighingTrays)
                .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
                .Join(_context.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
                .GroupJoin(randomizedStudents, w => w.rst.RandomizedStudentId, rs => rs.Id, (w, rs) => new { w.result1, rs })
                .SelectMany(x => x.rs.DefaultIfEmpty(),
                    (w, rs) => new { w.result1, rs })
                .Where(x => x.result1.w.InterventionDayId == interventionDayId)
                .Select(x => x.rs)
                //.GroupBy(x => new { x.Id, x.StudentId })
                //.Select(x => new { x.Key.Id, x.Key.StudentId })
                .Count();
            ViewBag.RandomizedStudentsCount = randomizedStudentsCount;

            ViewBag.Weighings = weighings;

            var school = _context.Schools
                .Where(x => x.Id == schoolId)
                .FirstOrDefault();
            ViewBag.SchoolName = school.Name;
            ViewBag.SchoolId = school.Id;
            ViewBag.InterventionDayId = interventionDayId;

            var interventionDay = _context.InterventionDays
                .Where(x => x.Id == interventionDayId)
                .FirstOrDefault();
            ViewBag.MeasurementDate = interventionDay.DtIntervention;

            // Dropdown menu options
            var weighStationTypes = _context.WeighStationTypes
                .Select(x => x)
                .ToList();

            var imageTypes = _context.ImageTypes
                .Where(x => x.Active == "Y")
                .Select(x => x)
                .ToList();

            var missingTrayTypes = weighStationTypes
                .SelectMany(wt => imageTypes, (wt, it) => new { Type = $"{wt.Type} - {it.Type}" })
                .Select(x => x.Type)
                .ToList();

            var interventionDayTrayTypes = _context.InterventionDayTrayTypes
                .Include(x => x.TrayType)
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.Active == "Y")
                .ToList();

            ViewBag.WeighStationTypes = weighStationTypes;
            ViewBag.ImageTypes = imageTypes;
            ViewBag.MissingTrayTypes = missingTrayTypes;
            ViewBag.InterventionDayTrayTypes = interventionDayTrayTypes;

            var menuId = _context.Menus
                .Where(x => x.InterventionDayId == interventionDayId
                    && x.Active == "Y")
                .Select(x => x.Id)
                .FirstOrDefault();

            var menuItems = _context.MenuItems
                .Include(x => x.MenuItemType)
                .Where(x => x.MenuId == menuId
                    && x.Active == "Y"
                    && x.Name != "None")
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Quantifiable)
                .Select(x => new MenuItemViewModel(x))
                .ToList();

            var noneMenuItems = _context.MenuItems
                .Include(x => x.MenuItemType)
                .Where(x => x.MenuId == menuId
                    && x.Active == "Y"
                    && x.Name == "None")
                .OrderBy(x => x.Name)
                .Select(x => new MenuItemViewModel(x))
                .ToList();

            menuItems.AddRange(noneMenuItems);

            ViewBag.MenuItems = menuItems;

            var menuItemTypes = _context.MenuItemTypes
                .Where(x => x.Active == "Y")
                .Select(x => new MenuItemTypeViewModel(x));

            ViewBag.MenuItemTypes = menuItemTypes;

            var imageMetadata = _context.ImageMetadata
                .Where(x => x.Active == "Y")
                .Select(x => new ImageMetadataViewModel(x));

            var duplicateImageOptions = _context.DuplicateImageOptions
                .Where(x => x.Active == "Y")
                .ToList();

            ViewBag.ImageMetadata = imageMetadata;
            ViewBag.DuplicateImageOptions = duplicateImageOptions;
            ViewBag.UserName = User.Identity.Name;

            // Global Info
            var globalInfoItems = _context.GlobalInfoItems
                .Where(x => x.Active == "Y")
                .Select(x => new GlobalInfoItemViewModel(x));

            ViewBag.GlobalInfoItems = globalInfoItems;

            ViewBag.Message = "Error occurred, please try again.";

            return View(model);
        }

        [HttpGet]
        public IActionResult ShowCompleted(string id)
        {
            var wmIdList = id
                .Split(",")
                .Select(x => Int64.Parse(x))
                .ToList();

            //var school = _context.Schools
            //    .Where(x => x.Id == schoolId)
            //    .FirstOrDefault();
            //ViewBag.SchoolName = school.Name;
            //ViewBag.SchoolId = school.Id;
            //ViewBag.InterventionDayId = interventionDayId;

            //var interventionDay = _context.InterventionDays
            //    .Where(x => x.Id == interventionDayId)
            //    .FirstOrDefault();
            //ViewBag.MeasurementDate = interventionDay.DtIntervention;

            //var weighings = _context.WeighingTrays
            //    .Include(x => x.Weighing)
            //    .Where(x => x.TrayId == trayId)
            //    .Select(x => x.Weighing);

            var wm = _context.WeighingMeasurements
                .Include(x => x.WeighingMeasurementTrays)
                .Include(x => x.WeighingMeasurementMenuItems)
                .Include(x => x.WeighingMeasurementImageMetadata)
                .Where(x => wmIdList.Contains(x.Id));

            return View(wm);
        }

        [HttpGet]
        public IActionResult Menu(bool activeOnly = false)
        {
            var menus = _context.Menus
                .Include(x => x.InterventionDay)
                .Include(x => x.InterventionDay.School)
                .Include(x => x.MenuItems)
                .OrderBy(x => x.InterventionDay.School.Name)
                .ThenBy(x => x.InterventionDay.DtIntervention)
                .Select(x => new MenuViewModel(x));

            if(activeOnly)
            {
                menus = menus.Where(x => x.Active);
            }

            return View(menus);
        }

        [HttpGet]
        public IActionResult MenuSchoolList()
        {
            var schools = _context.Schools
                .Where(x => x.Active == "Y")
                .OrderBy(x => x.Name)
                .Select(x => new SchoolViewModel(x));

            return View(schools);
        }

        [HttpGet]
        public IActionResult MenuBySchool(int schoolId, bool activeOnly = false)
        {
            var menus = _context.Menus
                .Include(x => x.InterventionDay)
                .Include(x => x.InterventionDay.School)
                .Include(x => x.MenuItems)
                .Where(x => x.InterventionDay.SchoolId == schoolId)
                .OrderBy(x => x.InterventionDay.DtIntervention)
                .Select(x => new MenuViewModel(x));

            if (activeOnly)
            {
                menus = menus.Where(x => x.Active);
            }

            return View(menus);
        }

        [HttpGet]
        public IActionResult AddMenu()
        {
            var measurementDays = _context.InterventionDays
                .Include(x => x.School)
                .Select(x => x)
                .OrderBy(x => x.SchoolId)
                .Select(x => new InterventionDayViewModel(x))
                .ToList();

            ViewBag.MeasurementDays = measurementDays;
            return View();
        }

        [HttpPost]
        public IActionResult AddMenu(MenuViewModel model)
        {
            if (ModelState.IsValid)
            {
                var menu = model.ConvertToMenus();
                _context.Menus.Add(menu);
                _context.SaveChanges();

                var menuItems = new List<MenuItems>();

                // Adding a default "Utensil Pack" option to Miscellaneous
                var miscItemTypeId = _context.MenuItemTypes
                    .Where(x => x.Type == "Miscellaneous" && x.Active == "Y")
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (miscItemTypeId > 0)
                {
                    var utensilPack = new MenuItems
                    {
                        MenuId = menu.Id,
                        MenuItemTypeId = miscItemTypeId,
                        Name = "Utensil pack",
                        Quantifiable = "Y",
                        Active = "Y",
                        CreatedBy = CurrentUser.Email
                    };

                    menuItems.Add(utensilPack);
                }

                // Adding a default "None" option to all active menu item types
                var menuItemTypeIds = _context.MenuItemTypes
                    .Where(x => x.Active == "Y")
                    .Select(x => x.Id);
                
                foreach(var menuItemTypeId in menuItemTypeIds)
                {
                    var menuItem = new MenuItems
                    {
                        MenuId = menu.Id,
                        MenuItemTypeId = menuItemTypeId,
                        Name = "None",
                        Quantifiable = "N",
                        Active = "Y",
                        CreatedBy = CurrentUser.Email
                    };

                    menuItems.Add(menuItem);
                }

                _context.MenuItems.AddRange(menuItems);
                _context.SaveChanges();

                return RedirectToAction("Menu");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult MenuItems(int menuId, bool activeOnly = false)
        {
            var menuItems = _context.MenuItems
                .Include(x => x.MenuItemType)
                .Where(x => x.MenuId == menuId)
                .OrderByDescending(x => x.Active)
                .ThenBy(x => x.MenuItemTypeId)
                .ThenBy(x => x.Name)
                .Select(x => new MenuItemViewModel(x));

            if (activeOnly)
            {
                menuItems = menuItems.Where(x => x.Active);
            }

            var menu = _context.Menus
                .Where(x => x.Id == menuId)
                .FirstOrDefault();

            ViewBag.MenuName = menu.Name;

            return View(menuItems);
        }

        [HttpGet]
        public IActionResult AddMenuItems(int menuId)
        {
            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));

            ViewBag.MenuItemTypes = menuItemTypes;
            ViewBag.MenuId = menuId;

            return View();
        }

        [HttpPost]
        public IActionResult AddMenuItems(List<MenuItemViewModel> model, int menuId)
        {
            if (ModelState.IsValid)
            {
                var menuItems = model.Select(x => x.ConvertToMenuItems())
                    .ToList();
                _context.MenuItems.AddRange(menuItems);
                _context.SaveChanges();
                return RedirectToAction("MenuItems", new { menuId });
            }

            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));

            ViewBag.MenuItemTypes = menuItemTypes;
            ViewBag.MenuId = menuId;
            return View(model);
        }

        [HttpGet]
        public IActionResult EditMenuItem(int menuItemId)
        {
            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));
            ViewBag.MenuItemTypes = menuItemTypes;

            var menuItem = _context.MenuItems
                .Where(x => x.Id == menuItemId)
                .Select(x => new MenuItemViewModel(x))
                .FirstOrDefault();

            return View(menuItem);
        }

        [HttpPost]
        public IActionResult EditMenuItem(MenuItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = CurrentUser.Email;
                model.DtModified = DateTime.Now;
                _context.Update(model.ConvertToMenuItems());
                _context.SaveChanges();

                return RedirectToAction("MenuItems", new { model.MenuId });
            }

            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));
            ViewBag.MenuItemTypes = menuItemTypes;

            return View(model);
        }

        [HttpGet]
        public IActionResult EditMenuItems(int menuId)
        {
            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));
            ViewBag.MenuItemTypes = menuItemTypes;
            ViewBag.MenuId = menuId;

            var menuItems = _context.MenuItems
                .Where(x => x.MenuId == menuId)
                .Select(x => new MenuItemViewModel(x));

            return View(menuItems);
        }

        [HttpPost]
        public IActionResult EditMenuItems(List<MenuItemViewModel> model, int menuId)
        {
            if (ModelState.IsValid)
            {
                model.ForEach(x => {
                    x.ModifiedBy = CurrentUser.Email;
                    x.DtModified = DateTime.Now;
                });
                var menuItems = model.Select(x => x.ConvertToMenuItems())
                    .ToList();

                _context.UpdateRange(menuItems);
                _context.SaveChanges();

                return RedirectToAction("MenuItems", new { menuId });
            }

            var menuItemTypes = _context.MenuItemTypes
                .Select(x => new MenuItemTypeViewModel(x));

            ViewBag.MenuItemTypes = menuItemTypes;
            ViewBag.MenuId = menuId;
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewTrayImages(int trayId)
        {
            var model = new ViewTrayImageViewModel();

            var weighings = _context.Weighings
                .Include(x => x.WeighStationType)
                .Join(_context.WeighingTrays, w => w.Id, wt => wt.WeighingId, (w, wt) => new { w, wt })
                .Where(x => x.wt.TrayId == trayId)
                .Select(x => x.w)
                .OrderBy(x => x.DtCreated)
                .ToList();

            model.Weighings = weighings;

            if (weighings.Count() > 0)
            {
                var interventionDayId = weighings[0].InterventionDayId;

                var interventionDay = _context.InterventionDays
                    .Include(x => x.School)
                    .Where(x => x.Id == interventionDayId)
                    .FirstOrDefault();

                if (interventionDay != null)
                {
                    model.SchoolName = interventionDay.School.Name;
                    model.MeasurementDate = interventionDay.DtIntervention;
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult TrayTypes()
        {
            var trayTypes = _context.TrayTypes
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Active)
                .Select(x => new TrayTypeViewModel(x));

            return View(trayTypes);
        }

        [HttpGet]
        public IActionResult AddTrayTypes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTrayTypes(List<TrayTypeViewModel> model)
        {
            if (ModelState.IsValid)
            {
                var trayTypes = model.Select(x => x.ConvertToTrayTypes())
                    .ToList();
                _context.TrayTypes.AddRange(trayTypes);
                _context.SaveChanges();
                return RedirectToAction("TrayTypes");
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult EditTrayTypes(int trayTypeId = -1)
        {
            IQueryable<TrayTypeViewModel> trayTypes;

            if (trayTypeId < 0)
            {
                trayTypes = _context.TrayTypes
                    .OrderBy(x => x.Type)
                    .ThenBy(x => x.Active)
                    .Select(x => new TrayTypeViewModel(x));
            }

            else
            {
                trayTypes = _context.TrayTypes
                   .Where(x => x.Id == trayTypeId)
                   .Select(x => new TrayTypeViewModel(x));
            }

            return View(trayTypes);
        }

        [HttpPost]
        public IActionResult EditTrayTypes(List<TrayTypeViewModel> model)
        {
            if (ModelState.IsValid)
            {
                model.ForEach(x => {
                    x.ModifiedBy = CurrentUser.Email;
                    x.DtModified = DateTime.Now;
                });
                var trayTypes = model.Select(x => x.ConvertToTrayTypes())
                    .ToList();

                _context.UpdateRange(trayTypes);
                _context.SaveChanges();

                return RedirectToAction("TrayTypes");
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult InterventionDayTrayTypes()
        {
            var interventionDayTrayTypes = _context.InterventionDayTrayTypes
                .Include(x => x.TrayType)
                .Include(x => x.InterventionDay)
                    .ThenInclude(y => y.School)
                .OrderBy(x => x.InterventionDay.School.Name)
                .ThenBy(x => x.TrayType.Type)
                .ThenBy(x => x.Active)
                .Select(x => new InterventionDayTrayTypeViewModel(x));

            return View(interventionDayTrayTypes);
        }

        [HttpGet]
        public IActionResult AddInterventionDayTrayTypes()
        {
            var trayTypes = _context.TrayTypes
                .Where(x => x.Active == "Y")
                .OrderBy(x => x.Type)
                .Select(x => new TrayTypeViewModel(x));

            var interventionDays = _context.InterventionDays
                .Include(x => x.School)
                .Where(x => x.Active == "Y")
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.DtIntervention)
                .Select(x => new Tuple<string, long>($"{x.School.Name}-{x.DtIntervention.ToString("yyyy-MM-dd")}", x.Id));

            ViewBag.TrayTypes = trayTypes;
            ViewBag.InteventionDays = interventionDays;

            return View();
        }

        [HttpPost]
        public IActionResult AddInterventionDayTrayTypes(List<InterventionDayTrayTypeViewModel> model)
        {
            if (ModelState.IsValid)
            {
                var interventionDayTrayTypes = model.Select(x => x.ConvertToInterventionDayTrayTypes())
                    .ToList();
                _context.InterventionDayTrayTypes.AddRange(interventionDayTrayTypes);
                _context.SaveChanges();
                return RedirectToAction("InterventionDayTrayTypes");
            }

            var trayTypes = _context.TrayTypes
                .Where(x => x.Active == "Y")
                .OrderBy(x => x.Type)
                .Select(x => new TrayTypeViewModel(x));

            var interventionDays = _context.InterventionDays
                .Include(x => x.School)
                .Where(x => x.Active == "Y")
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.DtIntervention)
                .Select(x => new Tuple<string, long>($"{x.School.Name}-{x.DtIntervention.ToString("yyyy-MM-dd")}", x.Id));

            ViewBag.TrayTypes = trayTypes;
            ViewBag.InteventionDays = interventionDays;

            return View(model);
        }

        [HttpGet]
        public IActionResult EditInterventionDayTrayTypes(int interventionDayTrayTypeId = -1)
        {
            IQueryable<InterventionDayTrayTypeViewModel> interventionDayTrayTypes;

            if (interventionDayTrayTypeId < 0)
            {
                interventionDayTrayTypes = _context.InterventionDayTrayTypes
                    .Include(x => x.TrayType)
                    .Include(x => x.InterventionDay)
                        .ThenInclude(y => y.School)
                    .OrderBy(x => x.InterventionDay.School.Name)
                    .ThenBy(x => x.TrayType.Type)
                    .ThenBy(x => x.Active)
                    .Select(x => new InterventionDayTrayTypeViewModel(x));
            }

            else
            {
                interventionDayTrayTypes = _context.InterventionDayTrayTypes
                    .Include(x => x.TrayType)
                    .Include(x => x.InterventionDay)
                        .ThenInclude(y => y.School)
                    .Where(x => x.Id == interventionDayTrayTypeId)
                    .Select(x => new InterventionDayTrayTypeViewModel(x));
            }

            var trayTypes = _context.TrayTypes
                .OrderBy(x => x.Type)
                .Select(x => new TrayTypeViewModel(x));

            var interventionDays = _context.InterventionDays
                .Include(x => x.School)
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.DtIntervention)
                .Select(x => new Tuple<string, long, bool>($"{x.School.Name}-{x.DtIntervention.ToString("yyyy-MM-dd")}", x.Id, x.Active == "Y"));

            ViewBag.TrayTypes = trayTypes;
            ViewBag.InteventionDays = interventionDays;

            return View(interventionDayTrayTypes);
        }

        [HttpPost]
        public IActionResult EditInterventionDayTrayTypes(List<InterventionDayTrayTypeViewModel> model)
        {
            if (ModelState.IsValid)
            {
                model.ForEach(x => {
                    x.ModifiedBy = CurrentUser.Email;
                    x.DtModified = DateTime.Now;
                });
                var interventionDayTrayTypes = model.Select(x => x.ConvertToInterventionDayTrayTypes())
                    .ToList();

                _context.UpdateRange(interventionDayTrayTypes);
                _context.SaveChanges();

                return RedirectToAction("InterventionDayTrayTypes");
            }

            var trayTypes = _context.TrayTypes
                .OrderBy(x => x.Type)
                .Select(x => new TrayTypeViewModel(x));

            var interventionDays = _context.InterventionDays
                .Include(x => x.School)
                .OrderBy(x => x.School.Name)
                .ThenBy(x => x.DtIntervention)
                .Select(x => new Tuple<string, long, bool>($"{x.School.Name}-{x.DtIntervention.ToString("yyyy-MM-dd")}", x.Id, x.Active == "Y"));

            ViewBag.TrayTypes = trayTypes;
            ViewBag.InteventionDays = interventionDays;

            return View(model);
        }

        [HttpGet]
        public IActionResult DataEntryLock()
        {
            var dataEntryLocks = _context.DataEntryLocks
                .Include(x => x.InterventionDay)
                    .ThenInclude(y => y.School)
                .Include(x => x.AspNetUser)
                .Join(_context.ResearchTeamMembers, del => del.AspNetUser.Email, rtm => rtm.Email, (del, rtm) => new { del, rtm })
                .Where(x => x.del.Locked == "Y")
                .OrderBy(x => x.del.InterventionDay.School.Name)
                .ThenBy(x => x.del.InterventionDay.DtIntervention)
                .Select(x => new DataEntryLockViewModel(x.del, x.rtm));

            return View(dataEntryLocks);
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDataEntryLockStatusForId(int dataEntryLockId)
        {
            var dataEntryLock = _context.DataEntryLocks
                .Where(x => x.Id == dataEntryLockId)
                .FirstOrDefault();

            if (dataEntryLock == null)
            {
                return BadRequest();
            }
            else
            {
                dataEntryLock.Locked = dataEntryLock.Locked == "Y" ? "N" : "Y";
                dataEntryLock.ModifiedBy = CurrentUser.Email;
                dataEntryLock.DtModified = DateTime.Now;

                try
                {
                    _context.DataEntryLocks.UpdateRange(dataEntryLock);
                    await _context.SaveChangesAsync();

                    return Ok(true);
                }
                catch (Exception ex)
                {
                    // TODO: Log error.
                    return BadRequest(ex.ToString());
                }
            }
        }

        [HttpGet]
        public IActionResult Leaderboard()
        {
            var leaderboard = DataEntryController.GetLeaders(_context);

            return View(leaderboard);
        }

        public static IQueryable<LeaderboardViewModel> GetLeaders(AppDbContext dbContext)
        {
            /********* Original Query  *********
            with count_by_ra as (
                select A.randomized_student_id, WM.created_by from
                    (select weighing_id, created_by, count(*) cnt from WeighingMeasurements

                    where active = 'Y'

                    group by weighing_id, created_by) WM

                join
                    (select WT.weighing_id, WT.tray_id, RST.randomized_student_id from RandomizedStudents RS

                    join RandomizedStudentTrays RST on RS.id = RST.randomized_student_id

                    join WeighingTrays WT on RST.tray_id = WT.tray_id) A
                on WM.weighing_id = A.weighing_id

                group by A.randomized_student_id, WM.created_by
            )
            select created_by, count(*) from count_by_ra
            group by created_by*/

            var leaderboard = dbContext.WeighingMeasurements
                .Where(x => x.Active == "Y")
                .GroupBy(x => new { x.WeighingId, x.CreatedBy })
                .Select(x => new { x.Key.WeighingId, x.Key.CreatedBy, Count = x.Count() })
                .Join(dbContext.WeighingTrays, wm => wm.WeighingId, wt => wt.WeighingId, (wm, wt) => new { wm, wt })
                .Join(dbContext.RandomizedStudentTrays, result1 => result1.wt.TrayId, rst => rst.TrayId, (result1, rst) => new { result1, rst })
                .Join(dbContext.RandomizedStudents, result2 => result2.rst.RandomizedStudentId, rs => rs.Id, (result2, rs) => new { result2, rs })
                .GroupBy(x => new { x.result2.rst.RandomizedStudentId, x.result2.result1.wm.CreatedBy })
                .Select(x => new { x.Key.RandomizedStudentId, x.Key.CreatedBy })
                .GroupBy(x => new { x.CreatedBy })
                .Select(x => new { x.Key.CreatedBy, Count = x.Count() })
                .Join(dbContext.ResearchTeamMembers, wmcnt => wmcnt.CreatedBy, rtm => rtm.Email, (wmcnt, rtm) => new { wmcnt.Count, rtm })
                .Select(x => new LeaderboardViewModel { Count = x.Count, ResearchTeamMember = x.rtm })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.ResearchTeamMember.FirstName);
            var leader = leaderboard.ToList();
            return leaderboard;
        }

    }
}
