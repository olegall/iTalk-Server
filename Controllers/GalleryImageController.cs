﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using WebApplication1.Utils;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GalleryImageController : ImageController
    {
        [HttpGet]
        [ImageOutputCache(VaryByParam = "galleryId", Duration = CACHE_DURATION)]
        public async Task<ActionResult> Index(long galleryId)
        {
            IGenericRepository<GalleryImage> rep = new Repositories().GalleryImages;
            GalleryImage image = rep.Get().SingleOrDefault(x => x.Id == galleryId);
            Check(image);
            string ext = GetExtension(image.FileName);
            CheckExtension(ext);
            HttpContext.Cache.Add(galleryId.ToString(),
                                  image.Bytes,
                                  null,
                                  Cache.NoAbsoluteExpiration,
                                  TimeSpan.FromSeconds(CACHE_DURATION),
                                  CacheItemPriority.Normal,
                                  null);
            Response.BinaryWrite((byte[])HttpContext.Cache[galleryId.ToString()]);
            Response.ContentType = $"image/{ext}";
            return new EmptyResult();
        }
    }
}