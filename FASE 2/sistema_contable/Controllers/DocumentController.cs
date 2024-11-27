using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiApi.Data;
using MiApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using sistema_contable.Data;

namespace MiApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DocumentController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public DocumentController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
		{
			return await _context.Documents.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Document>> GetDocument(int id)
		{
			var document = await _context.Documents.FindAsync(id);
			if (document == null) return NotFound();
			return document;
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Document>> PostDocument(Document document)
		{
			_context.Documents.Add(document);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
		}

		[HttpPut("{id}")]