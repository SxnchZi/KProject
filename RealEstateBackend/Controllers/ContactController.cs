using Microsoft.AspNetCore.Mvc;
using RealEstateBackend.Models;
using RealEstateBackend.Services;
using RealEstateBackend.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RealEstateBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;
        private static readonly List<ContactRequest> Requests = new();

        public ContactController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRequest([FromBody] ContactRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Дополнительная валидация на сервере
            var validationErrors = ValidateContactRequest(request);
            if (validationErrors.Any())
            {
                return BadRequest(new { errors = validationErrors });
            }

            _context.ContactRequests.Add(request);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendContactRequestEmailAsync(
                    request.Name, 
                    request.Phone, 
                    request.Comment);
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не прерываем выполнение
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
            }

            return Ok(new { success = true, message = "Заявка успешно отправлена!" });
        }

        private Dictionary<string, string> ValidateContactRequest(ContactRequest request)
        {
            var errors = new Dictionary<string, string>();

            // Проверка имени
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                errors.Add("name", "Имя обязательно для заполнения");
            }
            else if (request.Name.Length > 50)
            {
                errors.Add("name", "Имя не должно превышать 50 символов");
            }
            else if (!Regex.IsMatch(request.Name, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$"))
            {
                errors.Add("name", "Имя может содержать только буквы, пробелы и дефисы");
            }

            // Проверка телефона
            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                errors.Add("phone", "Телефон обязателен для заполнения");
            }
            else if (!Regex.IsMatch(request.Phone, @"^\+7\s?\(\d{3}\)\s?\d{3}-\d{2}-\d{2}$"))
            {
                errors.Add("phone", "Введите телефон в формате +7 (999) 999-99-99");
            }

            // Проверка комментария
            if (!string.IsNullOrEmpty(request.Comment) && request.Comment.Length > 500)
            {
                errors.Add("comment", "Комментарий не должен превышать 500 символов");
            }

            return errors;
        }
    }
}