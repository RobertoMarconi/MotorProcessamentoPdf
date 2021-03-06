﻿using Business.Shared.Models;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Core.ICore
{
    public interface ITransformaPdfCore
    {
        byte[] PdfConcatenation(IEnumerable<byte[]> files);
        Task<byte[]> PdfConcatenation(IEnumerable<string> urls);
        Task<byte[]> ConcatenarUrlEArquivo(string urlDocumento, byte[] file);
        byte[] HtmlPdf(string html);
        byte[] HtmlPdf(byte[] file);
        byte[] PdfPagination(byte[] file, int itemsByPage, int page);
        bool IsPdf(byte[] file);
        bool IsPdfa1b(byte[] file);
        byte[] RemoveAnnotations(byte[] file);
        byte[] MetaPDFA(byte[] file);
        bool PossuiRestricoes(byte[] file);
        Task<bool> PossuiRestricoes(string url);
        //Task<ApiResponse<PdfInfo>> PdfInfo(InputFile inputFile);
        ApiResponse<PdfInfo> PdfInfo(byte[] file);
        Task<ApiResponse<PdfInfo>> PdfInfo(string url);
        //Task<ValidationsResult> Validacoes(string url, string validations);
    }
}
