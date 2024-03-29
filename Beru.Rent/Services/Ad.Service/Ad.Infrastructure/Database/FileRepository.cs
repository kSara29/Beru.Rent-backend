﻿using Ad.Application.Contracts.File;
using Ad.Application.JsonOptions;
using Ad.Domain.Models;
using Ad.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;


namespace Ad.Infrastructure.Database;

public class FileRepository : IFileRepository
{
 private readonly AdContext _context;
 private readonly IMinioClient _client;
 private readonly string _endpoint;
 private readonly string _accessKey;
 private readonly string _secretKey;

 public FileRepository(AdContext context, IMinioClient client, IOptions<MinioOptions> minioOptions)
 {
     #region Задаем тестовые эндпоинты куда стучаться и создаем минио клиент для связи с сервером минио
     _endpoint = minioOptions.Value.Endpoint;
     _accessKey = minioOptions.Value.AccessKey;
     _secretKey = minioOptions.Value.SecretKey;
     // Initialize Minio client
     var minio = new MinioClient()
         .WithSSL(false)
         .WithEndpoint(_endpoint)
         .WithCredentials(_accessKey, _secretKey)
         .Build();
     #endregion
     _context = context;
     _client = minio;
 }

 private static async Task PutObjectsToMinioAsync(IMinioClient minio,
        string bucketName,
        string objectName, //my obj name
        Stream filestream, //stream
        IProgress<ProgressReport> progress = null,
        IServerSideEncryption sse = null)
    {
        #region Создаем бакет-папку с айти клиента отправившего файл
        try
        {
            Console.WriteLine("Running example for API: MakeBucketAsync");
            await minio.MakeBucketAsync(
                new MakeBucketArgs()
                    .WithBucket(bucketName)
                    .WithLocation("us-east-1")
            ).ConfigureAwait(false);
            Console.WriteLine($"Created bucket {bucketName}");
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
        }
        #endregion
        try
        {
            var metaData = new Dictionary<string, string>
                (StringComparer.Ordinal) { { "Test-Metadata", "Test  Test" } };
            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(filestream)
                .WithObjectSize(filestream.Length)
                .WithContentType("application/octet-stream")
                .WithHeaders(metaData)
                .WithProgress(progress)
                .WithServerSideEncryption(sse);
            _ = await minio.PutObjectAsync(args).ConfigureAwait(false);

            Console.WriteLine($"Uploaded object {objectName} to bucket {bucketName}");
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
        }
    }

    public async Task<Guid> UploadFileAsync(FileModel entity, IFormFile file)
    {
        try
        {
            #region Отправляем файл в Минио и определяем название папки и тд
            // я также беру оригинальное имя файла и делаю уникальное. Уникальное отрпаляю в Минио и в Постгрес, а оригинальное только в постгресс.
            // чтобы при доставании файла присваивать ему оригинальное, и чтобы исключить затирание одинаковых файлов в минио по одному имени


            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin); // Reset the position to the beginning of the stream
                // Use the Minio client to upload the file
                var bucketName = entity.BucketName; // Replace with your bucket name
                entity.MinioFileName = DateTime.Now.ToFileTime() + bucketName + entity.OriginFileName; // создаю уникальное имя
                await PutObjectsToMinioAsync(_client, bucketName, entity.MinioFileName, stream);
            }

            #endregion

            #region Добавляем модель в постгресс с определенными названиями папки = bucketName

            entity.CreatedAt = DateTime.Now;
            await _context.Files.AddAsync(entity);
            await _context.SaveChangesAsync();

            #endregion

            return entity.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            throw;
        }
    }

    public async Task<string> RemoveFileAsync(Guid id)
    {
        try
        {
            var file = await _context.Files.FindAsync(id);
            var bucketName = file.BucketName;
            var minioName = file.MinioFileName;

            await RemoveFileFromMinioAsync(_client, bucketName, minioName);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return "success";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }

    }

    public async Task<byte[]?> GetFileAsync(Guid id)
    {
        try
        {
            var file = await _context.Files.FindAsync(id);
            var bucketName = file.BucketName;
            var minioName = file.MinioFileName;

            using (Stream? stream = await GetFileFromMinio(_client, bucketName, minioName, file.OriginFileName))
            {
                if (stream == null)
                {
                    return null; // Or handle the case where the stream is null
                }

                var memoryStream = new MemoryStream();
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(memoryStream);
                var bytes = memoryStream.ToArray();
                return bytes;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task<List<byte[]?>> GetAllFilesAsync(Guid bucketId)
    {
        #region идем в Postgress И забираем массив id файлов внутри бакета с BucketId

        var fileIds = await _context.Files
            .Where(f => f.BucketName == bucketId.ToString())
            .Select(f=>f.Id)
            .ToListAsync();

        if (fileIds.Count == 0)
            return null;
        #endregion

        #region идем в Minio и по очереди забираем файлы, записываем в массив байтов

        var files = new List<byte[]?>();
        foreach (var fileId in fileIds)
        {
            files.Add(await GetFileAsync(fileId));
        }

        return files;
        #endregion
    }


    public static async Task RemoveFileFromMinioAsync(IMinioClient minio,
        string bucketName,
        string objectName,
        string versionId = null)
    {
        if (minio is null) throw new ArgumentNullException(nameof(minio));

        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
                
            var versions = "";
            if (!string.IsNullOrEmpty(versionId))
            {
                args = args.WithVersionId(versionId);
                versions = ", with version ID " + versionId + " ";
            }

            Console.WriteLine("Running example for API: RemoveObjectAsync");
            await minio.RemoveObjectAsync(args).ConfigureAwait(false);
            Console.WriteLine($"Removed object {objectName} from bucket {bucketName}{versions} successfully");
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket-Object]  Exception: {e}");
        }
    }
    
    public static async Task<Stream> GetFileFromMinio(IMinioClient minio,
        string bucketName,
        string objectName,
        string fileName)
    {
        try
        {
            #region запрашиваю у Минио, есть ли такой файл у них
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
            await minio.StatObjectAsync(statObjectArgs);
            #endregion

            Stream result = new MemoryStream();
            
            Console.WriteLine("Running example for API: GetObjectAsync");
            #region Запись в поток и присвоение имени оригинального
            
            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) => {stream.CopyTo(result);});
            #endregion
          
            var stat = await minio.GetObjectAsync(args).ConfigureAwait(false);
            Console.WriteLine($"Downloaded the file {fileName} in bucket {bucketName}");
            Console.WriteLine($"Stat details of object {objectName} in bucket {bucketName}\n" + stat);
            Console.WriteLine();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
            return null;
        }
        
    }
}