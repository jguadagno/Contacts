using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contacts.WebUi.Models;
using Microsoft.Identity.Web;

namespace Contacts.WebUi.Services
{
    public class ContactService: IContactService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly Settings _settings;

        public ContactService(ITokenAcquisition tokenAcquisition, HttpClient httpClient, Settings settings)
        {
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _settings = settings;
            Console.WriteLine($"Settings Url in ContactsService ='{settings.ApiRootUri}'.");
        }

        public async Task<Domain.Models.Contact> GetContactAsync(int contactId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.View);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}";
            return await ExecuteGetAsync<Domain.Models.Contact>(url);
        }

        public async Task<List<Domain.Models.Contact>> GetContactsAsync()
        {
            await SetRequestHeader(Domain.Permissions.Contacts.List);
            var url = $"{_settings.ApiRootUri}contacts";
            Console.WriteLine($"Contacts.GetContactsAsync Url='{url}'.");
            return await ExecuteGetAsync<List<Domain.Models.Contact>>(url);
        }

        public async Task<List<Domain.Models.Contact>> GetContactsAsync(string firstName, string lastName)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.Search);
            var url = $"{_settings.ApiRootUri}contacts/search?firstname={firstName}&lastname={lastName}";
            return await ExecuteGetAsync<List<Domain.Models.Contact>>(url);
        }

        public async Task<Domain.Models.Contact> SaveContactAsync(Domain.Models.Contact contact)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.Save);
            var url = $"{_settings.ApiRootUri}contacts/";
            var jsonRequest = JsonSerializer.Serialize(contact);
            var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);

            if (response.StatusCode != HttpStatusCode.Created)
                throw new HttpRequestException(
                    $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            contact = JsonSerializer.Deserialize<Domain.Models.Contact>(content, options);
            return contact;
        }

        public async Task<bool> DeleteContactAsync(Domain.Models.Contact contact)
        {
            return await DeleteContactAsync(contact.ContactId);
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.Delete);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}";
            var response = await _httpClient.DeleteAsync(url);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<Domain.Models.Phone> GetContactPhoneAsync(int contactId, int phoneId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.View);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}/phones/{phoneId}";
            return await ExecuteGetAsync<Domain.Models.Phone>(url);
        }

        public async Task<List<Domain.Models.Phone>> GetContactPhonesAsync(int contactId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.View);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}/phones";
            return await ExecuteGetAsync<List<Domain.Models.Phone>>(url);
        }
        
        public async Task<Domain.Models.Address> GetContactAddressAsync(int contactId, int addressId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.View);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}/addresses/{addressId}";
            return await ExecuteGetAsync<Domain.Models.Address>(url);
        }

        public async Task<List<Domain.Models.Address>> GetContactAddressesAsync(int contactId)
        {
            await SetRequestHeader(Domain.Permissions.Contacts.View);
            var url = $"{_settings.ApiRootUri}contacts/{contactId}/addresses";
            return await ExecuteGetAsync<List<Domain.Models.Address>>(url);
        }
        
        private async Task<T> ExecuteGetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestException(
                    $"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
            
            // Parse the Results
            var content = await response.Content.ReadAsStringAsync();
                
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var results = JsonSerializer.Deserialize<T>(content, options);

            return results;
        }
        
        private async Task SetRequestHeader(string scope, string mediaType = "application/json")
        {
            string fullScopeName = _settings.ApiScopeUri + scope;
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] {fullScopeName});
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }
    }
}