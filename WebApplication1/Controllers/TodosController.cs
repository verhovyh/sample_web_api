﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Todos
        [HttpGet]
        public IEnumerable<Todo> GetTodos()
        {
            return _context.Todos;
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // PUT: api/Todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo([FromRoute] int id, [FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Todos
        [HttpPost]
        public async Task<IActionResult> PostTodo([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}