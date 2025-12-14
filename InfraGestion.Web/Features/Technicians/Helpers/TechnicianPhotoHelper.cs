using System;

namespace InfraGestion.Web.Features.Technicians.Helpers
{
    public static class TechnicianPhotoHelper
    {
        // Available photos in wwwroot/images/techs
        private static readonly string[] Photos = new[]
        {
            "tech01.png","tech02.png","tech03.png","tech04.png","tech05.png",
            "tech06.png","tech07.png","tech08.png","tech09.png","tech10.png",
            "tech11.png","tech12.png","tech13.png","tech14.png","tech15.png"
        };

        /// <summary>
        /// Returns a photo URL for a technician. If an explicit photoUrl is provided it is returned as-is.
        /// Otherwise the helper deterministically selects one of the bundled photos using the technician id
        /// or, if id is not available, a name hash. This avoids changing pictures between reloads.
        /// </summary>
        public static string GetPhotoUrl(string? photoUrl, int id, string? name)
        {
            if (!string.IsNullOrWhiteSpace(photoUrl))
                return photoUrl!;

            int index = Math.Abs(id) % Photos.Length;

            return $"/images/techs/{Photos[index]}";
        }
    }
}
