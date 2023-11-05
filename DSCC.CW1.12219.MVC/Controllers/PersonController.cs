using DSCC.CW1._12219.MVC.Models;
using DSCC.CW1._12219.MVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DSCC.CW1._12219.MVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly IConfiguration _configuration;

        public PersonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        string BaseUrl = "http://ec2-54-198-86-192.compute-1.amazonaws.com/";

        // GET: PersonController
        public async Task<ActionResult> Index()
        {
            List<Person> _client = new List<Person>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Person/GetAllPersons");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = await Res.Content.ReadAsStringAsync();

                    _client = JsonConvert.DeserializeObject<List<Person>>(PrResponse);
                }
            }
            return View(_client);
        }

        // GET: PersonController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var customer = new Person();

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"api/Person/GetPerson/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                customer = JsonConvert.DeserializeObject<Person>(responseContent);
            }

            return View(customer);
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person viewModel)
        {
            var customer = new Person
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
                City = viewModel.City,
                Country = viewModel.Country,
                Phone = viewModel.Phone
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var customerJson = JsonConvert.SerializeObject(customer);
            var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Person/CreatePerson", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the customer list or another appropriate action
            }
            else
            {
                // Handle the error, possibly by displaying an error message or returning an error view
                return View("Error");
            }

            return View(viewModel);
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Person viewModel)
        {
            var customer = new Person
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Email = viewModel.Email,
                City = viewModel.City,
                Country = viewModel.Country,
                Phone = viewModel.Phone
            };

            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Serialize the modified customer object to JSON and send it in the request body
            var customerJson = JsonConvert.SerializeObject(customer);
            var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/Person/UpdatePerson/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); // Redirect to the customer list or another appropriate action
            }

            // Handle the case where the update failed or ModelState is not valid
            return View(customer);
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PersonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.DeleteAsync($"api/Person/DeletePerson/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index"); 
            }
            else
            {
                return View("Error");
            }
        }
    }
}
