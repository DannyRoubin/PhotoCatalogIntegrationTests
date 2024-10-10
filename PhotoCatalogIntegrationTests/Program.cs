using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;  // Add Newtonsoft.Json package for JSON serialization

namespace PhotoCatalogIntegrationTests
{
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8080") // The base URL of your Java API
        };

        private static int photoshootID; // Variable to store the ID of the created photoshoot

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting integration tests...");

            await TestGetAllPhotoshoots();
            await TestCreateNewPhotoshoot(); // This will set the photoshootID
            await TestGetPhotoshootById(photoshootID);
            await TestUpdatePhotoshoot(photoshootID);
            await TestAddPhotoToPhotoshoot(photoshootID, "D4E3504D-0B6D-4888-96E5-30C2CCE4E399");
            await TestGetAllPhotosForPhotoshoot(photoshootID); 
            await TestDeletePhotoshoot(photoshootID);

            Console.WriteLine("Integration tests completed.");
        }

        static async Task TestGetAllPhotoshoots()
        {
            try
            {
                var response = await _httpClient.GetAsync("/photoshoot");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine("GET /photoshoot:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {content}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestGetAllPhotoshoots: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestGetAllPhotoshoots: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestGetAllPhotoshoots: EXCEPTION - {ex.Message}");
            }
        }

        // Test method to create a new photoshoot
        static async Task TestCreateNewPhotoshoot()
        {
            try
            {
                // Create the object representing the request body, matching the structure expected by the API
                var newPhotoshoot = new
                {
                    date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    location = new
                    {
                        locationID = 1,  // Replace with a valid location ID from your database
                        locationName = "Redmond"
                    }
                };

                // Serialize the object into JSON
                var json = JsonConvert.SerializeObject(newPhotoshoot);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the POST request
                var response = await _httpClient.PostAsync("/photoshoot", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("POST /photoshoot:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response to extract the photoshootID
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    photoshootID = responseObject.photoshootID;

                    Console.WriteLine($"Extracted PhotoshootID: {photoshootID}");
                    Console.WriteLine("TestCreateNewPhotoshoot: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestCreateNewPhotoshoot: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestCreateNewPhotoshoot: EXCEPTION - {ex.Message}");
            }
        }

        static async Task TestGetPhotoshootById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/photoshoot/{id}");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"GET /photoshoot/{id}:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {content}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestGetPhotoshootById: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestGetPhotoshootById: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestGetPhotoshootById: EXCEPTION - {ex.Message}");
            }
        }

        static async Task TestUpdatePhotoshoot(int id)
        {
            try
            {
                var updatedPhotoshoot = new
                {
                    date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    location = new
                    {
                        locationID = 1,
                        locationName = "UpdatedLocation"
                    }
                };

                var json = JsonConvert.SerializeObject(updatedPhotoshoot);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/photoshoot/{id}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"PUT /photoshoot/{id}:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {responseBody}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestUpdatePhotoshoot: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestUpdatePhotoshoot: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestUpdatePhotoshoot: EXCEPTION - {ex.Message}");
            }
        }

        static async Task TestDeletePhotoshoot(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/photoshoot/{id}");
                Console.WriteLine($"DELETE /photoshoot/{id}:");
                Console.WriteLine($"Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestDeletePhotoshoot: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestDeletePhotoshoot: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestDeletePhotoshoot: EXCEPTION - {ex.Message}");
            }
        }

        static async Task TestAddPhotoToPhotoshoot(int photoshootID, string photoGUID)
        {
            try
            {
                var response = await _httpClient.PostAsync($"/photoshoot/{photoshootID}/addPhoto/{photoGUID}", null);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"POST /photoshoot/{photoshootID}/addPhoto/{photoGUID}:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {content}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestAddPhotoToPhotoshoot: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestAddPhotoToPhotoshoot: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestAddPhotoToPhotoshoot: EXCEPTION - {ex.Message}");
            }
        }

        static async Task TestGetAllPhotosForPhotoshoot(int photoshootId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/photoshoot/{photoshootId}/photo");
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"GET /photoshoot/{photoshootId}/photo:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {content}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("TestGetAllPhotosForPhotoshoot: SUCCESS");
                }
                else
                {
                    Console.WriteLine("TestGetAllPhotosForPhotoshoot: FAILED");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestGetAllPhotosForPhotoshoot: EXCEPTION - {ex.Message}");
            }
        }
    }
}
