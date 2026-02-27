using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edzések;

public class Edzesek
{
    [Key]
    public int Azon { get; set; }
    public string Nev { get; set; }
    public string Tipus { get; set; }
    public int Ido { get; set; }
    public int Kaloria { get; set; }
    public DateTime Datum { get; set; }
}
