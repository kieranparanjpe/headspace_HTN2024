using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class MasvService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseApiUrl = "https://api.massive.io/v1";

    public MasvService()
    {
        _httpClient = new HttpClient();
        this._apiKey = Environment.GetEnvironmentVariable("GROQ_CLIENT_SECRET");


        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }


    public async Task<string> GetDownloadLinkAsync(string packageId, string fileId)
    {
        try
        {
            string endpoint = $"{_baseApiUrl}/packages/{packageId}/files/{fileId}/download";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"Failed to get download link: {response.StatusCode}");
                return null;
            }

  
            string responseBody = await response.Content.ReadAsStringAsync();
            string downloadUrl = JObject.Parse(responseBody)?["downloadUrl"]?.ToString();

            if (string.IsNullOrEmpty(downloadUrl))
            {
                Debug.LogError("Download URL not found in response.");
                return null;
            }

            return downloadUrl;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while fetching the download link: {ex.Message}");
            return null;
        }
    }

    // Method to download the file from the MASV download link
    public async Task<bool> DownloadFileAsync(string fileUrl, string destinationPath)
    {
        try
        {
            Debug.Log($"Starting download from {fileUrl}");

            HttpResponseMessage response = await _httpClient.GetAsync(fileUrl);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError($"Failed to download file: {response.StatusCode}");
                return false;
            }

           
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

         
            using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                          fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
            {
                await contentStream.CopyToAsync(fileStream);
            }

            Debug.Log($"File successfully downloaded to {destinationPath}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error while downloading the file: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DownloadFileFromMasvAsync(string packageId, string fileId, string destinationPath)
    {
        string downloadUrl = await GetDownloadLinkAsync(packageId, fileId);

        if (string.IsNullOrEmpty(downloadUrl))
        {
            Debug.LogError("Failed to retrieve download URL.");
            return false;
        }

        return await DownloadFileAsync(downloadUrl, destinationPath);
    }
}
