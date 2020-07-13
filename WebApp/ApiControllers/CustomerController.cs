using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaladBarWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SaladBarWeb.ApiControllers
{
  [Produces("application/json")]
  [Route("api/customer")]
  public class CustomerController : Controller
  {
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin, Research Team Member")]
    [HttpGet]
    public List<Customer> Get()
    {
      List<Customer> customers = new List<Customer>
      {
        new Customer { Id = 1, Name = "Jack" },
        new Customer { Id = 2, Name = "Jill" },
        new Customer { Id = 3, Name = "Joe" },
        new Customer { Id = 4, Name = "John" },
        new Customer { Id = 5, Name = "Jason" }
      };

      return customers;
    }

  }
}