using Microsoft.AspNetCore.Mvc;
using PakrutiApp.Data;
using PakrutiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace PakrutiApp.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Display Quiz from DB
        public IActionResult Quiz()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            // Fetch all questions from database
            var questions = _context.Questions.ToList();

            // If database empty, fallback to default 7
            if (questions.Count == 0)
            {
                questions = new List<Question>
                {
                    new Question { Id = 1, Text = "Your body type is?", OptionVata = "Thin", OptionPitta = "Medium", OptionKapha = "Heavier" },
                    new Question { Id = 2, Text = "Your skin tends to be?", OptionVata = "Dry", OptionPitta = "Sensitive", OptionKapha = "Oily" },
                    new Question { Id = 3, Text = "Your appetite is?", OptionVata = "Irregular", OptionPitta = "Strong", OptionKapha = "Slow" },
                    new Question { Id = 4, Text = "Your sleep is?", OptionVata = "Light", OptionPitta = "Moderate", OptionKapha = "Heavy" },
                    new Question { Id = 5, Text = "You usually feel?", OptionVata = "Cold", OptionPitta = "Warm", OptionKapha = "Cool" },
                    new Question { Id = 6, Text = "Your nature is?", OptionVata = "Creative", OptionPitta = "Focused", OptionKapha = "Calm" },
                    new Question { Id = 7, Text = "You react to stress by?", OptionVata = "Getting anxious", OptionPitta = "Getting angry", OptionKapha = "Withdrawing" }
                };
            }

            return View(questions);
        }

        // ✅ Handle quiz submission
        [HttpPost]
        public IActionResult Quiz(Dictionary<int, string> answers)
        {
            int vata = 0, pitta = 0, kapha = 0;

            foreach (var ans in answers.Values)
            {
                if (ans == "Vata") vata++;
                else if (ans == "Pitta") pitta++;
                else if (ans == "Kapha") kapha++;
            }

            string dominant = (vata > pitta && vata > kapha) ? "Vata" :
                              (pitta > kapha) ? "Pitta" : "Kapha";

            string diet = GetDietAdvice(dominant);
            string tips = GetLifestyleAdvice(dominant);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user != null)
                {
                    // ✅ Store the prakruti type into Users table
                    user.PrakritiType = dominant;
                    _context.Users.Update(user);

                    // ✅ Also store the result in Results table
                    var result = new Result
                    {
                        UserId = user.Id,
                        UserName = user.Name ?? "Unknown",
                        PrakritiType = dominant,
                        DietRecommendation = diet,
                        LifestyleTips = tips
                    };

                    _context.Results.Add(result);
                    _context.SaveChanges();
                }
            }

            ViewBag.Dominant = dominant;
            ViewBag.Diet = diet;
            ViewBag.Tips = tips;

            return View("Result");
        }


        // ✅ Diet advice generator
        private string GetDietAdvice(string type)
        {
            return type switch
            {
                "Vata" => "Eat warm, moist, grounding foods. Avoid dry and cold meals.",
                "Pitta" => "Favor cool, fresh foods. Avoid spicy or fried items.",
                "Kapha" => "Eat light, warm, and spicy foods. Avoid heavy and oily dishes.",
                _ => ""
            };
        }

        // ✅ Lifestyle tips generator
        private string GetLifestyleAdvice(string type)
        {
            return type switch
            {
                "Vata" => "Maintain routine, get proper sleep, and stay warm.",
                "Pitta" => "Practice cooling activities like swimming and meditation.",
                "Kapha" => "Stay active, avoid oversleeping, and engage in daily exercise.",
                _ => ""
            };
        }
    }
}
