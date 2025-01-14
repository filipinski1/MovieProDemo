﻿
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MovieProDemo.Services.Interfaces
{
    public interface IImageService
    {
        Task<byte[]> EncodeImageAsnc(IFormFile poster);
        Task<byte[]> EncodeImageURLAsync(string imageURL);
        string DecodeImage(byte[] poster, string contentType);
    }
}
