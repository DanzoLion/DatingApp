using System;
using System.Collections.Generic;                                                                               // <IEnumerable> // allows simple iteration over a collection type <List> can be used but has more features than simple <IEnumerable>
using System.Linq;                                                                                                       // ToList() -> we are getting our data from database and converting into list
using System.Threading.Tasks;                                                                       // because we use asynchronous threading with Task<ActionResults>
using API.Data;                                                                                             // required because we are using DataContext
using API.Entities;                                                                                                     // AppUser
using Microsoft.AspNetCore.Mvc;                                                                     // Mvc: Model View Controller // view comes from client // how we use .NET to serve HTML pages // c is what we use as we are going to use Angular
using Microsoft.EntityFrameworkCore;                                                        // ToListAsync() - for asynchronous threading

namespace API.Controllers {
    [ApiController]                                                                                                             // attributes specified here
    [Route ("api/[controller]")]                                                                                            // to get to controller, users specify api/controller 
    public class UsersController : ControllerBase                                                               // first derive from ControllerBase         // dependency injection will be used to get data from database here
    {
        private readonly DataContext _context;                                                               // _context allows us to access DataBase
        public UsersController (DataContext context)                                                    // quick fix used to generate constructor for our controller // parameters added to constructors // quick fix used to initilise field from parameter
        {
            _context = context;                                                                                         // quick fix used to initialise field from parameter // inside our class we now have access to datavase via DataContext 
        }                                                                                                                           // two endpoints will be added to get a single user from database, and to get all users from database

        [HttpGet]                                                                                                               // getting data 
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()                                           // return the result from our get request, return result back as a list     //IEnumerable for returning collection list of type AppUser // method GetUsers()
        {                                                                                                                                   // async added for asynchronous threading // then wrap ActionResult<> into a task for asynchronous threading
            return await _context.Users.ToListAsync();                                                                                 // variable created here to store users // specify we want to get our users from list into database // await for asynchronous threading 
                                                                                                                                        // return users;   // we return users from endpoint // change .ToList() to ToListAsync() - for asynchronous threading
        }                                                                                                                                     // await results of task until appropriate results are returned

        [HttpGet("{id}")]                                                               // getting data of individual user and via their Id // id as root parameter // ie api/users/2  -where int is the id of user we are fetching
        public async Task<ActionResult<AppUser>> GetUsers(int id)       // return the result from our get request, return result back as a list     //remove: IEnumerable for returning collection list of type AppUser not returning a collection // method GetUsers(int id) -parameter req. int id
        {
            return await _context.Users.FindAsync(id);                            // variable created here to store users // specify we want to get our users from list into database // Find method finds entity with given primary key from database // don't need a variable as we dont use it
                                                                                        // return user; removed as we don't need to specify a variable  // we return users from endpoint // added async Task<> & await .FindAsync
        }

    }
}