﻿using APIItextSharp.Models;
using AutoMapper;
using BusinessItextSharp.Model.CertificadoDigital;
using BusinessItextSharp.Models;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace APIItextSharp.StartupConfigurations
{
    public static class AutomapperConfiguration
    {
        public static IServiceCollection ConfigurarAutomapper(this IServiceCollection services)
        {
            // configurar automapper
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<IFormFile, byte[]>().ConvertUsing<IFormFileToByteArray>();
                cfg.CreateMap<InputFileDto, InputFile>().ConvertUsing<InputFileDTOToInputFile>();
                
                cfg.CreateMap<CertificadoDigital, CertificadoDigitalDto>();
                cfg.CreateMap<PessoaFisica, PessoaFisicaDto>();
                cfg.CreateMap<PessoaJuridica, PessoaJuridicaDto>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public class InputFileDTOToInputFile : ITypeConverter<InputFileDto, InputFile>
        {
            public InputFile Convert(InputFileDto source, InputFile destination, ResolutionContext context)
            {
                InputFile inputFile = new InputFile();
                inputFile.FileUrl = source.FileUrl;
                // TODO(Marcelo): Pesquisar a possibilidade de usar async
                using (var memoryStream = new MemoryStream())
                {
                    source.FileBytes.CopyTo(memoryStream);
                    inputFile.FileBytes = memoryStream.ToArray(); 
                    memoryStream.Close();
                }

                return inputFile;
            }
        }

        public class IFormFileToByteArray : ITypeConverter<IFormFile, byte[]>
        {
            public byte[] Convert(IFormFile source, byte[] destination, ResolutionContext context)
            {
                byte[] byteArray;
                using (var memoryStream = new MemoryStream())
                {
                    source.CopyTo(memoryStream);
                    byteArray = memoryStream.ToArray();
                    memoryStream.Close();
                }

                return byteArray;
            }
        }
    }
}
