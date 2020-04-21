using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NisbetPhotography.DbObjects.Business;
using NisbetPhotography.DbObjects.Exception;

namespace NisbetPhotography.DbObjectsTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            User customer = new User(new Guid("2d569439-964f-4622-9a58-acf6ff753594"));
            CustomerAlbum album = new CustomerAlbum();
            album.Customer = customer;
            album.Name = "customer album";
            album.Description = "description";
            album.Save();

            for (int i = 0; i < 3; i++)
                album.AddImage("image url");

            album.SetThumbnailImage(2);

            album.Delete();
        }
    }
}
