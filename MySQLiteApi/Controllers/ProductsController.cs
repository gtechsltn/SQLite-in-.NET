using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySQLiteApi.Data;
using MySQLiteApi.Models;
using System;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MySQLiteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DapperContext _context;

        public ProductsController(DapperContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            using (var connection = _context.CreateConnection())
            {
                string sql = "SELECT * FROM Products";

                var dataList = await connection.QueryAsync<Product>(sql);

                return dataList.ToList();
            }            
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = new Product();
            using (var connection = _context.CreateConnection())
            {
                string sql = "SELECT * FROM Products where Id=@Id";

                product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }

                
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var result = 0; var response = new Product();
            using (var connection = _context.CreateConnection())
            {
                string sql = "INSERT INTO Products(Name,Price) VALUES (@Name,@Price)";

                result = await connection.ExecuteAsync(sql, product);
            }
            if (result > 0) return Ok(product);
            else return response;
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            var result = 0; var response = new Product();
            if (id != product.Id)
            {
                return BadRequest();
            }

            using (var connection = _context.CreateConnection())
            {
                string sql = "UPDATE Products SET Name=@Name,Price=@Price WHERE Id=@id";

                result = await connection.ExecuteAsync(sql, new { product.Name, product.Price, id });
            }

            if (result > 0) return Ok(product);
            else return BadRequest(response);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = 0; 

            using (var connection = _context.CreateConnection())
            {
                string sql = "DELETE FROM Products where ID=@id";

                result = await connection.ExecuteAsync(sql, id);
            }

            if (result > 0) return Ok(id);
            else return BadRequest("0");
        }
    }
}
