using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

var _apiKey = "AQ.Ab8RN6L6DACXLfC_xubhsecZWnCNjR8WmpbjKWpdmDhk7pC-gA";
var _http = new HttpClient();
_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
var url  = "https://generativelanguage.googleapis.com/v1beta/models";
var response = await _http.GetAsync(url);
var body = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Status: {response.StatusCode}");
