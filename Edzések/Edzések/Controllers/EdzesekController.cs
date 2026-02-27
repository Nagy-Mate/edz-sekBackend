
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edzések.Controllers;

[Route("/[controller]")]
[ApiController]
public class EdzesekController(EdzesekDbContext db) : ControllerBase
{
    [HttpGet("/hosszu/{perc}")]
    public ActionResult GetHosszus(int perc)
    {
        
        var res = db.Edzesek.Where(e => e.Ido >= perc).Select(e => new
        {
            név = e.Nev,
            típus = e.Tipus,
            idő = e.Ido,

        }).ToList();

        return Ok(res);
    }
    [HttpGet("/vendeg")]
    public ActionResult GetByVendeg([FromQuery] string nev)
    {
        var res = db.Edzesek.Where(e => e.Nev == nev).Select(g => new {
            típus = g.Tipus,
            idő = g.Ido,
            dátum = g.Datum,
        }).ToList();
      
        return Ok(res); 
    }


    [HttpGet("/aktiv/{darab}")]
    public ActionResult GetAkitv(int darab)
    {

        var res = db.Edzesek.GroupBy(e => e.Nev).Where(a => a.Count() >= darab).Select(g => new {név = g.Key}).ToList();
      
        return Ok(res);
    }

    [HttpPut("/modosit")]
    public ActionResult PutModosit([FromBody] putKaloriaDto kaloriaDto)
    {
        var edzes = db.Edzesek.FirstOrDefault(e => e.Azon == kaloriaDto.azon);
        if (edzes == null)
            return StatusCode(404, "Nincs ilyen edzés");
        
        if(edzes.Kaloria < kaloriaDto.kaloria)
            return StatusCode(400, "Nem megfelelő kalóriaérték");

        if (100 > kaloriaDto.kaloria)
            return StatusCode(400, "Túl alacsony érték");


        db.Edzesek.Where(e => e.Azon == kaloriaDto.azon).ExecuteUpdate(e => e.SetProperty(p => p.Kaloria, kaloriaDto.kaloria));

        return NoContent();
    }

}
