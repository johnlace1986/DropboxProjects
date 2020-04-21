using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NisbetPhotography.Website
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INisbetPhotographyService" in both code and config file together.
    [ServiceContract]
    public interface INisbetPhotographyService
    {
        [OperationContract]
        Business.AlbumTypeEnum GetAlbumTypeEnum(int enumAsInt);

        [OperationContract]
        void UploadImageToCustomerAlbum(Guid customerId, Int16 albumId, byte[] image);

        [OperationContract]
        void UploadImageToPortfolio(Int16 portfolioCategoryId, byte[] image);

        [OperationContract]
        void UploadImageToPublicAlbum(Int16 publicAlbumId, byte[] image);
    }
}
