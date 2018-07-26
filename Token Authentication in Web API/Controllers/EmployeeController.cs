using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Token_Authentication_in_Web_API.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (dbsampleEntities entites = new dbsampleEntities())
            {
                return entites.Employees.ToList();
            }
        }
    }
}
