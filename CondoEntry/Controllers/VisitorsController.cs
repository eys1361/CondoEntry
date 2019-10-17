using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CondoEntry.Data;
using CondoEntry.Models;

namespace CondoEntry.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visitors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Visitor.Include(v => v.Parking);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Visitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor
                .Include(v => v.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // GET: Visitors/Create
        public IActionResult Create()
        {
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingId");
            return View();
        }

        // POST: Visitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,TimeOfEntry,TimeOfExit,UnitNumber,ParkingId")] Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visitor);
                var parking = _context.Parking.Find(visitor.ParkingId);
                parking.IsOccupied = true;
                 _context.Update(parking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>().Where(p => p.IsOccupied == false), "ParkingId", "ParkingId", visitor.ParkingId);
            return View(visitor);
        }

        // GET: Visitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>().Where(p => p.IsOccupied == false), "ParkingId", "ParkingId", visitor.ParkingId);
            return View(visitor);
        }

        // POST: Visitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,TimeOfEntry,TimeOfExit,UnitNumber,ParkingId")] Visitor visitor)
        {
            if (id != visitor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldVisitor = await _context.Visitor.AsNoTracking().FirstOrDefaultAsync(v => v.Id == visitor.Id);
                    
                    var parking = await _context.Parking.AsNoTracking().FirstOrDefaultAsync(p => p.ParkingId == visitor.ParkingId);

                    if(parking.ParkingId != oldVisitor.ParkingId)
                    {
                        var oldParking = await _context.Parking.FindAsync(oldVisitor.ParkingId);
                        oldParking.IsOccupied = false;
                        _context.Parking.Update(parking);
                        await _context.SaveChangesAsync();

                        parking.IsOccupied = true;
                        _context.Parking.Update(parking);
                        await _context.SaveChangesAsync();
                    }                    
                    
                    _context.Visitor.Update(visitor);
                    
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!VisitorExists(visitor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingId", visitor.ParkingId);
            return View(visitor);
        }

        // GET: Visitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor
                .Include(v => v.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // POST: Visitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitor = await _context.Visitor.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            var parking = await _context.Parking.AsNoTracking().FirstOrDefaultAsync(p => p.ParkingId == visitor.ParkingId);
            parking.IsOccupied = false;
            _context.Parking.Update(parking);
            await _context.SaveChangesAsync();
            _context.Visitor.Remove(visitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitor.Any(e => e.Id == id);
        }
    }
}
