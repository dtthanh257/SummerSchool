using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySqlConnector;

namespace SummerSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly string _connectionString;

        public CoursesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// Lấy ra toàn bộ khoá học
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM course";
            var courses = await connection.QueryAsync<Course>(sql);
            return Ok(courses);
        }
        /// <summary>
        /// Lấy ra 1 khoá học theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM course WHERE id = @Id";
            var course = await connection.QueryFirstOrDefaultAsync<Course>(sql, new { Id = id });

            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            return Ok(course);
        }
        /// <summary>
        /// Đăng khoá học mới
        /// </summary>
        /// <param name="courseRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseRequest courseRequest)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"
                    INSERT INTO course (name, descr, start_date, end_date, instructor, thumbnail)
                    VALUES (@Name, @Descr, @StartDate, @EndDate, @Instructor, @Thumbnail)";

                var parameters = new
                {
                    Name = courseRequest.Name,
                    Descr = courseRequest.Descr,
                    StartDate = courseRequest.StartDate,
                    EndDate = courseRequest.EndDate,
                    Instructor = courseRequest.Instructor,
                    Thumbnail = courseRequest.Thumbnail
                };

                await connection.ExecuteAsync(sql, parameters, transaction);
                transaction.Commit();

                return Ok(new { message = "Course added successfully" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        /// <summary>
        /// Xoá khoá học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"DELETE FROM course WHERE id = @Id";

                var parameters = new { Id = id };

                var result = await connection.ExecuteAsync(sql, parameters, transaction);
                transaction.Commit();

                if (result > 0)
                {
                    return Ok(new { message = "Course deleted successfully" });
                }
                else
                {
                    return NotFound(new { message = "Course not found" });
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}
