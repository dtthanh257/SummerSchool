using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Threading.Tasks;
using MySqlConnector;

namespace SummerSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly string _connectionString;

        public RegistrationsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// Đăng ký khoá học
        /// </summary>
        /// <param name="registrationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterCourse([FromBody] RegistrationRequest registrationRequest)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"INSERT INTO registrations (student_id, course_id, registration_date, status, knowledge) 
                            VALUES (@StudentId, @CourseId, @RegistrationDate, @Status, @Knowledge)";

                var parameters = new
                {
                    StudentId = registrationRequest.StudentId,
                    CourseId = registrationRequest.CourseId,
                    RegistrationDate = DateTime.Now,
                    Status = "registered",
                    Knowledge = registrationRequest.Knowledge
                };

                await connection.ExecuteAsync(sql, parameters, transaction);
                transaction.Commit();

                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        /// <summary>
        /// Kiểm tra xem đã đăng ký khoá học hay chưa
        /// </summary>
        /// <param studentId="studentId"></param>
        /// <param courseId="courseId"></param>
        /// <returns></returns>
        [HttpGet("isRegistered")]
        public async Task<bool> IsRegistered(int studentId, int courseId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT COUNT(*) FROM registrations WHERE student_id = @StudentId AND course_id = @CourseId";
            var count = await connection.ExecuteScalarAsync<int>(sql, new { StudentId = studentId, CourseId = courseId });
            return count > 0;
        }
        /// <summary>
        /// Huỷ đăng ký khoá học
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelRegistration([FromBody] CancelRegistrationRequest request)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var sql = @"DELETE FROM registrations WHERE student_id = @StudentId AND course_id = @CourseId";

                var parameters = new
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId
                };

                var result = await connection.ExecuteAsync(sql, parameters, transaction);
                transaction.Commit();

                if (result > 0)
                {
                    return Ok(new { message = "Cancellation successful" });
                }
                else
                {
                    return NotFound(new { message = "Registration not found" });
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        [HttpGet("registeredStudentsByCourse/{courseId}")]
        public async Task<ActionResult<IEnumerable<RegisteredStudent>>> GetRegisteredStudentsByCourse(int courseId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"
                SELECT 
                    u.id,
                    u.name,
                    u.dob,
                    u.gender,
                    r.registration_date AS RegistrationDate,
                    u.object,
                    r.knowledge
                FROM 
                    registrations r
                JOIN 
                    user u ON r.student_id = u.id
                WHERE 
                    r.course_id = @CourseId";
            var registeredStudents = await connection.QueryAsync<RegisteredStudent>(sql, new { CourseId = courseId });
            return Ok(registeredStudents);
        }
    }

   
}
