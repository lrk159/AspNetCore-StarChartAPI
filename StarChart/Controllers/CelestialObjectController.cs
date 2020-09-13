using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")] 
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var celObj = _context.CelestialObjects.Where(c => c.Id == id).SingleOrDefault();

            if (celObj == null)
            {
                return NotFound();
            }

            celObj.Name = celestialObject.Name;
            celObj.OrbitalPeriod = celestialObject.OrbitalPeriod;
            celObj.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celObj);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var selCelObj = _context.CelestialObjects.Where(c => c.Id == id).SingleOrDefault();

            if (selCelObj == null)
            {
                return NotFound();
            }

            selCelObj.Name = name;

            _context.CelestialObjects.Update(selCelObj);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var selCelObjs = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();

            if (selCelObjs.Count == 0)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(selCelObjs);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
