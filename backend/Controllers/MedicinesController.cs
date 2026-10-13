using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Authorization;using Microsoft.EntityFrameworkCore;using EMedicine.Api.Data;using EMedicine.Api.Models;
namespace EMedicine.Api.Controllers;
[ApiController][Route("api/medicines")]public class MedicinesController(AppDbContext db):ControllerBase{
[HttpGet]public async Task<IActionResult> List([FromQuery]string? search)=>Ok(await db.Medicines.Where(x=>x.Active&&x.Quantity>0&&x.ExpDate.Date>=DateTime.UtcNow.Date&&(search==null||x.Name.Contains(search)||x.Manufacturer.Contains(search))).OrderBy(x=>x.Name).ToListAsync());
[HttpGet("{id:int}")]public async Task<IActionResult> Get(int id){var m=await db.Medicines.FindAsync(id);return m==null?NotFound():Ok(m);}
[Authorize(Roles="Admin")][HttpGet("admin")]public async Task<IActionResult> All()=>Ok(await db.Medicines.ToListAsync());
[Authorize(Roles="Admin")][HttpPost]public async Task<IActionResult> Add(Medicine m){if(!Valid(m))return BadRequest("Invalid medicine values");m.Id=0;db.Medicines.Add(m);await db.SaveChangesAsync();return Ok(m);}
[Authorize(Roles="Admin")][HttpPut("{id:int}")]public async Task<IActionResult> Update(int id,Medicine m){var old=await db.Medicines.FindAsync(id);if(old==null)return NotFound();if(!Valid(m))return BadRequest("Invalid medicine values");old.Name=m.Name;old.Manufacturer=m.Manufacturer;old.UnitPrice=m.UnitPrice;old.Discount=m.Discount;old.Quantity=m.Quantity;old.ExpDate=m.ExpDate;old.Disease=m.Disease;old.Uses=m.Uses;old.ImageUrl=m.ImageUrl;old.Active=m.Active;await db.SaveChangesAsync();return Ok(old);}
[Authorize(Roles="Admin")][HttpDelete("{id:int}")]public async Task<IActionResult> Delete(int id){var m=await db.Medicines.FindAsync(id);if(m==null)return NotFound();m.Active=false;await db.SaveChangesAsync();return NoContent();}
private static bool Valid(Medicine m)=>!string.IsNullOrWhiteSpace(m.Name)&&m.UnitPrice>=0&&m.Quantity>=0&&m.Discount>=0&&m.Discount<=100;}
