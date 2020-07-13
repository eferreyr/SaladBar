using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaladBarWeb.Data;
using SaladBarWeb.DBModels;
using SaladBarWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaladBarWeb.ApiControllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Authorize(Roles = "Admin, Research Team Member")]
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class SyncController : Controller
  {
    private readonly IdentityDbContext _dbContext;
    private readonly AppDbContext _appContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public SyncController(
        IdentityDbContext dbContext,
        AppDbContext appContext,
        IConfiguration config,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailSender emailSender,
        ILogger<SyncController> logger)
    {
      _dbContext = dbContext;
      _appContext = appContext;
      _config = config;
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
      _logger = logger;
    }

    [Route("Batch")]
    [HttpPost]
    public ActionResult CreateBatch([FromBody] JObject deviceId)
    {
      long batch_id = 0;
      Devices device = null;
      try
      {
        device = deviceId.ToObject<Devices>(new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = deviceId, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      bool deviceFound = _appContext.Devices.Where(x => x.DeviceId == device.DeviceId && x.Active == "Y").Any();

      if (device != null && deviceFound)
      {
        // device found
        // create new batch
        Batches batch = new Batches
        {
          DeviceId = device.DeviceId,
          CreatedBy = User.Identity.Name
        };
        _appContext.Add(batch);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
          return BadRequest(new { SentObject = batch, ErrorMessage = "DB Error: " + exception.InnerException });
        }

        batch_id = batch.Id;
      }
      else if (!deviceFound)
      {
        return BadRequest(new { SentObject = device, ErrorMessage = "Device sent does not exist on server" });
      }
      else
      {
        return BadRequest(new { SentObject = device, ErrorMessage = "Unknown Error" });
      }

      return Ok(batch_id);
    }

    [Route("School")]
    [HttpGet]
    public ActionResult GetAllSchools(bool includeLogo = false)
    {
      List<Schools> schools = null;
      if (includeLogo)
      {
        schools = _appContext.Schools.ToList();
      }
      else
      {
        schools = _appContext.Schools.Select(x => new Schools
        {
          Id = x.Id,
          Name = x.Name,
          District = x.District,
          Mascot = x.Mascot,
          SchoolLogo = null, // omit the school logo
          SchoolTypeId = x.SchoolTypeId,
          Active = x.Active,
          DtCreated = x.DtCreated,
          CreatedBy = x.CreatedBy,
          DtModified = x.DtModified,
          ModifiedBy = x.ModifiedBy,
          SchoolType = null,
          InterventionDays = null,
          Students = null
        }).ToList();
      }

      if (schools == null)
      {
        schools = new List<Schools>();
      }

      return Ok(schools);
    }

    [Route("RandomizedStudent")]
    [HttpGet]
    public ActionResult GetAllRandomizedStudents()
    {
      List<RandomizedStudents> students = _appContext.RandomizedStudents.ToList();

      if (students == null)
      {
        students = new List<RandomizedStudents>();
      }

      return Ok(students);
    }

    [Route("RandomizedStudentByInterventionDayId")]
    [HttpGet]
    public ActionResult GetAllRandomizedStudentsByInterventionDay(int intervention_day_id)
    {
      List<RandomizedStudents> students = _appContext.RandomizedStudents
                                                     .Where(x => x.InterventionDayId == intervention_day_id)
                                                     .ToList();

      if (students == null)
      {
        students = new List<RandomizedStudents>();
      }

      return Ok(students);
    }

    [Route("InterventionDay")]
    [HttpGet]
    public ActionResult GetInterventionDays()
    {
      List<InterventionDays> days = _appContext.InterventionDays.ToList();

      if (days == null)
      {
        days = new List<InterventionDays>();
      }

      return Ok(days);
    }

    [Route("ResearchTeamMember")]
    [HttpGet]
    public ActionResult GetAllResearchTeamMembers()
    {
      List<ResearchTeamMembers> people = _appContext.ResearchTeamMembers.ToList();

      if (people == null)
      {
        people = new List<ResearchTeamMembers>();
      }

      return Ok(people);
    }

    [Route("Device")]
    [HttpPost]
    public ActionResult PostDevices([FromBody] JObject obj)
    {
      Devices device = null;
      try
      {
        device = obj.ToObject<Devices>(new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      bool deviceExists = _appContext.Devices.Where(x => x.DeviceId == device.DeviceId && x.Active == "Y").Any();

      if (device != null && deviceExists)
      {
        // object already exists... get current object
        device = _appContext.Devices.Where(x => x.DeviceId == device.DeviceId).FirstOrDefault();
      }
      else if (device != null)
      {
        _appContext.Add(device);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = device, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }

      return Ok(device);
    }

    [Route("RandomizedStudentStaging")]
    [HttpPost]
    public ActionResult PostRandomizedStudentStaging([FromBody] JObject obj)
    {
      RandomizedStudentsStaging student = null;
      try
      {
        student = obj.ToObject<RandomizedStudentsStaging>(
          new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == student.BatchId && x.DeviceId == student.DeviceId).Any();
      bool validAssent = student.Assent != null && student.Assent != string.Empty;
      bool validStudent = _appContext.RandomizedStudents.Where(x => x.Id == student.RandomizedStudentId).Any();

      if (student != null && validAssent && validStudent && validBatchandDevice)
      {
        // valid student in the system so this is a valid record
        _appContext.Add(student);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = student, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validAssent)
      {
        return BadRequest(new { SentObject = student, ErrorMessage = "Assent was null or empty" });
      }
      else if (!validStudent)
      {
        return BadRequest(new { SentObject = student, ErrorMessage = "Randomized Student id was not found on server" });
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = student, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = student, ErrorMessage = "Unknown Error" });
      }

      return Ok(student);
    }

    [Route("RandomizedStudentTrayStaging")]
    [HttpPost]
    public ActionResult PostRandomizedStudentTrayStaging([FromBody] JObject obj)
    {
      RandomizedStudentTraysStaging tray = null;
      try
      {
        tray = obj.ToObject<RandomizedStudentTraysStaging>(
          new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == tray.BatchId && x.DeviceId == tray.DeviceId).Any();
      bool validStudent = _appContext.RandomizedStudents.Where(x => x.Id == tray.RandomizedStudentId).Any();

      if (tray != null && validStudent && validBatchandDevice)
      {
        // valid student in the system so this is a valid record
        _appContext.Add(tray);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = tray, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validStudent)
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Randomized Student id was not found on server" });
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Unknown Error" });
      }

      return Ok(tray);
    }

    [Route("AuditStaging")]
    [HttpPost]
    public ActionResult PostAuditStaging([FromBody] JObject obj)
    {
      AuditsStaging audit = null;
      try
      {
        audit = obj.ToObject<AuditsStaging>(
          new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == audit.BatchId && x.DeviceId == audit.DeviceId).Any();

      if (audit != null && validBatchandDevice)
      {
        // valid audit in the system
        _appContext.Add(audit);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = audit, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = audit, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = audit, ErrorMessage = "Unknown Error" });
      }

      return Ok(audit);
    }

    [Route("WeighingStaging")]
    [HttpPost]
    public ActionResult PostWeighingStaging([FromBody] JObject obj)
    {
      WeighingsStaging weighing = null;
      try
      {
        weighing = obj.ToObject<WeighingsStaging>(
          new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == weighing.BatchId && x.DeviceId == weighing.DeviceId).Any();
      bool validInterventionDay = _appContext.InterventionDays.Where(x => x.Id == weighing.InterventionDayId).Any();

      if (weighing != null && validInterventionDay && validBatchandDevice)
      {
        // valid weighing for given intervention day in the system
        _appContext.Add(weighing);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = weighing, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validInterventionDay)
      {
        return BadRequest(new { SentObject = weighing, ErrorMessage = "Intervention Day ID was not found on server" });
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = weighing, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = weighing, ErrorMessage = "Unknown Error" });
      }

      return Ok(weighing);
    }

    [Route("WeighingTrayStaging")]
    [HttpPost]
    public ActionResult PostWeighingTrayStaging([FromBody] JObject obj)
    {
      WeighingTraysStaging tray = null;
      try
      {
        tray = obj.ToObject<WeighingTraysStaging>(
        new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }
      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == tray.BatchId && x.DeviceId == tray.DeviceId).Any();

      if (tray != null && validBatchandDevice)
      {
        // need to accept anything submitted here
        _appContext.Add(tray);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {
          return BadRequest(new { SentObject = tray, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Unknown Error" });
      }

      return Ok(tray);
    }

    [Route("InterventionTrayStaging")]
    [HttpPost]
    public ActionResult PostInterventionTrayStaging([FromBody] JObject obj)
    {
      InterventionTraysStaging tray = null;
      try
      {
        tray = obj.ToObject<InterventionTraysStaging>(
        new JsonSerializer { NullValueHandling = NullValueHandling.Ignore });
      }
      catch (Exception ex)
      {
        return BadRequest(new { SentObject = obj, ErrorMessage = "Input Object Error: " + ex.InnerException });
      }

      // check batch id and device id
      bool validBatchandDevice = _appContext.Batches.Where(x => x.Id == tray.BatchId && x.DeviceId == tray.DeviceId).Any();

      if (tray != null && validBatchandDevice)
      {
        // need to accept anything submitted here
        _appContext.Add(tray);
        try
        {
          _appContext.SaveChanges();
        }
        catch (DbUpdateException exception)
        {

          return BadRequest(new { SentObject = tray, ErrorMessage = "DB Error: " + exception.InnerException });
        }
      }
      else if (!validBatchandDevice)
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Batch and/or Device ID was not found on server" });
      }
      else
      {
        return BadRequest(new { SentObject = tray, ErrorMessage = "Unknown Error" });
      }

      return Ok(tray);
    }
  }
}