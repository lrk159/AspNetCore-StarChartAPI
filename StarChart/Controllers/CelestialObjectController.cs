using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialResult = _context.CelestialObjects.Where(c => c.Id == id).SingleOrDefault();          

            if (celestialResult != null)
            {
                celestialResult.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
                return Ok(celestialResult);
            } else
            {
                return NotFound();
            }

        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (results.Count > 0)
            {
                foreach (var celObj in results)
                {
                    celObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celObj.Id).ToList();
                }
                return Ok(results);
            } else
            {
                return NotFound();
            }
            

           
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();

            if (results.Count > 0)
            {
                foreach (var celObj in results)
                {
                    celObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celObj.Id).ToList();
                }
                return Ok(results);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
