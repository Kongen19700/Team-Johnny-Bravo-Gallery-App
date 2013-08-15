using Gallery.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JohnnyBravoGalleryApp.Models
{
    public class AlbumFullModel : AlbumModel
    {
        public IEnumerable<AlbumModel> SubAlbums { get; set; }
        public IEnumerable<ImageModel> Images { get; set; }

        public static AlbumFullModel CreateFullAlbumModelFromEntity(Album entity)
        {
            return new AlbumFullModel()
            {
                AlbumId = entity.AlbumId,
                Title = entity.Title,
                ParentAlbumId = entity.ParentAlbumId,
                SubAlbums = entity.Albums1.Select(a => new AlbumModel()
                {
                    AlbumId = a.AlbumId,
                    ParentAlbumId = a.ParentAlbumId,
                    Title = a.Title,
                }),
                Images = entity.Images.Select(i => new ImageModel()
                {
                    AlbumId = i.AlbumId,
                    ImageId = i.ImageId,
                    Title = i.Title,
                    Url = i.Url,
                })
            };
        }
    }
}