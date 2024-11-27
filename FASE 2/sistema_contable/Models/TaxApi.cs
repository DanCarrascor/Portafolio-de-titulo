using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaxApi.Models;

namespace TaxApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			// Database context
			services.AddDbContext<TaxDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// JWT Authentication
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration["Jwt:Issuer"],
						ValidAudience = Configuration["Jwt:Issuer"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
					};
				});

			services.AddControllers();

			// Authorization policies
			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}

// Model
using System.ComponentModel.DataAnnotations;

namespace TaxApi.Models
{
	public class TaxDocument
	{
		[Key]
		public int Id { get; set; }
		public string TipoDoc { get; set; }
		public string TipoCompra { get; set; }
		public string RUTProveedor { get; set; }
		public string RazonSocial { get; set; }
		public string Folio { get; set; }
		public DateTime FechaDocto { get; set; }
		public DateTime FechaRecepcion { get; set; }
		public DateTime FechaAcuse { get; set; }
		public decimal MontoExento { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIVARecuperable { get; set; }
		public decimal MontoIvaNoRecuperable { get; set; }
		public string CodigoIVANoRec { get; set; }
		public decimal MontoTotal { get; set; }
		public decimal MontoNetoActivoFijo { get; set; }
		public decimal IVAActivoFijo { get; set; }
		public decimal IVAUsoComun { get; set; }
		public decimal ImptoSinDerechoACredito { get; set; }
		public decimal IVANoRetenido { get; set; }
		public decimal TabacosPuros { get; set; }
		public decimal TabacosCigarrillos { get; set; }
		public decimal TabacosElaborados { get; set; }
		public string NCEoNDEsobreFactDeCompra { get; set; }
		public string CodigoOtroImpuesto { get; set; }
		public decimal ValorOtroImpuesto { get; set; }
		public decimal TasaOtroImpuesto { get; set; }
	}
}

// Controller
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TaxApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TaxDocumentsController : ControllerBase
	{
		private readonly TaxDbContext _context;

		public TaxDocumentsController(TaxDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<IEnumerable<TaxDocument>> GetTaxDocuments()
		{
			return _context.TaxDocuments.ToList();
		}

		[HttpGet("{id}")]
		public ActionResult<TaxDocument> GetTaxDocument(int id)
		{
			var document = _context.TaxDocuments.Find(id);
			if (document == null)
			{
				return NotFound();
			}
			return document;
		}

		[HttpPost]
		public ActionResult<TaxDocument> CreateTaxDocument(TaxDocument taxDocument)
		{
			_context.TaxDocuments.Add(taxDocument);
			_context.SaveChanges();

			return CreatedAtAction(nameof(GetTaxDocument), new { id = taxDocument.Id }, taxDocument);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateTaxDocument(int id, TaxDocument taxDocument)
		{
			if (id != taxDocument.Id)
			{
				return BadRequest();
			}

			_context.Entry(taxDocument).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_context.SaveChanges();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteTaxDocument(int id)
		{
			var document = _context.TaxDocuments.Find(id);
			if (document == null)
			{
				return NotFound();
			}

			_context.TaxDocuments.Remove(document);
			_context.SaveChanges();

			return NoContent();
		}
	}
}

// Database Context
using Microsoft.EntityFrameworkCore;

namespace TaxApi
{
	public class TaxDbContext : DbContext
	{
		public TaxDbContext(DbContextOptions<TaxDbContext> options) : base(options) { }

		public DbSet<TaxApi.Models.TaxDocument> TaxDocuments { get; set; }
	}
}
